using System;

namespace QueixaAki.Models
{
    public class Queixa
    {
        public long Id { get; set; }
        public long IdUsuario { get; set; }
        public string NomeArquivo { get; set; }
        public Arquivo Arquivo { get; set; }
        public string Formato { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Endereco Endereco { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool Excluido { get; set; }
    }
}
