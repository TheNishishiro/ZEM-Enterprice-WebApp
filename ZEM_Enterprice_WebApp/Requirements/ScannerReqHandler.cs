using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZEM_Enterprice_WebApp.Utilities;

namespace ZEM_Enterprice_WebApp.Requirements
{
    public class ScannerReqHandler : AuthorizationHandler<ScannerRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScannerRequirement requirement)
        {
            if (context.User.IsInRole(DefaultRoles.Admin.ToString()) || context.User.IsInRole(DefaultRoles.Scanner.ToString()))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
