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
    public class StockReportTests
    {
        private static ApplicationDbContext context = default!;
        private static DbSet<StockReport> reports = default!;

        [ClassInitialize]
        public static void InitTestSuite(TestContext _)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Cinema")
                .Options;

            context = new ApplicationDbContext(options);
            reports = context.StockReport;

            reports.AddRange(
                new StockReport { Name = "Кола", Type = "Напиток", Amount = 200 },
                new StockReport { Name = "Попкорн", Type = "Еда", Amount = 300 }
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
            reports.Add(new StockReport { Name = "Начос", Type = "Еда", Amount = 150 });
            context.SaveChanges();

            Assert.AreEqual(3, reports.ToList().Count);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var del = reports.First(d => d.Name == "Попкорн");
            reports.Remove(del);
            context.SaveChanges();

            Assert.AreEqual(2, reports.ToList().Count);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var upd = reports.Find(1L);
            upd.Name = "Вода";
            context.SaveChanges();

            Assert.AreEqual("Вода", reports.Find(1L).Name);
        }

        [TestMethod]
        public void SortTest()
        {
            var sorted = reports.OrderByDescending(d => d.Amount).ToList();

            Assert.IsTrue(sorted[0].Amount >= sorted[1].Amount);
        }
    }
}
