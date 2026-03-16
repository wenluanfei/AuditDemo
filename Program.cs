using AuditDemo.Data;
using AuditDemo.Models;
using AuditDemo.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------
// SERVICE REGISTRATION
// This section registers all the services the app needs.
// ASP.NET Core uses Dependency Injection (DI) - instead of
// creating objects manually with 'new', we register them here
// and the framework injects them wherever they're needed.
// ---------------------------------------------------------------

// Register Razor Pages (used for the server-side login form handler)
builder.Services.AddRazorPages();

// Register Blazor with Interactive Server rendering
// InteractiveServer means components run on the server and communicate
// with the browser via SignalR (WebSocket), enabling real-time interactivity
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register EF Core with SQLite
// The connection string tells EF Core to use a local file called audit.db
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=audit.db"));

// Register ASP.NET Core Identity for authentication
// Identity handles password hashing, user storage, sign-in/out etc.
// We relax password rules here to make demo registration easier
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>() // Store users in our SQLite database
.AddDefaultTokenProviders();             // Enable password reset tokens etc.

// Allow access to HttpContext in Blazor components (needed for antiforgery)
builder.Services.AddHttpContextAccessor();

// Register our custom services as Scoped
// Scoped = one instance per HTTP request (appropriate for DB operations)
builder.Services.AddScoped<ChecklistService>();
builder.Services.AddScoped<PdfService>();

// Register the OpenAI client as Singleton
// Singleton = one instance for the entire app lifetime (appropriate for API clients)
var apiKey = builder.Configuration["OpenAI:ApiKey"] ?? "";
builder.Services.AddSingleton(new OpenAIClient(apiKey));

// ---------------------------------------------------------------
// BUILD THE APP
// ---------------------------------------------------------------
var app = builder.Build();

// Run database migrations automatically on startup
// This ensures the database schema is always up to date
// without needing to run 'dotnet ef database update' manually
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ---------------------------------------------------------------
// MIDDLEWARE PIPELINE
// Middleware runs in order for every HTTP request.
// Order matters - authentication must come before authorization.
// ---------------------------------------------------------------

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // Force HTTPS in production
}

app.UseHttpsRedirection();  // Redirect HTTP to HTTPS
app.UseAuthentication();    // Identify who the user is (reads the auth cookie)
app.UseAuthorization();     // Check if the user is allowed to access the resource
app.UseAntiforgery();       // Protect forms from CSRF attacks

// Map routes
app.MapRazorPages();        // Handles /LoginAction
app.MapStaticAssets();      // Serves files from wwwroot/

// Map all Blazor components and enable InteractiveServer mode
app.MapRazorComponents<AuditDemo.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();
