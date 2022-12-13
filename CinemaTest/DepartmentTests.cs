using Cinema.Data;
using Cinema.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CinemaTests
{
    [TestClass]
    public class DepartmentTests
    {
        private static ApplicationDbContext context = default!;
        private static DbSet<Department> departments = default!;

        [ClassInitialize]
        public static void InitTestSuite(TestContext _)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Cinema")
                .Options;
            
            context = new ApplicationDbContext(options);
            departments = context.Department;

            departments.AddRange(
                new Department { Name = "HR" },
                new Department { Name = "Stock" }
            );

            context.SaveChanges();
        }

        [TestMethod]
        public void GetAllTest()
        {
            var dep = departments.ToList();

            Assert.AreEqual(2, dep.Count);
        }

        [TestMethod]
        public void AddTest()
        {
            departments.Add(new Department { Name = "Accountant" });
            context.SaveChanges();

            Assert.AreEqual(3, departments.ToList().Count);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var del = departments.First(d => d.Name == "Stock");
            departments.Remove(del);
            context.SaveChanges();

            Assert.AreEqual(2, departments.ToList().Count);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var upd = departments.Find(1L);
            upd.Name = "Purchase";
            context.SaveChanges();

            Assert.AreEqual("Purchase", departments.Find(1L).Name);
        }

        [TestMethod]
        public void SortTest()
        {
            var sorted = departments.OrderBy(d => d.Name).ToList();

            Assert.IsTrue(sorted[0].Name[0] <= sorted[1].Name[0]);
        }
    }
}