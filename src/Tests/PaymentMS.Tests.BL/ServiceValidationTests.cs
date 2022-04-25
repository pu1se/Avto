using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.BL.Services;
using PaymentMS.BL.Services.Stripe;
using PaymentMS.DAL;
using PaymentMS.Tests.BL.Base;

namespace PaymentMS.Tests.BL
{
    [TestClass]
    public class ServiceValidationTests : BaseServiceTests<StripeService>
    {
        [TestMethod]
        public async Task ValidationFaildOnPaymentCreation()
        {
            var result = await Service.AddEditSendingWay(new AddEditSendingWayCommand());

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.IsSuccess == false);
            Assert.IsTrue(result.ErrorMessage.IsNullOrEmpty() == false);
            Assert.IsTrue(result.ErrorType == ErrorType.ValidationError400);
        }

        [TestMethod]
        public async Task ModelIsNullValidationFail()
        {
            var result = await Service.AddEditSendingWay(null);

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.IsSuccess == false);
            Assert.IsTrue(result.ErrorType == ErrorType.ModelWasNotInitialized400);
        }

        [TestMethod]
        public async Task NotEmptyGuidValidationFail()
        {
            var model = new AddEditSendingWayCommand();
            model.SenderEmail = "tmp@tmp.tmp";
            model.SenderName = "tmp";
            model.Token = "tmp";
            model.SenderOrganizationName = "tmp";
            var result = await Service.AddEditSendingWay(model);

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.ErrorType == ErrorType.ValidationError400);
        }
    }
}
