using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace WritingTask
{
  /// <summary>
  /// Interaction logic for GoWrite.xaml
  /// </summary>
  public partial class pgWrite : Page//, INotifyPropertyChanged
  {
    private int stage; // Session stage
    // Timing 
    private readonly Stopwatch _stopwatch = new Stopwatch();
    private DispatcherTimer _uiTimer;
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
      if (TypeSession.code[stage*2+1] == 'A') // AI Score
      {
        scoreFrame.Navigate(new pgAIDetect());
      }
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
      Dispatcher.BeginInvoke(new Action(() => // Start after form painted and ready
      {
        TypeSession.NewSession(); // Init new session
        _activeKeys.Clear(); _stopwatch.Restart();
        _uiTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(33), // UI time update
                DispatcherPriority.Normal,
                (s, args) => { lblElapsed.Text = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss"); },
                Dispatcher);
        _uiTimer.Start();
        txtTask.Focus(); }), DispatcherPriority.ContextIdle); /* +D+ */
    }

    private void Page_Unloaded(object sender, RoutedEventArgs e)
    {
      TypeSession.Complete();
      if (!_stopwatch.IsRunning) return;
      _stopwatch.Stop();
      _uiTimer?.Stop();
    }

    private void editKeyDown(object sender, KeyEventArgs e)
    {
      if (!_stopwatch.IsRunning) return;
      if (e.IsRepeat) return; // Ignore autorepeat +D+

      var key = e.Key;
      var time = _stopwatch.Elapsed;

      if (_activeKeys.ContainsKey(key)) return; // Key already down +D+

      _activeKeys[key] = new KeyLogEntry  // Fix rey is down
      {
        KeyName = GetKeyName(key, Keyboard.Modifiers),
        PressedAt = time,
        PressedAtMs = time.TotalMilliseconds
      };
      TypeSession.KeyLog.Add(_activeKeys[key]);       // Log key
    }

    private void editKeyUp(object sender, KeyEventArgs e)
    {
      if (!_stopwatch.IsRunning) return;

      var key = e.Key;
      if (!_activeKeys.TryGetValue(key, out var entry)) return; // Key not been pressed
      _activeKeys.Remove(key);        // Remove from pressed keys

      var time = _stopwatch.Elapsed;  // Fix release
      entry.ReleasedAt = time;
      entry.ReleasedAtMs = time.TotalMilliseconds;
      entry.DurationMs = (time - entry.PressedAt).TotalMilliseconds;
    }

    private void DoneClick(object sender, RoutedEventArgs e)
    {
      _stopwatch.Stop();
      _uiTimer?.Stop();

      App.StepBar.NextStep();
      if (stage<1) // 1 - Final stage +D+
        App.MainFrame.Navigate(new pgPause(stage+1)); else
        App.MainFrame.Navigate(new pgFinal());
    }

    // Notify
    /*public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); /* +D+ */
  }
}
