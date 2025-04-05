using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avto.DAL;
using System.IO;

namespace Avto.BL.Services.ParseWebPage
{
    public class ParseWebPageService : BaseService
    {
        public ParseWebPageService(Storage storage, IServiceProvider services) : base(storage, services)
        {
        }

        public async Task ParseBelavia()
        {
            try
            {
                using var Page = new WebPage();
                await Page.OpenUrl("https://belavia.by/novosti/", false);
                var headers = await Page.SelectAll(".news-list li");
                var list = headers.Take(1).ToArray();
                foreach (var header in list)
                {
                    var dateElement = header.QuerySelectorAsync(".dt").Result?.InnerTextAsync().Result;
                    var dayAsString = dateElement?.Split(' ')[0];
                    int.TryParse(dayAsString, out var dayAsInt);
                    var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, dayAsInt);

                    //if (DateTime.UtcNow.AddDays(-3) > date || DateTime.UtcNow < date)
                    //{
                    //    continue;
                    //}

                    var text = header.QuerySelectorAsync("a").Result?.InnerTextAsync().Result;
                    text = text.ToLower().Trim();

                    if (text.Contains("дари")
                        ||
                        text.Contains("скидк")
                        ||
                        text.Contains("акци")
                        ||
                        text.Contains("промо")
                        ||
                        text.Contains("распрод"))
                    {
                        text += Environment.NewLine + " Ссылка: https://belavia.by/novosti/";
                        await SendEmail.ToSubscribedPeople("Notify about air plain discount", text);
                    }                
                    else
                    {
                        await SendEmail.ToMyself("Belavia", "No discount found in" + text);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = exception.ToFormattedExceptionDescription();
                message += Environment.NewLine + LogAllFiles();
                await SendEmail.ToMyself("Error in Parser", message);
            }
        }

        static string LogAllFiles()
        {
            string solutionFolder = FindSolutionFolder();
            if (solutionFolder != null)
            {
                string allFilePaths = GetAllFilePathsInSolutionFolder(solutionFolder);
                return allFilePaths;
            }
            else
            {
                return "Solution folder not found.";
            }
        }

        static string FindSolutionFolder()
        {
            // Start from the execution directory
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Traverse up the directory tree to find the solution file (*.sln)
            while (currentDirectory != null && !Directory.GetFiles(currentDirectory, "*.exe").Any())
            {
                currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
            }

            return currentDirectory;
        }

        static string GetAllFilePathsInSolutionFolder(string solutionFolder)
        {
            // Get all files in the solution folder and its subdirectories
            string[] filePaths = Directory.GetFiles(solutionFolder, "*.*", SearchOption.AllDirectories);


            // Combine all file paths into a single string
            return string.Join(Environment.NewLine, filePaths) + Environment.NewLine + "Current exe path: " + Path.GetDirectoryName(typeof(ParseWebPageService).Assembly.Location);
        }
    }
}
