using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using QueixaAki.Models;

namespace QueixaAki.Services
{
    public class UsuarioService
    {
        public async Task<bool> Incluir(Usuario usuario)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using (var connection = new SqlConnection(App.ConnectionString))
                    {
                        connection.Open();
                        using (var sqlCommand = connection.CreateCommand())
                        {
                            sqlCommand.Transaction = connection.BeginTransaction();
                            sqlCommand.CommandText = "INSERT INTO UsuarioApp (Nome,Sobrenome,RG,CPF,DataNascimento,EMail,Senha,Telefone1,Telefone2,Cep,Rua,Numero,Complemento,Bairro,Cidade,Estado,DataCriacao,Excluido) " +
                                    "VALUES (@Nome,@Sobrenome,@RG,@CPF,@DataNascimento,@EMail,@Senha,@Telefone1,@Telefone2,@Cep,@Rua,@Numero,@Complemento,@Bairro,@Cidade,@Estado,@DataCriacao,@Excluido);";

                            sqlCommand.Parameters.AddWithValue("@Nome", usuario.Nome);
                            sqlCommand.Parameters.AddWithValue("@Sobrenome", usuario.Sobrenome);
                            sqlCommand.Parameters.AddWithValue("@RG", string.IsNullOrEmpty(usuario.RG) ? (object)DBNull.Value : usuario.RG);
                            sqlCommand.Parameters.AddWithValue("@CPF", usuario.CPF);
                            sqlCommand.Parameters.AddWithValue("@DataNascimento", $"{usuario.DataNascimento:yyyy/MM/dd}");
                            sqlCommand.Parameters.AddWithValue("@EMail", usuario.Email);
                            sqlCommand.Parameters.AddWithValue("@Senha", usuario.Senha);
                            sqlCommand.Parameters.AddWithValue("@Telefone1", string.IsNullOrEmpty(usuario.Telefone1) ? (object)DBNull.Value : usuario.Telefone1);
                            sqlCommand.Parameters.AddWithValue("@Telefone2", string.IsNullOrEmpty(usuario.Telefone2) ? (object)DBNull.Value : usuario.Telefone2);
                            sqlCommand.Parameters.AddWithValue("@Cep", usuario.Cep);
                            sqlCommand.Parameters.AddWithValue("@Rua", usuario.Rua);
                            sqlCommand.Parameters.AddWithValue("@Numero", usuario.Numero);
                            sqlCommand.Parameters.AddWithValue("@Complemento", string.IsNullOrEmpty(usuario.Complemento) ? (object)DBNull.Value : usuario.Complemento);
                            sqlCommand.Parameters.AddWithValue("@Bairro", usuario.Bairro);
                            sqlCommand.Parameters.AddWithValue("@Cidade", usuario.Cidade);
                            sqlCommand.Parameters.AddWithValue("@Estado", usuario.Estado);
                            sqlCommand.Parameters.AddWithValue("@DataCriacao", $"{usuario.DataCriacao:yyyy/MM/dd HH:mm:ss}");
                            sqlCommand.Parameters.AddWithValue("@Excluido", usuario.Excluido);

                            sqlCommand.ExecuteNonQuery();
                            sqlCommand.Transaction.Commit();
                        }

                        connection.Close();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            });
        }

        public async Task<List<Usuario>> BuscarTodos()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var usuarios = new DataTable();

                    using (var connection = new SqlConnection(App.ConnectionString))
                    {
                        connection.Open();
                        using (var sqlCommand = connection.CreateCommand())
                        {
                            sqlCommand.CommandText = "SELECT Id, RG, CPF, EMail FROM UsuarioApp WHERE Excluido = 0;";
                            using (var dataAdapter = new SqlDataAdapter())
                            {
                                dataAdapter.SelectCommand = sqlCommand;
                                dataAdapter.Fill(usuarios);
                            }
                        }
                        connection.Close();
                    }

                    return usuarios.AsEnumerable().ToList().Select(x => new Usuario
                    {
                        Id = x.Field<long>("Id"),
                        RG = x.Field<string>("RG"),
                        CPF = x.Field<string>("CPF"),
                        Email = x.Field<string>("EMail"),
                    }).ToList();
                }
                catch (Exception ex)
                {
                    return new List<Usuario>();
                }
            });
        }
    }
}
