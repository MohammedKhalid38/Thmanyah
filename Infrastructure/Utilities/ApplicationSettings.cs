namespace Infrastructure.Utilities;

public class ApplicationSettings
{
    public static string WebsiteLink { get { return "ApplicationSettings:WebsiteLink"; } }
    public static string SiteName { get { return "ApplicationSettings:siteName"; } }
    public static string SiteDescription { get { return "ApplicationSettings:siteDescription"; } }
    public static string SiteMediaFolder { get { return "ApplicationSettings:siteMediaFolder"; } }
    public static string SiteMediaMainFolder { get { return "ApplicationSettings:siteMediaMainFolder"; } }
    public static string MaxMediaFileSizeInMB { get { return "ApplicationSettings:maxMediaFileSizeInMB"; } }
    public static string MediaFileDocumentExtensions { get { return "ApplicationSettings:mediaFileDocumentExtensions"; } }
    public static string MediaFileExtensions { get { return "ApplicationSettings:mediaFileExtensions"; } }
    public static string MediaFileImageExtensions { get { return "ApplicationSettings:mediaFileImageExtensions"; } }
    public static string MediaFileVideoExtensions { get { return "ApplicationSettings:mediaFileVideoExtensions"; } }
}
