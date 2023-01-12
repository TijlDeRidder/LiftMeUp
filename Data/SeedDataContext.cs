using LiftMeUp.Areas.Identity.Data;
using LiftMeUp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LiftMeUp.Data
{
    public class SeedDataContext
    {
        public static void Initialize(IApplicationBuilder builder1)
        {
            using (var service = builder1.ApplicationServices.CreateScope())
            {
                var context = service.ServiceProvider.GetService<ApplicationDbContext>();
                var userManager = service.ServiceProvider.GetService<UserManager<LiftMeUpUser>>();
                context.Database.Migrate();
                context.Database.EnsureCreated();

                if (!context.Roles.Any())
                {

                    context.Roles.AddRange(
                        new IdentityRole { Id = "User", Name = "User", NormalizedName = "USER" },
                        new IdentityRole { Id = "Admin", Name = "Admin", NormalizedName = "ADMIN" }
                        );
                    LiftMeUpUser UserTijl = new LiftMeUpUser
                    {
                        Email = "kobe-de-ridder@hotmail.com",
                        EmailConfirmed = true,
                        LockoutEnabled = true,
                        UserName = "kobe-de-ridder@hotmail.com",
                        FirstName = "Kobe",
                        LastName = "De Ridder",

                    };
                    LiftMeUpUser UserAdmin = new LiftMeUpUser
                    {
                        Email = "admin@liftmeup.com",
                        EmailConfirmed = true,
                        LockoutEnabled = false,
                        UserName = "admin@liftmeup.com",
                        FirstName = "Admin",
                        LastName = "Admin"
                    };

                    var result1 = userManager.CreateAsync(UserTijl, "Test!123").Result;
                    if (result1.Succeeded)
                    {
                        userManager.AddToRoleAsync(UserTijl, "User").Wait();
                    }
                    var result2 = userManager.CreateAsync(UserAdmin, "Test!123").Result;
                    if (result2.Succeeded)
                    {
                        userManager.AddToRoleAsync(UserAdmin, "ADMIN").Wait();
                    }
                    Station station1 = new Station
                    {
                        stationName = "Elizabeth",
                        isAccesible = true,
                        hasElevator = true,
                        isDeleted = false
                    };
                    Station station2 = new Station
                    {
                        stationName = "Simonis",
                        isAccesible = false,
                        hasElevator = false,
                        isDeleted = false
                    };
                    Station station3 = new Station
                    {
                        stationName = "Koning Boudewijn",
                        isAccesible = true,
                        hasElevator = true,
                        isDeleted = false
                    };
                    context.Station.AddRange(station1, station2, station3);
                    context.SaveChanges();
                    Lift lift1 = new Lift
                    {
                        name = "Elizabeth 1",
                        stationId = station1.stationId,
                        isWorking = true,
                        isDeleted = false
                    };
                    Lift lift2 = new Lift
                    {
                        name = "Elizabeth 2",
                        stationId = station1.stationId,
                        isWorking = false,
                        isDeleted = false
                    };
                    Lift lift3 = new Lift
                    {
                        name = "Koning Boudewijn 1",
                        stationId = station3.stationId,
                        isWorking = true,
                        isDeleted = false
                    };
                    context.Lift.AddRange(lift1, lift2, lift3);
                    context.SaveChanges();
                    Melding melding1 = new Melding
                    {
                        liftId = lift2.liftId,
                        UserId = UserAdmin.Id,
                        stationId = station1.stationId,
                        startDate = DateTime.Now,
                        isDeleted = false,
                        uitleg = "lift werkt niet meer"
                    };
                    context.Melding.Add(melding1);
                    context.SaveChanges();
                }
                else
                {
                    LiftMeUpUser UserTijl = new LiftMeUpUser
                    {
                        Email = "kobe-de-ridder@hotmail.com",
                        EmailConfirmed = true,
                        LockoutEnabled = true,
                        UserName = "kobe-de-ridder@hotmail.com",
                        FirstName = "Tijl",
                        LastName = "De Ridder",

                    };
                    LiftMeUpUser UserAdmin = new LiftMeUpUser
                    {
                        Email = "admin@liftmeup.com",
                        EmailConfirmed = true,
                        LockoutEnabled = false,
                        UserName = "admin@liftmeup.com",
                        FirstName = "Admin",
                        LastName = "Admin"
                    };

                    var result1 = userManager.CreateAsync(UserTijl).Result;
                    if (result1.Succeeded)
                    {
                        userManager.AddToRoleAsync(UserTijl, "USER").Wait();
                    }
                    var result2 = userManager.CreateAsync(UserAdmin).Result;
                    if (result2.Succeeded)
                    {
                        userManager.AddToRoleAsync(UserAdmin, "ADMIN").Wait();
                    }
                }

  
            }
        }
    }
}
