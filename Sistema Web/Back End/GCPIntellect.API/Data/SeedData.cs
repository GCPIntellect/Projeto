using GCPIntellect.API.Models;
using System.Security.Cryptography;
using System.Text;

namespace GCPIntellect.API.Data
{
    public static class SeedData
    {
        public static void Initialize(ContextoBD context)
        {
            if (!context.Usuarios.Any())
            {
                var adminSenha = "admin123"; // Senha padrão para teste
                var senhaHash = GerarHashSenha(adminSenha);

                var admin = new Administrador
                {
                    Nome = "Administrador",
                    Login = "admin",
                    Email = "admin@gcpintellect.com",
                    SenhaHash = senhaHash,
                    Ativo = true,
                    DataCadastro = DateTime.UtcNow
                };

                context.Usuarios.Add(admin);
                context.SaveChanges();
            }
        }

        private static byte[] GerarHashSenha(string senha)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
            }
        }
    }
}