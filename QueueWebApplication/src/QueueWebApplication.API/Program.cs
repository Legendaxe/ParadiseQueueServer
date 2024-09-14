using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QueueWebApplication.API.ApiHandlers;
using QueueWebApplication.API.AuthorizationHandlers;
using QueueWebApplication.API.Middlewares;
using QueueWebApplication.Core.Db;
using QueueWebApplication.Core.DTOs.Messages;
using QueueWebApplication.Core.Hubs;
using QueueWebApplication.Core.Interfaces.Services;
using QueueWebApplication.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
	options.ForwardedHeaders =
		ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

var services = builder.Services;
services.AddDbContextFactory<ParadiseDbContext>(options =>
{
	var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
	options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidIssuer = builder.Configuration["Jwt:Issuer"],
		ValidAudience = builder.Configuration["Jwt:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey
			(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = false,
		ValidateIssuerSigningKey = true
	};
	options.MapInboundClaims = false;
	options.SaveToken = true;
} );
services.AddAuthorizationBuilder()
	.AddPolicy("IpWhitelistPolicy", policy => policy.Requirements.Add(new IpCheckRequirement { IpClaimRequired = true }));

services.AddLogging();
services.AddEndpointsApiExplorer();
services.AddDistributedMemoryCache();
services.AddSignalR().AddJsonProtocol(options =>
{
	options.PayloadSerializerOptions.Converters.Add(new IpAddressConverter());
});

services.AddSingleton<IAuthorizationHandler, IpCheckHandler>();
services.AddSingleton<IPlayersDbService, PlayersDbService>();
services.AddSingleton<IIpPassService, IpPassService>();
services.AddSingleton<IServerManagerService, ServerManagerService>();
services.AddSingleton<IQueueService, QueueService>();
services.AddSingleton<IPlayersDictionariesService, PlayersDictionariesService>();
services.AddSingleton<ITopicClientFactory, TopicClientFactory>();
services.AddSingleton<IByondApiService, ByondApiService>();
services.AddSingleton<IAuthorizationHandler, IpCheckHandler>();
services.AddHostedService<QueueBackgroundService>();
services.AddHostedService<FetchPlayersBackgroundService>();


var app = builder.Build();

app.UseAuthentication();
app.UseMiddleware<IpSafeListMiddleware>();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseForwardedHeaders();

// if (app.Environment.IsDevelopment())
// {
//      app.UseSwagger();
//      app.UseSwaggerUI();
// }

app.MapHub<IpTablesControllersHub>("/iptables-daemon");//.RequireAuthorization("IpWhitelistPolicy");

var apiGroup = app.MapGroup("/api");
var queueGroup = app.MapGroup("/queue");

apiGroup.MapGet("/five", () => 5);
queueGroup.MapPost("/add-client", ClientsQueue.AddClient).RequireAuthorization();
queueGroup.MapPost("/remove-client", ClientsQueue.RemoveClient).RequireAuthorization();
apiGroup.MapPost("/lobby-connect", AuthHandlers.LobbyConnect).RequireAuthorization("IpWhitelistPolicy");

app.Run();

