using System.Windows;
using System.Windows.Controls;

namespace WritingTask
{
  /// <summary>
  /// Interaction logic for pgStart.xaml
  /// </summary>
  public partial class pgStart : Page
  {
    public pgStart()
    {
      InitializeComponent();
    }

    private void StartClick(object sender, RoutedEventArgs e)
    {
      App.StepBar.NextStep();
      App.MainFrame.Navigate(new pgLogin());
    }
  }
}
