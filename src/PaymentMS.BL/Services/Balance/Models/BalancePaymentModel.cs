using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using PaymentMS.DAL.Entities;
using PaymentMS.DAL.Enums;
using Stripe;

namespace PaymentMS.BL.Services.Balance.Models
{
    public class BalancePaymentModel
    {
        public PaymentMethodType PaymentMethod { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentCurrency { get; set; }
        public DateTime CreatedDate { get; set; }
        public PaymentStatusType Status { get; set; }

        public static Expression<Func<PaymentEntity, BalancePaymentModel>> Map()
        {
            return entity => new BalancePaymentModel
            {
                PaymentMethod = entity.PaymentMethod,
                PaymentCurrency = entity.PaymentCurrency,
                PaymentAmount = entity.PaymentAmount,
                CreatedDate = entity.CreatedDateUtc,
                Status = entity.Status
            };
        }
    }
}
