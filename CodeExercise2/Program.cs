using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                new Dog() { Firstname = "Jim", Lastname = "Rogers", numberofVisits = 5, joinedPractice = DateTime.Now},
                new Dog() { Firstname = "Tony", Lastname = "Smith", numberofVisits = 10, joinedPractice = new DateTime(1985, 7, 13)},
                new Cat() { Firstname = "Steve", Lastname = "Roberts", numberofVisits = 20, joinedPractice = new DateTime(2002, 5, 6), numberOfLives = 9 }
            };

            new Pet().printReport(pets, "PetsReport.csv");
            var outPets = File.ReadAllLines("PetsReport.csv");

            Assert.AreEqual(4, outPets.Count());
        }
    }

    public class Pet : Owner
    {
        public int numberofVisits;
        public DateTime joinedPractice;

        // NOTE: This method prints a pets reports in csv format.
        public void printReport(IEnumerable<Pet> pets, string filename)
        {
            List<string> entries = new List<string>();
            entries.Add("Owners name,Date Joined Practice,Number Of Visits,Number of Lives");
            foreach (var p in pets)
            {
                var entry = string.Join(" ", p.Firstname, p.Lastname) + p.joinedPractice + "," + p.numberofVisits;
                if (p is Cat)
                {
                    var cat = p as Cat;
                    entry += "," + cat.numberOfLives;
                }

                entries.Add(entry);
            }
            File.WriteAllLines(filename, entries.ToArray());
        }
    }

    public class Dog : Pet
    {
        public double CostPerVisit;
    }

    public class Cat : Pet
    {
        public int? numberOfLives;
        public double CostPerVisit;
    }

    public class Owner
    {
        public string Firstname;

        public string Lastname;
    }

}
