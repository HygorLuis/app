using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using QueixaAki.ViewModels.Base;
using Xamarin.Essentials;

namespace QueixaAki.ViewModels
{
    public class LocalizacaoViewModel : BaseViewModel
    {
        public LocalizacaoViewModel()
        {

        }

        public async Task<string> GetEndereço(double latitude, double longitude)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var addrs = (await Geocoding.GetPlacemarksAsync(new Location(latitude, longitude))).FirstOrDefault();
                    if (addrs == null) return "";

                    var endereco = $"{addrs.Thoroughfare}, {addrs.SubThoroughfare}";
                    var bairro = addrs.SubLocality;
                    var cidade = $"{addrs.SubAdminArea} - {Estados[addrs.AdminArea]}";
                    var cep = addrs.PostalCode;
                    var pais = addrs.CountryName;

                    return $"{endereco} - {bairro}, {cidade}";
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
    }
}
