using System.ComponentModel;
using QueixaAki.Models;
using QueixaAki.ViewModels;

namespace QueixaAki.Views
{
    [DesignTimeVisible(false)]
    public partial class QueixaView
    {
        private QueixaViewModel _viewModel;

        public QueixaView(Queixa queixa)
        {
            InitializeComponent();

            _viewModel = new QueixaViewModel(queixa);
            BindingContext = _viewModel;
        }
    }
}