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

            var login = await _usuarioService.BuscarUsuario(Email, Senha);

            if (login.Item1 == null || login.Item1.Id <= 0)
            {
                MessagingCenter.Send(new Message
                {
                    Title = "Erro",
                    MessageText = "Usuário ou senha inválidos!"
                }, "Message");
                return false;
            }

            _usuario = login.Item1;

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

                SetRegistro();

                Application.Current.MainPage = new AppShell();
                await Shell.Current.GoToAsync($"//{nameof(InicioView)}");
            }
            catch (Exception e)
            {
                // ignored
            }
            finally
            {
                Carregando = false;
            }

            /*await Xamarin.Essentials.SecureStorage.SetAsync("isLogged", "1");
            Application.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync($"//{nameof(InicioView)}");*/
        }

        public async void SetRegistro()
        {
            try
            {
                Settings.IdUsuario = _usuario.Id.ToString();
                Settings.IdConexao = _usuario.Conexao.Id.ToString();

                /*Settings.Servidor = _usuario.Conexao.Servidor;
                Settings.Banco = _usuario.Conexao.Banco;
                Settings.Usuario = _usuario.Conexao.Usuario;
                Settings.Senha = _usuario.Conexao.Senha;*/
            }
            catch (Exception e)
            {
                // ignored
            }
        }
    }
}
