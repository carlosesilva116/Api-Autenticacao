using System.ComponentModel.DataAnnotations;

namespace apiAutenticacao.Models.DTO
{
    public class AlterarSenhaDTO
    {

        [Required(ErrorMessage = "O Email é obrigatório.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Senha Atual é obrigatória.")]
        public string SenhaAtual { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Nova Senha é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A Nova Senha deve ter entre 6 e 100 caracteres.")]
        public string NovaSenha { get; set; } = string.Empty;

        [Compare("NovaSenha", ErrorMessage = "A Confirmação de senha dever ser igual a Nova senha.")]
        public string ConfirmarNovaSenha { get; set; } = string.Empty;


    }
}
