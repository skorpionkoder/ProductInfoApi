using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ProductInfo.Data.Contexts;
using ProductInfo.Data.Interfaces;
using ProductInfo.Data.Repositories;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.TokenCacheProviders.InMemory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ProductContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ProductInfo");
    options.UseSqlServer(connectionString);
});
builder.Services.AddScoped<IProductInfoRepository, ProductInfoRepository>();

builder.Services.AddDbContext<ArticleContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ProductInfo");
    options.UseSqlServer(connectionString);
});
builder.Services.AddScoped<IArticleInfoRepository, ArticleInfoRepository>();

builder.Services.AddDbContext<ColourContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ProductInfo");
    options.UseSqlServer(connectionString);
});
builder.Services.AddScoped<IColourInfoRepository, ColourInfoRepository>();

builder.Services.AddDbContext<SizeScaleContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ProductInfo");
    options.UseSqlServer(connectionString);
});
builder.Services.AddScoped<ISizeScaleInfoRepository, SizeScaleInfoRepository>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Azure AD Authentication (Comment when testing locally)
//builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAdB2C")
//               .EnableTokenAcquisitionToCallDownstreamApi(new string[] { builder.Configuration["TodoList:TodoListScope"] })
//               .AddInMemoryTokenCaches();
//builder.Services.AddInMemoryTokenCaches();

// Validate bearer access tokens received by the Web API (Comment when testing locally)
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//           .AddMicrosoftIdentityWebApi(options =>
//           {
//               builder.Configuration.Bind("AzureAdB2C", options);

//               options.TokenValidationParameters.NameClaimType = "name";
//           },
//      options => { builder.Configuration.Bind("AzureAdB2C", options); });

// Enable Microsoft identity platform endpoint (Comment when testing locally)
//builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAdB2C")
//                    .EnableTokenAcquisitionToCallDownstreamApi(new string[] { builder.Configuration["TodoList:TodoListScope"] })
//                    .AddInMemoryTokenCaches();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();