using System.Windows.Input;
using QueixaAki.ViewModels.Base;
using Xamarin.Forms;

namespace QueixaAki.ViewModels
{
    public class InicioViewModel : BaseViewModel
    {
        public ICommand QueixaAkiCommand { get; set; }

        public string TextButton => "QUEIXA\nAKI";

        public InicioViewModel()
        {
            QueixaAkiCommand = new Command(() =>
            {
                MessagingCenter.Send("", "QueixaAki");
            });
        }
    }
}
