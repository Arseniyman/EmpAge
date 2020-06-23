using System;
using System.Collections.Generic;
using System.Text;
using EmpAge.Controllers;
using EmpAge.Models;
using Xunit;
using MyTested.AspNetCore.Mvc;

namespace EmpAge.Test.Routes
{
    public class VacanciesRouteTest
    {
        [Fact]
        public void GetDetailsShouldBeCorrectly()
            => MyRouting
            .Configuration()
            .ShouldMap("/Vacancies/Details/1")
            .To<VacanciesController>(c => c.Details(1));

        [Fact]
        public void PostCreateShouldBeRoutedCorrectly()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Vacancies/Create")
                .WithMethod(HttpMethod.Post))
            .To<VacanciesController>(c => c.Create(With.Any<Vacancy>()));
    }
}
