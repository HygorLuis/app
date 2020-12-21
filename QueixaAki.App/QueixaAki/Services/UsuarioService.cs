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
        public async Task<Tuple<bool, string>> Incluir(Usuario usuario)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using (var connection = new SqlConnection(App.ConnectionQueixaAki))
                    {
                        connection.Open();
                        using (var sqlCommand = connection.CreateCommand())
                        {
                            sqlCommand.Transaction = connection.BeginTransaction();
                            sqlCommand.CommandText = "INSERT INTO UsuarioApp (Nome,Sobrenome,RG,CPF,DataNascimento,EMail,Senha,Telefone1,Telefone2,Cep,Rua,Numero,Complemento,Bairro,Cidade,Estado,IdConexao,DataCriacao,Excluido) " +
                                    "VALUES (@Nome,@Sobrenome,@RG,@CPF,@DataNascimento,@EMail,@Senha,@Telefone1,@Telefone2,@Cep,@Rua,@Numero,@Complemento,@Bairro,@Cidade,@Estado,@IdConexao,@DataCriacao,@Excluido);";

                            sqlCommand.Parameters.AddWithValue("@Nome", usuario.Nome);
                            sqlCommand.Parameters.AddWithValue("@Sobrenome", usuario.Sobrenome);
                            sqlCommand.Parameters.AddWithValue("@RG", string.IsNullOrEmpty(usuario.RG) ? (object)DBNull.Value : usuario.RG);
                            sqlCommand.Parameters.AddWithValue("@CPF", usuario.CPF);
                            sqlCommand.Parameters.AddWithValue("@DataNascimento", $"{usuario.DataNascimento:yyyy/MM/dd}");
                            sqlCommand.Parameters.AddWithValue("@EMail", usuario.Email);
                            sqlCommand.Parameters.AddWithValue("@Senha", usuario.Senha);
                            sqlCommand.Parameters.AddWithValue("@Telefone1", string.IsNullOrEmpty(usuario.Telefone1) ? (object)DBNull.Value : usuario.Telefone1);
                            sqlCommand.Parameters.AddWithValue("@Telefone2", string.IsNullOrEmpty(usuario.Telefone2) ? (object)DBNull.Value : usuario.Telefone2);
                            sqlCommand.Parameters.AddWithValue("@Cep", usuario.Endereco.Cep);
                            sqlCommand.Parameters.AddWithValue("@Rua", usuario.Endereco.Rua);
                            sqlCommand.Parameters.AddWithValue("@Numero", usuario.Endereco.Numero);
                            sqlCommand.Parameters.AddWithValue("@Complemento", string.IsNullOrEmpty(usuario.Endereco.Complemento) ? (object)DBNull.Value : usuario.Endereco.Complemento);
                            sqlCommand.Parameters.AddWithValue("@Bairro", usuario.Endereco.Bairro);
                            sqlCommand.Parameters.AddWithValue("@Cidade", usuario.Endereco.Cidade);
                            sqlCommand.Parameters.AddWithValue("@Estado", usuario.Endereco.Estado);
                            sqlCommand.Parameters.AddWithValue("@IdConexao", usuario.Conexao.Id);
                            sqlCommand.Parameters.AddWithValue("@DataCriacao", $"{usuario.DataCriacao:yyyy/MM/dd HH:mm:ss}");
                            sqlCommand.Parameters.AddWithValue("@Excluido", usuario.Excluido);

                            sqlCommand.ExecuteNonQuery();
                            sqlCommand.Transaction.Commit();
                        }

                        connection.Close();
                    }

                    return new Tuple<bool, string>(true, "");
                }
                catch (Exception ex)
                {
                    return new Tuple<bool, string>(false, ex.Message);
                }
            });
        }

        public async Task<Tuple<List<Usuario>, string>> BuscarTodos()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var usuarios = new DataTable();

                    using (var connection = new SqlConnection(App.ConnectionQueixaAki))
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

                    return new Tuple<List<Usuario>, string>(usuarios.AsEnumerable().ToList().Select(x => new Usuario
                    {
                        Id = x.Field<long>("Id"),
                        RG = x.Field<string>("RG"),
                        CPF = x.Field<string>("CPF"),
                        Email = x.Field<string>("EMail"),
                    }).ToList(), "");
                }
                catch (Exception ex)
                {
                    return new Tuple<List<Usuario>, string>(new List<Usuario>(), ex.Message);
                }
            });
        }

        public async Task<Tuple<Usuario, string>> BuscarUsuario(string email, string senha)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var usuarios = new DataTable();

                    using (var connection = new SqlConnection(App.ConnectionQueixaAki))
                    {
                        connection.Open();
                        using (var sqlCommand = connection.CreateCommand())
                        {
                            sqlCommand.CommandText = "SELECT u.Id AS IdUsuario, u.EMail, u.Senha, c.* FROM UsuarioApp u INNER JOIN Conexao c ON u.IdConexao = c.Id WHERE u.Excluido = 0 AND c.Excluido = 0;";
                            using (var dataAdapter = new SqlDataAdapter())
                            {
                                dataAdapter.SelectCommand = sqlCommand;
                                dataAdapter.Fill(usuarios);
                            }
                        }
                        connection.Close();
                    }

                    return new Tuple<Usuario, string>(usuarios.AsEnumerable().ToList()
                        .Where(x => x.Field<string>("Email") == email && x.Field<string>("Senha") == senha).Select(x =>
                            new Usuario
                            {
                                Id = x.Field<long>("IdUsuario"),
                                Email = x.Field<string>("Email"),
                                Senha = x.Field<string>("Senha"),
                                Conexao = new Conexao
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
                                }
                            }).FirstOrDefault(), "");
                }
                catch (Exception ex)
                {
                    return new Tuple<Usuario, string>(new Usuario(), ex.Message);
                }
            });
        }

        public async Task<Usuario> BuscarUsuarioId(long id)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var usuario = new Usuario();

                    using (var connection = new SqlConnection(App.ConnectionQueixaAki))
                    {
                        connection.Open();
                        using (var sqlCommand = connection.CreateCommand())
                        {
                            sqlCommand.CommandText = $"SELECT * FROM UsuarioApp WHERE Id = {id};";
                            var dr = sqlCommand.ExecuteReader();

                            if (!dr.HasRows) return null;
                            dr.Read();

                            usuario.Id = long.Parse(dr["Id"].ToString());
                            usuario.Email = dr["EMail"].ToString();
                            usuario.Senha = dr["Senha"].ToString();
                            usuario.Telefone1 = dr["Telefone1"].ToString();
                            usuario.Telefone2 = dr["Telefone2"].ToString();
                            usuario.Endereco = new Endereco
                            {
                                Cep = dr["Cep"].ToString(),
                                Rua = dr["Rua"].ToString(),
                                Numero = dr["Numero"].ToString(),
                                Complemento = dr["Complemento"].ToString(),
                                Bairro = dr["Bairro"].ToString(),
                                Cidade = dr["Cidade"].ToString(),
                                Estado = dr["Estado"].ToString()
                            };

                            dr.Close();
                        }
                        connection.Close();
                    }

                    return usuario;
                }
                catch (Exception ex)
                {
                    return null;
                }
            });
        }
    }
}
