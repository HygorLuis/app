using QueixaAki.ViewModels;

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
    }
}