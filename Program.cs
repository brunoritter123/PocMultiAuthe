using ApiMultiAuthe.Auth.AuthOffline;
using ApiMultiAuthe.Auth.AuthOnline;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var services = builder.Services;

//services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//                .AddScheme<AuthOfflineOptions, AuthOfflineHandler>(AuthOfflineDefaults.AuthenticationScheme, null);

string publicXmlKey = File.ReadAllText("./public_key.xml");
RSA rsa = RSA.Create();
rsa.FromXmlString(publicXmlKey);

services.AddAuthentication()
        .AddJwtBearer(AuthOfflineDefaults.AuthenticationScheme, x =>
        {
            x.RequireHttpsMetadata = true;
            //x.SaveToken = true;
            x.IncludeErrorDetails = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(rsa),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = "http://localhost",
                ValidIssuer = "TechPet"
            };
        });

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddScheme<AuthOnlineOptions, AuthOnlineHandler>(AuthOnlineDefaults.AuthenticationScheme, null);

services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(AuthOnlineDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();