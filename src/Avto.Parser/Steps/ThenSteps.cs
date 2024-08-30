using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoTestic.RethinkUI._Core;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace AutoTestic.RethinkUI.Steps
{
    [Binding]
    public class ThenSteps : BaseSteps
    {
        public ThenSteps(WebPage page) : base(page)
        {
        }

        [Then("log {(.*)}")]
        public async Task ThenLog(string cssSelector)
        {
            var html = await Page.SelectVisibleElement(cssSelector.NormalizeCssSelector()).Html();
            Console.WriteLine($"Log: {html}");
        }

        [Then(@"can see url contains {(.*)}")]
        public async Task ThenUrlContains(string subUrl)
        {
            
            await CheckIsTrue(Page.Url.Trim().Contains(subUrl.Trim()), 
                GetStepName(subUrl) + $" is failed because current url is '{Page.Url}'");
        }


        [Then(@"can see {(.*)}")]
        public async Task ThenCanSee(string cssSelector)
        {
            await CheckIsTrue(await Page.IsSelectedElementExists(cssSelector),
                GetStepName(cssSelector) + " is failed");
        }

        [Then(@"can see {(.*)} is hidden")]
        public async Task ThenCanSeeElementIsHidden(string cssSelector)
        {
            Console.WriteLine(GetStepName(cssSelector));
            await Page.WaitUntilElementIsNotVisible(cssSelector);
        }

        [Then(@"can see in {(.*)} text {(.*)}")]
        public async Task ThenCanSeeWithText(string cssSelector, string textMustBeLikeThis)
        {
            var text = await Page.SelectVisibleElement(cssSelector).Value();
            var index = 0;
            while (text.ToLower() != textMustBeLikeThis.ToLower())
            {
                await Page.WaitFor(2);
                text = await Page.SelectVisibleElement(cssSelector).Value();

                if (index++ > 5)
                {
                    break;
                }
            }

            await CheckIsTrue(text.ToLower() == textMustBeLikeThis.ToLower(), 
                GetStepName(cssSelector, textMustBeLikeThis) + $" is failed because text is '{text}'");
        }

        [Then(@"can see in {(.*)} remembered {(.*)}")]
        public async Task ThenCanSeeRemembered(string cssSelector, string rememberedKey)
        {
            var text = await Page.SelectVisibleElement(cssSelector).Value();
            var remembered = Page.Remember[rememberedKey];
            var index = 0;

            while (text.ToLower() != remembered.ToLower())
            {
                await Page.WaitFor(2);
                text = await Page.SelectVisibleElement(cssSelector).Value();

                if (index++ > 5)
                {
                    break;
                }
            }
            
            var html = await Page.SelectVisibleElement(cssSelector).Html();
            await CheckIsTrue(text.ToLower() == remembered.ToLower(), 
                $"{GetStepName(cssSelector, rememberedKey)} is failed because remembered text {remembered} not equal to text {text}. Html is {html}");
        }

    }
}
