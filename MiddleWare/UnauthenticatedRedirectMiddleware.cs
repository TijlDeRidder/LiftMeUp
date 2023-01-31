namespace LiftMeUp.MiddleWare
{
    public class UnauthenticatedRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public UnauthenticatedRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Response.Redirect("/Identity/Account/Login");
                return;
            }

            await _next(context);
        }
    }
}
