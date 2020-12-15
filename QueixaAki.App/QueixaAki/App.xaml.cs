using QueixaAki.Helpers;
using Xamarin.Forms;
using QueixaAki.Services;
using QueixaAki.Views;

namespace QueixaAki
{
    public partial class App : Application
    {

        //public static string ConnectionString = "Server=10.0.2.2; Initial Catalog=QueixaAki; User ID=queixaaki; Password=3ky09p005";
        //public static string ConnectionString = "Server=queixaqui.database.windows.net; Initial Catalog=QueixaAki; User ID=queixaki; Password=/3XpLO5@*+57690/";
        public static string ConnectionQueixaAki = "Server=queixaaki.database.windows.net; Initial Catalog=QueixaAki; User ID=queixaaki; Password=/3XpLO5@*+57690/";
        public static string ConnectionBanco { get; set; }

        public App()
        {
            InitializeComponent();

            //DependencyService.Register<MockDataStore>();
            //var isLoogged = Xamarin.Essentials.SecureStorage.GetAsync("isLogged").Result;
            //if (isLoogged == "1")
            if (!string.IsNullOrEmpty(Settings.IdUsuario))
            {
                MainPage = new AppShell();
            }
            else
            {
                MainPage = new NavigationPage(new LoginView());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
