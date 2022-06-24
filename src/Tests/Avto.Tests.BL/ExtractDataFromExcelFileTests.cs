using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avto.BL.Services.Exchange;
using Avto.BL.Services.Exchange.Converter;
using Avto.BL.Services.Exchange.ExternalApis.ApiModels;
using Avto.DAL.Entities;
using Avto.DAL.Enums;
using Avto.Tests.BL.Base;
using Deedle;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Avto.Tests.BL
{
    [TestClass]
    public class ExtractDataFromExcelFileTests : BaseServiceTests<ExchangeService>
    {
        [TestMethod]
        public async Task SuccessExtractDataFromExcelFile()
        {
            try
            {
                await Service.ExtractDataFromCsvFileCollection();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
        
        
    }

    
}
