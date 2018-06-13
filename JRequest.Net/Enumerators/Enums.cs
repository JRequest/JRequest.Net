namespace JRequest.Net.Enumerators
{
    public enum Protocol
    {
        http,
        https,
        ftp
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
        json,
        xml
    }

    public enum RequestType
    {
        input,
        output
    }
}
