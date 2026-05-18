// Program.cs
using AuthCourse.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ── Services ────────────────────────────────────────────────────────────────
builder.Services.AddOpenApi();
builder.Services.AddDatabase(builder.Configuration);   // ← add this

// ── Pipeline ────────────────────────────────────────────────────────────────
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.Run();