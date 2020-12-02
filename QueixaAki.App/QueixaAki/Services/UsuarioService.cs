using System;
using System.Data.SqlClient;
using QueixaAki.Models;

namespace QueixaAki.Services
{
    public class UsuarioService
    {
        public bool Incluir(Usuario usuario)
        {
            try
            {
                using (var connection = new SqlConnection(App.ConnectionString))
                {
                    connection.Open();
                    using (var sqlCommand = connection.CreateCommand())
                    {
                        sqlCommand.Transaction = connection.BeginTransaction();
                        sqlCommand.CommandText = "INSERT INTO UsuarioApp (Nome,RG,CPF,DataNascimento,EMail,Senha,Telefone1,Telefone2,Cep,Rua,Numero,Complemento,Bairro,Cidade,Estado,DataCriacao,Excluido) " +
                            "VALUES (@Nome,@RG,@CPF,@DataNascimento,@EMail,@Senha,@Telefone1,@Telefone2,@Cep,@Rua,@Numero,@Complemento,@Bairro,@Cidade,@Estado,@DataCriacao,@Excluido);";

                        sqlCommand.Parameters.AddWithValue("@Nome", usuario.Nome);
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
        }
    }
}
