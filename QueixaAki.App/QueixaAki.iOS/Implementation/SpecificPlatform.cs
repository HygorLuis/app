using System;
using QueixaAki.Interfaces;
using QueixaAki.iOS.Implementation;
using Xamarin.Forms;

[assembly: Dependency(typeof(SpecificPlatform))]
namespace QueixaAki.iOS.Implementation
{
    public class SpecificPlatform : ISpecificPlatform
    {
        public string RootFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
    }
}