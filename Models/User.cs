using System;
using System.ComponentModel.DataAnnotations;

namespace CommerceApi.Data
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(20, ErrorMessage = "Este campo deve conter entre 3 e 80 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 80 caracteres")]
        public string Nome { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatório")]
        [MaxLength(200, ErrorMessage = "Este campo deve conter entre 3 e 200 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 80 caracteres")]
        public string Senha { get; set; }
    }
}

