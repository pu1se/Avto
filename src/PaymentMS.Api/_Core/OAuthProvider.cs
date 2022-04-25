using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Configuration;

namespace PaymentMS.Api
{
    public class OAuthProvider : OpenIdConnectServerProvider
    {
        private AppSettings Settings { get; }

        public OAuthProvider(AppSettings appSettings)
        {
            Settings = appSettings;
        }

        public override Task ValidateTokenRequest(ValidateTokenRequestContext context)
        {
            if (!context.Request.IsClientCredentialsGrantType()
                && !context.Request.IsRefreshTokenGrantType())
            {
                context.Reject(OpenIdConnectConstants.Errors.UnsupportedGrantType);
                return Task.CompletedTask;
            }

            if (!context.ClientId.IsNullOrEmpty() && !context.ClientSecret.IsNullOrEmpty())
            {
                context.Validate();
            }

            return Task.CompletedTask;
        }


        public override Task HandleTokenRequest(HandleTokenRequestContext context)
        {
            if (context.Request.ClientId != Settings.AuthClientId || context.Request.ClientSecret != Settings.AuthClientSecret)
            {
                context.Reject(OpenIdConnectConstants.Errors.InvalidClient);
                return Task.CompletedTask;
            }

            var claims = new List<Claim>();
            claims.Add(new Claim(OpenIdConnectConstants.Claims.Subject, context.Request.ClientId));
            var identity = new ClaimsIdentity(claims, context.Scheme.Name);
            identity.AddClaim(
                ClaimTypes.Name, 
                context.Request.ClientId,
                OpenIdConnectConstants.Destinations.AccessToken,
                OpenIdConnectConstants.Destinations.IdentityToken);

            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                new AuthenticationProperties(),
                context.Scheme.Name);
    
            // Call SetScopes with the list of scopes you want to grant
            // (specify offline_access to issue a refresh token).
            ticket.SetScopes(
                OpenIdConnectConstants.Scopes.Profile,
                OpenIdConnectConstants.Scopes.OfflineAccess);
    
            context.Validate(ticket);

            return Task.CompletedTask;
        }
    }
}
