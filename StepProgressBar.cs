using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace WritingTask
{
  //----------------------------------------------------------------- Steps model
  public class StepProgressBar : INotifyPropertyChanged
  {
    private int _currentStep = 1;
    private int _totalSteps;
    private int _radius;

    public string StatusText => $"Step {_currentStep} of {_totalSteps}";

    // Step items collection
    public ObservableCollection<StepItem> Steps { get; } = new ObservableCollection<StepItem>();

    public StepProgressBar(int total = 8, int radius = 24)
    {
      _totalSteps = total;
      _radius = radius;
      Steps.Clear();
      for (int i = 1; i <= _totalSteps; i++)
      {
        Brush color;
        if (i < _currentStep) color = Brushes.Green;      // Done steps
        else if (i == _currentStep) color = Brushes.DodgerBlue; // Current step
        else color = Brushes.LightGray;  // Steps to go

        Steps.Add(new StepItem { StepNumber = i, Radius = _radius, CircleColor = color });
      }
    }

    // Next step
    public void NextStep()
    {
      if (_currentStep < _totalSteps)
      {
        _currentStep++;
        OnPropertyChanged(nameof(StatusText));
        Update();
      }
    }

    // Previous step
    public void PrevStep()
    {
      if (_currentStep > 1)
      {
        _currentStep--;
        OnPropertyChanged(nameof(StatusText));
        Update();
      }
    }

    // Set step
    public void GotoStep(int n)
    {
      if (n > _totalSteps || n < 1) return;
      _currentStep = n;
      OnPropertyChanged(nameof(StatusText));
      Update();
    }

    // Update step items - any other parameter add here
    private void Update()
    {
      for (int i = 1; i <= _totalSteps; i++)
      {
        if (i < _currentStep) Steps[i - 1].CircleColor = Brushes.Green;      // Done steps
        else if (i == _currentStep) Steps[i - 1].CircleColor = Brushes.DodgerBlue; // Current step
        else Steps[i - 1].CircleColor = Brushes.LightGray;  // Steps to go
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
  }

  // Simple round item model
  public class StepItem : INotifyPropertyChanged
  {
    private Brush _circleColor;
    private int _radius;
    public int StepNumber { get; set; }
    public int Radius { get => _radius;
      set {
        if (_radius != value)
        { _radius = value; OnPropertyChanged(nameof(Radius)); } } }
    public Brush CircleColor { get => _circleColor;
      set {
        if (_circleColor != value)
        { _circleColor = value; OnPropertyChanged(nameof(CircleColor)); } } }

    public event PropertyChangedEventHandler PropertyChanged; // Notify UI
    protected void OnPropertyChanged([CallerMemberName] string name = null) => 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
  }
}
