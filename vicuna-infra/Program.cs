using System.Diagnostics;
using AutoMapper;
using Confluent.Kafka;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.OpenApi.Models;
using vicuna_ddd.Domain.Shared.Mapping;
using vicuna_ddd.Domain.Users.Messaging;
using vicuna_ddd.Shared.Provider;
using vicuna_infra.Filters;
using vicuna_infra.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UserDbContext>();
builder.Services.AddTransient<DbInitializer>();

var producerConfig = new ProducerConfig { BootstrapServers = builder.Configuration["Kafka:Bootstrapper"]};
builder.Services.AddSingleton(producerConfig);

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile<DeliveryConfirmationToDtoProfile>();
    mc.AddProfile<DtoToDeliveryConfirmationProfile>();
}, null);
builder.Services.AddSingleton(mapperConfig.CreateMapper());

var consumerConfig = new ExtendedConsumerConfig
{
    Topic = builder.Configuration["Kafka:TopicIn"]!,
    GroupId = builder.Configuration["Kafka:Group"]!,
    BootstrapServers = builder.Configuration["Kafka:Bootstrapper"]!,
    AutoOffsetReset = AutoOffsetReset.Earliest
};
builder.Services.AddSingleton(consumerConfig);

builder.Services.AddSingleton<ReactiveProducerBase>(
    _ => new ReactiveProducerBase(producerConfig.BootstrapServers!));
builder.Services.AddSingleton<IReactiveConsumer>(rcb => new ReactiveConsumerBase(consumerConfig,
    rcb.GetRequiredService<ILogger<ReactiveConsumerBase>>()
));

// builder.Services.AddCors(options => options.AddPolicy("AllowAllOrigins",
//     builder =>
//     {
//         builder.AllowAnyOrigin()
//             .AllowAnyHeader()
//             .AllowAnyMethod()
//             .DisallowCredentials();
//     }));
builder.Services.AddSwaggerGen(c =>
{
    // c.SwaggerDoc("v1", new OpenApiInfo { Title = "Protected API", Version = "v1" });
    // c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    // {
    //     Type = SecuritySchemeType.OAuth2,
    //     Flows = new OpenApiOAuthFlows
    //     {
    //         Implicit = new OpenApiOAuthFlow
    //         {
    //             AuthorizationUrl = new Uri("https://keycloak.host.internal:28443/realms/development/protocol/openid-connect/auth"),
    //             Scopes = new Dictionary<string, string>
    //             {
    //                 { "api1", "Your API 1" }
    //             }
    //         }
    //     }
    // });
    // c.AddSecurityRequirement(new OpenApiSecurityRequirement
    // {
    //     {
    //         new OpenApiSecurityScheme
    //         {
    //             Reference = new OpenApiReference
    //             {
    //                 Type = ReferenceType.SecurityScheme,
    //                 Id = "OIDC"
    //             }
    //         },
    //         new[] { "readAccess", "writeAccess" }
    //     }
    // });

    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vicuna API", Version = "v1" });

        c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri(builder.Configuration["Swagger:AuthUrl"]!),
                    TokenUrl = new Uri(builder.Configuration["Swagger:TokenUrl"]!),
                    Scopes = new Dictionary<string, string>
                    {
                        { "openid", "OpenID Connect scope" }
                    }
                }
            }
        });
        c.OperationFilter<AuthorizeCheckOperationFilter>();
    }
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.ConfigureHttpsDefaults(options =>
        options.ClientCertificateMode = ClientCertificateMode.NoCertificate);
});

// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
// }).AddJwtBearer(options =>
// {
//     options.Authority = "https://keycloak.host.internal:28443/realms/development/";
//     options.Audience = "backend-service";
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateAudience = true,
//     };
// });

builder.Services.AddAuthentication(options =>
    {
        // options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        // options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        // options.DefaultSignInScheme = "cookie";

        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect(options =>
    {
        options.Authority = "https://host.docker.internal:8443/realms/development";
        options.ClientId = builder.Configuration["OpenIdConnect:ClientId"];
        options.ClientSecret = builder.Configuration["OpenIdConnect:ClientSecret"];
        //options.RequireHttpsMetadata = false;
        //options.CallbackPath = builder.Configuration["OpenIdConnect:CallbackPath"];
        //options.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;

        options.ResponseType = "code";
        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true;
        options.Scope.Add("openid");

        options.UsePkce = true;

        // options.TokenValidationParameters = new TokenValidationParameters
        // {
        //     NameClaimType = "preferred_username",
        //     RoleClaimType = "roles"
        // };

        // options.Events = new OpenIdConnectEvents
        // {
        //     OnRedirectToIdentityProvider = context =>
        //     {
        //         context.ProtocolMessage. = CreateCodeChallenge(context.ProtocolMessage.CodeVerifier);
        //         return Task.CompletedTask;
        //     }
        // };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseCors("AllowAll");

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.OAuthClientId("backend-service");
        c.OAuthClientSecret("qQOkEGGd6JzzeDj0wkqjTFzrHdJiWdgz");
        c.OAuthRealm("development");
        c.OAuthAppName("backend-service");
        c.OAuthAdditionalQueryStringParams(new Dictionary<string, string>() { { "nonce", Guid.NewGuid().ToString() } });
        c.OAuthUsePkce();
    });

#pragma warning disable ASP0014
    app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
#pragma warning restore ASP0014

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var initializer = services.GetRequiredService<DbInitializer>();
    initializer.Run();
}

app.UseExceptionHandler("/error");
app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features
        .Get<IExceptionHandlerPathFeature>()?
        .Error;
    var response = new { exception!.Message };
    Debug.Assert(context != null, nameof(context) + " != null");
    await context.Response.WriteAsJsonAsync(response);
}));

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();