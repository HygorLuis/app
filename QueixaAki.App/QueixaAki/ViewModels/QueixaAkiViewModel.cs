using System.IO;
using System.Windows.Input;
using Octane.Xamarin.Forms.VideoPlayer;
using QueixaAki.Models;
using QueixaAki.ViewModels.Base;
using Xamarin.Forms;
using MediaFile = Plugin.Media.Abstractions.MediaFile;

namespace QueixaAki.ViewModels
{
    public class QueixaAkiViewModel : BaseViewModel
    {
        public ICommand EnviarQueixaCommand { get; set; }

        public Queixa Queixa { get; set; }

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

        public QueixaAkiViewModel(MediaFile mediaFile)
        {
            EnviarQueixaCommand = new Command(() =>
            {
                MessagingCenter.Send(Queixa, "EnivarQueixa");
            }, () => Queixa != null);

            setVideo(mediaFile);
        }

        private async void setVideo(MediaFile mediaFile)
        {
            var format = Path.GetExtension(mediaFile.Path);

            Queixa = new Queixa
            {
                IdUsuario = App.IdUsuario,
                NomeArquivo = Path.GetFileNameWithoutExtension(mediaFile.Path),
                Formato = format,
                Arquivo = new Arquivo
                {
                    ArquivoByte = File.ReadAllBytes(mediaFile.Path)
                }
            };

            VideoSource = VideoSource.FromStream(() =>
            {
                var stream = mediaFile.GetStream();
                mediaFile.Dispose();
                return stream;

            }, format);
        }
    }
}
