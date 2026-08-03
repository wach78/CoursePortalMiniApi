using System.Globalization;
using System.Runtime.ConstrainedExecution;
using CoursePortalMiniApi.Data;
using CoursePortalMiniApi.DTOs;
using CoursePortalMiniApi.Migrations;
using CoursePortalMiniApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Development-only CORS policy; restrict allowed origins before deploying to production.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' is missing.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddValidation();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider
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
}

app.UseHttpsRedirection();

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
    var normalizedSortBy = sortBy?.Trim().ToLowerInvariant();
    var normalizedDirection = direction?.Trim().ToLowerInvariant();

    if (normalizedSortBy is not null &&
        normalizedSortBy is not ("price" or "level"))
    {
        return Results.BadRequest(
            "SortBy must be 'price' or 'level'.");
    }

    if (normalizedDirection is not null &&
        normalizedDirection is not ("asc" or "desc"))
    {
        return Results.BadRequest(
            "Direction must be 'asc' or 'desc'.");
    }

    var query = db.Courses.AsNoTracking();

    query = (normalizedSortBy, normalizedDirection) switch
    {
        ("price", "desc") => query.OrderByDescending(course => course.Price),
        ("price", _) => query.OrderBy(course => course.Price),

        ("level", "desc") => query.OrderByDescending(course => course.Level),
        ("level", _) => query.OrderBy(course => course.Level),

        _ => query.OrderBy(course => course.Id)
    };

    var courses = await query
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
    var course = await db.Courses
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
    var courseName = request.Name.Trim();

    var courseExists = await db.Courses.AnyAsync(
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

    var course = new Course
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

    var response = new CourseResponseDto
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
    var course = await db.Courses.FindAsync([id], cancellationToken);

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
    var course = await db.Courses.FindAsync([id], cancellationToken);

    if (course is null)
    {
        return Results.NotFound($"Course with ID {id} not found.");
    }

    db.Courses.Remove(course);
    await db.SaveChangesAsync(cancellationToken);

    return Results.NoContent();
});

app.Run();
