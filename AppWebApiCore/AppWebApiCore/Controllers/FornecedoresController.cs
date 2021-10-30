using AppWebApiCore.Data;
using AppWebApiCore.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace AppWebApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedoresController : ControllerBase
    {

        private readonly ApiDbContext _context;

        public FornecedoresController(ApiDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable>> ObterTodosFonecedores()
        {
            return await _context.Fornecedores.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Fornecedor>> ObterFornecedor(Guid id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);

            if (FornecedorIsEmpty(fornecedor))
                return NotFound();

            return Ok(fornecedor);
        }

        [HttpPost]
        public async Task<ActionResult<Fornecedor>> InserirNovoFornecedor(Fornecedor fornecedor)
        {
            var fornecedorJaRegistrado = await _context.Fornecedores.AnyAsync(f => f.Documento.Equals(fornecedor.Documento));

            if (fornecedorJaRegistrado)
                return Ok("Já existe um fornecedor cadastrado com esse documento.");

            _context.Fornecedores.Add(fornecedor);
            await _context.SaveChangesAsync();

            return Ok(fornecedor);

        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Fornecedor>> AtualizarFornecedor(Guid id, Fornecedor fornecedorSerAtualizado)
        {
            var fornecedorJaRegistrado = await _context.Fornecedores.FindAsync(fornecedorSerAtualizado.Id);

            if (FornecedorIsEmpty(fornecedorJaRegistrado))
                return NotFound();

            if (VerificarIds(id, fornecedorSerAtualizado))
                return BadRequest();

            fornecedorJaRegistrado.Nome = fornecedorSerAtualizado.Nome;
            fornecedorJaRegistrado.Documento = fornecedorSerAtualizado.Documento;
            fornecedorJaRegistrado.TipoFornecedor = fornecedorSerAtualizado.TipoFornecedor;
            fornecedorJaRegistrado.Ativo = fornecedorSerAtualizado.Ativo;

            try
            {
                /*
                 * atenção 1 - Não preciso colocar, desta forma o EF core só cria a query para o 
                 *  a query para o campo que foi alterado.
                 */
                //atenção  1 - _context.Fornecedores.Update(fornecedorJaRegistrado);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Fornecedor>> ExcluirFornecedor(Guid id)
        {
            var fornecedorJaRegistrado = await _context.Fornecedores.FindAsync(id);

            if (FornecedorIsEmpty(fornecedorJaRegistrado))
                return NotFound();

            _context.Fornecedores.Remove(fornecedorJaRegistrado);
            await _context.SaveChangesAsync();

            return Ok(fornecedorJaRegistrado);
        }



        /// <summary>
        /// Verifica se os Ids são iguais.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fornecedorSerAtualizado"></param>
        /// <returns></returns>
        private static bool VerificarIds(Guid id, Fornecedor fornecedorSerAtualizado)
        {
            return id != fornecedorSerAtualizado.Id;
        }

        /// <summary>
        /// Verifica se o fornecedor passado é vazio.
        /// </summary>
        /// <param name="fornecedor"></param>
        /// <returns></returns>
        private static bool FornecedorIsEmpty(Fornecedor fornecedor)
        {
            return fornecedor == null;
        }
    }
}
