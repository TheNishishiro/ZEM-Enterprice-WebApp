using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZEM_Enterprice_WebApp.Utilities;

namespace ZEM_Enterprice_WebApp.Requirements
{
    public class CanViewSupplyReqHandler : AuthorizationHandler<CanViewSupplyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanViewSupplyRequirement requirement)
        {
            if (context.User.IsInRole(DefaultRoles.Admin.ToString()) || 
                context.User.IsInRole(DefaultRoles.Magazyn.ToString()) || 
                context.User.IsInRole(DefaultRoles.Office.ToString()) || 
                context.User.IsInRole(DefaultRoles.Tech.ToString()))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
