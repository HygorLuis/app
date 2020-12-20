using System;
using System.Linq;
using QueixaAki.Models;
using QueixaAki.ViewModels;
using Xamarin.Forms;

namespace QueixaAki.Views
{
    public partial class QueixasView
    {
        private QueixasViewModel _viewModel;
        public QueixasView()
        {
            InitializeComponent();

            _viewModel = new QueixasViewModel();
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

        private void QueixasList_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Queixa queixa)
                _viewModel.VerificarQueixa(queixa.Id);
        }
    }
}