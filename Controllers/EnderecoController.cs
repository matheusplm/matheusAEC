using AEC.Data;
using AEC.Filters;
using AEC.Models;
using AEC.Requests;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using System.Globalization;
using System.Text;

namespace AEC.Controllers
{
    [AuthenticatedUser]
    public class EnderecoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnderecoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Login");

            var enderecos = await _context.Enderecos
                .Where(e => e.UsuarioId == userId)
                .ToListAsync();

            return View(enderecos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEndereco endereco)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Login");

            if (ModelState.IsValid)
            {
                var newEndereco = new EnderecoModel
                {
                    CEP = endereco.CEP,
                    Logradouro = endereco.Logradouro,
                    Complemento = endereco.Complemento,
                    Bairro = endereco.Bairro,
                    Cidade = endereco.Cidade,
                    UF = endereco.UF,
                    Numero = endereco.Numero,
                    UsuarioId = (int)userId
                };

                _context.Add(newEndereco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return NoContent();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Login");

            var endereco = await _context.Enderecos
                .Where(e => e.Id == id && e.UsuarioId == userId)
                .FirstOrDefaultAsync();

            if (endereco == null) return NotFound();

            return View(endereco);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateEndereco endereco)
        {
            if (id != endereco.Id) return NotFound();

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Login");

            var enderecoExistente = await _context.Enderecos
                .Where(e => e.Id == id && e.UsuarioId == userId)
                .FirstOrDefaultAsync();

            if (enderecoExistente == null) return NotFound();

            if (ModelState.IsValid)
            {
                enderecoExistente.CEP = endereco.CEP;
                enderecoExistente.Logradouro = endereco.Logradouro;
                enderecoExistente.Complemento = endereco.Complemento;
                enderecoExistente.Bairro = endereco.Bairro;
                enderecoExistente.Cidade = endereco.Cidade;
                enderecoExistente.UF = endereco.UF;
                enderecoExistente.Numero = endereco.Numero;

                _context.Update(enderecoExistente);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(enderecoExistente);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Login");

            var endereco = await _context.Enderecos
                .Where(e => e.Id == id && e.UsuarioId == userId)
                .FirstOrDefaultAsync();

            if (endereco == null) return NotFound();

            return View(endereco);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Login");

            var endereco = await _context.Enderecos
                .Where(e => e.Id == id && e.UsuarioId == userId)
                .FirstOrDefaultAsync();

            if (endereco == null) return NotFound();

            _context.Enderecos.Remove(endereco);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> ExportToCsv()
        {
            var enderecos = await _context.Enderecos.ToListAsync();

            using (var writer = new StringWriter())
            {
                var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";"
                };

                using (var csv = new CsvWriter(writer, csvConfig))
                {
                    await csv.WriteRecordsAsync(enderecos.Select(e => new
                    {
                        e.Id,
                        e.CEP,
                        e.Logradouro,
                        e.Complemento,
                        e.Bairro,
                        e.Cidade,
                        e.UF,
                        e.Numero
                    }));

                    var result = writer.ToString();
                    var bytes = Encoding.UTF8.GetBytes(result);

                    return File(bytes, "text/csv", "enderecos.csv");
                }
            }
        }


    }
}
