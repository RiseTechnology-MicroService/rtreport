using rtreport.Database;
using rtreport.HostedServices;
using rtreport.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("Database"));

// Add services to the container.
builder.Services.AddScoped<ReportSupplierService>();
builder.Services.AddTransient<ReportService>();
builder.Services.AddSingleton<ReportRequestService>();

builder.Services.AddHostedService<ReportPreparationHostedService>();

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Services.GetService<ReportRequestService>()!.ListenTheQueue();

app.UseAuthorization();

app.MapControllers();

app.Run();
