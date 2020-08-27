using StrawberryMusic.Command;
using System.Windows;
using System.Windows.Input;

namespace StrawberryMusic.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;

        public ICommand updateViewCommand { get; set; }

        public BaseViewModel selectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged("selectedViewModel");
            }
        }

        public MainViewModel()
        {
            updateViewCommand = new UpdateViewCommand(this);
        }

    }
}
