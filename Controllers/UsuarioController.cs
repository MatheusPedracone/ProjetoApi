using CommerceApi.Data;
using CommerceApi.Models;
using CommerceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Usuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Faz o login do usuário
        /// </summary>
        ///
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /Produtos
        ///     {
        ///        "email": "email@email.com"
        ///        "senha": "123456123"
        ///     }
        ///
        /// </remarks>
        ///
        /// <response code="200">Login efetuado com sucesso</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] UserLoginModel model)
        {
            // Recupera o usuário
            var user = await _context.Usuarios.Where(x => x.Email.Equals(model.Email)).FirstOrDefaultAsync();

            // Verifica se o usuário existe
            if (user == null)
                return NotFound(new { message = "Usuário não encontrado" });

            bool verified = BCrypt.Net.BCrypt.Verify(model.Senha, user.Senha);

            if (verified == true)
            {
                // Gera o Token
                var token = TokenService.GenerateToken(user);

                // Oculta a senha
                user.Senha = "";
                // Retorna os dados
                return new
                {
                    user = user,
                    token = token
                };
            }
            else
            {
                return Unauthorized(new { message = "Usuário ou senha inválidos" });
            }
        }
        /// <summary>
        /// Busca um usuário pelo Id
        /// </summary>
        /// <response code="200">Usuário encontrado</response>
        /// <response code="400">falha ao encontrar usuário</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await _context.Usuarios.FindAsync(id);
            user.Senha = "";
            return user ?? (ActionResult<User>)NotFound();
        }
        // POST api/ Usuários
        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST / Usuários
        ///     {
        ///        "id": 1,
        ///        "name": "nome",
        ///        "email": email@email.com,
        ///        "senha": "123456123"
        ///     }
        ///
        /// </remarks>
        /// <returns>Um novo item criado</returns>
        /// <response code="200">novo usuário criado</response>
        [HttpPost]
        public async Task<ActionResult<User>> Post(User user)
        {
            user.Senha = BCrypt.Net.BCrypt.HashPassword(user.Senha);
            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();
            // Oculta a senha
            user.Senha = "";
            return user;
        }
        /// <summary>
        /// Lista os usuários
        /// </summary>
        /// <response code="200">usuários foram listados</response>
        /// <response code="400">falha ao listar usuário</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        /// <summary>
        /// Edita um usuário
        /// </summary>
        /// <response code="200">Usuários foram alterados</response>
        /// <response code="404">Usuário não encontrado</response>
        /// <response code="400">Id é diferente do usuário recebido por body</response>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, User user)
        {

            if (id != user.Id)
            {
                return BadRequest("");
            }
            user.Senha = BCrypt.Net.BCrypt.HashPassword(user.Senha);
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!UsuariosExists(id))
            {
                return NotFound();
            }

            return Ok();
        }

        /// <summary>
        /// Deleta um usuário
        /// </summary>
        /// <response code="204">Usuário deletado</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Usuarios.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool UsuariosExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}