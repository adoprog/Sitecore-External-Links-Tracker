namespace Analytics.ExternalLinksTracker
{
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using Sitecore.Analytics;

    /// <summary>
    /// Defines the links tracker class.
    /// </summary>
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class LinksTracker
    {
        /// <summary>
        /// Tracks the outgoing URL.
        /// </summary>
        /// <param name="url">The URL that should be stored.</param>
        [OperationContract]
        public void TrackExternalLink(string url)
        {
            AnalyticsTracker.StartTracking();
            var tracker = AnalyticsTracker.Current;
            if (tracker != null && tracker.PreviousPage != null && tracker.CurrentPage != null)
            {
                tracker.PreviousPage.TriggerEvent("External Link", url);
                tracker.CurrentPage.Url = url;
                tracker.Submit();
            }
        }
    }
}