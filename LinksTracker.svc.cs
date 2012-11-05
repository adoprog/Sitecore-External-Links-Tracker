namespace Analytics.ExternalLinksTracker
{
  using System.ServiceModel;
  using System.ServiceModel.Activation;
  using System.ServiceModel.Web;
  using System.Web;
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
    [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json)]
    public void TrackExternalLink(string url)
    {
      this.TrackExternalLinkAdvanced("External Link", url);
    }

    /// <summary>
    /// Tracks the external link and registers goal or event
    /// </summary>
    /// <param name="name">The name of the goal or event.</param>
    /// <param name="url">The URL.</param>
    [OperationContract]
    [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json)]
    public void TrackExternalLinkAdvanced(string name, string url)
    {
      if (!Tracker.IsActive)
      {
        Tracker.StartTracking();
      }

      Tracker.CurrentPage.Register(name, HttpUtility.HtmlEncode(url));
      Tracker.CurrentPage.Url = string.Format("<a href=\"{0}\">{0}</a>", HttpUtility.HtmlEncode(url));
      Tracker.CurrentPage.Duration = 1; 
    }
  }
}
