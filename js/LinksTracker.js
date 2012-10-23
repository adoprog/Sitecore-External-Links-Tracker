jQuery(document).ready(function() {
    jQuery.expr[':'].external = function(obj) {
        return !obj.href.match(/^mailto\:/)
                && (obj.hostname != location.hostname);
    };
    jQuery("a:external").click(function() {
        jQuery.ajax({
            type: "POST",
            url: "/sitecore modules/web/ExternalLinksTracker/LinksTracker.svc/TrackExternalLink",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"url": "' + jQuery(this).attr('href') + '"}'
        });
    });    
});