using System;
using System.Text.RegularExpressions;
using QueixaAki.ViewModels.Base;

namespace QueixaAki.Models
{
    public class Usuario : BaseViewModel
    {
        public long Id { get; set; }

        private string _nome;
        public string Nome
        {
            get => _nome?.ToUpper();
            set => _nome = value?.ToUpper();
        }

        private string _sobrenome;
        public string Sobrenome
        {
            get => _sobrenome?.ToUpper();
            set => _sobrenome = value?.ToUpper();
        }

        public string RG { get; set; }
        public string CPF { get; set; }

        private DateTime _dataNascimento;

        public DateTime DataNascimento
        {
            get => _dataNascimento;
            set
            {
                _dataNascimento = value;
                OnPropertyChanged();
            }
        }

        #region CONTATO

        #region EMAIL

        public string Email { get; set; }
        public string Senha { get; set; }

        #endregion

        #region TELEFONES

        public string Telefone1 { get; set; }
        public string Telefone2 { get; set; }

        #endregion

        #endregion

        #region ENDEREÇO

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
                DB = $"{value}_";
                OnPropertyChanged();
            }
        }

        private string _estado;
        public string Estado
        {
            get => _estado?.ToUpper().Trim();
            set
            {
                _estado = value;
                DB += value;
                OnPropertyChanged();
            }
        }

        #endregion

        public DateTime DataCriacao { get; set; }

        public bool Excluido { get; set; }

        private string _db;
        public string DB
        {
            get => _db?.ToUpper();
            set => _db = value;
        }
    }
}
