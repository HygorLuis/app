using System;
using System.Threading.Tasks;
using System.Windows.Input;
using QueixaAki.Helpers;
using QueixaAki.Models;
using QueixaAki.Services;
using QueixaAki.ViewModels.Base;
using QueixaAki.Views;
using Xamarin.Forms;

namespace QueixaAki.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private UsuarioService _usuarioService;

        public ICommand EntrarCommand { get; set; }
        public ICommand CadastrarCommand { get; set; }

        private Usuario _usuario;
        public string Email { get; set; }
        public string Senha { get; set; }

        public LoginViewModel()
        {
            _usuarioService = new UsuarioService();

            EntrarCommand = new Command(Entrar);
            CadastrarCommand = new Command(() =>
            {
                MessagingCenter.Send("", "Cadastrar");
            });
        }

        public async Task<bool> Validar()
        {
            #region CAMPOS OBRIGATORIOS

            var campos = "";

            #region LOGIN

            if (string.IsNullOrEmpty(Email))
                campos += string.IsNullOrEmpty(campos) ? "E-Mail" : ", E-Mail";

            if (string.IsNullOrEmpty(Senha))
                campos += string.IsNullOrEmpty(campos) ? "Senha" : ", Senha";

            #endregion

            if (!string.IsNullOrEmpty(campos))
            {
                MessagingCenter.Send(new Message
                {
                    Title = "Campos em Branco",
                    MessageText = campos
                }, "Message");
                return false;
            }

            #endregion

            var (usuario, erro) = await _usuarioService.BuscarUsuario(Email, Senha);

            if (usuario == null || usuario.Id <= 0)
            {
                MessagingCenter.Send(new Message
                {
                    Title = "Erro",
                    MessageText = "Usuário ou senha inválidos!"
                }, "Message");
                return false;
            }

            _usuario = usuario;

            return true;
        }

        public async void Entrar()
        {
            try
            {
                Carregando = true;

                if (!await VerificarAcessoInternet())
                {
                    MessagingCenter.Send(new Message
                    {
                        Title = "Erro",
                        MessageText = "Sem acesso à internet!"
                    }, "Message");
                    return;
                }

                if (!await Validar()) return;

                await SetRegistro();

                Application.Current.MainPage = new AppShell();
                await Shell.Current.GoToAsync($"//{nameof(InicioView)}");
            }
            catch (Exception e)
            {
                MessagingCenter.Send(new Message
                {
                    Title = "Erro ao Entrar",
                    MessageText = e.Message
                }, "Message");
            }
            finally
            {
                Carregando = false;
            }

            /*await Xamarin.Essentials.SecureStorage.SetAsync("isLogged", "1");
            Application.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync($"//{nameof(InicioView)}");*/
        }

        public async Task SetRegistro()
        {
            try
            {
                App.IdUsuario = _usuario.Id;
                App.IdConexao = _usuario.Conexao.Id;
                Settings.IdUsuario = _usuario.Id.ToString();
                Settings.IdConexao = _usuario.Conexao.Id.ToString();
            }
            catch (Exception e)
            {
                // ignored
            }
        }
    }
}
