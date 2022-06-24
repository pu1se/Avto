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
    public class DeedleFrameManipulationTests : BaseServiceTests<ExchangeService> 
    {
        [TestMethod]
        public void CreateIndexForColumn()
        {
            var dataFrame = GetSimpleDataFrame();
            dataFrame.Print();
            //output:
            //     Id Number  
            //0 -> 1  1       
            //1 -> 2  22      
            //2 -> 3  303     
            //3 -> 4  4004    
            //4 -> 5  50005   
            //5 -> 6  600006  
            //6 -> 7  7000007     
            
            dataFrame = dataFrame.IndexRows<int>("Number").SortRowsByKey();
            dataFrame.Print();
            //output:
            //           Id 
            //1       -> 1  
            //22      -> 2  
            //303     -> 3  
            //4004    -> 4  
            //50005   -> 5  
            //600006  -> 6  
            //7000007 -> 7  
 

            dataFrame = dataFrame.IndexRows<int>("Id").SortRowsByKey();
            dataFrame.Print();
            //output:
            //     Number  
            //1 -> 1       
            //2 -> 22      
            //3 -> 303     
            //4 -> 4004    
            //5 -> 50005   
            //6 -> 600006  
            //7 -> 7000007 

    
        }

        [TestMethod]
        public void RenameAllColumn()
        {
            var dataFrame = GetSimpleDataFrame();
            dataFrame.RenameColumns(x => x + "Some name");
            dataFrame.Print();
            //output:
            //     IdSome name NumberSome name 
            //0 -> 1           1               
            //1 -> 2           22              
            //2 -> 3           303             
            //3 -> 4           4004            
            //4 -> 5           50005           
            //5 -> 6           600006          
            //6 -> 7           7000007         
        }

        [TestMethod]
        public void RenameOneColumn()
        {
            var dataFrame = GetSimpleDataFrame().IndexRows<int>("Id").SortRowsByKey();
            dataFrame.RenameColumn("Number", "Random");
            dataFrame.Print();
            //output:
            //     Random  
            //1 -> 1       
            //2 -> 22      
            //3 -> 303     
            //4 -> 4004    
            //5 -> 50005   
            //6 -> 600006  
            //7 -> 7000007   

        }

        [TestMethod]
        public void GetOneColumn()
        {
            var dataFrame = GetSimpleDataFrame().IndexRows<int>("Id").SortRowsByKey();
            var numberColumn = dataFrame.GetColumn<int>("Number");
            numberColumn.Print();
            //output:
            //0 -> 1       
            //1 -> 22      
            //2 -> 303     
            //3 -> 4004    
            //4 -> 50005   
            //5 -> 600006  
            //6 -> 7000007 
        }

        [TestMethod]
        public void ConvertIntToBoolean()
        {
            var list = Enumerable.Range(1, 2).Select(i => new {Id = i-1});
            var dataFrame = Frame.FromRecords(list);
            dataFrame.Print();

            var idColumnAsBoolean = dataFrame.GetColumn<bool>("Id");
            idColumnAsBoolean.Print();
        }

        private Frame<int, string> GetFrameWithEmailData()
        {
            var fileName = "excel.csv";
            var fileIsExists = File.Exists(fileName);
            Assert.IsTrue(fileIsExists);
            var dataFrame = Frame.ReadCsv(fileName, hasHeaders:true, inferTypes: false, schema:"int,string,string,bool,int,int");
            return dataFrame;
        }

        private Frame<int, string> GetSimpleDataFrame()
        {
            var list = Enumerable.Range(1, 7).Select(i => new {Id = i, Number = i == 1 ? 1 : i*Math.Pow(10,i-1)+i});
            return Frame.FromRecords(list);
        }
    }
}
