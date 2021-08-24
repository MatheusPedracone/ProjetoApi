using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CommerceApi.Data;

namespace Produtos.Controllers
{
    [Route("v1/produtos")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Lista os produtos
        /// </summary>
        /// <response code="200">Returna os produtos cadastrados</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            return await _context.Produtos.ToListAsync();
        }
        /// <summary>
        /// Busca um produto pelo Id
        /// </summary>
        /// <returns>itens criados</returns>
        /// <response code="200">Returna os itens cadastrados</response>
        // GET: api/Produtos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            return produto ?? (ActionResult<Produto>)NotFound();
        }
        /// <summary>
        /// Edita um produto
        /// </summary>
        /// <returns>produtos criados</returns>
        /// <response code="200">produtos foram alterados</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, Produto produto)
        {
            if (id != produto.Id)
            {
                return BadRequest();
            }

            _context.Entry(produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ProdutoExists(id))
            {
                return NotFound();
            }

            return Ok();
        }
        // POST api/Produtos
        /// <summary>
        /// Cria um novo produto
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /Produtos
        ///     {
        ///        "id": 1,
        ///        "name": "estojo",
        ///        "preco": 10.00
        ///        "ativo": true,
        ///        "estoque": 99
        ///      }
        /// </remarks>
        /// <response code="201">Um novo produto foi criado</response>
        [HttpPost]
        public async Task<ActionResult<Produto>> PostProduto(Produto produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduto", new { id = produto.Id }, produto);
        }
        /// <summary>
        /// Deleta um produto
        /// </summary>
        /// <response code="204">Produto deletado</response>
        /// <response code="404">Produto não encontrado</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }
    }
}
