using System;
using System.IO;
using System.Text;
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

    private void PageLoaded(object sender, RoutedEventArgs e)
    {
      if (File.Exists("data/code.txt"))
      {
        TypeSession.code = File.ReadAllText("data/code.txt");
        txtCode.Text = TypeSession.code;
        txtCode.IsEnabled = false;
        btnSet.Visibility = Visibility.Collapsed;
      }
    }

    private void NextClick(object sender, RoutedEventArgs e)
    {
      using (var writer = new StreamWriter($"out\\login{TypeSession.Id}.csv", false, Encoding.UTF8)) // Login data
      {
        writer.WriteLine("Code;Age;Gender;Native;English;Keystroke;Start");
        writer.WriteLine($"{TypeSession.code};{TypeSession.age};{TypeSession.gender};{TypeSession.lang};{TypeSession.english};{TypeSession.prof};{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
      }

      // Check files
      for (int stage=0;stage<2;stage++)
        if (!File.Exists($"data/info{TypeSession.code.Substring(stage * 2, 2)}.rtf") || 
          !File.Exists($"data/topic{TypeSession.code[stage * 2]}.txt"))
        {
          MessageBox.Show($"Check rtf and txt files according {TypeSession.code}", "Warning",
                MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }

      App.StepBar.NextStep();
      App.MainFrame.Navigate(new pgPause());
    }

    private void SetClick(object sender, RoutedEventArgs e)
    {
      txtCode.IsEnabled = false;
      btnSet.IsEnabled = false;
    }
  }
}
