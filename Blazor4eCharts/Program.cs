using Blazor4eCharts.Data;
using Blazor4Learning;
using Blazor4Learning.Components;
using Blazor4Learning.Services;
using BlazorPro.BlazorSize;
using DataAccessLibrary;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.SignalR;
using MudBlazor.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<UploadPicAndVideoService>();

// Add services to the container.
builder.Services.AddBootstrapBlazor();
builder.Services.AddHttpClient();
builder.Services.AddMudServices();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMediaQueryService();
builder.Services.AddResizeListener();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddScoped<SqlDataAccess>();
builder.Services.AddScoped<KeycloakSqlDataAccess>();

/*
 Blazor Server apps use a SignalR WebSocket to communicate between the client (browser) and server. 
The SignalR WebSocket has a default maximum message size of 32 KB.
A large Editor Value, FileSelect file size, or a PDFViewer Data can exceed the maximum SignalR message size, which will close the connection and abort the current application task.
 */
builder.Services.Configure<HubOptions>(option => option.MaximumReceiveMessageSize = null);
builder.Services.AddServerSideBlazor().AddHubOptions(options => {
    options.MaximumReceiveMessageSize = null; // no limit or use a number
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
