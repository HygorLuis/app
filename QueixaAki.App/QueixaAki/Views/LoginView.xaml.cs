using System;
using QueixaAki.Models;
using QueixaAki.ViewModels;
using Xamarin.Forms;

namespace QueixaAki.Views
{
    public partial class LoginView
    {
        private LoginViewModel _viewModel;

        public LoginView()
        {
            InitializeComponent();

            _viewModel = new LoginViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<string>(this, "Entrar", msg =>
            {
                Navigation.PushAsync(new InicioView(), true);
            });

            MessagingCenter.Subscribe<string>(this, "Cadastrar", msg =>
            {
                Navigation.PushAsync(new CadastroView(), true);
            });

            MessagingCenter.Subscribe<Message>(this, "Message", msg =>
            {
                DisplayAlert(msg.Title, msg.MessageText, "OK");
                if (msg.Title == "Sucesso")
                    Navigation.PopToRootAsync(true);
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<string>(this, "Entrar");
            MessagingCenter.Unsubscribe<string>(this, "Cadastrar");
            MessagingCenter.Unsubscribe<Message>(this, "Message");
        }

        private void SenhaBox_OnOnTextChanged(object sender, EventArgs e)
        {
            var box = (Entry)sender;
            if (box == null) return;
            _viewModel.Senha = box.Text;
        }
    }
}