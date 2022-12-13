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
    public class MovieSessionTests
    {
        private static ApplicationDbContext context = default!;
        private static DbSet<MovieSession> sessions = default!;

        [ClassInitialize]
        public static void InitTestSuite(TestContext _)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Cinema")
                .Options;

            context = new ApplicationDbContext(options);
            sessions = context.MovieSession;

            sessions.AddRange(
                new MovieSession { MovieName = "The Best", SessionTime = DateTime.UtcNow },
                new MovieSession { MovieName = "Золушка", SessionTime = DateTime.UtcNow }
            );

            context.SaveChanges();
        }

        [TestMethod]
        public void GetAllTest()
        {
            var dep = sessions.ToList();

            Assert.AreEqual(2, dep.Count);
        }

        [TestMethod]
        public void AddTest()
        {
            sessions.Add(new MovieSession { MovieName = "Парки и зоны отдыха", SessionTime = DateTime.UtcNow });
            context.SaveChanges();

            Assert.AreEqual(3, sessions.ToList().Count);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var del = sessions.First(d => d.MovieName == "Золушка");
            sessions.Remove(del);
            context.SaveChanges();

            Assert.AreEqual(2, sessions.ToList().Count);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var upd = sessions.Find(1L);
            upd.MovieName = "The Worst";
            context.SaveChanges();

            Assert.AreEqual("The Worst", sessions.Find(1L).MovieName);
        }

        [TestMethod]
        public void SortTest()
        {
            var sorted = sessions.OrderByDescending(d => d.Id).ToList();

            Assert.IsTrue(sorted[0].Id >= sorted[1].Id);
        }
    }
}
