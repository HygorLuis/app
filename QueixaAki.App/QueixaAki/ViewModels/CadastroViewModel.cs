using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using QueixaAki.Helpers;
using QueixaAki.Models;
using QueixaAki.Services;
using QueixaAki.ViewModels.Base;
using Xamarin.Forms;

namespace QueixaAki.ViewModels
{
    public class CadastroViewModel : BaseViewModel
    {
        public ICommand SalvarCommand { get; set; }

        private Usuario _usuario;
        public Usuario Usuario
        {
            get => _usuario;
            set
            {
                _usuario = value;
                OnPropertyChanged();
            }
        }
        public string ConfirmarSenha { get; set; }
        public string MaskedTelefone { get; set; }

        private UsuarioService _usuarioService;

        public CadastroViewModel()
        {
            _usuarioService = new UsuarioService();
            Usuario = new Usuario
            {
                DataNascimento = DateTime.Today,
                Excluido = false
            };

            SalvarCommand = new Command(Salvar);
        }

        public void Salvar()
        {
            if (!Validar()) return;

            Usuario.DataCriacao = DateTime.Now;

            var result = _usuarioService.Incluir(Usuario);
            if (result)
                MessagingCenter.Send(new Message
                {
                    Title = "Sucesso",
                    MessageText = "Usuário salvo com sucesso!"
                }, "Message");
        }

        private bool Validar()
        {
            #region CAMPOS OBRIGATORIOS

            var campos = "";

            #region LOGIN

            if (string.IsNullOrEmpty(Usuario.Email))
                campos += string.IsNullOrEmpty(campos) ? "E-Mail" : ", E-Mail";

            if (string.IsNullOrEmpty(Usuario.Senha))
                campos += string.IsNullOrEmpty(campos) ? "Senha" : ", Senha";

            if (string.IsNullOrEmpty(ConfirmarSenha))
                campos += string.IsNullOrEmpty(campos) ? "Confirmar Senha" : ", Confirmar Senha";

            #endregion

            #region DADOS PESSOAIS

            if (string.IsNullOrEmpty(Usuario.Nome))
                campos += string.IsNullOrEmpty(campos) ? "Nome" : ", Nome";

            if (string.IsNullOrEmpty(Usuario.CPF))
                campos += string.IsNullOrEmpty(campos) ? "CPF" : ", CPF";

            if (Usuario.DataNascimento == null)
                campos += string.IsNullOrEmpty(campos) ? "Data de Nascimento" : ", Data de Nascimento";

            #endregion

            #region CONTATO

            if (string.IsNullOrEmpty(Usuario.Telefone1) && string.IsNullOrEmpty(Usuario.Telefone2))
                campos += string.IsNullOrEmpty(campos) ? "Telefone" : ", Telefone";

            #endregion

            #region ENDEREÇO

            if (string.IsNullOrEmpty(Usuario.Cep))
                campos += string.IsNullOrEmpty(campos) ? "Cep" : ", Cep";

            if (string.IsNullOrEmpty(Usuario.Rua))
                campos += string.IsNullOrEmpty(campos) ? "Rua" : ", Rua";

            if (string.IsNullOrEmpty(Usuario.Numero))
                campos += string.IsNullOrEmpty(campos) ? "Número" : ", Número";

            if (string.IsNullOrEmpty(Usuario.Bairro))
                campos += string.IsNullOrEmpty(campos) ? "Bairro" : ", Bairro";

            if (string.IsNullOrEmpty(Usuario.Cidade))
                campos += string.IsNullOrEmpty(campos) ? "Cidade" : ", Cidade";

            if (string.IsNullOrEmpty(Usuario.Estado))
                campos += string.IsNullOrEmpty(campos) ? "Estado" : ", Estado";

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

            #region VALIDAR CAMPOS

            #region LOGIN

            if (!Usuario.Email.ValidarEMail())
            {
                MessagingCenter.Send(new Message
                {
                    Title = "E-Mail",
                    MessageText = "E-Mail não é válido!"
                }, "Message");
                return false;
            }

            // VERIFICAR SE E-MAIL JÁ ESTÁ CADASTRADO
            /*if (!Usuario.CPF.ValidateCPF())
            {
                MessagingCenter.Send(new Message
                {
                    Title = "CPF",
                    MessageText = "CPF não é válido!"
                }, "Message");
                return false;
            }*/

            if (Usuario.Senha != ConfirmarSenha)
            {
                MessagingCenter.Send(new Message
                {
                    Title = "Confirmar Senha",
                    MessageText = "As senhas estão diferentes!"
                }, "Message");
                return false;
            }

            #endregion

            #region DADOS PESSOAIS

            // VERIFICAR SE RG JÁ ESTÁ CADASTRADO
            /*if (!Usuario.CPF.ValidateCPF())
            {
                MessagingCenter.Send(new Message
                {
                    Title = "CPF",
                    MessageText = "CPF não é válido!"
                }, "Message");
                return false;
            }*/

            if (!Usuario.CPF.ValidarCPF())
            {
                MessagingCenter.Send(new Message
                {
                    Title = "CPF",
                    MessageText = "CPF não é válido!"
                }, "Message");
                return false;
            }

            // VERIFICAR SE CPF JÁ ESTÁ CADASTRADO
            /*if (!Usuario.CPF.ValidateCPF())
            {
                MessagingCenter.Send(new Message
                {
                    Title = "CPF",
                    MessageText = "CPF não é válido!"
                }, "Message");
                return false;
            }*/

            #endregion

            #endregion

            return true;
        }

        public async Task BuscaCep(string cep)
        {
            try
            {
                if (string.IsNullOrEmpty(cep) || string.IsNullOrWhiteSpace(cep)) return;
                if (!cep.ValidarCep()) return;

                var request = (HttpWebRequest)WebRequest.Create("https://viacep.com.br/ws/" + cep + "/json/");
                request.AllowAutoRedirect = false;
                var checaServidor = (HttpWebResponse)request.GetResponse();

                if (checaServidor.StatusCode != HttpStatusCode.OK)
                {
                    MessagingCenter.Send(new Message
                    {
                        Title = "Servidor indisponível",
                        MessageText = "Erro ao buscar cep!"
                    }, "Message");
                    return;
                }

                using (var webStream = checaServidor.GetResponseStream())
                {
                    if (webStream != null)
                    {
                        using (var responseReader = new StreamReader(webStream))
                        {
                            var response = responseReader.ReadToEnd();
                            response = Regex.Replace(response, "[{},]", string.Empty);
                            response = response.Replace("\"", "");

                            var substrings = response.Split('\n');

                            if (substrings[1].Split(":".ToCharArray()).Contains("erro"))
                            {
                                MessagingCenter.Send(new Message
                                {
                                    Title = "Buscar cep",
                                    MessageText = "Cep não encontrado!"
                                }, "Message");
                                return;
                            }

                            Usuario.Rua = substrings[2].Split(":".ToCharArray())[1];
                            Usuario.Bairro = substrings[4].Split(":".ToCharArray())[1];
                            Usuario.Cidade = substrings[5].Split(":".ToCharArray())[1];
                            Usuario.Estado = substrings[6].Split(":".ToCharArray())[1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // ignored
            }
        }
    }
}
