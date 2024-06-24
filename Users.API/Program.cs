using Users.API.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataContext(builder.Configuration);

builder.Services.AddCors();

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

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials


app.MapControllers();

app.Run();
