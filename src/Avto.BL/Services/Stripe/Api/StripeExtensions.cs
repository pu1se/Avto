﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Avto.BL.Services.Stripe.Api;
using Avto.DAL.Entities;

namespace Avto.BL.Services.Stripe
{
    public static class StripeExtensions
    {
        public static StripePayment Stripe(this PaymentEntity paymentEntity)
        {
            return new StripePayment(paymentEntity);
        } 
    }

    public class StripePayment
    {
        private PaymentEntity PaymentEntity { get; }

        public StripePayment(PaymentEntity paymentEntity)
        {
            PaymentEntity = paymentEntity;
        }

        public StripeChargeTransactionModel Charge
        {
            get
            {
                return DeserializePaymentJson(PaymentEntity.TransactionLog);
            }
        }

        public static StripeChargeTransactionModel DeserializePaymentJson(string paymentAsJson)
        {
            if (paymentAsJson.IsNullOrEmpty())
            {
                return null;
            }

            return JsonConvert.DeserializeObject<StripeChargeTransactionModel>(paymentAsJson);
        }
    }
}