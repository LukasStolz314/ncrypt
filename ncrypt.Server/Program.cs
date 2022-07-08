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


RSACipher rsa = new ();
var keys = rsa.GenerateKeyPair (1024);
var cipherText = rsa.Encrypt (keys.PublicKey, "Hello, World!");
var plain = rsa.Decrypt (keys.PrivateKey, cipherText);

var sign = rsa.Sign (keys.PrivateKey, cipherText, HashAlgorithmName.SHA256);
var verify = rsa.Verify (keys.PublicKey, cipherText, sign, HashAlgorithmName.SHA256);





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
