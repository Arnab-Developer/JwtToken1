using JwtToken1.IdentityProvider.Endpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddTransient<ITokenGenerator, TokenGenerator>();
builder.Services.AddSqlServer<AuthContext>(builder.Configuration.GetConnectionString("Auth"));
builder.Services.AddOptions<TokenOptions>().BindConfiguration("Token");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapRegistrationEndpoint();
app.MapLoginEndpoint();

app.Run();