using System;
using System.Text.RegularExpressions;

namespace QueixaAki.Models
{
    public class Usuario
    {
        private string _nome;
        public string Nome
        {
            get => _nome?.ToUpper();
            set => _nome = value?.ToUpper();
        }

        public string RG { get; set; }
        public string CPF { get; set; }
        public DateTime? DataNascimento { get; set; }

        #region CONTATO

        #region EMAIL

        public string Email { get; set; }
        public string Senha { get; set; }

        #endregion

        #region TELEFONES

        public string DDD1 { get; set; }
        public string Telefone1 { get; set; }
        public string DDD2 { get; set; }
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

        private string _endereco;
        public string Endereco
        {
            get => _endereco?.ToUpper();
            set => _endereco = value;
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
            set => _bairro = value;
        }

        private string _cidade;
        public string Cidade
        {
            get => _cidade?.ToUpper();
            set => _cidade = value;
        }

        private string _estado;
        public string Estado
        {
            get => _estado?.ToUpper().Trim();
            set => _estado = value;
        }

        #endregion

        public DateTime DataCriacao { get; set; }

        public bool Excluido { get; set; }
    }
}
