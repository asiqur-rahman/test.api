
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using Test.Infrastructure;
using Test.Application;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container

// Preserve PascalCase JSON property names
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Swagger / OpenAPI configuration
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Clean Project API",
        Version = "v1"
    });

    // JWT Bearer authentication definition
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste the token returned by Account/Login. Do not add the word Bearer."
    });

    // Apply JWT authentication to all documented endpoints
    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] =
                new List<string>()
        });
});

#endregion


var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "Jwt:Key is required in configuration");

var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? "testissuer";

var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? "testaudience";

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProjectInfrastructure(
    builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddApplicationServices();


var app = builder.Build();

await DatabaseInitializer.InitializeAsync(app.Services);

#region HTTP Pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = "swagger";
        options.DocumentTitle = "Clean Project API";
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "Clean Project API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Redirect the root URL to Swagger UI
app.MapGet("/", (HttpContext context) =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

#endregion
try
{
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Fatal error: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
    throw;
}

