// Program.cs
using AuthCourse.Extensions;
using AuthCourse.Features.Auth.Login;
using AuthCourse.Features.Auth.Register;
using AuthCourse.Features.Users.DeleteUser;
using AuthCourse.Features.Users.GetAllUsers;
using AuthCourse.Features.Users.GetProfile;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ── Services ────────────────────────────────────────────────────────────────
builder.Services.AddOpenApi();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddMediatRWithBehaviours();
builder.Services.AddJwtAuthentication(builder.Configuration);

// ── Pipeline ────────────────────────────────────────────────────────────────
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapRegisterEndpoint();
app.MapLoginEndpoint();
app.MapGetProfileEndpoint();
app.MapGetAllUsersEndpoint();
app.MapDeleteUserEndpoint();

app.Run();