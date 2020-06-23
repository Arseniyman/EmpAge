using System;
using System.Collections.Generic;
using System.Text;
using EmpAge.Controllers;
using EmpAge.ViewModels;
using Xunit;
using MyTested.AspNetCore.Mvc;

namespace EmpAge.Test.Controllers
{
    public class AccountControllerTest
    {
        [Fact]
        public void PostLoginShouldHaveCorrectActionFilters()
            => MyMvc
                .Controller<AccountController>()
                .Calling(c => c.Login(
                    With.Default<LoginViewModel>()))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .AllowingAnonymousRequests()
                    .ValidatingAntiForgeryToken());

        [Fact]
        public void PostLoginShouldReturnDefaultViewWithInvalidModel()
            => MyMvc
            .Controller<AccountController>()
                .Calling(c => c.Login(
                    With.Default<LoginViewModel>()))
                .ShouldHave()
                .ModelState(modelState => modelState
                    .For<LoginViewModel>()
                    .ContainingErrorFor(m => m.Email)
                    .ContainingErrorFor(m => m.Password))
                .AndAlso()
                .ShouldReturn()
                .View();
    }
}