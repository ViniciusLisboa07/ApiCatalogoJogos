using ApiCatalogoJogos.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Repositories
{
    public class JogoSqlServerRepository :IJogoRepository
    {
        private readonly SqlConnection sqlConnection;

        public JogoSqlServerRepository(IConfiguration congiguration)
        {
            sqlConnection = new SqlConnection(congiguration.GetConnectionString("Default"));

        }

        public async Task Atualizar(Guid id, Jogo jogo)
        {
            var comando = $"update Jogos set Nome = '{jogo.Nome}', Produtora = '{jogo.Produtora}', Preco = {jogo.Preco.ToString().Replace(",", ".")} where id = '{jogo.id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }


        public async Task Inserir(Jogo jogo)
        {
            var comando = $"insert Jogos (id, Nome, Produtora, Preco) values ('{jogo.id}', '{jogo.Nome}', '{jogo.Produtora}', '{jogo.Preco}')";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public async Task<List<Jogo>> Obter(int pagina, int quantidade)
        {
            var jogos = new List<Jogo>();

            var comando = $"select * from Jogos order by id offset {((pagina - 1) * quantidade)} rows fetch next {quantidade} rows only";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                jogos.Add(new Jogo
                {
                    id = (Guid)sqlDataReader["id"],
                    Nome = (string)sqlDataReader["Nome"],
                    Produtora = (string)sqlDataReader["Produtora"],
                    Preco = (double)sqlDataReader["Preco"]
                }); ;
            }

            await sqlConnection.CloseAsync();

            return jogos;
        }

        public async Task<Jogo> Obter(Guid id)
        {
            Jogo jogo = null;

            var comando = $"select * from Jogos where id = '{id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while(sqlDataReader.Read())
            {
               jogo = new Jogo {
                    id = (Guid)sqlDataReader["id"],
                    Nome = (string)sqlDataReader["Nome"],
                    Produtora = (string)sqlDataReader["Produtora"],
                    Preco = (double)sqlDataReader["Preco"]
                };
            }

            await sqlConnection.CloseAsync();

            return jogo;
        }

        public async Task<List<Jogo>> Obter(string nome, string produtora)
        {
            var jogos = new List<Jogo>();

            var comando = $"select * from Jogos where Nome = '{nome}' and Produtora = '{produtora}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                jogos.Add(new Jogo
                {
                    id = (Guid)sqlDataReader["id"],
                    Nome = (string)sqlDataReader["Nome"],
                    Produtora = (string)sqlDataReader["Produtora"],
                    Preco = (double)sqlDataReader["Preco"]
                }); ;
            }

            await sqlConnection.CloseAsync();

            return jogos;
        }

        public async Task Remover(Guid id)
        {
            var comando = $"delete from Jogos where id = '{id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public void Dispose()
        {
            sqlConnection?.Close();
            sqlConnection?.Dispose();
        }
    }
}
