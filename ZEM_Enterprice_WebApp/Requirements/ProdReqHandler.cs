using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZEM_Enterprice_WebApp.Utilities;

namespace ZEM_Enterprice_WebApp.Requirements
{
    public class ProdReqHandler : AuthorizationHandler<ProdRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProdRequirement requirement)
        {
            if (context.User.IsInRole(DefaultRoles.Admin.ToString()) || context.User.IsInRole(DefaultRoles.Produkcja.ToString()))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
