using System;
using System.Collections.Generic;
using System.Text;

namespace RSMailboxLibrary.Models
{
    public class MailboxModel
    {
        public int Id { get; set; }
        public string MailboxName { get; set; }
        public string MailAlias { get; set; }
        public string Password { get; set; }

    }
}
