using Cinema.Data;
using Cinema.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaTests
{
    [TestClass]
    public class PurchaseReportTests
    {
        private static ApplicationDbContext context = default!;
        private static DbSet<PurchaseReport> reports = default!;

        [ClassInitialize]
        public static void InitTestSuite(TestContext _)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Cinema")
                .Options;

            context = new ApplicationDbContext(options);
            reports = context.PurchaseReport;

            reports.AddRange(
                new PurchaseReport { Name = "Main", Description = "Main report", DepartmentId = 1L },
                new PurchaseReport { Name = "Second", Description = "Not so important report", DepartmentId = 2L }
            );

            context.SaveChanges();
        }

        [TestMethod]
        public void GetAllTest()
        {
            var dep = reports.ToList();

            Assert.AreEqual(2, dep.Count);
        }

        [TestMethod]
        public void AddTest()
        {
            reports.Add(new PurchaseReport { Name = "Big deal", Description = "The best", DepartmentId = 2L });
            context.SaveChanges();

            Assert.AreEqual(3, reports.ToList().Count);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var del = reports.First(d => d.Name == "Second");
            reports.Remove(del);
            context.SaveChanges();

            Assert.AreEqual(2, reports.ToList().Count);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var upd = reports.Find(1L);
            upd.Name = "Good";
            context.SaveChanges();

            Assert.AreEqual("Good", reports.Find(1L).Name);
        }

        [TestMethod]
        public void SortTest()
        {
            var sorted = reports.OrderByDescending(d => d.Id).ToList();

            Assert.IsTrue(sorted[0].Id >= sorted[1].Id);
        }
    }
}
