﻿using Api.Extensions;
using Application;
using GrpcIntegrated.Services;
using Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(1000, listenOptions =>
    {
        listenOptions.UseHttps();
        listenOptions.Protocols = HttpProtocols.Http1;
    });

    options.ListenAnyIP(1001, listenOptions =>
    {
        listenOptions.UseHttps();
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

builder.Services.AddGrpc();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCorsPolicy();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddInfrastructure().AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(settings => settings.DisplayRequestDuration());
}

app.UseStaticFiles();

app.UseCors("eShopApi");
app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<HelloService>();
app.MapGrpcService<UploaderService>();

app.UseRouting();
app.MapControllers();

await app.RunAsync();
