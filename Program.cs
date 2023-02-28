using Microsoft.Extensions.Options;
using System.Configuration;
using WebShop.Main.DBContext;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Authenticate;
using Microsoft.Extensions.Configuration;
using WebShop.Main.BusinessLogic;
using Shop.Main.BusinessLogic;
using WebShop.Controllers;
using WebShop.Main.Interfaces;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

var authOptionsConfiguration = builder.Configuration.GetSection("Auth");
builder.Services.Configure<AuthOptions>(authOptionsConfiguration);


// Add services to the container.

builder.Services.AddDbContext<ShopContext>(options =>
                options.UseSqlServer("Server=tcp:anular-shop.database.windows.net,1433;Initial Catalog=project-database;Persist Security Info=False;User ID=oleg;Password=QWUngoSdd13Ss@123@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));

builder.Services.AddControllers();

var authOptions = builder.Configuration.GetSection("Auth").Get<AuthOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true, // set falel
            ValidIssuer = authOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = authOptions.Audience,

            ValidateLifetime = true,

            IssuerSigningKey = authOptions.GetSymmetricSecuritykey(),
            ValidateIssuerSigningKey = true,
        };
    });


// config cors policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// add services
builder.Services.AddScoped<IAdminActionsBL, AddAdminActionsBL>();
builder.Services.AddScoped<ICartItemActionsBL, CartItemActionsBL>();
builder.Services.AddScoped<ICategoryActionsBL, CategoryActionsBL>();
builder.Services.AddScoped<IComentsActionsBL, ComentsActionsBL>();
builder.Services.AddScoped<IDiscountActionsBL, DiscountActionsBL>();
builder.Services.AddScoped<IFilterActionsBL, FilterActionsBL>();
builder.Services.AddScoped<ILogInActionsBL, LogInActionsBL>();
builder.Services.AddScoped<IMoneyOnBalanceActionsBL, MoneyOnBalanceActionsBL>();
builder.Services.AddScoped<IOrderActionsBL, OrderActionsBL>();
builder.Services.AddScoped<IProductActionsBL, ProductActionsBL>();
builder.Services.AddScoped<IPromocodeActionsBL, PromocodeActionsBL>();
builder.Services.AddScoped<IRegistActionsBL, RegistActionsBL>();
builder.Services.AddScoped<IUserActionsBL, UserActionsBL>();
builder.Services.AddScoped<ILoggerBL, LoggerBL>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseAuthentication();

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

