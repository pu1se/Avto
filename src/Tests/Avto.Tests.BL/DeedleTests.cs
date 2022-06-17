using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avto.BL.Services.Exchange;
using Avto.Tests.BL.Base;
using Deedle;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Avto.Tests.BL
{
    [TestClass]
    public class DeedleTests : BaseServiceTests<ExchangeService>
    {
        [TestMethod]
        public void Successfully_generate_deedle_frame()
        {
            var rnd = new Random();
            var objects = Enumerable.Range(0, 10).Select(i =>
                new { Key = "ID_" + i, Number = rnd.Next() });

            // Create data frame with properties as column names
            var dfObjects = Frame.FromRecords(objects);
            dfObjects.Print();
        }
    }
}
