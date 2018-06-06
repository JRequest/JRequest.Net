using System;
using System.Collections.Generic;
using System.Text;

namespace JRequest.Net
{
    public class JRequestException : Exception
    {
        public JRequestException()
        {
        }

        public JRequestException(string message)
            : base(message)
        {
        }

        public JRequestException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
