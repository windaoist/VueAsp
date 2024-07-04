using Microsoft.EntityFrameworkCore;
using VueAsp.Server.Data;
using VueAsp.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddCorsPolicy();
builder.Services.AddControllers();
builder.Services.AddControllers();
builder.Services.AddScoped<GetArticleService>();
builder.Services.AddScoped<IGetArticleService, GetArticleService>();
builder.Services.AddScoped<IFormDataService, FormDataService>();
builder.Services.AddScoped<FormDataService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDistributedMemoryCache(); // 使用内存作为 Session 的底层存储
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 设置 Session 超时时间
    options.Cookie.HttpOnly = true; // 设置 Cookie 为 HttpOnly
    options.Cookie.IsEssential = true; // 设置 Cookie 为必要项
});
var app = builder.Build();
//app.UseCorsMiddleware();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseSession();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
