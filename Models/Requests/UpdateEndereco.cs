using System.ComponentModel.DataAnnotations;

namespace AEC.Requests
{
    public class UpdateEndereco
    {
        [Required]
        [StringLength(8)]
        public string CEP { get; set; }

        [Required]
        [StringLength(100)]
        public string Logradouro { get; set; }

        [StringLength(100)]
        public string? Complemento { get; set; }

        [Required]
        [StringLength(50)]
        public string Bairro { get; set; }

        [Required]
        [StringLength(50)]
        public string Cidade { get; set; }

        [Required]
        [StringLength(2)]
        public string UF { get; set; }

        [Required]
        public int Numero { get; set; }

        [Required]
        public int Id { get; set; }
    }
}