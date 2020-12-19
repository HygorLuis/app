﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
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
                _queixaSelected = value;
                if (value == null) return;
                WorkOnQueixaSelected(value);
            }
        }

        private async void WorkOnQueixaSelected(Queixa value)
        {
            Queixas.FirstOrDefault(x => x.Id == value.Id).Download = true;

            await BaixarArquivo(value.Id);

            Queixas.FirstOrDefault(x => x.Id == value.Id).Download = false;
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

        public async Task BaixarArquivo(long idArquivo)
        {
            await Task.Run(() =>
            {
                Thread.Sleep(10000);
            });
        }

        public async Task GetQueixas()
        {
            try
            {
                Carregando = true;

                var (queixas, erro) = await _queixaService.BuscarQueixasIdUsuario(App.IdUsuario);
                if (string.IsNullOrEmpty(erro))
                {
                    Queixas = queixas;
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