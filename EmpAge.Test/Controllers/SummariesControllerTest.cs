using System;
using System.Collections.Generic;
using System.Text;
using EmpAge.Controllers;
using EmpAge.Models;
using Xunit;
using MyTested.AspNetCore.Mvc;
using Shouldly;

namespace EmpAge.Test.Controllers
{
    public class SummariesControllerTest
    {
        [Fact]
        public void CreateShouldReturnAcessDeniedForUnauthorizedUser()
            => MyMvc
            .Controller<SummariesController>()
            .Calling(c => c.Create())
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForAuthorizedRequests());

        [Fact]
        public void CreateWithParametersShouldRestrictForHttpMethodPost()
            => MyMvc
            .Controller<SummariesController>()
            .Calling(c => c.Create(With.Default<Summary>()))
            .ShouldHave()
            .ActionAttributes(attr => attr
                .RestrictingForHttpMethod(HttpMethod.Post));

        [Fact]
        public void CreateShouldReturnViewWithTheSameModelWhenModelIsInvalid()
            => MyMvc
            .Controller<SummariesController>()
            .Calling(c => c.Create(With.Default<Summary>()))
            .ShouldHave()
            .InvalidModelState()
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<Summary>());

        [Theory]
        [InlineData("Simple name", "Simple description")]
        public void PostCreateShouldReturnRedirectPersonalPageAccountAndSaveSummary(
            string name,
            string description)
            => MyMvc
            .Controller<SummariesController>()
            .WithUser("applic@mail.ru", new[] { "applicant" })
            .Calling(c => c.Create(new Summary
            {
                Name = name,
                Description = description
            }))
            .ShouldHave()
            .Data(data => data
                .WithSet<Summary>(set =>
                {
                    set.ShouldNotBeNull();
                }))
            .AndAlso()
            .ShouldReturn()
            .Redirect(result => result
                .To<AccountController>(c => c.PersonalPage()));

        [Fact]
        public void IndexWithInputParamsShouldReturnValidVacancies()
            => MyMvc
            .Controller<SummariesController>()
            .WithData(new List<Summary> {
                new Summary {
                    Name = "Программист",
                    EmploymType = EmploymentType.Полная,
                    Description = "Владею Python"
                },
                new Summary {
                    Name = "Программист",
                    EmploymType = EmploymentType.Полная,
                    Description = "Опыт работы 7 лет PHP"
                },
                new Summary {
                    Name = "Python разработчик",
                    EmploymType = EmploymentType.Полная,
                    Description = "Опыт работы 1 год"
                }
            })
            .Calling(c => c.Index("python"))
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<List<Summary>>()
                .Passing(model =>
                { model.Count.ShouldBe(2); }));
    }
}
