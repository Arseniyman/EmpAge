using System;
using System.Collections.Generic;
using System.Text;
using EmpAge.Controllers;
using EmpAge.Models;
using Xunit;
using MyTested.AspNetCore.Mvc;

namespace EmpAge.Test.Routes
{
    public class SummariesRouteTest
    {
        [Fact]
        public void GetDetailsShouldBeCorrectly()
            => MyRouting
            .Configuration()
            .ShouldMap("/Summaries/Details/1")
            .To<SummariesController>(c => c.Details(1));

        [Fact]
        public void PostCreateShouldBeRoutedCorrectly()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Summaries/Create")
                .WithMethod(HttpMethod.Post))
            .To<SummariesController>(c => c.Create(With.Any<Summary>()));
    }
}
