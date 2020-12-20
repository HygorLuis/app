using System.IO;
using Octane.Xamarin.Forms.VideoPlayer;
using QueixaAki.Models;
using QueixaAki.ViewModels.Base;

namespace QueixaAki.ViewModels
{
    public class QueixaViewModel : BaseViewModel
    {
        private VideoSource _videoSource;
        public VideoSource VideoSource
        {
            get => _videoSource;
            set
            {
                _videoSource = value;
                OnPropertyChanged();
            }
        }

        public QueixaViewModel(Queixa queixa)
        {
            VideoSource = VideoSource.FromStream(() => new MemoryStream(queixa.Arquivo.ArquivoByte), queixa.Formato);
        }
    }
}
