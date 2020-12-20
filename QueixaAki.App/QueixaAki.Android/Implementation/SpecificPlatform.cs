using System.IO;
using Android.Content;
using Android.OS;
using QueixaAki.Droid.Implementation;
using QueixaAki.Interfaces;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Dependency(typeof(SpecificPlatform))]
namespace QueixaAki.Droid.Implementation
{
    public class SpecificPlatform : ISpecificPlatform
    {
        private Context CurrentContext => CrossCurrentActivity.Current.Activity;

        public string RootFolder()
        {
            return Path.Combine(Environment.ExternalStorageDirectory.AbsolutePath, Environment.DirectoryDownloads);
        }
    }
}