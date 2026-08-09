using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Threading;

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

    public List<int> RatingNumbers { get; } = new List<int>();

    public pgAIDetect()
    {
      InitializeComponent();
      DataContext = this;
      for (int i = 1; i <= 7; i++) RatingNumbers.Add(i);
    }

    public void aiRate()
    {
      TypeSession.NewQuiz("AIScore");
      lblQuiz.Text = "How comfortable do You feel being monitored by AI ?";
      lblQuiz.Visibility = Visibility.Visible;
      icQuiz.Visibility = Visibility.Visible;
    }

    private void RatingChecked(object sender, System.Windows.RoutedEventArgs e)
    {
      if (sender is RadioButton rb && int.TryParse(rb.Tag?.ToString(), out int rating))
      {
        TypeSession.Quiz.Rate = rating;
        ((RadioButton)sender).IsChecked = false;
        if (TypeSession.Quiz.QuizName == "AIScore") // First
        {
          TypeSession.NewQuiz("AIRate");
          lblQuiz.Text = "How accurate do You think the AI prediction of this emotion is?";
        }
        else   // Complete quiz 
        {
          lblQuiz.Visibility = Visibility.Collapsed;
          icQuiz.Visibility = Visibility.Collapsed;
        }
      }
    }
    
    // Notify
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private void PageLoaded(object sender, System.Windows.RoutedEventArgs e)
    {
      Emotion = "Undetected"; OnPropertyChanged(nameof(Emotion));
      EmotionImg = "/Resources/emneutral.png"; OnPropertyChanged(nameof(EmotionImg));
      ConfidenceVal = 0; OnPropertyChanged(nameof(ConfidenceVal));
      Confidence = $"Confidence {ConfidenceVal}%"; OnPropertyChanged(nameof(Confidence));
    }
  }
}
