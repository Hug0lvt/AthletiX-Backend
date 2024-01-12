using API.Services;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Repositories;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using API.Services.Notification;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration.AddJsonFile("appsettings.json");
var configuration = builder.Configuration;

//Database
builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

//Logging
builder.Services.AddLogging(builder => builder.AddConsole());

//Injection de dependance
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<ConversationService>();
builder.Services.AddScoped<ExerciseService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<SetService>();

//Notifications
builder.Services.AddScoped<FCMService>();
FirebaseApp.Create(new AppOptions()//TODO ggkey.json à UPDATE (pas le bon)
{
    Credential = GoogleCredential.FromFile("ggkey.json"),
});

//API And Swagger
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
