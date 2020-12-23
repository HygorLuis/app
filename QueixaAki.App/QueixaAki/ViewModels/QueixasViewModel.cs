using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Permissions.Abstractions;
using QueixaAki.Helpers;
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

        private bool _listViewCarregando = false;
        public bool ListViewCarregando
        {
            get => _listViewCarregando;
            set
            {
                _listViewCarregando = value;
                OnPropertyChanged();
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
                ListViewCarregando = true;

                if (!await VerificarAcessoInternet())
                {
                    MessagingCenter.Send(new Message
                    {
                        Title = "Erro",
                        MessageText = "Sem acesso à internet!"
                    }, "Message");
                    return;
                }

                var (queixas, erro) = await _queixaService.BuscarQueixasIdUsuario(App.IdUsuario);
                if (string.IsNullOrEmpty(erro))
                {
                    foreach (var queixa in queixas)
                    {
                        var exist = await FileHelper.FileExists(queixa.NomeArquivoCompleto);
                        if (exist == null)
                        {
                            queixa.DownloadVisible = true;
                        }
                        else
                        {
                            queixa.Arquivo = new Arquivo
                            {
                                ArquivoByte = File.ReadAllBytes(exist),
                                Path = exist
                            };
                        }
                    }
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
                ListViewCarregando = false;
            }
        }

        public async void BaixarArquivo(Queixa queixa)
        {
            try
            {
                if (App.PermissaoMidia != PermissionStatus.Granted)
                {
                    var permiss = App.PermissaoMidia != PermissionStatus.Granted ? "Mídia" : "";

                    MessagingCenter.Send(new Message
                    {
                        Title = "Permissões Necessárias",
                        MessageText = $"Favor dar permissão as seguintes solicitações nas configurações do seu telefone: {permiss}"
                    }, "Message");
                    return;
                }

                Queixas.FirstOrDefault(x => x.Id == queixa.Id).Download = true;

                var (arquivo, erro) = await _queixaService.BuscarArquivoIdQueixa(queixa.Id);

                if (arquivo == null || !string.IsNullOrEmpty(erro))
                {
                    MessagingCenter.Send(new Message
                    {
                        Title = "Erro ao Buscar Arquivo da Queixa",
                        MessageText = "Favor tentar novamente mais tarde!"
                    }, "Message");

                    return;
                }

                var (path, erroCreate) = await FileHelper.CreateFile(arquivo.ArquivoByte, queixa.NomeArquivoCompleto);

                if (!string.IsNullOrEmpty(erroCreate))
                {
                    MessagingCenter.Send(new Message
                    {
                        Title = "Erro ao Criar Arquivo",
                        MessageText = erroCreate
                    }, "Message");

                    return;
                }

                Queixas.FirstOrDefault(x => x.Id == queixa.Id).Arquivo = arquivo;
                Queixas.FirstOrDefault(x => x.Id == queixa.Id).Arquivo.Path = path;
            }
            catch (Exception ex)
            {
                MessagingCenter.Send(new Message
                {
                    Title = "Erro ao Baixar Arquivo",
                    MessageText = ex.Message
                }, "Message");
            }
            finally
            {
                Queixas.FirstOrDefault(x => x.Id == queixa.Id).Download = false;
                if (Queixas.FirstOrDefault(x => x.Id == queixa.Id).Arquivo != null) 
                    Queixas.FirstOrDefault(x => x.Id == queixa.Id).DownloadVisible = false;
            }
        }

        public async Task ExcluirArquivo(long id)
        {
            try
            {
                Carregando = true;

                var (concluido, erro) = await _queixaService.ExcluirArquivo(id);
                
                if (concluido)
                    MessagingCenter.Send(new Message
                    {
                        Title = "Sucesso",
                        MessageText = "Arquivo excluido com sucesso!"
                    }, "Message");
                else
                {
                    MessagingCenter.Send(new Message
                    {
                        Title = "Erro ao Excluir Arquivo",
                        MessageText = erro
                    }, "Message");
                }

            }
            catch (Exception e)
            {
                // ignored
            }
            finally
            {
                Carregando = false;
                await GetQueixas();
            }
        }
    }
}
