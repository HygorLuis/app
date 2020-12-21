using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Xamarin.Forms;
using QueixaAki.Models;
using QueixaAki.Services;
using Xamarin.Essentials;

namespace QueixaAki.ViewModels.Base
{
    public class BaseViewModel : Base
    {
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

        private bool _carregando = false;
        public bool Carregando
        {
            get => _carregando;
            set
            {
                _carregando = value;
                OnPropertyChanged();
            }
        }

        public async Task<bool> VerificarAcessoInternet()
        {
            return await Task.Run(() => CrossConnectivity.Current.IsConnected);
        }

        public async Task<Location> GetLocalizacao()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
            var cts = new CancellationTokenSource();
            return await Geolocation.GetLocationAsync(request, cts.Token);
        }

        public async Task<Endereco> GetEnderecoLocalizacao(double latitude, double longitude)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var adders = (await Geocoding.GetPlacemarksAsync(new Location(latitude, longitude))).FirstOrDefault();
                    if (adders == null) return null;

                    var endereco = new Endereco
                    {
                        Rua = adders.Thoroughfare,
                        Numero = adders.SubThoroughfare,
                        Bairro = adders.SubLocality,
                        Cidade = adders.SubAdminArea,
                        Estado = Estados[adders.AdminArea],
                        Cep = adders.PostalCode,
                };

                    return endereco;
                }
                catch (Exception e)
                {
                    return null;
                }
            });
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

        protected bool SetProperty<T>(ref T backingStore, T value,
        [CallerMemberName] string propertyName = "",
        Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
