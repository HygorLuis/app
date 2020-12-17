using QueixaAki.Helpers;
using Xamarin.Forms;
using QueixaAki.Views;
using Xamarin.Essentials;

namespace QueixaAki
{
    public partial class App : Application
    {

        //public static string ConnectionString = "Server=10.0.2.2; Initial Catalog=QueixaAki; User ID=queixaaki; Password=3ky09p005";
        //public static string ConnectionString = "Server=queixaqui.database.windows.net; Initial Catalog=QueixaAki; User ID=queixaki; Password=/3XpLO5@*+57690/";
        public static string ConnectionQueixaAki = "Server=queixaaki.database.windows.net; Initial Catalog=QueixaAki; User ID=queixaaki; Password=/3XpLO5@*+57690/";
        public static string ConnectionBanco { get; set; }
        public static PermissionStatus PermissaoLocalizacao { get; set; }
        public static PermissionStatus PermissaoMidia { get; set; }

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

        // Handle when your app start
        protected override async void OnStart()
        {
            PermissaoLocalizacao = await CheckPermissions.VerificarLocalizacao();
            PermissaoMidia = await CheckPermissions.VerificarMidia();
        }

        // Handle when your app sleeps
        protected override void OnSleep()
        {
        }

        // Handle when your app resumes
        protected override void OnResume()
        {
        }
    }
}
