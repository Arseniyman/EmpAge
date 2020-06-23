using EmpAge.Controllers;
using MyTested.AspNetCore.Mvc;
using System;
using Xunit;

namespace EmpAge.Test
{
    public class UnitTest1
    {
        [Fact]
        public void GetHomePage()
            => MyMvc
            .Controller<HomeController>()
            .Calling(c => c.Index())
            .ShouldReturn()
            .View();
    }
}