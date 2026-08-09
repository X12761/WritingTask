using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace WritingTask
{
  /// <summary>
  /// Interaction logic for pgLogin.xaml
  /// </summary>
  public partial class pgLogin : Page
  {
    public pgLogin()
    {
      InitializeComponent();
      DataContext = TypeSession.Instance;
    }

    private void NextClick(object sender, RoutedEventArgs e)
    {
      App.StepBar.NextStep();
      App.MainFrame.Navigate(new pgPause());
    }

    private void SetClick(object sender, RoutedEventArgs e)
    {
      txtCode.IsEnabled = false;
      btnSet.IsEnabled = false;
    }

    private void PageLoaded(object sender, RoutedEventArgs e)
    {
      if (File.Exists("data/code.txt"))
      {
        TypeSession.code = File.ReadAllText("data/code.txt");
        txtCode.IsEnabled = false;
        btnSet.Visibility = Visibility.Collapsed;
      }
    }
  }
}
