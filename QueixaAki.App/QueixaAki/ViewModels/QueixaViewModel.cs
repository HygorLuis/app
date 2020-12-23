using System.IO;
using QueixaAki.Models;
using QueixaAki.ViewModels.Base;
using Xam.Forms.VideoPlayer;

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
            VideoSource = VideoSource.FromFile(queixa.Arquivo.Path);
        }
    }
}
