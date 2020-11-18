using System;
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
            MessagingCenter.Subscribe<string>(this, "ErroGaleria", async msg =>
            {
                await DisplayAlert("Ops", "Galeria de videos não suportada.", "OK");
            });

            MessagingCenter.Subscribe<string>(this, "ErroCamera", async msg =>
            {
                await DisplayAlert("Ops", "Nenhuma câmera detectada.", "OK");
            });

            MessagingCenter.Subscribe<Exception>(this, "ErroEnviar", async msg =>
            {
                await DisplayAlert("Erro ao enviar queixa!", msg.Message, "OK");
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<string>(this, "ErroGaleria");
            MessagingCenter.Unsubscribe<string>(this, "ErroCamera");
            MessagingCenter.Unsubscribe<string>(this, "ErroEnviar");
        }
    }
}