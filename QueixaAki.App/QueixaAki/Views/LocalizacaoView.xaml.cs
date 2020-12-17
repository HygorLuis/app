using System;
using System.Threading.Tasks;
using QueixaAki.ViewModels;
using Xamarin.Forms.GoogleMaps;

namespace QueixaAki.Views
{
    public partial class LocalizacaoView
    {
        private LocalizacaoViewModel _viewModel;

        private Pin pinAtual = new Pin
        {
            Type = PinType.Place,
            Label = "Sua queixa está aki?",
            IsDraggable = true
        };

        public LocalizacaoView()
        {
            InitializeComponent();

            _viewModel = new LocalizacaoViewModel();
            BindingContext = _viewModel;

            LocalizacaoAtual();
        }

        private async void LocalizacaoAtual()
        {
            try
            {
                var localizacao = await _viewModel.GetLocalizacao();

                Map.MyLocationEnabled = true;
                Map.UiSettings.MyLocationButtonEnabled = true;

                await SelectedPin(localizacao.Latitude, localizacao.Longitude);
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        private async void Map_OnPinDragEnd(object sender, PinDragEventArgs e)
        {
            try
            {
                if (e == null || e.Pin == null) return;

                await SelectedPin(e.Pin.Position.Latitude, e.Pin.Position.Longitude);
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        private async Task SelectedPin(double latitude, double longitude)
        {
            try
            {
                pinAtual.Position = new Position(latitude, longitude);
                pinAtual.Address = await _viewModel.GetEndereço(latitude, longitude);

                if (Map.Pins.Count == 0)
                    Map.Pins.Add(pinAtual);
                else
                    Map.Pins[0] = pinAtual;

                Map.SelectedPin = pinAtual;
                Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(latitude, longitude), Distance.FromKilometers(0.5)));
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            var teste = Map.SelectedPin;
        }
    }
}