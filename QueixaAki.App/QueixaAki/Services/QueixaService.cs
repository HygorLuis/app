using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
                            sqlCommand.CommandText = $"SELECT * FROM Queixa WHERE Excluido = 0 AND IdUsuario = {idUsuario} ORDER BY DataCriacao DESC;";
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
    }
}
