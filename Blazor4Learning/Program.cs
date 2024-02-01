using Blazor4eCharts.Data;
using DataAccessLibrary;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<SqlDataAccess>();
builder.Services.AddScoped<KeycloakSqlDataAccess>();
builder.Services.AddScoped<IdentityData>();
builder.Services.AddScoped<DataByCaseData>();
builder.Services.AddScoped<SystemListData>();
builder.Services.AddScoped<LocationListData>();
builder.Services.AddScoped<StationListData>();
builder.Services.AddScoped<AggregatedData>();
builder.Services.AddScoped<StepByCaseData>();


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

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
