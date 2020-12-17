using QueixaAki.Models;
using QueixaAki.ViewModels;
using Xamarin.Forms;

namespace QueixaAki.Views
{
    public partial class InicioView
    {
        private InicioViewModel _viewModel;
        public InicioView()
        {
            InitializeComponent();

            _viewModel = new InicioViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<string>(this, "QueixaAki", msg =>
            {
                Navigation.PushAsync(new QueixaAkiView());
            });

            MessagingCenter.Subscribe<Message>(this, "Message", msg =>
            {
                DisplayAlert(msg.Title, msg.MessageText, "OK");
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<string>(this, "QueixaAki");
            MessagingCenter.Unsubscribe<Message>(this, "Message");
        }
    }
}