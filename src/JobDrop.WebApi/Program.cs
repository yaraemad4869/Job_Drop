using JobDrop.Application;
using JobDrop.Infrastructure;
using JobDrop.Application.Common;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Entities.Identity;
using JobDrop.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure Serilog
builder.Host.UseSerilog((context, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console());

// 2. Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Use Application and Infrastructure extension methods
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
builder.Services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());


    // --- FIX: Missing DI Registrations ---
    builder.Services.AddHttpContextAccessor();

    // ICurrentUser
    // TODO: Verify namespace for CurrentUserService
    // builder.Services.AddScoped<JobDrop.Application.Common.ICurrentUser, JobDrop.WebApi.Services.CurrentUserService>();
    // TODO: Verify namespace for TokenService
    // builder.Services.AddScoped<JobDrop.Application.Contracts.Identity.ITokenService, JobDrop.Infrastructure.Identity.TokenService>();
    // TODO: Verify namespace for PaymobClient
    // builder.Services.AddScoped<JobDrop.Application.Contracts.Billing.IPaymobClient, JobDrop.Infrastructure.Billing.PaymobClient>();
    // TODO: Verify namespace for DropWalletPolicy
    // builder.Services.AddScoped<JobDrop.Domain.Services.DropWalletPolicy>();
    // -------------------------------------


    


    // --- Accurate Mock Service Registrations for Development ---
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<JobDrop.Application.Common.ICurrentUser, JobDrop.WebApi.Services.MockCurrentUser>();
    builder.Services.AddScoped<JobDrop.Application.Contracts.Identity.ITokenService, JobDrop.WebApi.Services.MockTokenService>();
    builder.Services.AddScoped<JobDrop.Application.Contracts.Billing.IPaymobClient, JobDrop.WebApi.Services.MockPaymobClient>();
    
    // DropWalletPolicy is a sealed pure domain service, register it directly
    builder.Services.AddSingleton<JobDrop.Domain.Services.DropWalletPolicy>();
    // -----------------------------------------------------------


    // --- Accurate Mock Service Registrations for Development ---
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<JobDrop.Application.Common.ICurrentUser, JobDrop.WebApi.Services.MockCurrentUser>();
    builder.Services.AddScoped<JobDrop.Application.Contracts.Identity.ITokenService, JobDrop.WebApi.Services.MockTokenService>();
    builder.Services.AddScoped<JobDrop.Application.Contracts.Billing.IPaymobClient, JobDrop.WebApi.Services.MockPaymobClient>();
    
    // DropWalletPolicy is a sealed pure domain service, register it directly
    builder.Services.AddSingleton<JobDrop.Domain.Services.DropWalletPolicy>();
    // -----------------------------------------------------------

var app = builder.Build();

// 3. Configure middleware
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }







