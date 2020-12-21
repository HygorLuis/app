using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using QueixaAki.Models;

namespace QueixaAki.Services
{
    public class QueixaService
    {
        public async Task<Tuple<bool, string>> Incluir(Queixa queixa)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using (var connection = new SqlConnection(App.ConnectionBanco))
                    {
                        connection.Open();
                        using (var sqlCommand = connection.CreateCommand())
                        {
                            sqlCommand.Transaction = connection.BeginTransaction();
                            sqlCommand.CommandText = "INSERT INTO Queixa (IdUsuario,NomeArquivo,Formato,Latitude,Longitude,Cep,Rua,Numero,Complemento,Bairro,Cidade,Estado,DataCriacao,Excluido) " +
                                    "VALUES (@IdUsuario,@NomeArquivo,@Formato,@Latitude,@Longitude,@Cep,@Rua,@Numero,@Complemento,@Bairro,@Cidade,@Estado,@DataCriacao,@Excluido);SELECT SCOPE_IDENTITY();";

                            sqlCommand.Parameters.AddWithValue("@IdUsuario", queixa.IdUsuario);
                            sqlCommand.Parameters.AddWithValue("@NomeArquivo", queixa.NomeArquivo);
                            sqlCommand.Parameters.AddWithValue("@Formato", queixa.Formato);
                            sqlCommand.Parameters.AddWithValue("@Latitude", queixa.Latitude);
                            sqlCommand.Parameters.AddWithValue("@Longitude", queixa.Longitude);
                            sqlCommand.Parameters.AddWithValue("@Cep", string.IsNullOrEmpty(queixa.Endereco.Cep) ? (object)DBNull.Value : queixa.Endereco.Cep);
                            sqlCommand.Parameters.AddWithValue("@Rua", string.IsNullOrEmpty(queixa.Endereco.Rua) ? (object)DBNull.Value : queixa.Endereco.Rua);
                            sqlCommand.Parameters.AddWithValue("@Numero", string.IsNullOrEmpty(queixa.Endereco.Numero) ? (object)DBNull.Value : queixa.Endereco.Numero);
                            sqlCommand.Parameters.AddWithValue("@Complemento", string.IsNullOrEmpty(queixa.Endereco.Complemento) ? (object)DBNull.Value : queixa.Endereco.Complemento);
                            sqlCommand.Parameters.AddWithValue("@Bairro", string.IsNullOrEmpty(queixa.Endereco.Bairro) ? (object)DBNull.Value : queixa.Endereco.Bairro);
                            sqlCommand.Parameters.AddWithValue("@Cidade", string.IsNullOrEmpty(queixa.Endereco.Cidade) ? (object)DBNull.Value : queixa.Endereco.Cidade);
                            sqlCommand.Parameters.AddWithValue("@Estado", string.IsNullOrEmpty(queixa.Endereco.Estado) ? (object)DBNull.Value : queixa.Endereco.Estado);
                            sqlCommand.Parameters.AddWithValue("@DataCriacao", $"{queixa.DataCriacao:yyyy/MM/dd HH:mm:ss}");
                            sqlCommand.Parameters.AddWithValue("@Excluido", queixa.Excluido);
                            queixa.Id = long.Parse(sqlCommand.ExecuteScalar().ToString());

                            sqlCommand.CommandText = "INSERT INTO Arquivo (IdQueixa,Arquivo) " +
                                    "VALUES (@IdQueixa,@Arquivo);";

                            sqlCommand.Parameters.AddWithValue("@IdQueixa", queixa.Id);
                            sqlCommand.Parameters.AddWithValue("@Arquivo", queixa.Arquivo.ArquivoByte);

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

        public async Task<Tuple<ObservableCollection<Queixa>, string>> BuscarQueixasIdUsuario(long idUsuario)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var queixas = new DataTable();

                    using (var connection = new SqlConnection(App.ConnectionBanco))
                    {
                        connection.Open();
                        using (var sqlCommand = connection.CreateCommand())
                        {
                            var condicao = idUsuario == 1 ? "" : $"AND IdUsuario = {idUsuario}";
                            sqlCommand.CommandText = $"SELECT * FROM Queixa WHERE Excluido = 0 {condicao} ORDER BY DataCriacao DESC;";
                            using (var dataAdapter = new SqlDataAdapter())
                            {
                                dataAdapter.SelectCommand = sqlCommand;
                                dataAdapter.Fill(queixas);
                            }
                        }
                        connection.Close();
                    }

                    return new Tuple<ObservableCollection<Queixa>, string>(new ObservableCollection<Queixa>(queixas.AsEnumerable().ToList().Select(x => new Queixa
                    {
                        Id = x.Field<long>("Id"),
                        NomeArquivo = x.Field<string>("NomeArquivo"),
                        Formato = x.Field<string>("Formato"),
                        Latitude = x.Field<string>("Latitude"),
                        Longitude = x.Field<string>("Longitude"),
                        DataCriacao = x.Field<DateTime>("DataCriacao"),
                        Excluido = x.Field<bool>("Excluido"),
                        Endereco = new Endereco
                        {
                            Cep = x.Field<string>("Cep"),
                            Rua = x.Field<string>("Rua"),
                            Numero = x.Field<string>("Numero"),
                            Complemento = x.Field<string>("Complemento"),
                            Bairro = x.Field<string>("Bairro"),
                            Cidade = x.Field<string>("Cidade"),
                            Estado = x.Field<string>("Estado")
                        }

                    }).ToList()), "");
                }
                catch (Exception ex)
                {
                    return new Tuple<ObservableCollection<Queixa>, string>(new ObservableCollection<Queixa>(), ex.Message);
                }
            });
        }

        public async Task<Tuple<Arquivo, string>> BuscarArquivoIdQueixa(long id)
        {
            var tentativas = 3;
            var erro = "";

            return await Task.Run(() =>
            {
                while (tentativas > 0)
                {
                    if (tentativas < 3)
                        Thread.Sleep(5000);

                    try
                    {
                        var arquivo = new Arquivo();

                        using (var connection = new SqlConnection(App.ConnectionBanco))
                        {
                            connection.Open();
                            using (var sqlCommand = connection.CreateCommand())
                            {
                                sqlCommand.CommandTimeout = 0;
                                sqlCommand.CommandText = $"SELECT * FROM Arquivo WHERE IdQueixa = {id};";
                                var dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);

                                if (!dr.HasRows) return null;
                                dr.Read();

                                arquivo.Id = long.Parse(dr["Id"].ToString());
                                arquivo.IdQueixa = long.Parse(dr["IdQueixa"].ToString());
                                arquivo.ArquivoByte = (byte[])dr["Arquivo"];

                                dr.Close();
                            }
                            connection.Close();
                        }

                        return new Tuple<Arquivo, string>(arquivo, "");
                    }
                    catch (Exception ex)
                    {
                        tentativas--;
                        if (!string.IsNullOrEmpty(ex.Message))
                            erro = ex.Message;
                        //return new Tuple<Arquivo, string>(null, ex.Message);
                    }
                }
                return new Tuple<Arquivo, string>(null, erro);
            });
        }

        public async Task<Tuple<bool, string>> ExcluirArquivo(long id)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using (var connection = new SqlConnection(App.ConnectionBanco))
                    {
                        connection.Open();
                        using (var sqlCommand = connection.CreateCommand())
                        {
                            sqlCommand.Transaction = connection.BeginTransaction();
                            sqlCommand.CommandText = $"UPDATE Queixa SET Excluido = 1 WHERE Id = {id};";
                            sqlCommand.ExecuteNonQuery();

                            sqlCommand.CommandText = $"DELETE FROM Arquivo WHERE IdQueixa = {id};";

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
    }
}
