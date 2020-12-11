using System;

namespace QueixaAki.Models
{
    public class Conexao
    {
        public long Id { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Servidor { get; set; }
        public string Banco { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool Excluido { get; set; }
    }
}
