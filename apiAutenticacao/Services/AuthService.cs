using apiAutenticacao.Data;
using apiAutenticacao.Models;
using apiAutenticacao.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;


namespace apiAutenticacao.Services
{
    public class AuthService
    {

        private readonly AppDbContext _context;

        public AuthService(AppDbContext context) {
        
            _context = context; ;

        }

        public async Task<string> Login(LoginDTO dadosUsuario)
        {

            Usuario? usuarioEncontrado = await _context.Usuarios.FirstOrDefaultAsync(usuario => usuario.Email == dadosUsuario.Email);

            if (usuarioEncontrado != null)
            {
                bool isValiPassword = Verify(dadosUsuario.Senha, usuarioEncontrado.Senha);

                if (isValiPassword)
                {
                    return "Login realizado com sucesso";
                }

                return "Login náo realizado. Email ou senha incorreta";
            }

            return "Usuário não encontrado";

        }



    }
}
