#pragma checksum "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\Measurements\CreateTemperatureMeasurements.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7450474c9a7e1ded47ed98f350e4254b3896d4b9"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Measurements_CreateTemperatureMeasurements), @"mvc.1.0.view", @"/Views/Measurements/CreateTemperatureMeasurements.cshtml")]
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
#line 1 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\_ViewImports.cshtml"
using OfficeManager;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\_ViewImports.cshtml"
using OfficeManager.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\Measurements\CreateTemperatureMeasurements.cshtml"
using OfficeManager.ViewModels.Measurements;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7450474c9a7e1ded47ed98f350e4254b3896d4b9", @"/Views/Measurements/CreateTemperatureMeasurements.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cdf4cebb9c1002223be927ab439ae66385b1cd0e", @"/Views/_ViewImports.cshtml")]
    public class Views_Measurements_CreateTemperatureMeasurements : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<CreateTemperatureMeasurementsInputViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-control"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-format", "{0:MM/dd/yyyy}", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-horizontal"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.LabelTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\Measurements\CreateTemperatureMeasurements.cshtml"
  
    ViewData["Title"] = "Create Temperature Measurements";

#line default
#line hidden
#nullable disable
            WriteLiteral("<h1>Create Temperature Measurements</h1>\r\n<hr />\r\n<i style=\"color:orangered;\"><b>Last period: ");
#nullable restore
#line 8 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\Measurements\CreateTemperatureMeasurements.cshtml"
                                       Write(Model.LastPeriod);

#line default
#line hidden
#nullable disable
            WriteLiteral("</b></i>\r\n<hr />\r\n\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7450474c9a7e1ded47ed98f350e4254b3896d4b95864", async() => {
                WriteLiteral("\r\n    <div class=\"row\" style=\"margin-bottom:20px\">\r\n        <div class=\"col-3\">\r\n            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("label", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7450474c9a7e1ded47ed98f350e4254b3896d4b96223", async() => {
                    WriteLiteral("Start of Period");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.LabelTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper);
#nullable restore
#line 14 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\Measurements\CreateTemperatureMeasurements.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.StarOfPeriod);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "7450474c9a7e1ded47ed98f350e4254b3896d4b97813", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
#nullable restore
#line 15 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\Measurements\CreateTemperatureMeasurements.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.StarOfPeriod);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.Format = (string)__tagHelperAttribute_1.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n        </div>\r\n        <div class=\"col-3\">\r\n            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("label", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7450474c9a7e1ded47ed98f350e4254b3896d4b99691", async() => {
                    WriteLiteral("End of Period");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.LabelTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper);
#nullable restore
#line 18 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\Measurements\CreateTemperatureMeasurements.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.EndOfPeriod);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "7450474c9a7e1ded47ed98f350e4254b3896d4b911278", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
#nullable restore
#line 19 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\Measurements\CreateTemperatureMeasurements.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.EndOfPeriod);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.Format = (string)__tagHelperAttribute_1.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n        </div>\r\n    </div>\r\n    <table class=\"table table-striped\">\r\n        <tr>\r\n            <th>Temperature Meter Number</th>\r\n            <th>Heating Measurement</th>\r\n            <th>Cooling Measurement</th>\r\n        </tr>\r\n");
#nullable restore
#line 28 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\Measurements\CreateTemperatureMeasurements.cshtml"
         for (var i = 0; i < Model.TemperatureMeters.Count; i++)
        {

#line default
#line hidden
#nullable disable
                WriteLiteral("            <tr>\r\n                <td>\r\n                    <input type=\"text\"");
                BeginWriteAttribute("name", " name=\"", 1166, "\"", 1199, 3);
                WriteAttributeValue("", 1173, "TemperatureMeters[", 1173, 18, true);
#nullable restore
#line 32 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\Measurements\CreateTemperatureMeasurements.cshtml"
WriteAttributeValue("", 1191, i, 1191, 2, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 1193, "].Name", 1193, 6, true);
                EndWriteAttribute();
                BeginWriteAttribute("value", " value=\"", 1200, "\"", 1240, 1);
#nullable restore
#line 32 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\Measurements\CreateTemperatureMeasurements.cshtml"
WriteAttributeValue("", 1208, Model.TemperatureMeters[i].Name, 1208, 32, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" readonly/>\r\n                </td>\r\n                <td>\r\n                    <input type=\"text\"");
                BeginWriteAttribute("name", " name=\"", 1337, "\"", 1384, 3);
                WriteAttributeValue("", 1344, "TemperatureMeters[", 1344, 18, true);
#nullable restore
#line 35 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\Measurements\CreateTemperatureMeasurements.cshtml"
WriteAttributeValue("", 1362, i, 1362, 2, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 1364, "].HeatingMeasurement", 1364, 20, true);
                EndWriteAttribute();
                WriteLiteral(" />\r\n                </td>\r\n                <td>\r\n                    <input type=\"text\"");
                BeginWriteAttribute("name", " name=\"", 1473, "\"", 1520, 3);
                WriteAttributeValue("", 1480, "TemperatureMeters[", 1480, 18, true);
#nullable restore
#line 38 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\Measurements\CreateTemperatureMeasurements.cshtml"
WriteAttributeValue("", 1498, i, 1498, 2, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 1500, "].CoolingMeasurement", 1500, 20, true);
                EndWriteAttribute();
                WriteLiteral(" />\r\n                </td>\r\n            </tr>\r\n");
#nullable restore
#line 41 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\Measurements\CreateTemperatureMeasurements.cshtml"
        }

#line default
#line hidden
#nullable disable
                WriteLiteral("    </table>\r\n    <div class=\"form-group\">\r\n        <div class=\"col-sm-offset-2 col-sm-10\">\r\n            <button type=\"submit\" class=\"btn btn-primary\">Submit</button>\r\n        </div>\r\n    </div>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<CreateTemperatureMeasurementsInputViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
