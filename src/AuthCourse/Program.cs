// Program.cs

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ── Services ────────────────────────────────────────────────────────────────
builder.Services.AddOpenApi();

// ── Pipeline ────────────────────────────────────────────────────────────────
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.Run();