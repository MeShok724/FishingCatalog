using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingCatalog.Core
{
    public class EmailMessage(string email, string subject, string text)
    {
        public string Email { get; set; } = email;
        public string Subject { get; set; } = subject;
        public string Text { get; set; } = text;
    }
}
