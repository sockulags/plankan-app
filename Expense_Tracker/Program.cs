using Expense_Tracker.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connString = builder.Configuration
    .GetConnectionString("DefaultConnection");

// Registrera Context-klassen för dependency injection
builder.Services.AddDbContext<ApplicationDbContext>
    (o =>
    {
        o.UseSqlServer(connString);
    });
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt+QHFqVkNrXVNbdV5dVGpAd0N3RGlcdlR1fUUmHVdTRHRcQllhT35VdkVjUHtdcXQ=;Mgo+DSMBPh8sVXJ1S0d+X1RPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9gSXpRcEVlWX9eeXBQTmk=;ORg4AjUWIQA/Gnt2VFhhQlJBfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5XdENjX39ecH1RTmVV;MTg0NTE4MkAzMjMxMmUzMTJlMzMzNUxzV2RLMFRDY3BkRlowbFZwOVJtcGVnc2hibXd1aGRrdlZJT2NWN29rTVk9;MTg0NTE4M0AzMjMxMmUzMTJlMzMzNWZYZ3lIbjFsU29qZS93M0hNbFJ2ZEE5aFV3c05WYmVHWE9rYUxlaDBsM1E9;NRAiBiAaIQQuGjN/V0d+XU9Hc1RDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS31TckdlWXlecHRcQGFbWA==;MTg0NTE4NUAzMjMxMmUzMTJlMzMzNUZ1SXlTc2tSNWR5a2F5ZHIxNFdMenl5NXAzZlZWdm1pN0dLK0FYVEE0N1U9;MTg0NTE4NkAzMjMxMmUzMTJlMzMzNVQyT2NoemVUaTZVMUcrckUzZ3l0eDJma1ZiTmg2T1hXeEZPbStZS3A1Qmc9;Mgo+DSMBMAY9C3t2VFhhQlJBfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5XdENjX39ecH1dRGRV;MTg0NTE4OEAzMjMxMmUzMTJlMzMzNUl2bG4yMWxnNis1WExPaHRtR0JkWHpKdC9BSXVDSjJzaHlRdWE4Vlp2dFE9;MTg0NTE4OUAzMjMxMmUzMTJlMzMzNUN2dmxuSTFvSjdxcUx1eEszQzY4Um5MazBEazV6MDV0K1loTDFnNFF4N2M9;MTg0NTE5MEAzMjMxMmUzMTJlMzMzNUZ1SXlTc2tSNWR5a2F5ZHIxNFdMenl5NXAzZlZWdm1pN0dLK0FYVEE0N1U9");
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
