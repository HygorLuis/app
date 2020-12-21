using System.Threading.Tasks;
using QueixaAki.Models;
using QueixaAki.ViewModels;
using Xamarin.Forms;
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

        public LocalizacaoView(Queixa queixa)
        {
            InitializeComponent();

            _viewModel = new LocalizacaoViewModel(queixa);
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
            pinAtual.Address = await _viewModel.GetEndereco(latitude, longitude);

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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<Message>(this, "Message", msg =>
            {
                DisplayAlert(msg.Title, msg.MessageText, "OK");
                if (msg.Title != "Sucesso") return;

                Navigation.PopToRootAsync(true);
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<Message>(this, "Message");
        }
    }
}