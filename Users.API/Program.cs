using Users.API.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDataContext(builder.Configuration)
    .AddIdentityConfiguration()
    .AddAuthenticationSetup()
    .AddServicesSetup();

builder.Services.AddControllers();

builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
