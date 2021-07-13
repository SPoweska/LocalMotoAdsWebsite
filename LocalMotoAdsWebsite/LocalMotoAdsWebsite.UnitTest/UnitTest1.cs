using FluentAssertions;
using LocalMotoAdsWebsite.Controllers;
using LocalMotoAdsWebsite.Models;
using LocalMotoAdsWebsite.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace LocalMotoAdsWebsite.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var mock = new Mock<IMakeRepository>();
            mock.Setup(repo => repo.GetAllMakes()).Returns(GetMakes());
            var controller = new HomeController(mock.Object);
            var result = controller.Index();
            var actionResult = Assert.IsType<ViewResult>(result);
            var make = Assert.IsType<List<Make>>(actionResult.ViewData["make"]);
            Assert.Equal(7, make.Count);
        }

    
        [Fact]
        public void Test2()
        {
            var mock = new Mock<IMakeRepository>();
            mock.Setup(repo => repo.GetMake(1)).Returns(new Make { Id = 1, Name = "Audi" });
            var controller = new HomeController(mock.Object);
            var result = controller.Privacy();
            var actionResult = Assert.IsType<ViewResult>(result);
            var make = Assert.IsType<Make>(actionResult.ViewData["make"]);
            make.Should().BeEquivalentTo(new Make { Id = 1, Name = "Audi" });
        }

        private List<Make> GetMakes()
            {
                return new List<Make>
                {
                    new Make {Id = 1, Name = "Audi"},
                    new Make {Id = 2, Name = "BMW"},
                    new Make {Id = 3, Name = "Opel"},
                    new Make {Id = 4, Name = "Honda"},
                    new Make {Id = 5, Name = "Ford"},
                    new Make {Id = 6, Name = "Ferrari"},
                    new Make {Id = 7, Name = "Mercedes"},
                };
            }
        }
}
