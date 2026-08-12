using System.IO;
using System.Linq;
using System.Windows;

namespace WritingTask
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      App.MainFrame = MainFrame;
      App.StepBar = new StepProgressBar();
      DataContext = App.StepBar;
    
      iniData("WritingTask.ini");
      App.MainFrame.Navigate(new pgStart());
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      App.StepBar.NextStep();
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
      App.StepBar.PrevStep();
    }

    private void iniData(string ininame= "config.ini")
    {
      var ini = File.ReadLines(ininame)
        .Where(l => !string.IsNullOrWhiteSpace(l) && !l.StartsWith("#") && !l.StartsWith(";"))
        .Select(l => l.Split(new[] { '=' }, 2))
        .ToDictionary(p => p[0].Trim(), p => p[1].Trim());

      int x; string v;
      string val = ini.TryGetValue("PauseTime", out v) ? v : null;
      TypeSession.PauseTime = int.TryParse(val, out x) ? x : TypeSession.PauseTime;
      val=ini.TryGetValue("PauseStart", out v) ? v : null;
      TypeSession.PauseNext = int.TryParse(val, out x) ? x : TypeSession.PauseNext;
      val = ini.TryGetValue("KeyCount", out v) ? v : null;
      TypeSession.KeyCount = int.TryParse(val, out x) ? x : TypeSession.KeyCount;
      val = ini.TryGetValue("KeyTime", out v) ? v : null;
      TypeSession.KeyTime = int.TryParse(val, out x) ? x : TypeSession.KeyTime;
    }
  }
}
