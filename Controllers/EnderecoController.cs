using AEC.Data;
using AEC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AEC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnderecoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnderecoController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os endereços.
        /// </summary>
        /// <returns>Uma lista de endereços.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var enderecos = await _context.Enderecos.ToListAsync();
            return Ok(enderecos);
        }

        /// <summary>
        /// Cria um novo endereço.
        /// </summary>
        /// <param name="endereco">O endereço a ser criado.</param>
        /// <returns>O endereço criado.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EnderecoModel endereco)
        {
            if (ModelState.IsValid)
            {
                _context.Add(endereco);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Index), new { id = endereco.Id }, endereco);
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Obtém um endereço para edição.
        /// </summary>
        /// <param name="id">ID do endereço a ser editado.</param>
        /// <returns>O endereço correspondente ao ID.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var endereco = await _context.Enderecos.FindAsync(id);
            if (endereco == null) return NotFound();

            return Ok(endereco);
        }

        /// <summary>
        /// Atualiza um endereço existente.
        /// </summary>
        /// <param name="id">ID do endereço a ser atualizado.</param>
        /// <param name="endereco">Os novos dados do endereço.</param>
        /// <returns>Uma resposta sem conteúdo.</returns>
        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EnderecoModel endereco)
        {
            if (id != endereco.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(endereco);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Deleta um endereço existente.
        /// </summary>
        /// <param name="id">ID do endereço a ser deletado.</param>
        /// <returns>Uma resposta sem conteúdo.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var endereco = await _context.Enderecos.FindAsync(id);
            if (endereco == null) return NotFound();

            _context.Enderecos.Remove(endereco);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
