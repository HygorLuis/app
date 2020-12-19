using System;
using QueixaAki.ViewModels.Base;

namespace QueixaAki.Models
{
    public class Queixa : BaseViewModel
    {
        public long Id { get; set; }
        public long IdUsuario { get; set; }
        public string NomeArquivo { get; set; }
        public string Formato { get; set; }
        public string Tamanho { get; set; }
        public Arquivo Arquivo { get; set; }
        public string Path { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Endereco Endereco { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool Excluido { get; set; }

        private bool _download;
        public bool Download 
        { 
            get => _download;
            set
            {
                _download = value;
                OnPropertyChanged();
            }
        }
        public string NomeArquivoCompleto => $"{NomeArquivo}{Formato}";
    }
}
