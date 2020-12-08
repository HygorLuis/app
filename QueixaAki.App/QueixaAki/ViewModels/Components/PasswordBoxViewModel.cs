using System.Windows.Input;
using QueixaAki.ViewModels.Base;
using Xamarin.Forms;

namespace QueixaAki.ViewModels.Components
{
    public class PasswordBoxViewModel : BaseViewModel
    {
        public PasswordBoxViewModel()
        {
            ShowHideTapCommand = new Command(() =>
            {
                IsPasswordShow = !IsPasswordShow;
            });
        }

        public ICommand ShowHideTapCommand { get; set; }

        private bool _isPasswordShow = true;
        public bool IsPasswordShow
        {
            get { return _isPasswordShow; }
            set
            {
                _isPasswordShow = value;
                OnPropertyChanged();
                OnPropertyChanged("ShowHideIcon");
            }
        }

        public string ShowHideIcon => IsPasswordShow ? "eye.png" : "hide.png";
    }
}
