using System.Text.RegularExpressions;
using QueixaAki.ViewModels.Base;

namespace QueixaAki.Models
{
    public class Endereco : Base
    {
        private string _cep;
        public string Cep
        {
            get => _cep != null && Regex.IsMatch(_cep, "[0-9]+") ? _cep.Trim() : "";
            set => _cep = value;
        }

        private string _rua;
        public string Rua
        {
            get => _rua?.ToUpper();
            set
            {
                _rua = value;
                OnPropertyChanged();
            }
        }

        private string _numero;
        public string Numero
        {
            get => _numero?.ToUpper();
            set => _numero = value;
        }

        private string _complemento;
        public string Complemento
        {
            get => _complemento?.ToUpper();
            set => _complemento = value;
        }

        private string _bairro;
        public string Bairro
        {
            get => _bairro?.ToUpper();
            set
            {
                _bairro = value;
                OnPropertyChanged();
            }
        }

        private string _cidade;
        public string Cidade
        {
            get => _cidade?.ToUpper();
            set
            {
                _cidade = value;
                OnPropertyChanged();
            }
        }

        private string _estado;
        public string Estado
        {
            get => _estado?.ToUpper();
            set
            {
                _estado = value;
                OnPropertyChanged();
            }
        }

        public string EnderecoCompleto => $"{Rua}, {Numero} - {Bairro}, {Cidade} - {Estado}";
    }
}
