using QueixaAki.Models;
using QueixaAki.ViewModels;
using Xamarin.Forms;

namespace QueixaAki.Views
{
    public partial class ConfiguracoesView
    {
        private ConfiguracoesViewModel _viewModel;

        public ConfiguracoesView()
        {
            InitializeComponent();

            _viewModel = new ConfiguracoesViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<Message>(this, "Questao", async msg =>
            {
                var confirma = await DisplayAlert(msg.Title, msg.MessageText, "Sim", "Não");
                if (confirma)
                    await _viewModel.Sair();
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<Message>(this, "Questao");
        }
    }
}