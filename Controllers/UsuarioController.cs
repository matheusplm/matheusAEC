using AEC.Data;
using AEC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AEC.Filters;

namespace AEC.Controllers
{
    [AuthenticatedUser]
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Index", "Login");

            var usuarioLogado = await _context.Usuarios.SingleOrDefaultAsync(u => u.Id == userId);
            if (usuarioLogado == null) return RedirectToAction("Index", "Login");

            return View(usuarioLogado);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateName(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                ModelState.AddModelError(nameof(nome), "O nome é obrigatório.");
                var usuarioAtual = await _context.Usuarios.SingleOrDefaultAsync(u => u.Id == HttpContext.Session.GetInt32("UserId"));
                return View("Index", usuarioAtual);
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            var usuario = await _context.Usuarios.SingleOrDefaultAsync(u => u.Id == userId);

            if (usuario == null) return NotFound("Usuário não encontrado.");

            usuario.Nome = nome;
            _context.Update(usuario);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(string senhaAtual, string novaSenha, string confirmacaoSenha)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var usuario = await _context.Usuarios.SingleOrDefaultAsync(u => u.Id == userId);

            if (usuario == null) return NotFound("Usuário não encontrado.");

            var passwordHasher = new PasswordHasher<UsuarioModel>();
            var verificaSenha = passwordHasher.VerifyHashedPassword(usuario, usuario.Senha, senhaAtual);

            if (verificaSenha == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Senha atual incorreta.");
                return View("Index", usuario);
            }

            if (novaSenha != confirmacaoSenha)
            {
                ModelState.AddModelError(string.Empty, "A nova senha e a confirmação devem coincidir.");
                return View("Index", usuario);
            }

            usuario.Senha = passwordHasher.HashPassword(usuario, novaSenha);
            _context.Update(usuario);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
