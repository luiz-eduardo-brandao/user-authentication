using Users.API.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataContext(builder.Configuration);
    
builder.Services.AddIdentityConfiguration();
    
builder.Services.AddControllers();

builder.Services.AddSwaggerConfiguration();

builder.Services.AddAuthenticationSetup();
    
builder.Services.AddServicesSetup();

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
