using System;
using QueixaAki.Components;
using QueixaAki.Helpers;
using QueixaAki.Models;
using QueixaAki.ViewModels;
using Xamarin.Forms;

namespace QueixaAki.Views
{
    public partial class CadastroView
    {
        private CadastroViewModel _viewModel;
        public CadastroView()
        {
            InitializeComponent();

            _viewModel = new CadastroViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<Message>(this, "Message", msg =>
            {
                DisplayAlert(msg.Title, msg.MessageText, "OK");
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<Message>(this, "Message");
        }

        private async void Cep_OnUnfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(_viewModel.Usuario.Cep)) return;

            var entry = (BaseEntry)sender;
            await _viewModel.BuscaCep(entry.Text);
        }
    }
}