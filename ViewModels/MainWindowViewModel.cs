using System.ComponentModel;

namespace MaquinaRam.ViewModels;

public class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
{
  public event PropertyChangedEventHandler? PropertyChanged;

  protected virtual void OnPropertyChanged(string propertyName)
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }
  
}
