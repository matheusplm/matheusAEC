using System.Data.Entity;

namespace AEC.Models
{
    public class AECContext : DbContext
    {
        public AECContext() : base("AECContext")
        {
        }

        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<EnderecoModel> Enderecos { get; set; }
    }
}
