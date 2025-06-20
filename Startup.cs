using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(GestiondTransaccionesBancarias.Startup))]
namespace GestiondTransaccionesBancarias
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            var tenantId = "33a93990-8bb4-4b9d-b422-4f8851217d7e";
            var clientId = "a3978411-dafe-43d0-b198-0848cac7b4c8";
            var issuer = $"https://login.microsoftonline.com/{tenantId}/v2.0";
            var metadataUrl = $"{issuer}/.well-known/openid-configuration";

            // Obtener dinámicamente las claves de firma (issuer signing keys)
            var configurationManager = new Microsoft.IdentityModel.Protocols.ConfigurationManager<Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration>(
                metadataUrl,
                new Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfigurationRetriever()
            );

            var openIdConfig = Task.Run(() => configurationManager.GetConfigurationAsync()).Result;

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = issuer,
                ValidAudience = $"api://{clientId}",
                IssuerSigningKeys = openIdConfig.SigningKeys,

                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5)
            };

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                TokenValidationParameters = tokenValidationParameters
            });
        }
    }
}
