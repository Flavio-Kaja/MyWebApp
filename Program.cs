using FastEndpoints;
using MyWebApp;

var builder = WebApplication.CreateBuilder();
builder.Services.AddFastEndpoints();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRenderViewService, RenderViewService>();
var app = builder.Build();
app.UseFastEndpoints();
app.Run();
