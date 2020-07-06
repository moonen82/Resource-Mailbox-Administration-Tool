using System;
using System.Collections.Generic;
using System.Text;

namespace RSMailboxLibrary.Models
{
    public class ResourceUser
    {
        public int Id { get; set; }
        public int MailboxId { get; set; }
        public int UserId { get; set; }
    }
}
