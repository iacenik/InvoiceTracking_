using BusinessLayer.Services;
using DataAccessLayer.Concrete;
using DataAccessLayer.Data;
using DataAccessLayer.Repository;
using FluentAssertions.Common;
using Microsoft.EntityFrameworkCore;
using static DataAccessLayer.Repository.IGenericRepository;
using Microsoft.AspNetCore.Identity;
using EntityLayer.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Şifre gereksinimlerini basitleştir
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
    
    // SignIn ayarları
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Repository Register
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICashRegisterRepository, CashRegisterRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IExpenseCategoryRepository, ExpenseCategoryRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceDetailRepository, InvoiceDetailRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentDetailRepository, PaymentDetailRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


// Service Register
builder.Services.AddScoped<ICashRegisterService, CashRegisterService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IExpenseCategoryService, ExpenseCategoryService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IInvoiceDetailService, InvoiceDetailService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentDetailService, PaymentDetailService>();


// Add services to the container.

builder.Services.AddHttpClient();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

// Admin kullanıcısını oluştur
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var logger = services.GetRequiredService<ILogger<Program>>();

        // Admin rolünü oluştur
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole("Admin"));
            if (roleResult.Succeeded)
            {
                logger.LogInformation("Admin rolü başarıyla oluşturuldu.");
            }
            else
            {
                logger.LogError("Admin rolü oluşturulurken hata: {Errors}", 
                    string.Join(", ", roleResult.Errors.Select(e => e.Description)));
            }
        }

        // Admin kullanıcısını oluştur
        var adminUser = await userManager.FindByEmailAsync("ismetalpiso@gmail.com");
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = "ismetalpiso@gmail.com",
                Email = "ismetalpiso@gmail.com",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "ismet123");
            if (result.Succeeded)
            {
                logger.LogInformation("Admin kullanıcısı başarıyla oluşturuldu.");
                
                var roleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
                if (roleResult.Succeeded)
                {
                    logger.LogInformation("Admin kullanıcısına Admin rolü başarıyla atandı.");
                }
                else
                {
                    logger.LogError("Admin kullanıcısına rol atanırken hata: {Errors}", 
                        string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                logger.LogError("Admin kullanıcısı oluşturulurken hata: {Errors}", 
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
        else
        {
            logger.LogInformation("Admin kullanıcısı zaten mevcut. Kullanıcı adı: {UserName}", adminUser.UserName);
            
            // Şifreyi sıfırla
            var token = await userManager.GeneratePasswordResetTokenAsync(adminUser);
            var resetResult = await userManager.ResetPasswordAsync(adminUser, token, "ismet123");
            if (resetResult.Succeeded)
            {
                logger.LogInformation("Admin kullanıcısının şifresi başarıyla sıfırlandı.");
            }
            else
            {
                logger.LogError("Admin kullanıcısının şifresi sıfırlanırken hata: {Errors}", 
                    string.Join(", ", resetResult.Errors.Select(e => e.Description)));
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Admin kullanıcısı oluşturulurken hata oluştu.");
    }
}

app.Run();
