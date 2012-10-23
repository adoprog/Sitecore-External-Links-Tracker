namespace Analytics.ExternalLinksTracker
{
    using System;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using System.ServiceModel.Description;
    using System.Web;

    /// <summary>
    /// Defines the custom host factory class.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    public class CustomHostFactory : ServiceHostFactory
    {
        /// <summary>
        /// Creates a <see cref="T:System.ServiceModel.ServiceHost"/> for a specified type of service with a specific base address.
        /// </summary>
        /// <param name="serviceType">Specifies the type of service to host.</param>
        /// <param name="baseAddresses">The <see cref="T:System.Array"/> of type <see cref="T:System.Uri"/> that contains the base addresses for the service hosted.</param>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.ServiceHost"/> for the type of service specified with a specific base address.
        /// </returns>
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ServiceHost result;
            if (baseAddresses != null && baseAddresses.Length > 0 && HttpContext.Current != null
                && HttpContext.Current.Request != null)
            {
                Uri baseAddress = baseAddresses.First();
                UriBuilder httpUriBuilder = new UriBuilder(baseAddress)
                {
                    Scheme = Uri.UriSchemeHttp,
                    Port = 80,
                    Host = HttpContext.Current.Request.Url.Host
                };

                UriBuilder httpsUriBuilder = new UriBuilder(baseAddress)
                {
                    Scheme = Uri.UriSchemeHttps,
                    Port = 443,
                    Host = HttpContext.Current.Request.Url.Host
                };

                result = new ServiceHost(serviceType, httpUriBuilder.Uri, httpsUriBuilder.Uri);
            }
            else
            {
                result = new ServiceHost(serviceType, baseAddresses);
            }

            //Add service endpoint and behavior to avoid error message if it's missing in web.config
            ServiceEndpoint endpoint = result.AddServiceEndpoint(serviceType, new WebHttpBinding(), string.Empty);
            endpoint.Behaviors.Add(new WebScriptEnablingBehavior());

            return result;
        }
    }
}