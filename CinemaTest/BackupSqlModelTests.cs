using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cinema.Areas.Admin.Pages;
using Cinema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTests
{
    [TestClass]
    public class BackupSqlModelTests
    {
        private static IOptions<ExportSqlOptions> options = default!;
        private static FileContentResult result = default!;

        [ClassInitialize]
        public static void InitTestSuite(TestContext _)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            options = Options.Create(configuration.GetSection("SqlExport").Get<ExportSqlOptions>());
            Directory.SetCurrentDirectory(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + @"\Cinema");

            var backupSqlModel = new BackupSqlModel(options);
            result = backupSqlModel.OnGet();
        }

        [TestMethod]
        public void NotNullTest()
        {
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CorrectTypeTest()
        {
            Assert.IsTrue(result is FileContentResult);
        }

        [TestMethod]
        public void CorrectNameTest()
        {
            Assert.IsTrue(result.FileDownloadName == options.Value.ResultFile);
        }

        [TestMethod]
        public void CorrectContentTypeTest() 
        {
            Assert.IsTrue(result.ContentType == "application/force-download");
        }
    }
}