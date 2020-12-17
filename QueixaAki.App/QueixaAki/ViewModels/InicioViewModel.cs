using System.Threading.Tasks;
using System.Windows.Input;
using QueixaAki.Helpers;
using QueixaAki.Models;
using QueixaAki.ViewModels.Base;
using Xamarin.Essentials;
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
                if (App.PermissaoLocalizacao == PermissionStatus.Granted && App.PermissaoMidia == PermissionStatus.Granted)
                    MessagingCenter.Send("", "QueixaAki");
                else
                {
                    var permiss = App.PermissaoLocalizacao != PermissionStatus.Granted ? "Localização" : "";
                    permiss += !string.IsNullOrEmpty(permiss) && App.PermissaoMidia != PermissionStatus.Granted 
                        ? ", Mídia" 
                        : string.IsNullOrEmpty(permiss) && App.PermissaoMidia != PermissionStatus.Granted 
                            ? "Mídia" : "";

                    MessagingCenter.Send(new Message
                    {
                        Title = "Permissões Necessárias",
                        MessageText = $"Favor dar permissão as seguintes solicitações nas configurações do seu telefone: {permiss}"
                    }, "Message");
                }
            });
        }
    }
}
