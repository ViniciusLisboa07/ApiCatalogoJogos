using ApiCatalogoJogos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Repositories
{
    public class JogoRepository : IJogoRepository
    {
        private static Dictionary<Guid, Jogo> jogos = new Dictionary<Guid, Jogo>()
        {
            {Guid.Parse("698dc19d489c4e4db73e28a713eab07b"), new Jogo{ id = Guid.Parse("698dc19d489c4e4db73e28a713eab07b"), Nome = "Fifa 21", Produtora = "EA", Preco = 200 } },
            {Guid.Parse("74b87337454200d4d33f80c4663dc5e5"), new Jogo{ id = Guid.Parse("74b87337454200d4d33f80c4663dc5e5"), Nome = "Fifa 20", Produtora = "EA", Preco = 90 } },
            {Guid.Parse("b26986ceee60f744534aaab928cc12df"), new Jogo{ id = Guid.Parse("b26986ceee60f744534aaab928cc12df"), Nome = "Fifa 19", Produtora = "EA", Preco = 100 } },
        };

        public Task Atualizar(Guid id, Jogo jogo)
        {
            jogos[jogo.id] = jogo;
            return Task.CompletedTask;
        }

        public Task Inserir(Jogo jogo)
        {
            jogos.Add(jogo.id, jogo);
            return Task.CompletedTask;
        }

        public Task<List<Jogo>> Obter(int pagina, int quantidade)
        {
            return Task.FromResult(jogos.Values.Skip((pagina - 1) * quantidade).Take(quantidade).ToList());
        }

        public Task<Jogo> Obter(Guid id)
        {
            if (!jogos.ContainsKey(id))
                return null;

            return Task.FromResult(jogos[id]);
        }

        public Task<List<Jogo>> Obter(string nome, string produtora)
        {
            return Task.FromResult(jogos.Values.Where(jogo => jogo.Nome.Equals(nome) && jogo.Produtora.Equals(produtora)).ToList());
        }

        public Task Remover(Guid id)
        {
            jogos.Remove(id);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
          // throw new NotImplementedException();
        }
    }
}
