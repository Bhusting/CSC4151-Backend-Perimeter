using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CSC4151_Backend_Perimeter.Middleware
{
    public class IdentityMiddleware
    { 
        private readonly RequestDelegate _next;

        public IdentityMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var x = context.User.Identity;

            await _next.Invoke(context);
        }

    }
}
