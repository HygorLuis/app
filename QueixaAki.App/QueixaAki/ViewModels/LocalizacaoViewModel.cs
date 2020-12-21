using System;
using System.Threading.Tasks;
using System.Windows.Input;
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
        private Queixa _queixa;

        public LocalizacaoViewModel(Queixa queixa)
        {
            _queixaService = new QueixaService();

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
