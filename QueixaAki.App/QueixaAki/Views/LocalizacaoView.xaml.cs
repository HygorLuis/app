using System;
using System.Linq;
using System.Threading;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

namespace QueixaAki.Views
{
    public partial class LocalizacaoView
    {
        public LocalizacaoView()
        {
            InitializeComponent();

            LocalizacaoAtual();
        }

        private async void LocalizacaoAtual()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                var cts = new CancellationTokenSource();
                var locationActual = await Geolocation.GetLocationAsync(request, cts.Token);

                Map.MyLocationEnabled = true;
                Map.UiSettings.MyLocationButtonEnabled = true;

                var pinAtual = new Pin
                {
                    Type = PinType.Place,
                    Label = "Sua queixa está aki?",
                    Position = new Position(locationActual.Latitude, locationActual.Longitude),
                    IsDraggable = true
                };

                Map.Pins.Add(pinAtual);
                Map.SelectedPin = pinAtual;
                Map.MoveToRegion(MapSpan.FromCenterAndRadius(pinAtual.Position, Distance.FromKilometers(0.5)));


                var addrs = (await Geocoding.GetPlacemarksAsync(new Location(locationActual.Latitude, locationActual.Longitude))).FirstOrDefault();
                var Street = $"{addrs.Thoroughfare} {addrs.SubThoroughfare}";
                var City = $"{addrs.PostalCode} {addrs.Locality}";
                var Country = addrs.CountryName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
        }



        /*public async void btn_clicked(object sender, System.EventArgs e)
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                var cts = new CancellationTokenSource();
                var locationActual = await Geolocation.GetLocationAsync(request, cts.Token);
    
                var location = new Location(locationActual.Latitude, locationActual.Longitude);
                var options = new MapLaunchOptions
                {
                    Name = EntryNome.Text
                };
                await Map.OpenAsync(location, options);
            }
            catch (Exception ex)
            {
                // Não foi possivel acessar o mapa
                await DisplayAlert("Erro : ", ex.Message, "Ok");
            }
        }*/
        private void Button_OnClicked(object sender, EventArgs e)
        {
            var teste = Map.SelectedPin;
        }
    }
}