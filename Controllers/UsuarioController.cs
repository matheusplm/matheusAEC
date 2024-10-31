using AEC.Data;
using AEC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AEC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context) => _context = context;

        /// <summary>
        /// Obtém todos os usuários.
        /// </summary>
        /// <returns>Uma lista de usuários.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios);
        }

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        /// <param name="usuario">O usuário a ser criado.</param>
        /// <returns>O usuário criado.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(UsuarioModel usuario)
        {
            if (ModelState.IsValid)
            {
                var passwordHasher = new PasswordHasher<UsuarioModel>();
                usuario.Senha = passwordHasher.HashPassword(usuario, usuario.Senha);

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Index), new { id = usuario.Id }, new { message = "Usuário criado com sucesso", usuarioId = usuario.Id });
            }
            return BadRequest(ModelState);
        }


        /// <summary>
        /// Obtém um usuário específico para edição.
        /// </summary>
        /// <param name="id">ID do usuário a ser editado.</param>
        /// <returns>O usuário correspondente ao ID.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            return Ok(usuario);
        }

        /// <summary>
        /// Atualiza um usuário existente.
        /// </summary>
        /// <param name="id">ID do usuário a ser atualizado.</param>
        /// <param name="usuario">Os novos dados do usuário.</param>
        /// <returns>Uma resposta sem conteúdo.</returns>
        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioModel usuario)
        {
            if (id != usuario.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(usuario);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Deleta um usuário existente.
        /// </summary>
        /// <param name="id">ID do usuário a ser deletado.</param>
        /// <returns>Uma resposta sem conteúdo.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
