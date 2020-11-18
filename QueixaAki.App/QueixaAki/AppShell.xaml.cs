using QueixaAki.ViewModels.Base;
using QueixaAki.Views;
using Xamarin.Forms;

namespace QueixaAki
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            //Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));

            //Routing.RegisterRoute(nameof(InicioView), typeof(InicioView));
            Routing.RegisterRoute(nameof(QueixasView), typeof(QueixasView));
            Routing.RegisterRoute(nameof(ConfiguracoesView), typeof(ConfiguracoesView));

            BindingContext = new BaseViewModel();
        }

    }
}
