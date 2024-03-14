using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using API.Services.Notification;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Versioning;
using API.Repositories;

var builder = WebApplication.CreateBuilder(args);

#region Configuration
builder.Configuration.AddJsonFile("appsettings.json");
var configuration = builder.Configuration;
#endregion

#region SWITCH DATABASE
string typedata = "";
typedata = Environment.GetEnvironmentVariable("SGBD", EnvironmentVariableTarget.Process);
switch (typedata)
{
    case "PG_SGBD":
        Console.WriteLine("SGBD : PostgreSql - variables d'environement");
        string? host = Environment.GetEnvironmentVariable("SGBD_HOST", EnvironmentVariableTarget.Process);
        string? port = Environment.GetEnvironmentVariable("SGBD_PORT", EnvironmentVariableTarget.Process);
        string? user = Environment.GetEnvironmentVariable("SGBD_USER", EnvironmentVariableTarget.Process);
        string? pass = Environment.GetEnvironmentVariable("SGBD_PSWD", EnvironmentVariableTarget.Process);
        string? name = Environment.GetEnvironmentVariable("SGBD_NAME", EnvironmentVariableTarget.Process);
        if (host == null || port == null || user == null || pass == null || name == null) throw new ArgumentNullException("PG_SGBD : il manque des variables d'environement !");
        builder.Services.AddDbContext<IdentityAppDbContext>(options =>
           options.UseNpgsql($"Host={host};Port={port};Database={name};Username={user};Password={pass};"));
        break;
    default:
        Console.WriteLine("SGBD : PostgreSql - connexion par defaut");
        builder.Services.AddDbContext<IdentityAppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        break;

}
#endregion

#region Authentification
builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();

builder.Services.AddIdentityCore<AppUser>().AddEntityFrameworkStores<IdentityAppDbContext>().AddApiEndpoints();

#endregion

#region Logging
builder.Services.AddLogging(builder => builder.AddConsole());
#endregion

#region Dependency Injection
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<ConversationService>();
builder.Services.AddScoped<ExerciseService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<SetService>();
#endregion

#region Firebase Tools
FirebaseApp.Create(new AppOptions()//TODO ggkey.json � UPDATE (pas le bon)
{
    Credential = GoogleCredential.FromFile("ggkey.json"),
});
builder.Services.AddScoped<FCMService>(); //Notifications
#endregion

#region API And Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "1.0",
        Title = "Athletix API",
        Description = "Available APIs for dialog with front AthlethixApp."
    });

    opts.TagActionsBy(api => new[] { api.GroupName });
    opts.DocInclusionPredicate((_, _) => true);

    opts.DescribeAllParametersInCamelCase();

    opts.OrderActionsBy(api => api.RelativePath);
    opts.UseInlineDefinitionsForEnums();

    opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    opts.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] { }
        }
    });

    opts.AddServer(new OpenApiServer { Url = "https://codefirst.iut.uca.fr/containers/AthletiX-ath-api" });
    opts.AddServer(new OpenApiServer { Url = "http://localhost:5020" });
    opts.AddServer(new OpenApiServer { Url = "https://localhost:7028" });
});

builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.ReportApiVersions = true;
    o.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"));
});
#endregion

var app = builder.Build();

#region Use Swagger
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/
app.UseSwagger();// NE PAS LAISSER EN PROD
app.UseSwaggerUI();// NE PAS LAISSER EN PROD
#endregion

//app.UseHttpsRedirection(); //Casser avec Codefirst

app.UseAuthorization();
app.MapIdentityApi<AppUser>();
app.MapControllers();

app.Run();
