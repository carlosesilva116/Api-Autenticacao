using System.ComponentModel.DataAnnotations;

namespace apiAutenticacao.Models.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage ="O email é obrigatório")]
        [EmailAddress(ErrorMessage ="O email é válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage ="A senha é obrigatória")]
        public string Senha { get; set; } = string.Empty;

    }
}
