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
    public class VacanciesControllerTest
    {
        [Fact]
        public void CreateShouldReturnAcessDeniedForUnauthorizedUser()
            => MyMvc
            .Controller<VacanciesController>()
            .Calling(c => c.Create())
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForAuthorizedRequests());

        [Fact]
        public void CreateWithParametersShouldRestrictForHttpMethodPost()
            => MyMvc
            .Controller<VacanciesController>()
            .Calling(c => c.Create(With.Default<Vacancy>()))
            .ShouldHave()
            .ActionAttributes(attr => attr
                .RestrictingForHttpMethod(HttpMethod.Post));

        [Fact]
        public void PostCreateShouldReturnViewWithTheSameModelWhenModelIsInvalid()
            => MyMvc
            .Controller<VacanciesController>()
            .Calling(c => c.Create(With.Default<Vacancy>()))
            .ShouldHave()
            .InvalidModelState()
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<Vacancy>());

        [Theory]
        [InlineData("Simple name", "Simple description")]
        public void PostCreateShouldReturnRedirectPersonalPageAccountAndSaveVacancy(
            string name,
            string description)
            => MyMvc
            .Controller<VacanciesController>()
            .WithUser("emplo@mail.ru", new[] { "employer" })
            .Calling(c => c.Create(new Vacancy
            {
                Name = name,
                Description = description
            }))
            .ShouldHave()
            .Data(data => data
                .WithSet<Vacancy>(set =>
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
            .Controller<VacanciesController>()
            .WithData(new List<Vacancy> { 
                new Vacancy {
                    Name = "Строитель",
                    EmploymentType = EmploymentType.Полная,
                    JobSector = JobSector.Строительство,
                    Description = "Опыт работы 15 лет"
                },
                new Vacancy {
                    Name = "Разнорабочий",
                    EmploymentType = EmploymentType.Полная,
                    JobSector = JobSector.Строительство,
                    Description = "Опыт работы 7 лет"
                },
                new Vacancy {
                    Name = "Разнорабочий",
                    EmploymentType = EmploymentType.Полная,
                    JobSector = JobSector.Строительство,
                    Description = "Опыт работы 1 год"
                }
            })
            .Calling(c => c.Index("разнорабочий"))
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<List<Vacancy>>()
                .Passing(model =>
                    { model.Count.ShouldBe(2); }));
    }
}