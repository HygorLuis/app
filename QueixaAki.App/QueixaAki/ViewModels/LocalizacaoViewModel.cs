using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using QueixaAki.Models;
using QueixaAki.Services;
using QueixaAki.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace QueixaAki.ViewModels
{
    public class LocalizacaoViewModel : BaseViewModel
    {
        public ICommand ConfirmarLocalizacaoCommand { get; set; }
        private QueixaService _queixaService;
        private Queixa _queixa;
        private static IReadOnlyDictionary<string, string> Estados = new Dictionary<string, string>
        {
            {"Acre", "AC"},
            {"Alagoas", "AL"},
            {"Amapá", "AP"},
            {"Amazonas", "AM"},
            {"Bahia", "BA"},
            {"Ceara", "CE"},
            {"Distrito Federal", "DF"},
            {"Espirito Santo", "ES"},
            {"Goiás", "GO"},
            {"Maranhão", "MA"},
            {"Mato Grosso", "MT"},
            {"Mato Grosso do Sul", "MS"},
            {"Minas Gerais", "MG"},
            {"Para", "PA"},
            {"Paraíba", "PB"},
            {"Paraná", "PR"},
            {"Pernambuco" , "PE"},
            {"Piauí", "PI"},
            {"Rio de Janeiro", "RJ"},
            {"Rio Grande do Norte", "RN"},
            {"Rio Grande do Sul", "RS"},
            {"Rondônia", "RO"},
            {"Roraima", "RR"},
            {"Santa Catarina", "SC"},
            {"São Paulo", "SP"},
            {"Sergipe", "SE"},
            {"Tocantins", "TO"},
        };

        public LocalizacaoViewModel(Queixa queixa)
        {
            _queixaService = new QueixaService();

            _queixa = queixa;
            _queixa.Endereco = new Endereco();

            ConfirmarLocalizacaoCommand = new Command(ConfirmarLocalizacao);
        }

        public async Task<string> GetEndereço(double latitude, double longitude)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    _queixa.Latitude = $"{latitude}";
                    _queixa.Longitude = $"{longitude}";

                    var adders = (await Geocoding.GetPlacemarksAsync(new Location(latitude, longitude))).FirstOrDefault();
                    if (adders == null) return "";

                    _queixa.Endereco.Rua = adders.Thoroughfare;
                    _queixa.Endereco.Numero = adders.SubThoroughfare;
                    _queixa.Endereco.Bairro = adders.SubLocality;
                    _queixa.Endereco.Cidade = adders.SubAdminArea;
                    _queixa.Endereco.Estado = Estados[adders.AdminArea];
                    _queixa.Endereco.Cep = adders.PostalCode;

                    /*var endereco = $"{addrs.Thoroughfare}, {addrs.SubThoroughfare}";
                    var bairro = addrs.SubLocality;
                    var cidade = $"{addrs.SubAdminArea} - {Estados[addrs.AdminArea]}";
                    var cep = addrs.PostalCode;
                    var pais = addrs.CountryName;*/

                    return $"{_queixa.Endereco.Rua}, {_queixa.Endereco.Numero} - {_queixa.Endereco.Bairro}, {_queixa.Endereco.Cidade} - {_queixa.Endereco.Estado}";
                }
                catch (Exception e)
                {
                    return $"Latitude: {latitude}; Longitude: {longitude}";
                }
            });
        }

        public async Task<Location> GetLocalizacao()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
            var cts = new CancellationTokenSource();
            return await Geolocation.GetLocationAsync(request, cts.Token);
        }

        public async void ConfirmarLocalizacao()
        {
            try
            {
                Carregando = true;

                _queixa.DataCriacao = DateTime.Now;
                _queixa.Arquivo.DataCriacao = DateTime.Now;

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
