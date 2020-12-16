using System;
using QueixaAki.Models;
using QueixaAki.ViewModels;
using Xamarin.Forms;

namespace QueixaAki.Views
{
    public partial class QueixaAkiView
    {
        public QueixaAkiViewModel _viewModel;

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

            MessagingCenter.Subscribe<Exception>(this, "ErroEnviar", async msg =>
            {
                await DisplayAlert("Erro ao enviar queixa", msg.Message, "OK");
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<Message>(this, "Message");
            MessagingCenter.Unsubscribe<Exception>(this, "ErroEnviar");
        }
    }
}