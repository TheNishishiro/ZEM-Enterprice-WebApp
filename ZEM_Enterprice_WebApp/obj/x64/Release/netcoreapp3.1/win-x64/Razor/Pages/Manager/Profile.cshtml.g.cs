#pragma checksum "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Manager\Profile.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4c6adcff815e80e50d5246d51f9363f1370c0d5c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(ZEM_Enterprice_WebApp.Pages.Manager.Pages_Manager_Profile), @"mvc.1.0.razor-page", @"/Pages/Manager/Profile.cshtml")]
namespace ZEM_Enterprice_WebApp.Pages.Manager
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\_ViewImports.cshtml"
using ZEM_Enterprice_WebApp;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\_ViewImports.cshtml"
using ZEM_Enterprice_WebApp.Data;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4c6adcff815e80e50d5246d51f9363f1370c0d5c", @"/Pages/Manager/Profile.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bf1f4c0c739ce8e5d7c0354cc5cfc29853d8ad6b", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Manager_Profile : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 5 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Manager\Profile.cshtml"
  
    ViewData["Title"] = "Profil";
    Layout = "~/Pages/Shared/_ProfileLayout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"text-center bg-info border rounded\" id=\"information-div\">\r\n    <h5 id=\"infor-heading\">");
#nullable restore
#line 11 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Manager\Profile.cshtml"
                      Write(Model.StatusHeader);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h5>\r\n    <p id=\"information-para\" style=\"color: rgb(255,255,255);font-size: 15px;\">");
#nullable restore
#line 12 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Manager\Profile.cshtml"
                                                                         Write(Model.StatusMessage);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n</div>\r\n<h1>Panel użytkownika</h1>\r\n<p>Witaj ");
#nullable restore
#line 15 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Manager\Profile.cshtml"
    Write(User.Identity.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("!</p>\r\n<p>Tutaj możesz zmienić kilka rzeczy odnośnie Twojego konta, użyj panelu nawigacyjnego po lewej aby przejść do odpowiedniej skecji.</p>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public UserManager<MyUser> UserManager { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public SignInManager<MyUser> SignInManager { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ZEM_Enterprice_WebApp.Pages.Manager.ProfileModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ZEM_Enterprice_WebApp.Pages.Manager.ProfileModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ZEM_Enterprice_WebApp.Pages.Manager.ProfileModel>)PageContext?.ViewData;
        public ZEM_Enterprice_WebApp.Pages.Manager.ProfileModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
