﻿using AspNetCoreHero.ToastNotification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MVCCoachBuster.Data;
using MVCCoachBuster.Helpers;
using MVCCoachBuster.Models;

var builder = WebApplication.CreateBuilder(args);

//Vamos añadir nuestra base de datos
builder.Services.AddDbContext<CoachBusterContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CoachBusterContext") ??
    throw new InvalidOperationException("Connection string 'CoachBusterContext' not found.")));


// Add services to the container.
builder.Services.AddControllersWithViews();

//Autorizacion con esquemas especificos
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/AccesoDenegado";
        options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
        options.SlidingExpiration = true;
    });

builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.BottomRight;
});

// Agregamos en instancia de Singleton, ya que solo queremos una instancia estos elementos en nuestros componentes
builder.Services.AddSingleton<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();
builder.Services.AddSingleton<UsuarioFactoria>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Solución temporal, solo usable en modo desarrollo
// Creación de una base de datos en caso de que no exista
using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<CoachBusterContext>();
    //context.Database.EnsureCreated();

    //Llamamos a DBInitializer
    // 1º) Como ya teniamos la bdd creada, tenemo que eliminarla
    // 2º) Herramientas -> Adm,instrador paque NuGets -> Consola : Drop-Database -confirm
    DbInitializer.Initialize(context); 
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();