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
    public class OrderTests
    {
        private static ApplicationDbContext context = default!;
        private static DbSet<Order> orders = default!;

        [ClassInitialize]
        public static void InitTestSuite(TestContext _)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Cinema")
                .Options;

            context = new ApplicationDbContext(options);
            orders = context.Order;

            orders.AddRange(
                new Order { BuyerId = "1", TicketId = 1L, IsConfirmed = false, IsRejected = false },
                new Order { BuyerId = "2", TicketId = 2L, IsConfirmed = false, IsRejected = false }
            );

            context.SaveChanges();
        }

        [TestMethod]
        public void GetAllTest()
        {
            var dep = orders.ToList();

            Assert.AreEqual(2, dep.Count);
        }

        [TestMethod]
        public void AddTest()
        {
            orders.Add(new Order { BuyerId = "3", TicketId = 3L, IsConfirmed = false, IsRejected = false });
            context.SaveChanges();

            Assert.AreEqual(3, orders.ToList().Count);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var del = orders.First(d => d.BuyerId == "2");
            orders.Remove(del);
            context.SaveChanges();

            Assert.AreEqual(2, orders.ToList().Count);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var upd = orders.Find(1L);
            upd.IsConfirmed = true;
            context.SaveChanges();

            Assert.AreEqual(true, orders.Find(1L).IsConfirmed);
        }

        [TestMethod]
        public void SortTest()
        {
            var sorted = orders.OrderByDescending(d => d.Id).ToList();

            Assert.IsTrue(sorted[0].Id >= sorted[1].Id);
        }
    }
}
