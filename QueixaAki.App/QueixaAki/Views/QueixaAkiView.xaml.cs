using QueixaAki.Models;
using QueixaAki.ViewModels;
using Xamarin.Forms;

namespace QueixaAki.Views
{
    public partial class QueixaAkiView
    {
        private QueixaAkiViewModel _viewModel;

        public QueixaAkiView()
        {
            InitializeComponent();

            _viewModel = new QueixaAkiViewModel();
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

            _viewModel.VideoPlayerVisible = !string.IsNullOrEmpty(_viewModel.Queixa.NomeArquivo);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<Message>(this, "Message");
            MessagingCenter.Unsubscribe<string>(this, "EnivarQueixa");

            _viewModel.VideoPlayerVisible = false;
        }
    }
}