using System;
using System.IO;
using System.Net.Mail;
using System.Threading;
using System.Windows.Input;
using Octane.Xamarin.Forms.VideoPlayer;
using Plugin.Media;
using Plugin.Media.Abstractions;
using QueixaAki.Models;
using QueixaAki.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace QueixaAki.ViewModels
{
    public class QueixaAkiViewModel : BaseViewModel
    {
        public ICommand EscolherVideoCommand { get; set; }
        public ICommand GravarVideoCommand { get; set; }
        public ICommand EnviarQueixaCommand { get; set; }

        private Stream _fileStream;

        private VideoSource _videoSource;
        public VideoSource VideoSource
        {
            get => _videoSource;
            set
            {
                _videoSource = value;
                VideoPlayerVisible = value != null;
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
            EnviarQueixaCommand = new Command(EnivarQueixa);
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

            if (file == null)
                return;

            _fileStream = file.GetStream();
            var format = Path.GetExtension(file.Path);

            VideoSource = VideoSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;

            }, format);
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
                    CompressionQuality = 50,
                    Quality = VideoQuality.Medium,
                    SaveToAlbum = true,
                    DefaultCamera = CameraDevice.Rear
                });

            if (file == null)
                return;

            _fileStream = file.GetStream();
            var format = Path.GetExtension(file.Path);

            VideoSource = VideoSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;

            }, format);
        }

        private void EnivarQueixa()
        {
            MessagingCenter.Send("", "EnivarQueixa");

            //var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
            //var cts = new CancellationTokenSource();
            //var locationActual = await Geolocation.GetLocationAsync(request, cts.Token);

            /*try
            {
                var host = "smtp-mail.outlook.com";
                var port = 587;
                var from = "hyg_or-guinho@live.com";
                var to = "hyg_or-guinho@live.com";

                var user = "hyg_or-guinho@live.com";
                var password = "3ky09p000";

                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                var cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                var mail = new MailMessage
                {
                    From = new MailAddress(from),
                    To = { to },
                    Subject = "TESTE DE ENVIO",
                    Body = $"TESTE\n{location}",
                    Attachments = { new Attachment(_fileStream, "attachment.mp4") }
                };

                var smtpServer = new SmtpClient
                {
                    Port = port,
                    Host = host,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(user, password)
                };

                smtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                MessagingCenter.Send(ex, "ErroEnviar");
            }*/
        }
    }
}
