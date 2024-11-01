using AEC.Data;
using AEC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AEC.Controllers
{
    [Route("Login")]
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.ErrorMessage = HttpContext.Session.GetString("ErrorMessage");
            HttpContext.Session.Remove("ErrorMessage");
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string usuario, string senha)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Usuarios.SingleOrDefaultAsync(u => u.Usuario == usuario);
                if (user != null)
                {
                    var passwordHasher = new PasswordHasher<UsuarioModel>();
                    var passwordVerification = passwordHasher.VerifyHashedPassword(user, user.Senha, senha);

                    if (passwordVerification == PasswordVerificationResult.Success)
                    {
                        HttpContext.Session.SetInt32("UserId", user.Id);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Credenciais inválidas");
            }
            return View("Index");
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(UsuarioModel novoUsuario)
        {
            if (ModelState.IsValid)
            {
                var usuarioExistente = await _context.Usuarios
                    .AnyAsync(u => u.Usuario == novoUsuario.Usuario);

                if (usuarioExistente)
                {
                    ModelState.AddModelError("Usuario", "O nome de usuário já está em uso.");
                    return View("Create", novoUsuario);
                }

                var passwordHasher = new PasswordHasher<UsuarioModel>();
                novoUsuario.Senha = passwordHasher.HashPassword(novoUsuario, novoUsuario.Senha);

                _context.Add(novoUsuario);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("Create", novoUsuario);
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index");
        }
    }
}
