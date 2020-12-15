using System.Threading.Tasks;
using System.Windows.Input;
using QueixaAki.Helpers;
using QueixaAki.Models;
using QueixaAki.ViewModels.Base;
using QueixaAki.Views;
using Xamarin.Forms;

namespace QueixaAki.ViewModels
{
    public class ConfiguracoesViewModel : BaseViewModel
    {
        public ICommand SairCommand { get; set; }

        public ConfiguracoesViewModel()
        {
            SairCommand = new Command(() =>
            {
                MessagingCenter.Send(new Message
                {
                    Title = "Sair",
                    MessageText = "Deseja sair do App?"
                }, "Questao");
            });
        }

        public async Task Sair()
        {
            Settings.LimparRegistro();
            Application.Current.MainPage = new NavigationPage(new LoginView());
        }
    }
}
