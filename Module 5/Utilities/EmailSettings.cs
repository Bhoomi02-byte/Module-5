﻿
namespace Module_5.Utilities
{
    internal class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public bool EnableSsl { get; set; }


        public string Password { get; set; }
    }
}