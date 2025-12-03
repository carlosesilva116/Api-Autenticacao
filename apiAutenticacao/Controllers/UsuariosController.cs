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

            ResponseCadastro response = await _authService.CadastrarUsuarioAsync(dadosUsuario);

            if (response.Erro)
            {

                return BadRequest(response);

            }

            return Ok(response);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dadosUsuario)
        {

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



        [HttpPut ("AlterarSenha")]
        public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaDTO dadosAlterarSenha)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ResponseAlterarSenha response = await _authService.AlterarSenhaAsync(dadosAlterarSenha);

            if (response.Erro)
            {
                return BadRequest(response);

            }
            return Ok(response);



        }
    }
}
    

