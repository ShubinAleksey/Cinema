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
    public class TicketTests
    {
        private static ApplicationDbContext context = default!;
        private static DbSet<Ticket> tickets = default!;

        [ClassInitialize]
        public static void InitTestSuite(TestContext _)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Cinema")
                .Options;

            context = new ApplicationDbContext(options);
            tickets = context.Ticket;

            tickets.AddRange(
                new Ticket { SessionId = 1L, RowNumber = 1, SeatNumber = 1, IsBought = false },
                new Ticket { SessionId = 1L, RowNumber = 1, SeatNumber = 2, IsBought = false }
            );

            context.SaveChanges();
        }

        [TestMethod]
        public void GetAllTest()
        {
            var dep = tickets.ToList();

            Assert.AreEqual(2, dep.Count);
        }

        [TestMethod]
        public void AddTest()
        {
            tickets.Add(new Ticket { SessionId = 2L, RowNumber = 3, SeatNumber = 2, IsBought = false });
            context.SaveChanges();

            Assert.AreEqual(3, tickets.ToList().Count);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var del = tickets.First(d => d.SessionId == 1L && d.RowNumber == 1 && d.SeatNumber == 2);
            tickets.Remove(del);
            context.SaveChanges();

            Assert.AreEqual(2, tickets.ToList().Count);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var upd = tickets.Find(1L);
            upd.IsBought = true;
            context.SaveChanges();

            Assert.AreEqual(true, tickets.Find(1L).IsBought);
        }

        [TestMethod]
        public void SortTest()
        {
            var sorted = tickets.OrderBy(d => d.Id).ToList();

            Assert.IsTrue(sorted[0].Id <= sorted[1].Id);
        }
    }
}
