using System;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using PeopleSearch.Controllers;
using System.Web.Mvc;
using PeopleSearch.Models;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;

namespace PeoplesearchTest
{
   

    [TestClass]
    public class UnitTest1 
    {
        PeopleController pc = null;
        
        public UnitTest1()
        {
            var data = new List<Person>
            {
                new Person(){ FirstName="Alex", LastName="Luka"},
                new Person(){ FirstName="Johnn", LastName="Depp"},
                new Person(){ FirstName="Christian", LastName="Christian"},
                new Person(){ FirstName="Kevin", LastName="Spacey"}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Person>>();
            mockSet.As<IQueryable<Person>>().Setup(m => m.Provider).Returns(data.Provider); 
            mockSet.As<IQueryable<Person>>().Setup(m => m.Expression).Returns(data.Expression); 
            mockSet.As<IQueryable<Person>>().Setup(m => m.ElementType).Returns(data.ElementType); 
            mockSet.As<IQueryable<Person>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            Mock<PeopleDBContext> myMock = new Mock<PeopleDBContext>();
            myMock.Setup(m => m.People).Returns(mockSet.Object);
            pc= new PeopleController();
            pc.setContext(mockSet.Object);
        }
        // Testing for Edit Method of People Controller...
        [TestMethod]
        public void editTest()
        {
           
            ActionResult result = pc.Edit(1);
          // NUnit.Framework.Assert.IsInstanceOf(result.GetType(), typeof(ViewResult));
            var vResult = result as ViewResult;
            var prn = new Person();
            prn.FirstName = "Bob";
            prn.LastName = "Nelson";            
            if (vResult != null)
            {
               // Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(vResult.Model, typeof(PeopleSearch.Models.Person));
                var model = vResult.Model as Person;
                if (model != null)
                {
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(prn.FirstName, model.FirstName);
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(prn.LastName, model.LastName);
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(prn.Address, model.Address); 
                }

            }
        }
        // Testing for details method of People Controller...
        [TestMethod]
        public void detailsTest()
        {

            ActionResult result = pc.Details(1);
            // NUnit.Framework.Assert.IsInstanceOf(result.GetType(), typeof(ViewResult));
            ViewResult vResult = result as ViewResult;
            var prn = new Person();
            prn.FirstName = "Alex";
            prn.LastName = "Luka";
            if (vResult != null)
            {
                // Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(vResult.Model, typeof(PeopleSearch.Models.Person));
                var model = vResult.Model as Person;
                if (model != null)
                {
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(prn.FirstName);
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(prn.FirstName, model.FirstName);
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(prn.LastName, model.LastName);
                  //  Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(prn.Address, model.Address); 
                }
               
            }
        }

        [TestCase("Alex", "luka")]
        [TestCase("John","Depp")]
        [TestCase("Christian","Bale")]
        public void PeopleController_Search_NotNull(string firstName, string lastName)
        {
            var peopleController = new PeopleController();

            var result = peopleController.Search(firstName, lastName);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
        }


        [TestCase("fake", "user1")]
        [TestCase("fake", "user2")]
        [TestCase("fake", "user3")]
        public void PeopleController_Search_Null(string firstName, string lastName)
        {
            var peopleController = new PeopleController();

            var result = peopleController.Search(firstName, lastName);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNull(result);
        }


    }
}