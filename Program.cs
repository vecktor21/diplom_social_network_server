using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using server;
using server.Hubs;
using server.Services;
using server.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AccountService>();
builder.Services.AddCors(opts =>
{
    opts.AddPolicy("default", policy =>
    {
        policy.WithOrigins("http://localhost:3000");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowCredentials();
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.RequireHttpsMetadata = true;
        opts.TokenValidationParameters = AuthOptions.GetTokenValidationParameters();
    });
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationContext>(opts =>
{

    opts.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<GroupService>();
builder.Services.AddTransient<PublicationService>();
builder.Services.AddTransient<MessageService>();

//сервис записи файлов в файловую систему
builder.Services.AddTransient<FileService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseCors("default");


//необходимо перенести MapControllers в UseEndpoints
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoing =>
{
    endpoing.MapHub<MessengerHub>("/chat");
    endpoing.MapControllers();
});
//запуск приложения
//app.MapControllers();
app.Run();
