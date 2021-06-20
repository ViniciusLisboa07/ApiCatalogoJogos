using ApiCatalagoJogos.Exceptions;
using ApiCatalogoJogos.Exceptions;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.Services;
using ApiCatalogoJogos.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        private readonly IJogoService _jogoService;

        public JogosController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }

        /// <summary>
        /// Buscar todos os jogos de forma paginada
        /// </summary>
        /// <remarks>
        /// Não é possível buscar os jogos
        /// </remarks>
        /// <param name="pagina">Indica qual pagina esta sendo consultada [minimo 1]</param>
        /// <param name="quantidade">Indica a quantidade de registros por página. [mínimo 1 máximo 50]</param>
        /// <response code="200">Retorna lista de jogos</response>
        /// <response code="204">Caso não haja jogos</response>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoViewModel>>> Obter ([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            var jogos = await _jogoService.Obter(pagina, quantidade);

            if(jogos.Count() == 0)
                return NoContent();

            return Ok(jogos);
        }

        /// <summary>
        /// Buscar um jogo por id
        /// </summary>
        /// <remarks>
        /// Não é possível buscar os jogos
        /// </remarks>
        /// <param name="idJogo">Indica qual pagina esta sendo consultada [minimo 1]</param>
        /// <response code="200">Retorna o jogo</response>
        /// <response code="204">Caso não haja jogo com esse id</response>
        /// <returns></returns>
        [HttpGet("{idJogo:guid}")]
        public async Task<ActionResult<JogoViewModel>> Obter([FromRoute] Guid idJogo)
        {
            var jogo = await _jogoService.Obter(idJogo);

            if (jogo == null)
                return NoContent();

            return Ok(jogo);
        }

        /// <summary>
        /// Inserir um novo jogo
        /// </summary>
        /// <remarks>
        /// Não é possível criar o jogo
        /// </remarks>
        /// <response code="200">Sucesso ao inserir</response>
        /// <response code="204">Error ao inserir um novo jogo</response>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> InserirJogo([FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                var jogo = await _jogoService.Inserir(jogoInputModel);

                return Ok(jogo);
            }
            catch(JogoJaCadastradoException ex)
            {
                return UnprocessableEntity("Já existe um jogo cadastrado com esse nome para esta produtora!");
            }

        }

        /// <summary>
        /// Atualizar um jogo por id
        /// </summary>
        /// <remarks>
        /// Não é possível atualizar o jogo
        /// </remarks>
        /// <response code="200">Sucesso ao atualizar</response>
        /// <response code="404">Não existe esse jogo!</response>
        /// <returns></returns>
        [HttpPut("{idJogo:guid}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid idJogo, [FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, jogoInputModel);

                return Ok();
            }
            catch(JogoNaoCadastradoException ex)
            {
                return NotFound("Não existe esse jogo!");
            }

        }

        /// <summary>
        /// Atualizar um atributo do jogo (preço)
        /// </summary>
        /// <remarks>
        /// Não é possível atualizar o preço do jogo
        /// </remarks>
        /// <response code="200">Sucesso ao atualizar</response>
        /// <response code="404">Não existe esse jogo!</response>
        /// <returns></returns>
        [HttpPatch("{idJogo:guid}/preco/{preco:double}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid idJogo, [FromRoute] double preco)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, preco);

                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Não existe esse jogo!");
            }
        }

        /// <summary>
        /// Deletar um jogo por id
        /// </summary>
        /// <remarks>
        /// Não é possível deletar o jogo
        /// </remarks>
        /// <response code="200">Sucesso ao deletar</response>
        /// <response code="404">Não existe esse jogo!</response>
        /// <returns></returns>
        [HttpDelete("{idJogo:guid}")]
        public async Task<ActionResult> DeletarJogo([FromRoute] Guid idJogo)
        {
            try
            {
                await _jogoService.Remover(idJogo);

                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Não existe esse jogo!");
            }
        }
    }
}
