using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetReporting;

namespace CodeExcercise
{
    [TestClass]
    public class MyTestClass
    {
        [TestMethod]
        public void Test1()
        {
            var pets = new List<Pet>()
            {
                new Dog() { Name = "Buddy", Owner = new Owner { FirstName = "Jim", LastName = "Rogers"}, NumberOfVisits = 5, JoinedPractice = DateTime.Now},
                new Dog() { Name = "Max", Owner = new Owner{FirstName = "Tony", LastName = "Smith"}, NumberOfVisits = 10, JoinedPractice = new DateTime(1985, 7, 13)},
                new Cat() { Name = "Whiskers", Owner = new Owner{FirstName = "Steve", LastName="Roberts"}, NumberOfVisits = 20, JoinedPractice = new DateTime(2002, 5, 6), numberOfLives = 9 }
            };

            new PetReportWriter().WriteReport(pets, "PetsReport.csv");
            var outPets = File.ReadAllLines("PetsReport.csv");

            Assert.AreEqual(4, outPets.Count());
        }
    }


}
