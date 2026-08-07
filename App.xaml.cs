using System.Windows;
using System.Windows.Controls;

namespace WritingTask
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>

  public partial class App : Application
  {
    public static Frame MainFrame { get; set; }
    public static StepProgressBar StepBar { get; set; }
  }
}
