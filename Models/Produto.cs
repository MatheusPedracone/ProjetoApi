using System.ComponentModel.DataAnnotations;

namespace CommerceApi.Data
{
    public class Produto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatoório")]
        [MaxLength(60, ErrorMessage = "Este campo deve conter entre 3 e 80 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 80 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public  bool Ativo { get; set; }

        public int Estoque { get; set; }
    }
}