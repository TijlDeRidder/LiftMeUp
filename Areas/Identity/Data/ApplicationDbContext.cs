using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LiftMeUp.Areas.Identity.Data;
using LiftMeUp.Models.ViewModels;
using System.Reflection.Emit;

namespace LiftMeUp.Data;

public class ApplicationDbContext : IdentityDbContext<LiftMeUpUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<LiftMeUp.Models.Lift> Lift { get; set; }

    public DbSet<LiftMeUp.Models.Melding> Melding { get; set; }

    public DbSet<LiftMeUp.Models.Station> Station { get; set; } 

    public DbSet<LiftMeUp.Models.Notification> Notification { get; set; }
}
