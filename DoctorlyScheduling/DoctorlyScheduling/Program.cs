using DoctorlyScheduling.Interfaces;
using DoctorlyScheduling.Models;
using DoctorlyScheduling.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SchedulingContext>(options =>
{
    var folder = Environment.SpecialFolder.LocalApplicationData;
    var path = Environment.GetFolderPath(folder);
    string DbPath = System.IO.Path.Join(path, "doctorly_assessment.db");

    options.UseSqlite($"Data Source={DbPath}");
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAttendeeService, AttendeeService>();
builder.Services.AddScoped<IEventService,EventService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
