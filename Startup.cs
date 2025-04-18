using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.ActiveDirectory;
using Owin;
using System.IdentityModel.Tokens;

[assembly: OwinStartup(typeof(GestiondTransaccionesBancarias.Startup))]
namespace GestiondTransaccionesBancarias
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 1) Habilita CORS para que Swagger y tu front puedan llamar sin bloqueos
            app.UseCors(CorsOptions.AllowAll);

            // 2) Configura Azure AD Bearer Auth
            var tenantId = "{33a93990-8bb4-4b9d-b422-4f8851217d7e}";
            var clientId = "{a3978411-dafe-43d0-b198-0848cac7b4c8}";
            var authority = $"https://login.microsoftonline.com/{tenantId}";

            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
                new WindowsAzureActiveDirectoryBearerAuthenticationOptions
                {
                    Tenant = tenantId,
                    TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = clientId
                    },
                    MetadataAddress = $"{authority}/v2.0/.well-known/openid-configuration"
                });
        }
    }
}
