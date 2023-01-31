using LiftMeUp.Data;
using LiftMeUp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LiftMeUp.MiddleWare
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class GlobalNotifications
    {
        private readonly RequestDelegate _next;
        public static Dictionary<string, List<Notification>> NotificationDictionary = new Dictionary<string, List<Notification>>();
        public GlobalNotifications(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext, ApplicationDbContext context)
        {
            List<Notification> list = new List<Notification>();
            list = context.Notification.ToList();
            list.Reverse();
            if (list.Count > 9)
            {
                list.RemoveRange(9, list.Count - 9);

            }
            NotificationDictionary["list"] = list;
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class GlobalNotificationsExtensions
    {
        public static IApplicationBuilder UseGlobalNotifications(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalNotifications>();
        }
    }
}
