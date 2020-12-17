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

            SetLocalizacaoAtual();
        }

        private async void SetLocalizacaoAtual()
        {
            var localizacao = await _viewModel.GetLocalizacao();

            Map.MyLocationEnabled = true;
            Map.UiSettings.MyLocationButtonEnabled = true;

            await SelectedPin(localizacao.Latitude, localizacao.Longitude);
        }

        private async void Map_OnPinDragEnd(object sender, PinDragEventArgs e)
        {
            if (e == null || e.Pin == null) return;

            await SelectedPin(e.Pin.Position.Latitude, e.Pin.Position.Longitude);
        }

        private async Task SelectedPin(double latitude, double longitude)
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

        private void Map_OnMyLocationButtonClicked(object sender, MyLocationButtonClickedEventArgs e)
        {
            SetLocalizacaoAtual();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            var teste = Map.SelectedPin;
        }
    }
}