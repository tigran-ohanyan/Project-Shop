using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Shop;

public static class AuthEndpoints
{
    public static void SeedAdmin(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ShopDb>();
        if (db.Users.Any()) return;
        var password = app.Configuration["AdminPassword"]
                       ?? throw new InvalidOperationException("Задайте AdminPassword через dotnet user-secrets");
        var admin = new User { Login = "admin", Role = "Admin" };
        admin.PasswordHash = new PasswordHasher<User>().HashPassword(admin, password);
        db.Users.Add(admin);
        db.SaveChanges();
    }

    public static void MapAuth(this WebApplication app)
    {
        app.MapPost("/login", async ([FromForm] string login, [FromForm] string password, [FromForm] string? returnUrl,
            ShopDb db, HttpContext http) =>
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Login == login);
            if (user is null || new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, password)
                == PasswordVerificationResult.Failed)
                return Results.Redirect("/login?error=true&returnUrl=" + Uri.EscapeDataString(returnUrl ?? ""));
            var identity = new ClaimsIdentity(
                [new Claim(ClaimTypes.Name, user.Login), new Claim(ClaimTypes.Role, user.Role)],
                CookieAuthenticationDefaults.AuthenticationScheme);
            await http.SignInAsync(new ClaimsPrincipal(identity));
            return Results.LocalRedirect(IsLocal(returnUrl) ? returnUrl! : "/");
        });
        app.MapPost("/logout", async (HttpContext http) =>
        {
            await http.SignOutAsync();
            return Results.Redirect("/");
        });
    }

    static bool IsLocal(string? url) =>
        !string.IsNullOrEmpty(url) && url[0] == '/' && (url.Length == 1 || (url[1] != '/' && url[1] != '\\'));
}