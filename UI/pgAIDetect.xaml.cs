using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace WritingTask
{
  /// <summary>
  /// Interaction logic for pgAIScore.xaml
  /// </summary>
  public partial class pgAIDetect : Page, INotifyPropertyChanged
  {
    public string Emotion { get; set; }
    public string EmotionImg { get; set; }
    public int ConfidenceVal { get; set; }
    public string Confidence { get; set; }

    public pgAIDetect()
    {
      InitializeComponent();
      DataContext = this;
    }

    // Notify
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private void FrameLoaded(object sender, System.Windows.RoutedEventArgs e)
    {
      Emotion = "Undetected"; OnPropertyChanged(nameof(Emotion));
      EmotionImg = "/Resources/emneutral.png"; OnPropertyChanged(nameof(EmotionImg));
      ConfidenceVal = 0; OnPropertyChanged(nameof(ConfidenceVal));
      Confidence = $"Confidence {ConfidenceVal}%"; OnPropertyChanged(nameof(Confidence));

    }
  }
}
