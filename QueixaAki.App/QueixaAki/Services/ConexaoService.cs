using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using QueixaAki.Helpers;
using QueixaAki.Models;

namespace QueixaAki.Services
{
    public class ConexaoService
    {
        public async Task<Tuple<Conexao, string>> BuscarConexao(string cidade, string estado)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var conexoes = new DataTable();

                    using (var connection = new SqlConnection(App.ConnectionQueixaAki))
                    {
                        connection.Open();
                        using (var sqlCommand = connection.CreateCommand())
                        {
                            sqlCommand.CommandText = "SELECT * FROM Conexao WHERE Excluido = 0;";
                            using (var dataAdapter = new SqlDataAdapter())
                            {
                                dataAdapter.SelectCommand = sqlCommand;
                                dataAdapter.Fill(conexoes);
                            }
                        }
                        connection.Close();
                    }

                    var conexoesList = conexoes.AsEnumerable().ToList().Select(x => new Conexao
                    {
                        Id = x.Field<long>("Id"),
                        Cidade = x.Field<string>("Cidade"),
                        Estado = x.Field<string>("Estado"),
                        Servidor = x.Field<string>("Servidor"),
                        Banco = x.Field<string>("Banco"),
                        Usuario = x.Field<string>("Usuario"),
                        Senha = x.Field<string>("Senha"),
                        DataCriacao = x.Field<DateTime>("DataCriacao"),
                        Excluido = x.Field<bool>("Excluido")
                    });

                    return new Tuple<Conexao, string>(conexoesList.FirstOrDefault(x => x.Cidade.ApenasLetras().ToUpper() == cidade.ApenasLetras().ToUpper()
                                                                                       && x.Estado.ApenasLetras().ToUpper() == estado.ApenasLetras().ToUpper()), "");
                }
                catch (Exception ex)
                {
                    return new Tuple<Conexao, string>(new Conexao(), ex.Message);
                }
            });
        }
    }
}
