using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace WritingTask
{
  /// <summary>
  /// Interaction logic for GoWrite.xaml
  /// </summary>
  public partial class pgWrite : Page, INotifyPropertyChanged
  {
    private int stage; // Session stage (how mach times write been started)
    private Char aiPred;  // Last AI result (4Quiz)
    private double aiConf;
    private int aiQuiz = 0;   // How mush time Quiz started
    public int KeyCount { get => TypeSession.KeyLog!=null? TypeSession.KeyLog.Count:0; } 
    // Timing 
    private readonly Stopwatch _typewatch = new Stopwatch();
    private DispatcherTimer _uiTimer;
    private bool _onProgress = false; // Prediction on progress
    private bool _needQuiz;           // Needs AI prediction scoring
    // Log and not released (Down-state) keys
    private readonly Dictionary<Key, KeyLogEntry> _activeKeys = new Dictionary<Key, KeyLogEntry>();
    //--------------------------------------------------------------------------- HELPER
    private static string GetKeyName(Key key, ModifierKeys modifiers)
    {
      var name = key.ToString();
      var parts = new List<string>();

      if (modifiers.HasFlag(ModifierKeys.Control)) parts.Add("Ctrl");
      if (modifiers.HasFlag(ModifierKeys.Shift)) parts.Add("Shift");
      if (modifiers.HasFlag(ModifierKeys.Alt)) parts.Add("Alt");

      // Don't log key-modifier
      if (key != Key.LeftCtrl && key != Key.RightCtrl &&
          key != Key.LeftShift && key != Key.RightShift &&
          key != Key.LeftAlt && key != Key.RightAlt) parts.Add(name);

      return string.Join("+", parts);
    }
    //---------------------------------------------------------------------------
    public pgWrite(int stage = 0)
    {
      InitializeComponent();
      DataContext = this;
      this.stage = stage;
      if (TypeSession.code[stage * 2] == 'P') // AI Score
        lblTask.Text = "Describe a time when You felt very happy";
      else
        lblTask.Text = "Describe a time when You felt UNHAPPY";

      if (TypeSession.code[stage * 2 + 1] == 'A') // AI Score
        scoreFrame.Navigate(new pgAIDetect());
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
      Dispatcher.BeginInvoke(new Action(() => // Start after form painted and ready
      {
        TypeSession.NewSession(TypeSession.code[stage * 2 + 1] == 'A'); // Init new session
        _activeKeys.Clear(); _typewatch.Restart();
        _uiTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(33) };
        _uiTimer.Tick += TimerTick;
        _uiTimer.Start();
        txtTask.Focus();
      }), DispatcherPriority.ContextIdle); /* +D+ */
    }

    private void Page_Unloaded(object sender, RoutedEventArgs e)
    {
      TypeSession.Complete();
      if (!_typewatch.IsRunning) return;
      _typewatch.Stop();
      _uiTimer?.Stop();
    }

    private async void TimerTick(object sender, EventArgs e)
    {
      lblElapsed.Text = _typewatch.Elapsed.ToString(@"hh\:mm\:ss");
      OnPropertyChanged(nameof(KeyCount));

      if (_needQuiz)  // Prediction done, need to score it
        if (scoreFrame.Content is pgAIDetect detectPage && // Quiz not started
          detectPage.lblQuiz.Visibility != Visibility.Visible)
        {
          _needQuiz = false;
          detectPage.aiRate(aiPred,aiConf);
        }

      if (_onProgress) return; // Next code for prediction
      if (TypeSession.code[stage * 2 + 1] == 'A') // AI detection session
        // if ((int)_typewatch.Elapsed.TotalSeconds == TypeSession.KeyTime) // Probe trigger +D+
        if (TypeSession.KeyLog.Count >= TypeSession.KeyCount && 
            (int)_typewatch.Elapsed.TotalSeconds >= TypeSession.KeyTime && aiQuiz==0) // Probe trigger
        {
          _onProgress = true;
          aiQuiz++; ((pgAIDetect)scoreFrame.Content).lblQuiz.Text = "Detecting..";
          aiProbe probe = new aiProbe(TypeSession.KeyLog);
          try
          {
            await probe.MakeData();
            string result = await probe.ExecAsync();
            OnScriptResult(result, (pgAIDetect)scoreFrame.Content);
          }
          catch (Exception ex)
          {
            OnScriptError(ex.Message, (pgAIDetect)scoreFrame.Content); 
          }
          finally
          {
            _onProgress = false;
          }
        }
    }

    private void editKeyDown(object sender, KeyEventArgs e)
    {
      if (!_typewatch.IsRunning) return;
      if (e.IsRepeat) return; // Ignore autorepeat +D+

      var key = e.Key;
      var time = _typewatch.Elapsed;

      if (_activeKeys.ContainsKey(key)) return; // Key already down +D+

      _activeKeys[key] = new KeyLogEntry    // Fix key is down
      {
        KeyName = GetKeyName(key, Keyboard.Modifiers),
        PressedAt = time,
        PressedAtMs = time.TotalMilliseconds
      };
      TypeSession.KeyLog.Add(_activeKeys[key]);       // Log key
    }

    private void editKeyUp(object sender, KeyEventArgs e)
    {
      if (!_typewatch.IsRunning) return;

      var key = e.Key;
      if (!_activeKeys.TryGetValue(key, out var entry)) return; // Key not been pressed
      _activeKeys.Remove(key);        // Remove from pressed keys

      var time = _typewatch.Elapsed;  // Fix release
      entry.ReleasedAt = time;
      entry.ReleasedAtMs = time.TotalMilliseconds;
      entry.DurationMs = (time - entry.PressedAt).TotalMilliseconds;
    }

    private void DoneClick(object sender, RoutedEventArgs e)
    {
      _typewatch.Stop();
      _uiTimer?.Stop();

      App.StepBar.NextStep();
      if (stage < 1) // 1 - Final stage +D+
        App.MainFrame.Navigate(new pgPause(stage + 1));
      else
        App.MainFrame.Navigate(new pgFinal());
    }

    private void OnScriptResult(string result, pgAIDetect frame)
    {
      _needQuiz = true;
      switch (result[0])
      {
        case 'A':
          frame.Emotion = "Angry";
          frame.EmotionImg = "/Resources/emangry.png";
          break;
        case 'C':
          frame.Emotion = "Calm";
          frame.EmotionImg = "/Resources/emcalm.png";
          break;
        case 'H':
          frame.Emotion = "Happy";
          frame.EmotionImg = "/Resources/emhappy.png";
          break;
        case 'N':
          frame.Emotion = "Neutral";
          frame.EmotionImg = "/Resources/emneutral.png";
          break;
        case 'S':
          frame.Emotion = "Sad";
          frame.EmotionImg = "/Resources/emsad.png";
          break;
      }

      aiPred = result[0]; // Fix pred resut 4Quiz
      result = result.Substring(2);
      if (double.TryParse(result.Substring(2), out double conf))
      {
        frame.ConfidenceVal = conf;
        frame.Confidence = $"Confidence {conf}%";
        aiConf = conf; // Fix pred resut 4Quiz
      }
      frame.Confidence = result;
    }

    private void OnScriptError(string error, pgAIDetect frame)
    {
      frame.Emotion = error;
    }

    // Notify 4 RichEdit and panel
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); /* +D+ */
  }
}
