using Cinema.Data;
using Cinema.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
    public class AccountingReportTests
    {
        private static ApplicationDbContext context = default!;
        private static DbSet<AccountingReport> reports = default!;

        [ClassInitialize]
        public static void InitTestSuite(TestContext _)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Cinema")
                .Options;

            context = new ApplicationDbContext(options);
            reports = context.AccountingReport;

            reports.AddRange(
                new AccountingReport { Name = "Main", Salary = 20000, Total = 20000, EmployeeId = "1" },
                new AccountingReport { Name = "Second", Salary = 15000, Bonus = 2000, Total = 170000, EmployeeId = "1" }
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
            reports.Add(new AccountingReport { Name = "Accountant", Salary = 25000, Total = 25000, EmployeeId = "2" });
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
            upd.Name = "HR";
            context.SaveChanges();

            Assert.AreEqual("HR", reports.Find(1L).Name);
        }

        [TestMethod]
        public void SortTest()
        {
            var sorted = reports.OrderByDescending(d => d.Id).ToList();

            Assert.IsTrue(sorted[0].Id >= sorted[1].Id);
        }
    }
}
