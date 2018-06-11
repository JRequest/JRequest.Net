namespace JRequest.Net.Enumerators
{
    public enum Protocol
    {
        http,
        https,
        ftp
    }
    public enum RequestType
    {
        Output,
        Input
    }

    enum HttpMethod
    {
        GET,
        POST
    }
    public enum AuthType
    {
        NoAuth,
        Basic,
        Bearer,
        FtpLogin
    }
    public enum TokenExtension
    {
        BearerToken,
        Basic
    }

    public enum OutputType
    {
        joson,
        xml
    }
}
