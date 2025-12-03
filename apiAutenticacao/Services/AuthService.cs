using apiAutenticacao.Data;
using apiAutenticacao.Models;
using apiAutenticacao.Models.DTO;
using apiAutenticacao.Models.Response;
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

        public async Task<ResponseLogin> Login(LoginDTO dadosUsuario)
        {

            Usuario? usuarioEncontrado = await _context.Usuarios.FirstOrDefaultAsync(usuario => usuario.Email == dadosUsuario.Email);

            if (usuarioEncontrado != null)
            {
                bool isValiPassword = Verify(dadosUsuario.Senha, usuarioEncontrado.Senha);

                if (isValiPassword)
                {
                    return new ResponseLogin
                    {
                        Erro = false,
                        Message = "Login realizado com sucesso",
                        Usuario = usuarioEncontrado
                    };
                }

                return new ResponseLogin
                {
                    Erro = true,
                    Message = "login não realizado. Email ou senha incorretos",
         
                };
            }

            return new ResponseLogin
            {
                Erro = true,
                Message = "Usuário não encontrado",
               
            };

        }

        public async Task<ResponseCadastro> CadastrarUsuarioAsync (CadastroUsuarioDTO dadosUsuarioCadastro)
        {
            Usuario? usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(usuario => usuario.Email == dadosUsuarioCadastro.Email);

            if (usuarioExistente != null)
                {
                return new ResponseCadastro
                { 

                    Erro = true,
                    Message = "Este email já esta cadastrado"

                };
                

            }

            Usuario usuario = new Usuario
            {
                Nome = dadosUsuarioCadastro.Nome,
                Email = dadosUsuarioCadastro.Email,
                Senha = HashPassword(dadosUsuarioCadastro.Senha),
                ConfirmarSenha = HashPassword(dadosUsuarioCadastro.Senha)

            };
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return new ResponseCadastro
            {
                Erro = false,
                Message = "Usuário criado com sucesso",
                Usuario = usuario
            };

        }

        public async Task<ResponseAlterarSenha> AlterarSenhaAsync (AlterarSenhaDTO dadosAlterarsenha)
        {
            Usuario? usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(usuario => usuario.Email == dadosAlterarsenha.Email);

            if (usuarioExistente == null)
            {
                return new ResponseAlterarSenha
                {
                    Erro = true,
                    Message = "Usuário não encontrado"
                };
            }

            bool isValidPassword = Verify(dadosAlterarsenha.SenhaAtual, usuarioExistente.Senha);
;
            if (!isValidPassword)
            {
                return new ResponseAlterarSenha
                {
                    Erro = true,
                    Message = "A senha atual está incorreta"
                };
            }
           
            if (dadosAlterarsenha.NovaSenha != dadosAlterarsenha.ConfirmarNovaSenha)
            {
                return new ResponseAlterarSenha
                {
                    Erro = true,
                    Message = "A confirmação de senha deve ser igual a nova senha"
                };
            }

            usuarioExistente.Senha = HashPassword(dadosAlterarsenha.NovaSenha);

            Usuario? usuario = new Usuario
            {
                Nome = usuarioExistente.Nome,
                Email = usuarioExistente.Email,
                Senha = usuarioExistente.Senha,
                ConfirmarSenha = usuarioExistente.Senha
            };

            _context.Usuarios.Update(usuarioExistente);
            await _context.SaveChangesAsync();

            return new ResponseAlterarSenha {
                Erro = false,
                Message = "Senha alterada com sucesso",
                Usuario = usuario
           
            };


        }



    }
}
