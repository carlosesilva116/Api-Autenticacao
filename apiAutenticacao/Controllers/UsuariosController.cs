using apiAutenticacao.Data;
using apiAutenticacao.Models;
using apiAutenticacao.Models.DTO;
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

        public UsuariosController(AppDbContext context)
        {
            _context = context;


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

          Usuario? usuarioEncontrado = await _context.Usuarios.FirstOrDefaultAsync(usuario => usuario.Email == dadosUsuario.Email);

            if (usuarioEncontrado != null)
            {
                bool isValiPassword = Verify(dadosUsuario.Senha, usuarioEncontrado.Senha);

                if (isValiPassword )
                {
                    return Ok("Login realizado com sucesso");
                }

                return Unauthorized("Login náo realizado. Email ou senha incorreta");
            }

            return NotFound("Usuário não encontrado");

        }

    }
}
    

