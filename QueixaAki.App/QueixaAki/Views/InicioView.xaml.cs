using QueixaAki.Models;
using QueixaAki.ViewModels;
using Xamarin.Forms;
using MediaFile = Plugin.Media.Abstractions.MediaFile;

namespace QueixaAki.Views
{
    public partial class InicioView
    {
        private readonly InicioViewModel _viewModel;
        public InicioView()
        {
            InitializeComponent();

            _viewModel = new InicioViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<MediaFile>(this, "QueixaAki", msg =>
            {
                Navigation.PushAsync(new QueixaAkiView(msg), true);
            });

            MessagingCenter.Subscribe<Message>(this, "Message", msg =>
            {
                DisplayAlert(msg.Title, msg.MessageText, "OK");
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<MediaFile>(this, "QueixaAki");
            MessagingCenter.Unsubscribe<Message>(this, "Message");
        }
    }
}