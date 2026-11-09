using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;
using MyWebApp.Models;
using Radzen;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRadzenComponents();

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=app.db";

builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
});

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorPages();

app.MapRazorComponents<MyWebApp.Components.App>()
    .AddInteractiveServerRenderMode();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    // Bảng hồ sơ riêng, bổ sung an toàn vào app.db mà không đụng các bảng Identity hiện có.
    await db.Database.ExecuteSqlRawAsync("""
        CREATE TABLE IF NOT EXISTS "UserProfiles" (
            "UserId" TEXT NOT NULL PRIMARY KEY,
            "FullName" TEXT NULL,
            "DateOfBirth" TEXT NULL,
            "Gender" TEXT NULL,
            CONSTRAINT "FK_UserProfiles_AspNetUsers_UserId"
                FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
        );
        """);
    //Tạo các vai trò nếu chưa tồn tại vai trò đó
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = { "Admin", "CanBo", "HocVien" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Không thể tạo vai trò {role}: " +
                    string.Join(" | ", roleResult.Errors.Select(x => x.Description)));
            }
        }
    }

    //Kiểm tra xem có đang ở Development Mode hay không , nếu có thì kiểm tra xem tài khoản admin tồn tại chưa , nếu chưa thtạo tài khoản admin mẫu
    if (app.Environment.IsDevelopment())
    {
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        const string adminName = "admin@qlrl.local";
        const string adminEmail = adminName;
        const string adminPassword = "Admin123!";

        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin is null)
        {
            admin = new IdentityUser
            {
                UserName = adminName,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(admin, adminPassword);
            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Không thể tạo tài khoản admin mẫu: " +
                    string.Join(" | ", createResult.Errors.Select(x => x.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(admin, "Admin"))
        {
            var assignResult = await userManager.AddToRoleAsync(admin, "Admin");
            if (!assignResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Không thể gán vai trò Admin: " +
                    string.Join(" | ", assignResult.Errors.Select(x => x.Description)));
            }
        }

        if (!await db.UserProfiles.AnyAsync(profile => profile.UserId == admin.Id))
        {
            db.UserProfiles.Add(new UserProfile
            {
                UserId = admin.Id,
                FullName = "Quản trị viên"
            });
            await db.SaveChangesAsync();
        }
    }
   
}

app.Run();
