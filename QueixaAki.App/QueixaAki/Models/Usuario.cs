using System;
using QueixaAki.ViewModels.Base;

namespace QueixaAki.Models
{
    public class Usuario : Base
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
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

        private Endereco _endereco;
        public Endereco Endereco
        {
            get => _endereco;
            set
            {
                _endereco = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public Conexao Conexao { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool Excluido { get; set; }
    }
}
