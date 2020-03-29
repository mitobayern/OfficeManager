#pragma checksum "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Areas\Administration\Views\TemperatureMeters\All.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "65ede0fa7ca97e87c27743de9c1ecf9a7d49ba79"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Administration_Views_TemperatureMeters_All), @"mvc.1.0.view", @"/Areas/Administration/Views/TemperatureMeters/All.cshtml")]
namespace AspNetCore
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
#line 1 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Areas\Administration\Views\_ViewImports.cshtml"
using OfficeManager;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Areas\Administration\Views\_ViewImports.cshtml"
using OfficeManager.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Areas\Administration\Views\TemperatureMeters\All.cshtml"
using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"65ede0fa7ca97e87c27743de9c1ecf9a7d49ba79", @"/Areas/Administration/Views/TemperatureMeters/All.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cdf4cebb9c1002223be927ab439ae66385b1cd0e", @"/Areas/Administration/Views/_ViewImports.cshtml")]
    public class Areas_Administration_Views_TemperatureMeters_All : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<AllTemperatureMetersViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 4 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Areas\Administration\Views\TemperatureMeters\All.cshtml"
  
    ViewData["Title"] = "All Temperature Meters";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<h1 class=""display-4"" style=""margin: 2%; text-align: center;"">All Temperature Meters</h1>
<div style=""padding: 5%;"">
    <table class=""table table-striped"">
        <thead class=""thead-dark"">
            <tr>
                <th scope=""col"">Temperature Meter Number</th>
                <th scope=""col"" colspan=""2"">Office Number</th>
            </tr>
        </thead>
        <tbody>
");
#nullable restore
#line 18 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Areas\Administration\Views\TemperatureMeters\All.cshtml"
             foreach (var temperatureMeter in Model.TemperatureMeters.OrderBy(x => x.Name))
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <tr>\r\n                    <td>");
#nullable restore
#line 21 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Areas\Administration\Views\TemperatureMeters\All.cshtml"
                   Write(temperatureMeter.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 22 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Areas\Administration\Views\TemperatureMeters\All.cshtml"
                   Write(temperatureMeter.OfficeNumber);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>\r\n                        <div class=\"button-holder text-right\">\r\n                            <a");
            BeginWriteAttribute("href", " href=\"", 931, "\"", 1000, 2);
            WriteAttributeValue("", 938, "/Administration/TemperatureMeters/Edit?id=", 938, 42, true);
#nullable restore
#line 25 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Areas\Administration\Views\TemperatureMeters\All.cshtml"
WriteAttributeValue("", 980, temperatureMeter.Id, 980, 20, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"btn btn-primary\">Edit</a>\r\n                        </div>\r\n                    </td>\r\n                </tr>\r\n");
#nullable restore
#line 29 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Areas\Administration\Views\TemperatureMeters\All.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </tbody>\r\n    </table>\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<AllTemperatureMetersViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
