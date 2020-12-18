using System;
using System.IO;
using System.Windows.Input;
using Octane.Xamarin.Forms.VideoPlayer;
using Plugin.Media;
using Plugin.Media.Abstractions;
using QueixaAki.Helpers;
using QueixaAki.Models;
using QueixaAki.ViewModels.Base;
using Xamarin.Forms;
using MediaFile = Plugin.Media.Abstractions.MediaFile;

namespace QueixaAki.ViewModels
{
    public class QueixaAkiViewModel : BaseViewModel
    {
        public ICommand EscolherVideoCommand { get; set; }
        public ICommand GravarVideoCommand { get; set; }
        public ICommand EnviarQueixaCommand { get; set; }

        public Queixa Queixa { get; set; }
        //public Stream FileStream { get; set; }

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

        private bool _videoPlayerVisible;
        public bool VideoPlayerVisible
        {
            get => _videoPlayerVisible;
            set
            {
                _videoPlayerVisible = value;
                OnPropertyChanged();
            }
        }

        public QueixaAkiViewModel()
        {
            EscolherVideoCommand = new Command(EscolherVideo);
            GravarVideoCommand = new Command(GravarVideo);
            EnviarQueixaCommand = new Command(() =>
            {
                MessagingCenter.Send(Queixa, "EnivarQueixa");
            });

            Queixa = new Queixa
            {
                IdUsuario = long.Parse(Settings.IdUsuario),
                Arquivo = new Arquivo()
            };
        }

        private async void EscolherVideo()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickVideoSupported)
            {
                MessagingCenter.Send(new Message
                {
                    Title = "Ops",
                    MessageText = "Galeria de videos não suportada!"
                }, "Message");
                return;
            }

            var file = await CrossMedia.Current.PickVideoAsync();

            setVideo(file);
        }

        private async void GravarVideo()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsTakeVideoSupported || !CrossMedia.Current.IsCameraAvailable)
            {
                MessagingCenter.Send(new Message
                {
                    Title = "Ops",
                    MessageText = "Nenhuma câmera detectada!"
                }, "Message");
                return;
            }

            var file = await CrossMedia.Current.TakeVideoAsync(
                new StoreVideoOptions
                {
                    Directory = "Queixas",
                    DesiredLength = new TimeSpan(0, 1, 0),
                    CompressionQuality = 0,
                    Quality = VideoQuality.Low,
                    SaveToAlbum = true,
                    DefaultCamera = CameraDevice.Rear
                });

            setVideo(file);
        }

        private void setVideo(MediaFile file)
        {
            if (file == null)
                return;

            VideoPlayerVisible = true;

            var format = Path.GetExtension(file.Path);

            Queixa.NomeArquivo = Path.GetFileNameWithoutExtension(file.Path);
            Queixa.Arquivo.ArquivoByte = File.ReadAllBytes(file.Path);
            Queixa.Formato = format;

            VideoSource = VideoSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;

            }, format);
        }
    }
}
