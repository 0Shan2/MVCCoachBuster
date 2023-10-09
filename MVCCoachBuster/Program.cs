using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MVCCoachBuster.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//Vamos añadir nuestra base de datos
builder.Services.AddDbContext<CoachBusterContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CoachBusterContext") ??
    throw new InvalidOperationException("Connection string 'CoachBusterContext' not found.")));

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
    context.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
