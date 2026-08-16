using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WritingTask
{
  /// <summary>
  /// Interaction logic for pgFinal.xaml
  /// </summary>
  public partial class pgFinal : Page
  {
    public pgFinal()
    {
      InitializeComponent();

#if DEBUG
      btnDebug.Visibility = Visibility.Visible;
#else
      btnDebug.Visibility = Visibility.Collapsed;
#endif
    }

    private void PageLoaded(object sender, System.Windows.RoutedEventArgs e)
    {
      // Write session log
      foreach (SingleSession s in TypeSession.Sessions)
        using (var writer = new StreamWriter($"out\\key{TypeSession.Id}_{s.Started.ToString("HH.mm.ss")}.csv", false, Encoding.UTF8))
        {
          writer.WriteLine($"Started: {s.Started}; Completed: {s.Completed}; AI score: {s.withAI}");
          writer.WriteLine($"{KeyLogEntry.KeyLogHead}");
          foreach (KeyLogEntry p in s.KeyLog)
            writer.WriteLine($"{p.KeyLogLine()}");
        }
      // Write sesssion quiz data
      using (var writer = new StreamWriter($"out\\quiz{TypeSession.Id}.csv", false, Encoding.UTF8))
      {
        writer.WriteLine("Name;Start;Complete;Score;AI prediction;AI Confidence");
        foreach (QuizLogEntry q in TypeSession.QuizLog)
          writer.WriteLine($"{q.QuizName};{q.Start};{q.Complete};{q.Rate};{q.aiPredict};{q.aiConfidence}");
      }

      // Final RTF
      using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText($"data/infoFinal.rtf"))))
      {
        var textRange = new TextRange(lblInfo.Document.ContentStart, lblInfo.Document.ContentEnd);
        textRange.Load(stream, DataFormats.Rtf);
      }
    }

    private void DebugClick(object sender, System.Windows.RoutedEventArgs e)
    {
      App.MainFrame.Navigate(new pgDebug());
    }
  }
}
