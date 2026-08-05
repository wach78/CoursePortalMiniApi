using System.Globalization;
using System.Runtime.ConstrainedExecution;
using CoursePortalMiniApi.Data;
using CoursePortalMiniApi.DTOs;
using CoursePortalMiniApi.Migrations;
using CoursePortalMiniApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Development-only CORS policy; restrict allowed origins before deploying to production.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

string connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' is missing.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddValidation();

WebApplication app = builder.Build();

await using (AsyncServiceScope scope = app.Services.CreateAsyncScope())
{
    AppDbContext dbContext = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    await dbContext.Database.MigrateAsync();
    await SeedData.InitializeAsync(dbContext);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "Course Portal API v1");
    });

    app.UseHsts();
}

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        context.Response.Headers.XContentTypeOptions = "nosniff";

        return Task.CompletedTask;
    });

    await next();
});

app.UseCors();

// Endpoints

// READ ALL (GET /api/courses)
app.MapGet("/api/courses", async (
    AppDbContext db,
    CancellationToken cancellationToken,
    string? sortBy,
    string? direction
    ) =>
{
    string? normalizedSortBy = sortBy?.Trim().ToLowerInvariant();
    string? normalizedDirection = direction?.Trim().ToLowerInvariant();

    if (normalizedSortBy is not null &&
        normalizedSortBy is not ("price" or "level" or "name"))
    {
        return Results.BadRequest(
            "SortBy must be 'price' or 'level' or name.");
    }

    if (normalizedDirection is not null &&
        normalizedDirection is not ("asc" or "desc"))
    {
        return Results.BadRequest(
            "Direction must be 'asc' or 'desc'.");
    }

    IQueryable<Course> query = db.Courses.AsNoTracking();

    query = (normalizedSortBy, normalizedDirection) switch
    {
        ("price", "desc") => query.OrderByDescending(course => course.Price),
        ("price", _) => query.OrderBy(course => course.Price),

        ("level", "desc") => query.OrderByDescending(course => course.Level),
        ("level", _) => query.OrderBy(course => course.Level),

        ("name", "desc") => query.OrderByDescending(course => course.Name),
        ("name", _) => query.OrderBy(course => course.Name),

        _ => query.OrderBy(course => course.Id)
    };

    List<CourseResponseDto> courses = await query
        .Select(course => new CourseResponseDto
        {
            Id = course.Id,
            Name = course.Name,
            Description = course.Description,
            StartDate = course.StartDate,
            DurationInWeeks = course.DurationInWeeks,
            Price = course.Price,
            Level = course.Level
        })
        .ToListAsync(cancellationToken);

    return Results.Ok(courses);
});

// READ ONE (GET /api/courses/{id})
app.MapGet("/api/courses/{id:int}", async ([FromRoute] int id, AppDbContext db, CancellationToken cancellationToken) =>
{
    CourseResponseDto? course = await db.Courses
       .AsNoTracking()
       .Where(course => course.Id == id)
       .Select(course => new CourseResponseDto
       {
           Id = course.Id,
           Name = course.Name,
           Description = course.Description,
           StartDate = course.StartDate,
           DurationInWeeks = course.DurationInWeeks,
           Price = course.Price,
           Level = course.Level
       })
       .FirstOrDefaultAsync(cancellationToken);

    return course is null ? Results.NotFound() : Results.Ok(course);
});

// CREATE (POST /api/courses)
app.MapPost("/api/courses", async (CourseRequestDto request, AppDbContext db, CancellationToken cancellationToken) =>
{
    string courseName = request.Name.Trim();

    bool courseExists = await db.Courses.AnyAsync(
        course =>
            course.Name == courseName &&
            course.StartDate == request.StartDate,
        cancellationToken);

    if (courseExists)
    {
        return Results.Conflict(new
        {
            message = "A course with the same name and start date already exists."
        });
    }

    Course course = new()
    {
        Name = courseName,
        Description = request.Description.Trim(),
        StartDate = request.StartDate,
        DurationInWeeks = request.DurationInWeeks,
        Price = request.Price,
        Level = request.Level
    };

    db.Courses.Add(course);
    await db.SaveChangesAsync(cancellationToken);

    CourseResponseDto response = new()
    {
        Id = course.Id,
        Name = course.Name,
        Description = course.Description,
        StartDate = course.StartDate,
        DurationInWeeks = course.DurationInWeeks,
        Price = course.Price,
        Level = course.Level
    };

    return Results.Created($"/api/courses/{course.Id}", response);
});

// UPDATE (PUT /api/courses/{id})
app.MapPut("/api/courses/{id:int}", async ([FromRoute] int id, CourseRequestDto request, AppDbContext db, CancellationToken cancellationToken) =>
{
    Course? course = await db.Courses.FindAsync([id], cancellationToken);

    if (course is null)
    {
        return Results.NotFound($"Course with ID {id} not found.");
    }

    course.Name = request.Name.Trim();
    course.Description = request.Description.Trim();
    course.StartDate = request.StartDate;
    course.DurationInWeeks = request.DurationInWeeks;
    course.Price = request.Price;
    course.Level = request.Level;

    await db.SaveChangesAsync(cancellationToken);

    return Results.NoContent();
});

// DELETE (DELETE /api/courses/{id}) - Nivå 3
app.MapDelete("/api/courses/{id:int}", async ([FromRoute] int id, AppDbContext db, CancellationToken cancellationToken) =>
{
    Course? course = await db.Courses.FindAsync([id], cancellationToken);

    if (course is null)
    {
        return Results.NotFound($"Course with ID {id} not found.");
    }

    db.Courses.Remove(course);
    await db.SaveChangesAsync(cancellationToken);

    return Results.NoContent();
});

app.Run();
