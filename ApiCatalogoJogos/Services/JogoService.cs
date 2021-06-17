using ApiCatalogoJogos.Entities;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.Repositories;
using ApiCatalogoJogos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Services
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _jogoRepository;

        public JogoService(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        
        public async Task<List<JogoViewModel>> Obter(int pagina, int quantidade)
        {
            var jogos = _jogoRepository.Obter(pagina, quantidade);

            return jogos.Select(jogo => new JogoViewModel
            {
                Id = jogo.id,
                Nome = jogo.Nome,
                Preco = jogo.Preco,
                Produtora = jogo.Produtora
            }).ToList();
        }

        public async Task<JogoViewModel> Obter(Guid id)
        {
            var jogo = await _jogoRepository.Obter(id);

            if (jogo == null)
                return null;

            return  new JogoViewModel
            {
                id = jogo.id,
                Nome = jogo.Nome,
                Preco = jogo.Preco,
                Produtora = jogo.Produtora
            };
        }

        public async Task<JogoViewModel> Inserir(JogoInputModel jogo)
        {
            var entidadeJogo = await _jogoRepository.Obter(jogo.Nome, jogo.Produtora);

            if (entidadeJogo.Count() > 0)
                throw new JogoJaCadastradoException();

            var jogoInsert = new Jogo
            {
                id = Guid.NewGuid(),
                Nome = jogo.Nome,
                Preco = jogo.Preco,
                Produtora = jogo.Produtora
            };

            await _jogoRepository.Inserir(jogoInsert);

            return new JogoViewModel
            {
                id = jogoInsert.id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };
            
        }

        public Task Atualizar(Guid id, JogoInputModel jogo)
        {
            throw new NotImplementedException();
        }

        public Task Atualizar(Guid id, double preco)
        {
            throw new NotImplementedException();
        }

        public Task Remover(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _jogoRepository?.Dispose();
        }
    }
}
