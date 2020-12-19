using QueixaAki.Models;
using QueixaAki.ViewModels;
using Xamarin.Forms;
using MediaFile = Plugin.Media.Abstractions.MediaFile;

namespace QueixaAki.Views
{
    public partial class QueixaAkiView
    {
        private QueixaAkiViewModel _viewModel;

        public QueixaAkiView(MediaFile mediaFile)
        {
            InitializeComponent();

            _viewModel = new QueixaAkiViewModel(mediaFile);
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<Message>(this, "Message", msg =>
            {
                DisplayAlert(msg.Title, msg.MessageText, "OK");
            });

            MessagingCenter.Subscribe<Queixa>(this, "EnivarQueixa", msg =>
            {
                Navigation.PushAsync(new LocalizacaoView(msg), true);
            });

            VPvideoPlayer.IsVisible = true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<Message>(this, "Message");
            MessagingCenter.Unsubscribe<string>(this, "EnivarQueixa");
            VPvideoPlayer.IsVisible = false;
        }
    }
}