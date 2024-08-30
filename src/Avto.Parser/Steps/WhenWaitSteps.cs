using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoTestic.RethinkUI._Core;
using TechTalk.SpecFlow;

namespace AutoTestic.RethinkUI.Steps
{
    [Binding]
    public class WhenWaitSteps : BaseSteps
    {
        public WhenWaitSteps(WebPage page) : base(page)
        {
        }

        [When(@"wait for loader and {(.*)} and reload page {(.*)} if it is not visible")]
        public async Task ReloadPageIfElementIsNotVisible(string cssSelector, string url)
        {
            await WaitForLoader();
            var isElementVisible = await Page.WaitUntilElementIsVisible(cssSelector);
            if (isElementVisible == false)
            {
                await Page.MakeScreenShot();
                await Page.OpenUrl(url);
                await Page.WaitFor(2);
                Console.WriteLine($"Reloaded page {url} because element {cssSelector} is not visible.");
                await Page.MakeScreenShot();
            }
        }

        [When(@"wait for {(.*)} seconds")]
        public async Task WaitForSomeSeconds(int seconds)
        {
            await Page.WaitFor(seconds);
        }


        [When(@"wait for {(.*)}")]
        public async Task WaitFor(string cssSelector)
        {
            var isElementVisible = await Page.WaitUntilElementIsVisible(cssSelector);
            if (isElementVisible == false)
            {
                throw new Exception($"Element {cssSelector} is not visible.");
            }
        }


        [When(@"wait for one time loader")]
        public async Task WaitForOneTimeLoader()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            await Page.WaitForOneTimeLoader();
            await Page.WaitFor(2);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine($"WaitForOneTimeLoader: {elapsedMs} ms");
        }

        
        [When(@"wait for grid")]
        public async Task WaitForGrid()
        {
            await Page.WaitForGrid();
        }


        [When(@"wait for loader")]
        public async Task WaitForLoader()
        {
            await Page.WaitFor(1);
            await Page.WaitForLoader();
        }

        [When(@"wait for url contains {(.*)}")]
        public async Task WaitForUrl(string subUrl)
        {
            await Page.WaitForUrlContains(subUrl);
        }

        [When(@"wait for url NOT contains {(.*)}")]
        public async Task WaitForUrlNotContains(string subUrl)
        {
            await Page.WaitForUrlNotContains(subUrl);
        }
    }
}
