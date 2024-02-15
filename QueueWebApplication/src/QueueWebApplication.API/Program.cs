using QueueWebApplication.API.Middlewares;
using QueueWebApplication.Core.Hubs;
using QueueWebApplication.Core.Interfaces.Services;
using QueueWebApplication.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSignalR();
builder.Services.AddHostedService<QueueService>();
builder.Services.AddScoped<IByondApiService, ByondApiService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseMiddleware<IpSafeListMiddleware>(builder.Configuration.GetValue<string>("IpSafeList"));

app.UseHttpsRedirection();

app.MapHub<IpTablesControllersHub>("/iptables-daemon");
app.Run();
