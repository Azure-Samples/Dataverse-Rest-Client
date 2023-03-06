namespace Dataverse.RestClient
{
    using Microsoft.Identity.Client;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class ConfidentialClientAuthDelegatingHandler : DelegatingHandler
    {
        private readonly IConfidentialClientApplication app;
        private readonly IEnumerable<string> scopes;

        public ConfidentialClientAuthDelegatingHandler(IConfidentialClientApplication app, IEnumerable<string> scopes)
        {
            this.app = app;
            this.scopes = scopes;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AuthenticationResult? result = null;
            try
            {
                result = await app.AcquireTokenForClient(scopes)
                                 .ExecuteAsync(cancellationToken);
            }
            catch (MsalUiRequiredException ex)
            {
                // The application doesn't have sufficient permissions.
                // - Did you declare enough app permissions during app creation?
                // - Did the tenant admin grant permissions to the application?
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                // Invalid scope. The scope has to be in the form "https://resourceurl/.default"
                // Mitigation: Change the scope to be as expected.
            }

            if (result != null)
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.AccessToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
