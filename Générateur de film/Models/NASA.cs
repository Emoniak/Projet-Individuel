using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Générateur_de_film
{
    public class NASA
    {
        public string callback { get; set; }
        public ContextWrites contextWrites { get; set; }
    }
    public class To
    {
        public string copyright { get; set; }
        public string date { get; set; }
        public string explanation { get; set; }
        public string hdurl { get; set; }
        public string media_type { get; set; }
        public string service_version { get; set; }
        public string title { get; set; }
        public string url { get; set; }
    }

    public class ContextWrites
    {
        public To to { get; set; }
    }
}