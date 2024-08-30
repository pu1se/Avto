using System.Text;
using AutoTestic.RethinkUI._Core;
using TechTalk.SpecFlow;

namespace AutoTestic.RethinkUI.Steps
{
    [Binding]
    public class WhenActionSteps : BaseSteps
    {
        public WhenActionSteps(WebPage page) : base(page)
        {
        }

        [When ("setup base url {(.*)}")]
        public async Task WhenSetupBaseUrl(string baseUrl)
        {
            Console.WriteLine(GetStepName(baseUrl));
            Page.BaseUrl = baseUrl;
        }

        [When("log {(.*)}")]
        public async Task WhenLog(string cssSelector)
        {
            var html = await Page.SelectVisibleAndHiddenElement(cssSelector.NormalizeCssSelector()).Html();

            Console.WriteLine($"Log: {html}");
        }

        [When(@"open {(.*)}")]
        public async Task OpenPage(string url)
        {
            var isAbsoluteUrl = url.StartsWith("http");
            var isRelativeUrl = !isAbsoluteUrl;
            await Page.OpenUrl(url, isRelativeUrl);
        }


        [When(@"click {(.*)}")]
        public async Task Click(string cssSelector)
        {
            var isElementVisible = await Page.WaitUntilElementIsVisible(cssSelector);
            if (!isElementVisible)
            {
                await Page.MakeScreenShot();
                throw new Exception($"I can't click on element {cssSelector}, because it is not visible.");
            }

            await Page.SelectVisibleElement(cssSelector).Click();

            await Page.WaitFor(1);
        }


        [When(@"type in {(.*)} text {(.*)}")]
        public async Task TypeText(string cssSelector, string text)
        {
            var isElementVisible = await Page.WaitUntilElementIsVisible(cssSelector);
            if (!isElementVisible)
            {
                await Page.MakeScreenShot();
                throw new Exception($"I can't type text for element {cssSelector}, because it is not visible.");
            }
            
            await Page.SelectVisibleElement(cssSelector).Value(text);
        }

        
        [When(@"type in {(.*)} random number")]
        public async Task TypeRandomNumber(string cssSelector)
        {
            var isElementVisible = await Page.WaitUntilElementIsVisible(cssSelector);
            if (!isElementVisible)
            {
                await Page.MakeScreenShot();
                throw new Exception($"I can't type random number for element {cssSelector}, because it is not visible.");
            }


            var element = Page.SelectVisibleElement(cssSelector);
            var currentText = await element.Value();
            int randomNumber;

            do
            {
                randomNumber = new Random().Next(1, 100);
            } while (randomNumber.ToString() == currentText);


            await Page.SelectVisibleElement(cssSelector).Value(randomNumber.ToString());
        }

        
        [When(@"type in {(.*)} plus one")]
        public async Task TypeDoubleNumber(string cssSelector)
        {
            var isElementVisible = await Page.WaitUntilElementIsVisible(cssSelector);
            if (!isElementVisible)
            {
                await Page.MakeScreenShot();
                throw new Exception($"I can't type random number for element {cssSelector}, because it is not visible.");
            }


            var element = Page.SelectVisibleElement(cssSelector);
            var currentText = await element.Value();
            var currentNumber = int.Parse(currentText);
            var newNumber = currentNumber + 1;

            await Page.SelectVisibleElement(cssSelector).Value(newNumber.ToString());
        }

        
        [When(@"type in {(.*)} remembered {(.*)}")]
        public async Task EnterRememberedText(string cssSelector, string name)
        {
            var text = Page.Remember[name];

            if (text.IsNullOrEmpty())
            {
                throw new Exception($"Remembered text {name} is null or empty.");
            }
            
            var isElementVisible = await Page.WaitUntilElementIsVisible(cssSelector);
            if (!isElementVisible)
            {
                await Page.MakeScreenShot();
                throw new Exception($"I can't type into element {cssSelector}, because it is not visible.");
            }

            await Page.SelectVisibleElement(cssSelector).Value(text);
        }


        [When(@"type in {(.*)} enter")]
        public async Task ClickEnter(string cssSelector)
        {
            var isElementVisible = await Page.WaitUntilElementIsVisible(cssSelector);
            if (!isElementVisible)
            {
                await Page.MakeScreenShot();
                throw new Exception($"I can't type enter for element {cssSelector}, because it is not visible.");
            }
            
            await Page.SelectVisibleElement(cssSelector).PressKeyEnter();
            await Page.WaitFor(1);
        }


        [When(@"remember text in {(.*)} as {(.*)}")]
        public async Task RememberText(string cssSelector, string name)
        {
            var value = await Page.SelectVisibleAndHiddenElement(cssSelector).Value();
            Page.Remember[name] = value;
            Console.WriteLine($"Remembered {value} as {name}");
        }


        [When(@"run script {(.*)}")]
        public async Task RunScript(string script)
        {
            await Page.RunScript(script);
            Console.WriteLine($"Run script {script}");
        }

        
        [When(@"remember text in script {(.*)} as {(.*)}")]
        public async Task RememberTextFromScript(string script, string name)
        {
            var value = await Page.RunScriptAndGetValue<string>(script);
            Page.Remember[name] = value;
            Console.WriteLine($"Remembered {value} as {name}");
        }

        [When(@"make screenshot")]
        public async Task MakeScreenshot()
        {
            await Page.MakeScreenShot();
        }
    }
}
