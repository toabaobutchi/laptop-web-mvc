using MyLaptopWebsite.Models;
using MyLaptopWebsite.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.Use(async (context, next) =>
{
    var area = context.GetRouteValue("area");
    if (area != null && (string)area == "Admin")
    {
        var action = context.GetRouteValue("action");
        if (action != null && (string)action != "LoginAdmin")
        {
            var admin = context.Session.Get<NguoiDung>("admin");
            if (admin == null) // chưa đăng nhập
            {
                context.Response.Redirect("/Admin/HomePage/LoginAdmin");
            }
            else
            {
                await next(); // nếu đã có session, chắc chắn đã đăng nhập rồi
            }
        }
        else
        {
            await next(); // tiếp tục ở trang đăng nhập
        }
    }
    else
    {
        await next(); // trường hợp không truy cập vào area `Admin`
    }

});

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=HomePage}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
