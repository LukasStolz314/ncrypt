using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ncrypt.Library;
using ncrypt.Library.Cipher;
using ncrypt.Library.Code;
using ncrypt.Server.Data;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder (args);

// Add services to the container.
builder.Services.AddRazorPages ();
builder.Services.AddServerSideBlazor ();
builder.Services.AddSingleton<WeatherForecastService> ();

var app = builder.Build ();




AESCipher cipher = new("2222222222222222", CipherMode.CBC, ConvertType.UTF8, ConvertType.HEX);

var result1 = cipher.Encrypt ("Hello, World", "2222222222222222");

AESCipher cipher2 = new("32323232323232323232323232323232", CipherMode.CBC, ConvertType.HEX, ConvertType.ASCII);

var result2 = cipher2.Decrypt (result1.CipherText, "32323232323232323232323232323232");







// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment ())
{
    app.UseExceptionHandler ("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts ();
}

app.UseHttpsRedirection ();

app.UseStaticFiles ();

app.UseRouting ();

app.MapBlazorHub ();
app.MapFallbackToPage ("/_Host");

app.Run ();
