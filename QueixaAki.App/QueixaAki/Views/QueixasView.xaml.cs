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
            if (!(e.Item is Queixa queixa)) return;
            if (_viewModel.Queixas.FirstOrDefault(x => x.Id == queixa.Id).Download) return;

            if (queixa.DownloadVisible)
                _viewModel.BaixarArquivo(queixa);
            else
                Navigation.PushAsync(new QueixaView(queixa), true);
        }

        private async void DeleteBtn_OnClicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;

            var queixa = button.CommandParameter as Queixa;
            var confirma = await DisplayAlert("Excluir", $"Deseja realmente excluir a queixa:\n{queixa.NomeArquivoCompleto}\n{queixa.Endereco.EnderecoCompleto}\n{queixa.DataCriacao:dd/MM/yyyy}.\n\nESSE PROCESSO É IRREVERSÍVEL!", "Sim", "Não");
            if (confirma)
                await _viewModel.ExcluirArquivo(queixa.Id);
        }
    }
}