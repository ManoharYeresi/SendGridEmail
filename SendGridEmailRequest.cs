using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendGrid
{
    public class SendGridEmailRequest
    {
        public List<Personalization> personalizations { get; set; }
        public From from { get; set; }
        public List<Content> content { get; set; }
    }
    public class Personalization
    {
        public Personalization()
        {
            to = new List<To>();
            cc = new List<CC>();
        }
        public List<To> to { get; set; }
        public List<CC> cc { get; set; }
        public string subject { get; set; }
    }
    public class To
    {
        public string email  { get; set; }
    }
    public class CC
    {
        public string email { get; set; }
    }
    public class From
    {
        public string email { get; set; }
    }
    public class Content
    {
        public string type { get; set; }
        public string value { get; set; }
    }

}
