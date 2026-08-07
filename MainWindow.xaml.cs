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
  }
}
