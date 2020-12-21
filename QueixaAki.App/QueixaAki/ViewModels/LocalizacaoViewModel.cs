using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using QueixaAki.Helpers;
using QueixaAki.Models;
using QueixaAki.Services;
using QueixaAki.ViewModels.Base;
using Xamarin.Forms;

namespace QueixaAki.ViewModels
{
    public class LocalizacaoViewModel : BaseViewModel
    {
        public ICommand ConfirmarLocalizacaoCommand { get; set; }
        private QueixaService _queixaService;
        private ConexaoService _conexaoService;
        private UsuarioService _usuarioService;
        private Queixa _queixa;

        public LocalizacaoViewModel(Queixa queixa)
        {
            _queixaService = new QueixaService();
            _conexaoService = new ConexaoService();
            _usuarioService = new UsuarioService();

            _queixa = queixa;
            _queixa.Endereco = new Endereco();

            ConfirmarLocalizacaoCommand = new Command(ConfirmarLocalizacao);
        }

        public async Task<string> GetEndereco(double latitude, double longitude)
        {
            try
            {
                _queixa.Latitude = $"{latitude}";
                _queixa.Longitude = $"{longitude}";

                var endereco = await GetEnderecoLocalizacao(latitude, longitude);

                if (endereco == null) return $"Latitude: {latitude}; Longitude: {longitude}";

                _queixa.Endereco.Rua = endereco.Rua;
                _queixa.Endereco.Numero = endereco.Numero;
                _queixa.Endereco.Bairro = endereco.Bairro;
                _queixa.Endereco.Cidade = endereco.Cidade;
                _queixa.Endereco.Estado = endereco.Estado;
                _queixa.Endereco.Cep = endereco.Cep;

                return $"{_queixa.Endereco.Rua}, {_queixa.Endereco.Numero} - {_queixa.Endereco.Bairro}, {_queixa.Endereco.Cidade} - {_queixa.Endereco.Estado}";
            }
            catch (Exception e)
            {
                return $"Latitude: {latitude}; Longitude: {longitude}";
            }
        }

        public async void ConfirmarLocalizacao()
        {
            try
            {
                Carregando = true;

                _queixa.DataCriacao = DateTime.Now;

                var conexao = await _conexaoService.BuscarConexao(new List<string> { _queixa.Endereco.Cidade }, new List<string> { _queixa.Endereco.Estado });
                var usuario = await _usuarioService.BuscarUsuarioId(App.IdUsuario);

                if (conexao.Item1 != null && conexao.Item1.Id > 0
                                          && usuario.Endereco.Cidade.ApenasLetras().ToUpper() == _queixa.Endereco.Cidade.ApenasLetras().ToUpper()
                                          && usuario.Endereco.Estado.ApenasLetras().ToUpper() == _queixa.Endereco.Estado.ApenasLetras().ToUpper())
                {
                    var (sucesso, erro) = await _queixaService.Incluir(_queixa);
                    if (sucesso)
                    {
                        MessagingCenter.Send(new Message
                        {
                            Title = "Sucesso",
                            MessageText = "Queixa enviada com sucesso!"
                        }, "Message");
                    }
                    else
                    {
                        MessagingCenter.Send(new Message
                        {
                            Title = "Erro ao Incluir Queixa",
                            MessageText = erro
                        }, "Message");
                    }
                }
                else
                {
                    MessagingCenter.Send(new Message
                    {
                        Title = "Erro",
                        MessageText = "Sua queixa não está disponível para essa localização!"
                    }, "Message");
                }
            }
            catch (Exception e)
            {
                MessagingCenter.Send(new Message
                {
                    Title = "Erro ao Enviar Queixa",
                    MessageText = e.Message
                }, "Message");
            }
            finally
            {
                Carregando = false;
            }
        }
    }
}
