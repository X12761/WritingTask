using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

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

    // Block navigation hotkeys
    private void MainFrame_PreviewCanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
    {
      if (e.Command == NavigationCommands.BrowseBack ||
            e.Command == NavigationCommands.BrowseForward ||
            e.Command == NavigationCommands.BrowseHome ||
            e.Command == NavigationCommands.BrowseStop ||
            e.Command == NavigationCommands.Refresh)
      {
        e.CanExecute = false; e.Handled = true;
      }
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
