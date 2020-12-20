using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
//using Xamarin.Essentials;
//using PermissionStatus = Xamarin.Essentials.PermissionStatus;

namespace QueixaAki.Helpers
{
    public static class CheckPermissions
    {
        public static async Task<PermissionStatus> VerificarLocalizacao()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationWhenInUse);

            if (status == PermissionStatus.Granted)
                return status;

            var conceded = await CrossPermissions.Current.RequestPermissionsAsync(Permission.LocationWhenInUse);
            return conceded[Permission.LocationWhenInUse];
        }

        public static async Task<PermissionStatus> VerificarMidia()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (status == PermissionStatus.Granted)
                return status;

            var conceded = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
            return conceded[Permission.Storage];
        }











        /*public static async Task<PermissionStatus> VerificarLocalizacao()
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
        public static async Task<PermissionStatus> VerificarMidia()
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

        public static async Task<PermissionStatus> CheckAndRequestCameraPermission()
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
        }*/
    }
}
