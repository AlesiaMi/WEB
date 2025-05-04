using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB.Components;
using WEB.Components.Account;
using WEB.Data;
using WEB.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting; 


namespace WEB
{
    public class Program
    {
        public static async Task Main(string[] args)  
        {
            var builder = WebApplication.CreateBuilder(args);

            // Добавьте службу для работы с контроллерами
            builder.Services.AddControllers();

            // Настройка CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", policy =>
                {
                    policy.WithOrigins("https://localhost:7257", "http://localhost:5171")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Lax;
            });

            // Настройка Razor компонентов и Blazor
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityUserAccessor>();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
            builder.Services.AddHttpClient("API", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7257/"); 
            });

            // Настройка Identity
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
                .AddIdentityCookies();

            // Подключение БД
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>() 
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
            builder.Services.AddScoped<TemplateService>();

            var app = builder.Build();

            

            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            // Настройка middleware
            app.UseCors("AllowAllOrigins");
            app.UseAntiforgery();

            // Настройка маршрутов
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.MapAdditionalIdentityEndpoints();
            app.MapControllers();

            // Инициализация ролей (теперь с await)
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    // Создаем стандартные роли
                    string[] roleNames = { "Admin", "User" };
                    foreach (var roleName in roleNames)
                    {
                        if (!await roleManager.RoleExistsAsync(roleName))
                        {
                            await roleManager.CreateAsync(new IdentityRole(roleName));
                        }
                    }

                    // Опционально: создаем тестового админа
                    if (app.Environment.IsDevelopment())
                    {
                        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                        const string adminEmail = "admin@example.com";
                        const string adminPassword = "Admin123!";

                        if (await userManager.FindByEmailAsync(adminEmail) == null)
                        {
                            var adminUser = new ApplicationUser
                            {
                                UserName = adminEmail,
                                Email = adminEmail,
                                EmailConfirmed = true
                            };

                            var result = await userManager.CreateAsync(adminUser, adminPassword);
                            if (result.Succeeded)
                            {
                                await userManager.AddToRoleAsync(adminUser, "Admin");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Ошибка при инициализации ролей");
                }
            }

            await app.RunAsync();  // Используем асинхронный запуск
        }
    }
}