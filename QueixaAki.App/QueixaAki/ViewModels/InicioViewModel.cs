using System.Threading.Tasks;
using System.Windows.Input;
using QueixaAki.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace QueixaAki.ViewModels
{
    public class InicioViewModel : BaseViewModel
    {
        public ICommand QueixaAkiCommand { get; set; }

        public string TextButton => "QUEIXA\nAKI";

        public InicioViewModel()
        {
            QueixaAkiCommand = new Command(() =>
            {
                MessagingCenter.Send("", "QueixaAki");
            });

            //QueixaAkiCommand = new Command(Permissoes);
        }

        private void Permissoes()
        {
            throw new System.NotImplementedException();
        }

        public async Task<PermissionStatus> CheckAndRequestLocationPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            return status;
        }

        //APENAS NO iOS
        public async Task<PermissionStatus> CheckAndRequestMediaPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Media>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.Media>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.Media>();

            return status;
        }

        public async Task<PermissionStatus> CheckAndRequestCameraPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.Camera>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.Camera>();

            return status;
        }
    }
}
