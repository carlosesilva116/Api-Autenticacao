using System.ComponentModel.DataAnnotations;

namespace apiAutenticacao.Models.DTO
{
    public class CadastroUsuarioDTO
    {
        [Required(ErrorMessage="O Nome é obrigatório.")]
        [StringLength(100, MinimumLength = 3)]
        public string Nome { get; set; } = string.Empty;


        [Required(ErrorMessage = "O Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O Email inválido.")]
        public string Email { get; set; } = string.Empty;  
        
        [Required(ErrorMessage = "A Senha é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A Senha deve ter entre 6 e 100 caracteres.")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Senha é obrigatória.")]
        [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmarSenha { get; set; } = string.Empty;
    }
}
