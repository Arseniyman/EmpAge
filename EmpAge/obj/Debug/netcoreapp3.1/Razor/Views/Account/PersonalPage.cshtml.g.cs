#pragma checksum "C:\Users\Home\source\repos\EmpAge\EmpAge\Views\Account\PersonalPage.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d88281fa477ac3cb36c1a515eb91b8a9484ffee0"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Account_PersonalPage), @"mvc.1.0.view", @"/Views/Account/PersonalPage.cshtml")]
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
#line 1 "C:\Users\Home\source\repos\EmpAge\EmpAge\Views\_ViewImports.cshtml"
using EmpAge;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Home\source\repos\EmpAge\EmpAge\Views\_ViewImports.cshtml"
using EmpAge.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d88281fa477ac3cb36c1a515eb91b8a9484ffee0", @"/Views/Account/PersonalPage.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"11dc80465dc8b931ede87197803401588b20183b", @"/Views/_ViewImports.cshtml")]
    public class Views_Account_PersonalPage : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\Home\source\repos\EmpAge\EmpAge\Views\Account\PersonalPage.cshtml"
  
    ViewData["Title"] = "Личный кабинет";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 5 "C:\Users\Home\source\repos\EmpAge\EmpAge\Views\Account\PersonalPage.cshtml"
 if (User.IsInRole("applicant"))
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <h1>Личный кабинет соискателя</h1>\r\n");
#nullable restore
#line 8 "C:\Users\Home\source\repos\EmpAge\EmpAge\Views\Account\PersonalPage.cshtml"
Write(await Html.PartialAsync("ApplicantPartial"));

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\Home\source\repos\EmpAge\EmpAge\Views\Account\PersonalPage.cshtml"
                                                
}

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\Home\source\repos\EmpAge\EmpAge\Views\Account\PersonalPage.cshtml"
 if (User.IsInRole("employer"))
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <h1>Личный кабинет работодателя</h1>\r\n");
#nullable restore
#line 13 "C:\Users\Home\source\repos\EmpAge\EmpAge\Views\Account\PersonalPage.cshtml"
}

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "C:\Users\Home\source\repos\EmpAge\EmpAge\Views\Account\PersonalPage.cshtml"
 if (User.IsInRole("admin"))
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <h1>Личный кабинет администратора</h1>\r\n");
#nullable restore
#line 18 "C:\Users\Home\source\repos\EmpAge\EmpAge\Views\Account\PersonalPage.cshtml"
Write(await Html.PartialAsync("AdminPartial"));

#line default
#line hidden
#nullable disable
#nullable restore
#line 18 "C:\Users\Home\source\repos\EmpAge\EmpAge\Views\Account\PersonalPage.cshtml"
                                            ;
}

#line default
#line hidden
#nullable disable
#nullable restore
#line 20 "C:\Users\Home\source\repos\EmpAge\EmpAge\Views\Account\PersonalPage.cshtml"
 if (User.IsInRole("moder"))
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <h1>Личный кабинет модератора</h1>\r\n");
#nullable restore
#line 23 "C:\Users\Home\source\repos\EmpAge\EmpAge\Views\Account\PersonalPage.cshtml"
}

#line default
#line hidden
#nullable disable
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
