using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using QueixaAki.Models;
using QueixaAki.Services;
using QueixaAki.ViewModels.Base;
using Xamarin.Forms;

namespace QueixaAki.ViewModels
{
    public class QueixasViewModel : BaseViewModel
    {
        private QueixaService _queixaService;

        private ObservableCollection<Queixa> _queixas;
        public ObservableCollection<Queixa> Queixas
        {
            get => _queixas;
            set
            {
                _queixas = value;
                OnPropertyChanged();
            }
        }

        private Queixa _queixaSelected;
        public Queixa QueixaSelected
        {
            get => _queixaSelected;
            set
            {
                if (value == null) return;
                QueixaSelected = value;
            }
        }

        public ICommand AtualizarCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await GetQueixas();
                });
            }
        }

        public QueixasViewModel()
        {
            Queixas = new ObservableCollection<Queixa>();
            _queixaService = new QueixaService();

            GetQueixas();
        }

        public async Task GetQueixas()
        {
            try
            {
                Carregando = true;

                var (queixas, erro) = await _queixaService.BuscarQueixasIdUsuario(App.IdUsuario);
                if (string.IsNullOrEmpty(erro))
                {
                    Queixas = new ObservableCollection<Queixa>(queixas);
                    //QueixaSelected = Queixas.FirstOrDefault();
                }
                else
                {
                    MessagingCenter.Send(new Message
                    {
                        Title = "Erro ao Buscar Queixas do Usuário",
                        MessageText = erro
                    }, "Message");
                }
            }
            catch (Exception e)
            {
                MessagingCenter.Send(new Message
                {
                    Title = "Erro ao Carregar Queixas",
                    MessageText = e.Message
                }, "Message");
            }
            finally
            {
                Carregando = false;
            }
        }
    }
}
