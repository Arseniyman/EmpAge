using System;
using System.Collections.Generic;
using System.Text;
using EmpAge.Controllers;
using Xunit;
using MyTested.AspNetCore.Mvc;

namespace EmpAge.Test.Controllers
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnViewForAllUsers()
            => MyMvc
            .Controller<HomeController>()
            .Calling(c => c.Index())
            .ShouldReturn()
            .View();

        [Fact]
        public void PrivacyShouldReturnViewForAllUsers()
            => MyMvc
            .Controller<HomeController>()
            .Calling(c => c.Privacy())
            .ShouldReturn()
            .View();
    }
}
