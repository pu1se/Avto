using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Avto.BL.Services.Exchange;
using Avto.Tests.BL.Base;
using Deedle;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Avto.Tests.BL
{
    [TestClass]
    public class DeedleFrameCreateCollectionTests : BaseServiceTests<ExchangeService>
    {
        [TestMethod]
        public void CreateCollectionFromRecords()
        {
            var random = new Random();
            var numberCollection = Enumerable.Range(1, 10).Select(
                number => 
                    new
                    {
                        KeyColumn = "Id_" + number,
                        RandomNumberColumn = random.Next(100)
                    });

            var numberDataFrame = Frame.FromRecords(numberCollection);
            numberDataFrame.Print();
        }

        [TestMethod]
        public void CreateCollectionFromSeriesBuilder()
        {
            var random = new Random();
            var rowsCollection = Enumerable.Range(1, 10).Select(
                number =>
                {
                    var row = new SeriesBuilder<string>();
                    row.Add("KeyColumn", "Id_" + number);
                    row.Add("RandowNumberColumn", random.Next(100));
                    return KeyValue.Create(number-1, row.Series);
                });

            var numberDataFrame = Frame.FromRows(rowsCollection);
            numberDataFrame.Print();
        }

        [TestMethod]
        public void CreateCollectionFromFile()
        {
            var fileName = "file.txt";
            var fileIsExists = File.Exists(fileName);
            Assert.IsTrue(fileIsExists);

            var dataFrame = Frame.ReadCsv(fileName, hasHeaders:false, separators:" ", schema:"int,string");
            dataFrame.Print();
        }

        [TestMethod]
        public void CreateCollectionFromExcel()
        {
            var fileName = "excel.csv";
            var fileIsExists = File.Exists(fileName);
            Assert.IsTrue(fileIsExists);

            var dataFrame = Frame.ReadCsv(fileName, hasHeaders:true, schema:"int,string,string,bool,int,int");
            dataFrame.Print();
        }

        [TestMethod]
        public void Tmp()
        {
            var filePathes = Directory.GetFiles("../../../../../../docs/exchange-rates-csv/");
            foreach (var pathToFile in filePathes)
            {
                var dataFrame = Frame.ReadCsv(pathToFile, hasHeaders: true, schema: "string,string,string,string");
            }
            
            var tmp = System.IO.Directory.GetCurrentDirectory();
            
        }
    }
}
