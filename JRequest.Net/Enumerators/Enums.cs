﻿using System;
using System.Collections.Generic;
using System.Text;

namespace JRequest.Net.Enumerators
{
    public enum Protocol
    {
        http,
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