using Application;
using Core.Application.Policies;
using Core.CrossCuttingConcerns.BrowserControl;
using Core.CrossCuttingConcerns.Exceptions.Extensions;
using Core.Security;
using Core.Security.Encryption;
using Core.Security.JWT;
using Core.WebAPI.Extensions.Swagger;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Contexts;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.Threading.RateLimiting;
using WebAPI;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddSecurityServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddRouting(options => { options.LowercaseQueryStrings = true; options.LowercaseUrls = true; });
builder.Services.AddHttpContextAccessor();


if (builder.Environment.IsProduction())
{
    //builder.Services.AddDbContext<BaseDbContext>(options => options.UseInMemoryDatabase("nArchitecture"));
    builder.Services.AddDbContext<BaseDbContext>(options => options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["BaseDb"]));
}
if (builder.Environment.IsDevelopment()){
    builder.Services.AddDbContext<BaseDbContext>(options => options.UseInMemoryDatabase("nArchitecture"));
}

const string tokenOptionsConfigurationSection = "TokenOptions";
TokenOptions tokenOptions =
    builder.Configuration.GetSection(tokenOptionsConfigurationSection).Get<TokenOptions>()
    ?? throw new InvalidOperationException($"\"{tokenOptionsConfigurationSection}\" section cannot found in configuration.");

RateLimeterOptions authRateLimeterOptions = builder.Configuration.GetSection(PolicyNames.AuthFixedRateLimitConfiguration).Get<RateLimeterOptions>() ?? throw new InvalidOperationException($"\"{PolicyNames.GeneralFixedRateLimitConfiguration}\" section cannot found in configuration.");
RateLimeterOptions generalRateLimeterOptions = builder.Configuration.GetSection(PolicyNames.GeneralFixedRateLimitConfiguration).Get<RateLimeterOptions>() ?? throw new InvalidOperationException($"\"{PolicyNames.GeneralFixedRateLimitConfiguration}\" section cannot found in configuration.");


builder.Services.AddRateLimiter(_ =>
{
    _.RejectionStatusCode= StatusCodes.Status429TooManyRequests;
    _.AddPolicy(PolicyNames.AuthFixedPolicyName, options => RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: options.Connection.RemoteIpAddress?.ToString(),
        factory: _ => new FixedWindowRateLimiterOptions
        {
            AutoReplenishment = authRateLimeterOptions.AutoReplenishment,
            PermitLimit = authRateLimeterOptions.PermitLimit,
            QueueLimit = authRateLimeterOptions.QueueLimit,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            Window = TimeSpan.FromSeconds(authRateLimeterOptions.Window)
        }));
    _.AddPolicy(PolicyNames.GeneralFixedPolicyName, options => RateLimitPartition.GetFixedWindowLimiter(
       partitionKey: options.Connection.RemoteIpAddress?.ToString(),
       factory: _ => new FixedWindowRateLimiterOptions
       {
           AutoReplenishment = generalRateLimeterOptions.AutoReplenishment,
           PermitLimit = generalRateLimeterOptions.PermitLimit,
           QueueLimit = generalRateLimeterOptions.QueueLimit,
           QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
           Window = TimeSpan.FromSeconds(generalRateLimeterOptions.Window)
       }));
});



//builder.Services.AddRateLimiter(_ => {
//    _
//    .AddFixedWindowLimiter(policyName: PolicyNames.AuthFixedPolicyName, options =>
//    {
//        options.PermitLimit = authRateLimeterOptions.PermitLimit;
//        options.Window = TimeSpan.FromSeconds(authRateLimeterOptions.Window);
//        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
//        options.QueueLimit = authRateLimeterOptions.QueueLimit;

//    });
//    _
//   .AddFixedWindowLimiter(policyName: PolicyNames.GeneralFixedPolicyName, options =>
//   {
//       options.PermitLimit = generalRateLimeterOptions.PermitLimit;
//       options.Window = TimeSpan.FromSeconds(generalRateLimeterOptions.Window);
//       options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
//       options.QueueLimit = generalRateLimeterOptions.QueueLimit;
//   });
//    _.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

//});

//builder.Services.AddRateLimiter(_ => _
//    .AddSlidingWindowLimiter(policyName: "Sliding", options =>
//    {
//        options.PermitLimit = rateLimeterOptions.PermitLimit;
//        options.Window = TimeSpan.FromSeconds(rateLimeterOptions.Window);
//        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
//        options.QueueLimit = rateLimeterOptions.QueueLimit;
//        options.SegmentsPerWindow= rateLimeterOptions.SegmentsPerWindow;

//    }));

//builder.Services.AddRateLimiter(_ => {
//    _
//       .AddTokenBucketLimiter(policyName: "Token", options =>
//       {
//           options.TokenLimit = rateLimeterOptions.TokenLimit;
//           options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
//           options.QueueLimit = rateLimeterOptions.QueueLimit;
//           options.ReplenishmentPeriod = TimeSpan.FromSeconds(rateLimeterOptions.ReplenishmentPeriod);
//           options.TokensPerPeriod = rateLimeterOptions.TokensPerPeriod;
//           options.AutoReplenishment = rateLimeterOptions.AutoReplenishment;
//       });_.RejectionStatusCode = StatusCodes.Status429TooManyRequests; } );

builder.Services
    .AddAuthentication(o =>
    {
        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
        };
    });
    //.AddGoogle(googleOptions =>
    //{
    //    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? throw new InvalidOperationException($"Authentication section cannot found in configuration.");
    //    ;
    //    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? throw new InvalidOperationException($"Authentication section cannot found in configuration.");
    //    ;
    //});
    

builder.Services.AddDistributedMemoryCache(); // InMemory
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(tokenOptions.AccessTokenExpiration+tokenOptions.RefreshTokenTTL);
    options.IOTimeout = TimeSpan.FromMinutes(tokenOptions.AccessTokenExpiration + tokenOptions.RefreshTokenTTL);
    
});
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
// builder.Services.AddStackExchangeRedisCache(opt => opt.Configuration = "localhost:6379"); // Redis

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(
    opt =>
        opt.AddDefaultPolicy(p =>
        {
            p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        })
);
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "1.5",
        Title = "FallowDeer",
        Description = "Bu API Kodkop Teknoloji Ekibi Tarafýndan Geliþtirilmektedir.",
        TermsOfService = new Uri("https://erkin-akdeniz.gitbook.io/fallowdeer/"),
        Contact = new OpenApiContact
        {
            Name = "kodkop.com",
            Url = new Uri("https://kodkop.com/")
        },
        License = new OpenApiLicense
        {
            Name = "Github",
            Url = new Uri("https://example.com/license")
        }

    });
    opt.AddSecurityDefinition(
        name: "Bearer",
        securityScheme: new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                "JWT Authorization header using the Bearer scheme. Example: \"Authorization:YOUR_TOKEN\". \r\n\r\n"
                + "`Enter your only token in the text input below.`"
        }
    );
    opt.OperationFilter<BearerSecurityRequirementOperationFilter>();
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    opt.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});
WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.DocExpansion(DocExpansion.None);
        
    });
}



if (app.Environment.IsProduction())
{
    app.UseBrowserControl();
    app.ConfigureCustomExceptionMiddleware();
    app.UseRateLimiter();
    app.UseHsts();
   
}
    
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

const string webApiConfigurationSection = "WebAPIConfiguration";
WebApiConfiguration webApiConfiguration =
    app.Configuration.GetSection(webApiConfigurationSection).Get<WebApiConfiguration>()
    ?? throw new InvalidOperationException($"\"{webApiConfigurationSection}\" section cannot found in configuration.");
app.UseCors(opt => opt.WithOrigins(webApiConfiguration.AllowedOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
app.UseSession();
app.UseStaticFiles();
app.Run();
