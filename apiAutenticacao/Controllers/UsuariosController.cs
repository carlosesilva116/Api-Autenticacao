using apiAutenticacao.Data;
using apiAutenticacao.Models;
using apiAutenticacao.Models.DTO;
using apiAutenticacao.Models.Response;
using apiAutenticacao.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;



namespace apiAutenticacao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthService _authService;

        public UsuariosController(AppDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;

        }
        [HttpPost("cadastrar")]
        public async Task<ActionResult> CadastrarUsuarioAsync([FromBody] CadastroUsuarioDTO dadosUsuario)
        {
            {
                if (!ModelState.IsValid)

                    return BadRequest(ModelState);
            }
            Usuario? usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(usuario => usuario.Email == dadosUsuario.Email);

            if (usuarioExistente != null)

                return BadRequest(new { erro = true, mensagem = "Este email já esta cadastrado" });

            Usuario usuario = new Usuario
            {
                Nome = dadosUsuario.Nome,
                Email = dadosUsuario.Email,
                Senha = HashPassword(dadosUsuario.Senha),
                ConfirmarSenha = HashPassword(dadosUsuario.Senha)

            };
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                erro = false,
                mensagem = "Usuário criado com sucesso",
                usuario = new
                {

                    id = usuario.Id,
                    nome = usuario.Nome,
                    email = usuario.Email
                }


            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dadosUsuario) {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ResponseLogin response = await _authService.Login(dadosUsuario);

           if (response.Erro)
            {
                return BadRequest(response.Message);

            }
           return Ok(response);
        }

    }
}
    

