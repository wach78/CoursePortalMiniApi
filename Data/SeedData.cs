using CoursePortalMiniApi.Enums;
using CoursePortalMiniApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CoursePortalMiniApi.Data;

public static class SeedData
{
    public static async Task InitializeAsync(
        AppDbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        if (await dbContext.Courses.AnyAsync(cancellationToken))
        {
            return;
        }

        Course[] courses =
[
    new Course
    {
        Name = "Grundläggande C#",
        Description = "En introduktionskurs i C# med variabler, villkor, loopar, metoder och grundläggande objektorienterad programmering.",
        StartDate = new DateOnly(2026, 9, 1),
        DurationInWeeks = 8,
        Price = 4995,
        Level = CourseLevel.Beginner,
        CreatedAt = new DateTime(2026, 1, 15, 10, 0, 0, DateTimeKind.Utc)
    },
    new Course
    {
        Name = "Objektorienterad programmering",
        Description = "Lär dig arbeta med klasser, objekt, inkapsling, arv, polymorfism och interface i C#.",
        StartDate = new DateOnly(2026, 9, 15),
        DurationInWeeks = 8,
        Price = 5495,
        Level = CourseLevel.Beginner,
        CreatedAt = new DateTime(2026, 2, 15, 10, 0, 0, DateTimeKind.Utc)
    },
    new Course
    {
        Name = "Databaser och SQL",
        Description = "En praktisk kurs i relationsdatabaser, SQL-frågor, tabeller, relationer, index och normalisering.",
        StartDate = new DateOnly(2026, 10, 1),
        DurationInWeeks = 6,
        Price = 4995,
        Level = CourseLevel.Beginner,
        CreatedAt = new DateTime(2026, 3, 15, 10, 0, 0, DateTimeKind.Utc)
    },
    new Course
    {
        Name = "Webbutveckling med HTML och CSS",
        Description = "Lär dig bygga responsiva och tillgängliga webbsidor med semantisk HTML, CSS och modern layout.",
        StartDate = new DateOnly(2026, 10, 12),
        DurationInWeeks = 6,
        Price = 3995,
        Level = CourseLevel.Beginner,
        CreatedAt = new DateTime(2026, 4, 15, 10, 0, 0, DateTimeKind.Utc)
    },
    new Course
    {
        Name = "JavaScript och TypeScript",
        Description = "Utveckla interaktiva webbapplikationer med JavaScript, TypeScript, moduler, DOM-hantering och fetch.",
        StartDate = new DateOnly(2026, 11, 2),
        DurationInWeeks = 8,
        Price = 5995,
        Level = CourseLevel.Intermediate,
        CreatedAt = new DateTime(2026, 5, 15, 10, 0, 0, DateTimeKind.Utc)
    },
    new Course
    {
        Name = "ASP.NET Core Minimal API",
        Description = "Bygg moderna REST-API:er med ASP.NET Core Minimal API, DTO:er, validering och asynkrona endpoints.",
        StartDate = new DateOnly(2026, 11, 16),
        DurationInWeeks = 8,
        Price = 6995,
        Level = CourseLevel.Intermediate,
        CreatedAt = new DateTime(2026, 6, 15, 10, 0, 0, DateTimeKind.Utc)
    },
    new Course
    {
        Name = "Entity Framework Core",
        Description = "Lär dig använda Entity Framework Core för databasåtkomst, relationer, migrationer och LINQ-frågor.",
        StartDate = new DateOnly(2027, 1, 11),
        DurationInWeeks = 7,
        Price = 6495,
        Level = CourseLevel.Intermediate,
        CreatedAt = new DateTime(2026, 7, 15, 10, 0, 0, DateTimeKind.Utc)
    },
    new Course
    {
        Name = "Testning i .NET",
        Description = "En kurs i enhetstester, integrationstester, xUnit, Moq och testbar kod i .NET-applikationer.",
        StartDate = new DateOnly(2027, 2, 1),
        DurationInWeeks = 6,
        Price = 5995,
        Level = CourseLevel.Intermediate,
        CreatedAt = new DateTime(2026, 8, 15, 10, 0, 0, DateTimeKind.Utc)
    },
    new Course
    {
        Name = "Säker webbutveckling",
        Description = "Lär dig identifiera och förebygga vanliga säkerhetsrisker enligt OWASP, exempelvis XSS, SQL-injektion och bristande åtkomstkontroll.",
        StartDate = new DateOnly(2027, 2, 22),
        DurationInWeeks = 8,
        Price = 7995,
        Level = CourseLevel.Advanced,
        CreatedAt = new DateTime(2026, 9, 15, 10, 0, 0, DateTimeKind.Utc)

    },
    new Course
    {
        Name = "Avancerad systemutveckling i .NET",
        Description = "Fördjupa dig i arkitektur, SOLID, prestanda, felhantering, skalbarhet och underhållbara .NET-lösningar.",
        StartDate = new DateOnly(2027, 3, 15),
        DurationInWeeks = 12,
        Price = 9995,
        Level = CourseLevel.Advanced,
        CreatedAt = new DateTime(2026, 10, 15, 10, 0, 0, DateTimeKind.Utc)
    }
];

        await dbContext.Courses.AddRangeAsync(
            courses,
            cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
