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
