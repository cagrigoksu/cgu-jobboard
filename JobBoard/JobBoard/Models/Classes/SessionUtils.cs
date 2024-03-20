using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Models.Classes
{
    public class SessionUtils: Controller
    {
        public bool EmptySession()
        {
            var accessor = new HttpContextAccessor();

            return accessor.HttpContext.Session.Keys.ToList().Count == 0;
        }
    }
}
