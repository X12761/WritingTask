using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
