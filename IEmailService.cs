using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment_SoftwareDevelopment
{
    interface IEmailService
    {
        void SendEmail(string emailAddress, string subject, string message);
    }
}
