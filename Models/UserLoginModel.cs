using System.ComponentModel.DataAnnotations;

namespace CommerceApi.Models
{
    public class UserLoginModel
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatório")]
        [MaxLength(80, ErrorMessage = "Este campo deve conter entre 3 e 80 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 80 caracteres")]
        public string Senha { get; set; }
    }
}