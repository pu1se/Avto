using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avto.DAL;
using Avto.DAL.Entities;
using Avto.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace Avto.BL.Services.Exchange.ExtractDataFromCsvFileCollection
{
    public class ExtractDataFromCsvFileCollectionCommandHandler : CommandHandler<EmptyCommand, CallResult>
    {
        private ExchangeService ExchangeService { get; }

        public ExtractDataFromCsvFileCollectionCommandHandler(Storage storage, LogService logService, ExchangeService exchangeService) : base(storage, logService)
        {
            ExchangeService = exchangeService;
        }

        protected override async Task<CallResult> HandleCommandAsync(EmptyCommand command)
        {
            await ExtractDataFromCsvFilesToDatabase();
            await MakeCortesianProductBasedOnExtractedData();

            return SuccessResult();
        }

        private async Task MakeCortesianProductBasedOnExtractedData()
        {
            var currentDate = new DateTime(1999, 1, 1);
            var baseCurrency = ExchangeService.GetBaseCurrency();
            var bulkAmount = 0;
            while (currentDate < new DateTime(2021, 1, 1))
            {
                var exchangeRatesForCurrentDate = await Storage.ExchangeRates.Where(x => x.ExchangeDate == currentDate).ToListAsync();
                if (exchangeRatesForCurrentDate.Count == 0)
                {
                    currentDate = currentDate.AddDays(1);
                    continue;
                }

                var exchangeRatesForCurrentDateForBaseCurrency = exchangeRatesForCurrentDate.Where(x => x.FromCurrencyCode == baseCurrency.ToString()).ToList();
                var csvCurrencies = new List<string>{ "USD", "EUR", "CHF", "GBP", "JPY"};
                var currencyList = EnumHelper.ToList<CurrencyType>().Select(x => x.ToString()).Where(x => csvCurrencies.Contains(x));
                
                foreach (var fromCurrency in currencyList)
                {
                    foreach (var toCurrency in currencyList)
                    {
                        var rate = Convert(
                            baseCurrency.ToString(), 
                            exchangeRatesForCurrentDateForBaseCurrency, 
                            fromCurrency, 
                            toCurrency, 
                            currentDate);

                        if (! await Storage.ExchangeRates.AnyAsync(
                                e => 
                                    e.FromCurrencyCode == fromCurrency 
                                    && 
                                    e.ToCurrencyCode == toCurrency 
                                    && 
                                    e.ExchangeDate == currentDate))
                        {
                            Storage.ExchangeRates.Add(new ExchangeRateEntity
                            {
                                FromCurrencyCode = rate.FromCurrency,
                                ToCurrencyCode = rate.ToCurrency,
                                ExchangeDate = rate.ExchangeDate,
                                Rate = rate.CloseRate,
                                MaxDayRate = rate.HighRate,
                                MinDayRate = rate.LowRate,
                            });
                            bulkAmount += 1;
                        }
                    }
                }
                if (bulkAmount > 100)
                {
                    await Storage.SaveChangesAsync();
                    bulkAmount = 0;
                }

                currentDate = currentDate.AddDays(1);
            }

            await Storage.SaveChangesAsync();
        }

        private ExchangeRate Convert(
            string baseCurrency,
            List<ExchangeRateEntity> exchangeRateList,
            string fromCurrency, 
            string toCurrency,
            DateTime exchangeDate)
        {
            decimal fromRate = 1;
            decimal fromHighRate = 1;
            decimal fromLowRate = 1;
            decimal toRate = 1;
            decimal toHighRate = 1;
            decimal toLowRate = 1;

            if (fromCurrency != baseCurrency)
            {
                var exchangeRateEntity = exchangeRateList.First(
                    e => 
                    e.FromCurrencyCode == baseCurrency &&
                    e.ToCurrencyCode == fromCurrency
                );
                fromRate = exchangeRateEntity.Rate;
                fromHighRate = exchangeRateEntity.MaxDayRate;
                fromLowRate = exchangeRateEntity.MinDayRate;
                if (exchangeRateEntity.ExchangeDate < exchangeDate)
                {
                    exchangeDate = exchangeRateEntity.ExchangeDate;
                }
            }

            if (toCurrency != baseCurrency)
            {
                var exchangeRateEntity = exchangeRateList.First(
                    e => 
                    e.FromCurrencyCode == baseCurrency &&
                    e.ToCurrencyCode == toCurrency
                );
                toRate = exchangeRateEntity.Rate;
                toHighRate = exchangeRateEntity.MaxDayRate;
                toLowRate = exchangeRateEntity.MinDayRate;
                if (exchangeRateEntity.ExchangeDate < exchangeDate)
                {
                    exchangeDate = exchangeRateEntity.ExchangeDate;
                }
            }

            var rate = toRate / fromRate;
            var highRate = toHighRate / fromHighRate;
            var lowRate = toLowRate / fromLowRate;
            if (lowRate > highRate)
            {
                (lowRate, highRate) = (highRate, lowRate);
            }

            if (toCurrency != fromCurrency)
            {
                rate = rate.ToRoundedRate();
                highRate = highRate.ToRoundedRate();
                lowRate = lowRate.ToRoundedRate();
            }

            return new ExchangeRate()
            {
                FromCurrency = fromCurrency,
                ToCurrency= toCurrency,
                CloseRate = rate,
                HighRate = highRate,
                LowRate = lowRate,
                ExchangeDate = exchangeDate
            };
        }

        private async Task ExtractDataFromCsvFilesToDatabase()
        {
            var currentDirectory = Directory.GetParent(Directory.GetCurrentDirectory());
            while (!currentDirectory.ToString().EndsWith(@"\src"))
            {
                currentDirectory = Directory.GetParent(currentDirectory.ToString());
            }

            currentDirectory = Directory.GetParent(currentDirectory.ToString());

            var directoryWithCsvFiles = Directory.GetDirectories(currentDirectory + "\\docs")?.FirstOrDefault();
            if (directoryWithCsvFiles == null)
            {
                return;
            }

            var csvFiles = Directory.GetFiles(directoryWithCsvFiles.ToString());
            var supportedCurrencies = EnumHelper.ToList<CurrencyType>().Select(x => x.ToString());
            foreach (var pathToFile in csvFiles)
            {
                // read rows from csv file
                var rows = await File.ReadAllLinesAsync(pathToFile);
                var firstLine = rows.First();
                var columns = firstLine.Split(',');
                var currencies = columns.Skip(1).Take(1).First().Split(" ").First().Split("/");
                var fromCurrency = currencies[0];
                var toCurrency = currencies[1];

                if (!supportedCurrencies.Contains(fromCurrency) || !supportedCurrencies.Contains(toCurrency))
                {
                    continue;
                }

                rows = rows.Skip(1).ToArray();
                var balkAmount = 0;
                foreach (var row in rows)
                {
                    var date = DateTime.Parse(row.Split(',').First());
                    var closeDayExchangeRate = decimal.Parse(row.Split(',').Skip(1).First());
                    var highestDayExchangeRate = decimal.Parse(row.Split(',').Skip(2).First());
                    var lowestDayExchangeRate = decimal.Parse(row.Split(',').Skip(3).First());

                    if (! await Storage.ExchangeRates.AnyAsync(
                            e => 
                                e.FromCurrencyCode == fromCurrency 
                                && 
                                e.ToCurrencyCode == toCurrency 
                                && 
                                e.ExchangeDate == date))
                    {
                        Storage.ExchangeRates.Add(new ExchangeRateEntity
                        {
                            FromCurrencyCode = fromCurrency,
                            ToCurrencyCode = toCurrency,
                            Rate = closeDayExchangeRate,
                            ExchangeDate = date,
                            MinDayRate = lowestDayExchangeRate,
                            MaxDayRate = highestDayExchangeRate
                        });
                    }
                    

                    balkAmount += 1;
                    if (balkAmount >= 100)
                    {
                        await Storage.SaveChangesAsync();
                        balkAmount = 0;
                    }
                }

                await Storage.SaveChangesAsync();
            }
        }

        private class ExchangeRate
        {
            public string FromCurrency { get; set; }
            public decimal CloseRate { get; set; }
            public decimal HighRate { get; set; }
            public decimal LowRate { get; set; }
            public string ToCurrency { get; set; }
            public DateTime ExchangeDate { get; set; }
        }
    }
}
