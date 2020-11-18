using System.Threading.Tasks;
using System.Windows.Input;
using QueixaAki.ViewModels.Base;
using QueixaAki.Views;
using Xamarin.Forms;

namespace QueixaAki.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand EntrarCommand { get; set; }
        public ICommand CadastrarCommand { get; set; }

        public string Email { get; set; }
        public string Senha { get; set; }

        public LoginViewModel()
        {
            EntrarCommand = new Command(Entrar);
            CadastrarCommand = new Command(() =>
            {
                MessagingCenter.Send("", "Cadastrar");
            });
        }

        public async void Entrar()
        {
            await Xamarin.Essentials.SecureStorage.SetAsync("isLogged", "1");
            Application.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync($"//{nameof(InicioView)}");
        }
    }
}
