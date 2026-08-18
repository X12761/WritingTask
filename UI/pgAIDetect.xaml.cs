using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Threading;
using System;

namespace WritingTask
{
  /// <summary>
  /// Interaction logic for pgAIScore.xaml
  /// </summary>
  public partial class pgAIDetect : Page, INotifyPropertyChanged
  {
    private string _emotion;
    public string Emotion { get => _emotion; set { _emotion = value; OnPropertyChanged(nameof(Emotion)); } }
    private string _emimg; 
    public string EmotionImg { get => _emimg; set { _emimg = value; OnPropertyChanged(nameof(EmotionImg)); } }
    private double _confVal;
    public double ConfidenceVal { get => _confVal; set { _confVal = value; OnPropertyChanged(nameof(ConfidenceVal)); } }
    private string _conf;
    public string Confidence { get => _conf; set { _conf = value; OnPropertyChanged(nameof(Confidence)); } }

    public List<int> RatingNumbers { get; } = new List<int>();

    public pgAIDetect()
    {
      InitializeComponent();
      DataContext = this;
      for (int i = 1; i <= 7; i++) RatingNumbers.Add(i);
    }

    public void aiRate(Char predisction, double confidence)
    {
      TypeSession.NewQuiz("AIScore");
      TypeSession.Quiz.aiPredict = predisction;
      TypeSession.Quiz.aiConfidence = confidence;
      lblQuiz.Text = "How comfortable do You feel being monitored by AI ?";
      lblQuiz.Visibility = Visibility.Visible;
      icQuiz.Visibility = Visibility.Visible;
      gridImg.Visibility = Visibility.Visible;
    }

    private void RatingChecked(object sender, RoutedEventArgs e)
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

    private void PageLoaded(object sender, RoutedEventArgs e)
    {
      Emotion = "Undetected"; 
      EmotionImg = "/Resources/emneutral.png"; 
      ConfidenceVal = 0; 
      Confidence = $"Confidence {ConfidenceVal}%"; 
    }
  }
}
