using System;

namespace QueixaAki.Models
{
    public class Arquivo
    {
        public long Id { get; set; }
        public long IdQueixa { get; set; }
        public byte[] ArquivoByte { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool Excluido { get; set; }
    }
}
