﻿using System;
using System.Collections.Generic;
using System.Text;
using Avto.BL.Services.Stripe.Handlers.Queries.DefaultSendingWay;
using Avto.BL.Services.Stripe.Models;
using Avto.DAL.Entities;

namespace Avto.BL.Services.Stripe.Handlers.Commands.AddEditSendingWay
{
    public class AddEditSendingWayCommandResponse
    {
        public Guid SenderOrganizationId { get; set; }
        public Guid ReceiverOrganizationId { get; set; }
        public Guid ReceivingWayId { get; set; }
        public StripeCardModel Card { get; set; }
        public Guid Id { get; set; }
    }

    public static partial class MappingHelper{

        public static AddEditSendingWayCommandResponse ToSendingWayModel(this SendingWayEntity entity)
        {
            return new AddEditSendingWayCommandResponse
            {
                Id = entity.Id,
                SenderOrganizationId = entity.OrganizationId,
                ReceiverOrganizationId = entity.ReceivingWay.OrganizationId,
                ReceivingWayId = entity.ReceivingWay.Id,
                Card = entity.Configuration.ToStripeCard()
            };
        }
    }
}