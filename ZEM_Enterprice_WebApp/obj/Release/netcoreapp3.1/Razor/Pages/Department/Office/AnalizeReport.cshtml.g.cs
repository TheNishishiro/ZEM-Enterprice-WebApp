#pragma checksum "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "96c6b2234b0a3e34118f1a328484be1b44aea6da"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(ZEM_Enterprice_WebApp.Pages.Department.Office.Pages_Department_Office_AnalizeReport), @"mvc.1.0.razor-page", @"/Pages/Department/Office/AnalizeReport.cshtml")]
namespace ZEM_Enterprice_WebApp.Pages.Department.Office
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
#nullable restore
#line 2 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
using System.Globalization;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemMetadataAttribute("RouteTemplate", "{day:int},{month:int},{year:int}")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"96c6b2234b0a3e34118f1a328484be1b44aea6da", @"/Pages/Department/Office/AnalizeReport.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bf1f4c0c739ce8e5d7c0354cc5cfc29853d8ad6b", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Department_Office_AnalizeReport : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/assets/customcss/FixedTable.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", "hidden", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("nav-link"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "/Department/Office/AnalizeReport", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-route-Filter_Complete", "true", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-route-Filter_NotComplete", "true", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 4 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
  
    ViewData["Title"] = "Analiza - raport";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "96c6b2234b0a3e34118f1a328484be1b44aea6da7335", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "96c6b2234b0a3e34118f1a328484be1b44aea6da8453", async() => {
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "96c6b2234b0a3e34118f1a328484be1b44aea6da8715", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_2.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
#nullable restore
#line 11 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => Model.ForDateStart);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
    <div class=""text-center"" style=""width:50%; margin:auto"">
        <a class=""btn btn-primary"" data-toggle=""collapse"" aria-expanded=""false"" aria-controls=""collapse-1"" href=""#collapse-1"" role=""button"" id=""collapse-button"" style=""width:50%"">Pokaż opcje filtrowania</a>
        <div class=""collapse"" id=""collapse-1"">
            <ul class=""nav navbar-nav ml-auto"">
                <li role=""presentation"" class=""nav-item"">");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "96c6b2234b0a3e34118f1a328484be1b44aea6da10913", async() => {
                    WriteLiteral("Pokaż komplene");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_4.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
                if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
                {
                    throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-Filter_Complete", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
                }
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["Filter_Complete"] = (string)__tagHelperAttribute_5.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_5);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("</li>\r\n                <li role=\"presentation\" class=\"nav-item\">");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "96c6b2234b0a3e34118f1a328484be1b44aea6da12852", async() => {
                    WriteLiteral("Pokaż niekompletne");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_4.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
                if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
                {
                    throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-Filter_NotComplete", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
                }
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["Filter_NotComplete"] = (string)__tagHelperAttribute_6.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_6);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("</li>\r\n                <li role=\"presentation\" class=\"nav-item\">");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "96c6b2234b0a3e34118f1a328484be1b44aea6da14801", async() => {
                    WriteLiteral("Resetuj");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_4.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"</li>
            </ul>
        </div>
        <button class=""btn btn-primary btn-block"" id=""filter-confirm-button"" type=""submit"" style=""width:50%"">
            Pobierz plik csv
        </button>
    </div>
    <hr>
    <div class=""tableFixHead"">
        <table>
            <thead>
            <th>Data raportu</th>
            </thead>
            <tbody>
                <tr>
                    <td>
                        ");
#nullable restore
#line 34 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                   Write(Model.ForDateStart.ToString("d", CultureInfo.CreateSpecificCulture("de-DE")));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
                    </td>
                </tr>
            </tbody>
        </table>
        <table>
            <thead>
                <tr>
                    <th>Rodzina</th>
                    <th>Wiązka</th>
                    <th>Przewodów na Wiązke</th>
                    <th>Komplet</th>
                    <th>Kod cięty</th>
                    <th>Suma</th>
                    <th>Status</th>
                </tr>
            </thead>
            <tbody>
");
#nullable restore
#line 52 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                 foreach (var record in Model.analizeEntriesFiltered)
                {

#line default
#line hidden
#nullable disable
                WriteLiteral("                <tr>\r\n");
#nullable restore
#line 55 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                     if (record.NextRodzina)
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                        <td>");
#nullable restore
#line 57 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                       Write(record.Rodzina);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n");
#nullable restore
#line 58 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                    }
                    else
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                        <td></td>\r\n");
#nullable restore
#line 62 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 63 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                     if (record.NextWiazka)
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                        <td>");
#nullable restore
#line 65 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                       Write(record.Wiazka);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                        <td>");
#nullable restore
#line 66 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                       Write(record.Komplet);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n");
#nullable restore
#line 67 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                    }
                    else
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                        <td></td>\r\n                        <td></td>\r\n");
#nullable restore
#line 72 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 73 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                     if (record.NewSet)
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                        <td>");
#nullable restore
#line 75 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                       Write(record.NrKompletu);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n");
#nullable restore
#line 76 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                    }
                    else
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                        <td></td>\r\n");
#nullable restore
#line 80 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                    }

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    <td>");
#nullable restore
#line 82 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                   Write(record.KodCiety);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n");
#nullable restore
#line 83 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                     if (record.Suma == 0)
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                        <td style=\"background-color: red\">");
#nullable restore
#line 85 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                                                     Write(record.Suma);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n");
#nullable restore
#line 86 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                    }
                    else
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                        <td>");
#nullable restore
#line 89 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                       Write(record.Suma);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n");
#nullable restore
#line 90 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 91 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                     if (record.Status)
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                        <td style=\"color:rgb(0, 255, 0)\">Komplet</td>\r\n");
#nullable restore
#line 94 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                    }
                    else
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                        <td style=\"color:rgb(255, 0, 0)\">Brak kompletu</td>\r\n");
#nullable restore
#line 98 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                    }

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                </tr>\r\n");
#nullable restore
#line 101 "D:\Programming\C_sharp\ZEM_Enterprice_WebApp_Release\ZEM_Enterprice_WebApp\Pages\Department\Office\AnalizeReport.cshtml"
                }

#line default
#line hidden
#nullable disable
                WriteLiteral("            </tbody>\r\n        </table>\r\n    </div>\r\n\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_7.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ZEM_Enterprice_WebApp.Pages.Department.Office.AnalizeReportModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ZEM_Enterprice_WebApp.Pages.Department.Office.AnalizeReportModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ZEM_Enterprice_WebApp.Pages.Department.Office.AnalizeReportModel>)PageContext?.ViewData;
        public ZEM_Enterprice_WebApp.Pages.Department.Office.AnalizeReportModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
