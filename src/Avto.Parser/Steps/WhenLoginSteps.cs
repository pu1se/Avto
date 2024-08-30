using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoTestic.RethinkUI._Core;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace AutoTestic.RethinkUI.Steps
{
    [Binding]
    public class WhenLoginSteps : BaseSteps
    {
        public WhenLoginSteps(WebPage page) : base(page)
        {
        }

        /*private static Dictionary<string, object> localStorage = null;

        [When(@"login as seller admin")]
        public async Task LogInAsSellerAdmin()
        {
            if (localStorage == null)
            {
                await Page.OpenUrl("Login");
                await Page.WaitForLoader();
                await Page.Select("#login-content-way-RethinkUserDb").Click();
                await Page.Select("input[type='email']").Value(Page.Settings.SellerAdminAuthLogin);
                await Page.Select("input[type='password']").Value(Page.Settings.SellerAdminAuthPassword);
                await Page.Select("button[type='submit']").Click();
            
                await Page.WaitForUrlContains("/Catalog");
                await Page.WaitForLoader();


                var token = await Page.GetAuthorizationBearerTokenOf(UserRole.SellerAdmin);
                await Page.AddHeader("Authorization", token);
                var items = await Page.RunScriptAndGetValue<object>("() => {const items = { ...localStorage };return items;}");
                var json = items.ToJson();
                localStorage = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                foreach (var item in localStorage.ToDictionary(x => x.Key, x => x.Value))
                {
                    localStorage[item.Key] = item.Value;
                }
            }
            else
            {
                await Page.OpenUrl("/Catalog");
                await Page.WaitForLoader();

                // set local storage
                foreach (var item in localStorage)
                {
                    var value = item.Value.ToString().Replace("'", @"\'");
                    await Page.RunScript($"localStorage.setItem('{item.Key}', '{item.Value}');");
                }
                var token = await Page.GetAuthorizationBearerTokenOf(UserRole.SellerAdmin);
                await Page.AddHeader("Authorization", token);
                await Page.ReloadPage();
                await Page.OpenUrl("/Catalog");
                await Page.WaitFor(60);
            }
        }*/

        [When(@"login as seller admin")]
        public async Task LogInAsSellerAdmin()
        {
            await Page.OpenUrl("Login");
            await Page.WaitForLoader();
            await Page.SelectVisibleElement("#login-content-way-RethinkUserDb").Click();
            await Page.SelectVisibleElement("input[type='email']").Value(Page.Settings.SellerAdminAuthLogin);
            await Page.SelectVisibleElement("input[type='password']").Value(Page.Settings.SellerAdminAuthPassword);
            await Page.SelectVisibleElement("button[type='submit']").Click();
            
            await Page.WaitForUrlContains("/Catalog");
            await Page.WaitForLoader();
        }

        [When(@"login on behalf of customer")]
        public async Task LogInOnBehalfOfCustomer()
        {
            await Page.AddHeader("X-On-Behalf-Of", Page.Settings.CustomerOrganizationId.ToString());
            await Page.OpenUrl("/Catalog");
            await Page.WaitForLoader();
        }

        [When(@"login as customer admin")]
        public async Task LogInAsCustomerAdmin()
        {
            await Page.OpenUrl("Login");
            await Page.WaitForLoader();
            await Page.SelectVisibleElement("#login-content-way-RethinkUserDb").Click();
            await Page.SelectVisibleElement("input[type='email']").Value(Page.Settings.CustomerAdminAuthLogin);
            await Page.SelectVisibleElement("input[type='password']").Value(Page.Settings.CustomerAdminAuthPassword);
            await Page.SelectVisibleElement("button[type='submit']").Click();
    
            await Page.WaitForUrlContains("/Catalog");
            await Page.WaitForLoader();
        }
    }
}
