using System;
using System.Collections.Generic;
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
        public async Task<Tuple<Conexao, string>> BuscarConexao(List<string> cidade, List<string> estado)
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

                    return new Tuple<Conexao, string>(conexoesList.FirstOrDefault(x => cidade.All(c => c.ApenasLetras().ToUpper() == x.Cidade.ApenasLetras().ToUpper())
                                                                                       && estado.All(e => e.ApenasLetras().ToUpper() == x.Estado.ApenasLetras().ToUpper())), "");
                }
                catch (Exception ex)
                {
                    return new Tuple<Conexao, string>(new Conexao(), ex.Message);
                }
            });
        }

        public Tuple<Conexao, string> BuscarConexaoId(long id)
        {
            try
            {
                var conexao = new Conexao();

                using (var connection = new SqlConnection(App.ConnectionQueixaAki))
                {
                    connection.Open();
                    using (var sqlCommand = connection.CreateCommand())
                    {
                        sqlCommand.CommandText = $"SELECT * FROM Conexao WHERE Excluido = 0 AND Id = {id};";
                        var dr = sqlCommand.ExecuteReader();

                        if (!dr.HasRows) return null;
                        dr.Read();

                        conexao.Id = long.Parse(dr["Id"].ToString());
                        conexao.Cidade = dr["Cidade"].ToString();
                        conexao.Estado = dr["Estado"].ToString();
                        conexao.Servidor = dr["Servidor"].ToString();
                        conexao.Banco = dr["Banco"].ToString();
                        conexao.Usuario = dr["Usuario"].ToString();
                        conexao.Senha = dr["Senha"].ToString();
                        conexao.DataCriacao = DateTime.Parse(dr["DataCriacao"].ToString());
                        conexao.Excluido = bool.Parse(dr["Excluido"].ToString());

                        dr.Close();
                    }
                    connection.Close();
                }

                return new Tuple<Conexao, string>(conexao, "");
            }
            catch (Exception ex)
            {
                return new Tuple<Conexao, string>(new Conexao(), ex.Message);
            }
        }
    }
}
