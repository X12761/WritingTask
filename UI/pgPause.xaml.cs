using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;

namespace WritingTask
{
  /// <summary>
  /// Interaction logic for pgPause.xaml
  /// </summary>
  public partial class pgPause : Page
  {
    private int stage;

    private DispatcherTimer _waitTimer;
    private TimeSpan _remain;

    public pgPause(int stage = 0)
    {
      InitializeComponent();
      this.stage = stage;

      _waitTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1)  };
      _remain = TimeSpan.FromMinutes(5);
      _waitTimer.Tick += TimerTick;
      _waitTimer.Start();

      if (stage==0) btnContinue.IsEnabled = true;

      using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText($"data/info{TypeSession.code.Substring(stage*2,2)}.rtf"))))
      {
        var textRange = new TextRange(lblInfo.Document.ContentStart, lblInfo.Document.ContentEnd);
        textRange.Load(stream, DataFormats.Rtf);
      }

#if DEBUG
      btnDebug.Visibility = Visibility.Visible;
      btnContinue.IsEnabled = true; 
#else
      btnDebug.Visibility = Visibility.Collapsed;
#endif
    }

    private void ContinueClick(object sender, RoutedEventArgs e)
    {
      App.StepBar.NextStep();
      App.MainFrame.Navigate(new pgWrite(stage));
    }

    private void DebugClick(object sender, RoutedEventArgs e)
    {
      App.MainFrame.Navigate(new pgDebug());
    }

    private void TimerTick(object sender, EventArgs e)
    {
      if (!btnContinue.IsEnabled && _remain < TimeSpan.FromMinutes(3)) // 2 minutes elapsed
        btnContinue.IsEnabled = true;

      if (_remain <= TimeSpan.Zero) // 5 minutes elapsed - go next
      {
        _waitTimer.Stop();
        ContinueClick(this, new RoutedEventArgs());
      }

      lblRemain.Text = $"Starts in {_remain.ToString(@"hh\:mm\:ss")}"; 
      _remain -= TimeSpan.FromSeconds(1);
    }
  }
}
