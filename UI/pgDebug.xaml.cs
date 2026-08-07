using System.Windows;
using System.Windows.Controls;

namespace WritingTask
{
  /// <summary>
  /// Interaction logic for pgDebug.xaml
  /// </summary>
  public partial class pgDebug : Page
  {
    private int curSess = -1;

    public pgDebug()
    {
      InitializeComponent();
      NextClick(this, new RoutedEventArgs());
    }

    private void NextClick(object sender, RoutedEventArgs e)
    {
      curSess++;
      if (curSess >= TypeSession.Sessions.Count) curSess = 0;
      LogDataGrid.ItemsSource = TypeSession.Sessions[curSess].KeyLog;
      lblSession.Text = $"Session {curSess} stated {TypeSession.Sessions[curSess].Started} completed {TypeSession.Sessions[curSess].Completed}";
    }

    private void BackClick(object sender, RoutedEventArgs e)
    {
      App.MainFrame.GoBack();
    }
  }
}
