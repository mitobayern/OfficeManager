#pragma checksum "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5a5dcdf2d9d883504e20f4ff71f7ff2ce8db49d2"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_AccountingReports_Details), @"mvc.1.0.view", @"/Views/AccountingReports/Details.cshtml")]
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
#line 1 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
using OfficeManager.ViewModels.AccountingReports;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a5dcdf2d9d883504e20f4ff71f7ff2ce8db49d2", @"/Views/AccountingReports/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cdf4cebb9c1002223be927ab439ae66385b1cd0e", @"/Views/_ViewImports.cshtml")]
    public class Views_AccountingReports_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<AccountingReportViewModel>
    {
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
  
    ViewData["Title"] = "Generate Accounting Report";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<!DOCTYPE html>\r\n<html>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "5a5dcdf2d9d883504e20f4ff71f7ff2ce8db49d23778", async() => {
                WriteLiteral("\r\n    <style>\r\n        table {\r\n            width: 100%;\r\n        }\r\n\r\n        td, th {\r\n            border: 1px solid #dddddd;\r\n            text-align: left;\r\n            padding: 5px;\r\n        }\r\n    </style>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "5a5dcdf2d9d883504e20f4ff71f7ff2ce8db49d24980", async() => {
                WriteLiteral("\r\n    <div class=\"row\">\r\n        <div class=\"col-7\">\r\n            <p><i><b>ПОЛУЧАТЕЛ</b></i></p>\r\n            <p>\r\n                <i>");
#nullable restore
#line 28 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
              Write(Model.Tenant.CompanyName.ToUpper());

#line default
#line hidden
#nullable disable
                WriteLiteral("</i><br>\r\n                ЕИК: <b>");
#nullable restore
#line 29 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                   Write(Model.Tenant.EIK);

#line default
#line hidden
#nullable disable
                WriteLiteral("</b><br>\r\n                AДРЕС: <b>");
#nullable restore
#line 30 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                     Write(Model.Tenant.Address);

#line default
#line hidden
#nullable disable
                WriteLiteral("</b><br>\r\n                ИН по ЗДДС: <b>");
#nullable restore
#line 31 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                          Write(Model.Tenant.Bulstat);

#line default
#line hidden
#nullable disable
                WriteLiteral("</b>\r\n            </p>\r\n        </div>\r\n        <div class=\"col-5\">\r\n            <p><i><b>ДОСТАВЧИК</b></i></p>\r\n            <p>\r\n                <i>");
#nullable restore
#line 37 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
              Write(Model.Landlord.LandlordName.ToUpper());

#line default
#line hidden
#nullable disable
                WriteLiteral("</i><br>\r\n                ЕИК: <b>");
#nullable restore
#line 38 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                   Write(Model.Landlord.Bulstat.Substring(2));

#line default
#line hidden
#nullable disable
                WriteLiteral("</b><br>\r\n                AДРЕС: <b>");
#nullable restore
#line 39 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                     Write(Model.Landlord.Address);

#line default
#line hidden
#nullable disable
                WriteLiteral("</b><br>\r\n                ИН по ЗДДС: <b>");
#nullable restore
#line 40 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                          Write(Model.Landlord.Bulstat);

#line default
#line hidden
#nullable disable
                WriteLiteral("</b>\r\n            </p>\r\n        </div>\r\n    </div>\r\n    <br />\r\n    <h1 class=\"display-4\" style=\"margin: 2%; text-align: center;\">Счетоводна справка</h1>\r\n    <p style=\"font-size:20px; text-align:center;\"><i>№ <b>");
#nullable restore
#line 46 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                                     Write(Model.Number);

#line default
#line hidden
#nullable disable
                WriteLiteral("</b> / <b>");
#nullable restore
#line 46 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                                                            Write(Model.CreatedOn.ToString("d.MM.yyyy"));

#line default
#line hidden
#nullable disable
                WriteLiteral(" </b>г.</i></p>\r\n    <p style=\"font-size:17px; text-align:center; line-height:35px\"><i>Относно режийни разходи на <b>");
#nullable restore
#line 47 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                                                                               Write(Model.Tenant.Offices);

#line default
#line hidden
#nullable disable
                WriteLiteral("</b> в сграда на ул. Осогово №86 (дог. от <b>");
#nullable restore
#line 47 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                                                                                                                                                 Write(Model.Tenant.StartOfContract.ToString("d MMMM yyyy", new System.Globalization.CultureInfo("bg-BG")));

#line default
#line hidden
#nullable disable
                WriteLiteral(" г.</b>)<br /> за периода: <b>");
#nullable restore
#line 47 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                                                                                                                                                                                                                                                                                   Write(Model.Period);

#line default
#line hidden
#nullable disable
                WriteLiteral(@" г.</b></i></p>
    <br />
    <br />
    <br />
    <p><i>Консумация на електрическа енергия</i></p>
    <table>
        <tr>
            <th>Наименование</th>
            <th style=""text-align:center"">Количество</th>
            <th style=""text-align:center"">Мярка</th>
            <th style=""text-align:center"">Ед. Цена <br /> без ДДС</th>
            <th style=""text-align:center"">Стойност в лева</th>
        </tr>
        <tr>
            <td>Консумирана ел. енергия - дневна тарифа</td>
            <td style=""text-align:center"">");
#nullable restore
#line 62 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                     Write(Model.DayTimeElectricityConsummation);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n            <td style=\"text-align:center\">кВтч</td>\r\n            <td style=\"text-align:center\">");
#nullable restore
#line 64 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                     Write(Model.PricesInformation.ElectricityPerKWh);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n            <td style=\"text-align:right\">");
#nullable restore
#line 65 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                     Write((Model.DayTimeElectricityConsummation * Model.PricesInformation.ElectricityPerKWh).ToString("F2"));

#line default
#line hidden
#nullable disable
                WriteLiteral(" лв.</td>\r\n        </tr>\r\n        <tr>\r\n            <td>Консумирана ел. енергия - нощна тарифа</td>\r\n            <td style=\"text-align:center\">");
#nullable restore
#line 69 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                     Write(Model.NightTimeElectricityConsummation);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n            <td style=\"text-align:center\">кВтч</td>\r\n            <td style=\"text-align:center\">");
#nullable restore
#line 71 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                     Write(Model.PricesInformation.ElectricityPerKWh);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n            <td style=\"text-align:right\">");
#nullable restore
#line 72 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                     Write((Model.NightTimeElectricityConsummation * Model.PricesInformation.ElectricityPerKWh).ToString("F2"));

#line default
#line hidden
#nullable disable
                WriteLiteral(" лв.</td>\r\n        </tr>\r\n        <tr>\r\n            <td>Акциз</td>\r\n            <td style=\"text-align:center\">");
#nullable restore
#line 76 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                      Write(Model.DayTimeElectricityConsummation + Model.NightTimeElectricityConsummation);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n            <td style=\"text-align:center\">кВтч</td>\r\n            <td style=\"text-align:center\">");
#nullable restore
#line 78 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                     Write(Model.PricesInformation.Excise);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n            <td style=\"text-align:right\">");
#nullable restore
#line 79 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                     Write(((Model.DayTimeElectricityConsummation + Model.NightTimeElectricityConsummation) * Model.PricesInformation.Excise).ToString("F2"));

#line default
#line hidden
#nullable disable
                WriteLiteral(" лв.</td>\r\n        </tr>\r\n        <tr>\r\n            <td>Мрежови такси и услуги</td>\r\n            <td style=\"text-align:center\">");
#nullable restore
#line 83 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                      Write(Model.DayTimeElectricityConsummation + Model.NightTimeElectricityConsummation);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n            <td style=\"text-align:center\">кВтч</td>\r\n            <td style=\"text-align:center\">");
#nullable restore
#line 85 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                      Write(Model.PricesInformation.AccessToDistributionGrid + Model.PricesInformation.NetworkTaxesAndUtilities);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n            <td style=\"text-align:right\">");
#nullable restore
#line 86 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                     Write(((Model.DayTimeElectricityConsummation + Model.NightTimeElectricityConsummation) * (Model.PricesInformation.AccessToDistributionGrid + Model.PricesInformation.NetworkTaxesAndUtilities)).ToString("F2"));

#line default
#line hidden
#nullable disable
                WriteLiteral(" лв.</td>\r\n        </tr>\r\n    </table>\r\n    <br />\r\n    <p style=\"text-align:right\"><i>Стойност за консумация на електрическа енергия:   <b>");
#nullable restore
#line 90 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                                                                    Write(Model.AmountForElectricity.ToString("F2"));

#line default
#line hidden
#nullable disable
                WriteLiteral(" лв.</b></i></p>\r\n    <br />\r\n");
#nullable restore
#line 92 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
     if (Model.HeatingConsummation > 0)
    {

#line default
#line hidden
#nullable disable
                WriteLiteral(@"        <p><i>Снабдяване с енергия за отопление</i></p>
        <table>
            <tr>
                <th>Наименование</th>
                <th style=""text-align:center"">Количество</th>
                <th style=""text-align:center"">Мярка</th>
                <th style=""text-align:center"">Ед. Цена <br /> без ДДС</th>
                <th style=""text-align:center"">Стойност в лева</th>
            </tr>
            <tr>
                <td>Консумирана енергия за отопление</td>
                <td style=""text-align:center"">");
#nullable restore
#line 105 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                         Write(Model.HeatingConsummation);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                <td style=\"text-align:center\">кВтч</td>\r\n                <td style=\"text-align:center\">");
#nullable restore
#line 107 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                         Write(Model.PricesInformation.HeatingPerKWh);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                <td style=\"text-align:right\">");
#nullable restore
#line 108 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                         Write(Model.AmountForHeating.ToString("F2"));

#line default
#line hidden
#nullable disable
                WriteLiteral(" лв.</td>\r\n            </tr>\r\n        </table>\r\n        <br />\r\n        <p style=\"text-align:right\"><i>Стойност за консумация на енергия за отопление:   <b>");
#nullable restore
#line 112 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                                                                        Write((Model.AmountForHeating).ToString("F2"));

#line default
#line hidden
#nullable disable
                WriteLiteral(" лв.</b></i></p>\r\n        <br />\r\n");
#nullable restore
#line 114 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 115 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
     if (Model.CoolingConsummation > 0)
    {

#line default
#line hidden
#nullable disable
                WriteLiteral(@"        <p><i>Снабдяване с енергия за охлаждане</i></p>
        <table>
            <tr>
                <th>Наименование</th>
                <th style=""text-align:center"">Количество</th>
                <th style=""text-align:center"">Мярка</th>
                <th style=""text-align:center"">Ед. Цена <br /> без ДДС</th>
                <th style=""text-align:center"">Стойност в лева</th>
            </tr>
            <tr>
                <td>Консумирана енергия за охлаждане</td>
                <td style=""text-align:center"">");
#nullable restore
#line 128 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                         Write(Model.CoolingConsummation);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                <td style=\"text-align:center\">кВтч</td>\r\n                <td style=\"text-align:center\">");
#nullable restore
#line 130 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                         Write(Model.PricesInformation.CoolingPerKWh);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                <td style=\"text-align:right\">");
#nullable restore
#line 131 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                         Write(Model.AmountForCooling.ToString("F2"));

#line default
#line hidden
#nullable disable
                WriteLiteral(" лв.</td>\r\n            </tr>\r\n        </table>\r\n        <br />\r\n        <p style=\"text-align:right\"><i>Стойност за консумация на енергия за охлаждане:   <b>");
#nullable restore
#line 135 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                                                                        Write((Model.AmountForCooling).ToString("F2"));

#line default
#line hidden
#nullable disable
                WriteLiteral(" лв.</b></i></p>\r\n        <br />\r\n");
#nullable restore
#line 137 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
    }

#line default
#line hidden
#nullable disable
                WriteLiteral("    <br />\r\n    <br />\r\n    <p style=\"text-align:right\"><i>Данъчна основа: <b>");
#nullable restore
#line 140 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                                  Write((Model.AmountForElectricity + Model.AmountForHeating + Model.AmountForCooling).ToString("F2"));

#line default
#line hidden
#nullable disable
                WriteLiteral(" лв.</b></i></p>\r\n    <p style=\"text-align:right\"><i>Размер на ДДС 20%: <b>");
#nullable restore
#line 141 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                                     Write(((Model.AmountForElectricity + Model.AmountForHeating + Model.AmountForCooling) * 0.20M).ToString("F2"));

#line default
#line hidden
#nullable disable
                WriteLiteral(" лв.</b></i></p>\r\n    <p style=\"text-align:right\"><i>Обща стойност на сделката: <b>");
#nullable restore
#line 142 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                                                            Write(Model.TotalAmount.ToString("F2"));

#line default
#line hidden
#nullable disable
                WriteLiteral(" лв.</b></i></p>\r\n    <br />\r\n    <br />\r\n    <div class=\"row\">\r\n        <div class=\"col-9\">\r\n            <p>\r\n                Дата: <b>");
#nullable restore
#line 148 "C:\Users\mitob\Desktop\OfficeManager\OfficeManager\Views\AccountingReports\Details.cshtml"
                    Write(Model.CreatedOn.ToString("d.MM.yyyy"));

#line default
#line hidden
#nullable disable
                WriteLiteral(@" г.</b><br>
                гр. София
            </p>
        </div>
        <div class=""col-3"">
            <p>
                Съставил: ......................................<br>
                (Теодор Опренов)
            </p>
        </div>
    </div>
    <a href=""/AccountingReports/All"" class=""btn btn-dark btn-lg btn-block"" style=""margin-top:50px"">Back</a>
");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</html>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<AccountingReportViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
