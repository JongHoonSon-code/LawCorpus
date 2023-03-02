using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawCorpus.Dto
{
    public class PapagoDto
    {
        public PapagoMessage message { get; set; }
    }

    public class PapagoParam
    {
        public string source { get; set; }
        public string target { get; set; }
        public string text { get; set; }
    }

    public class PapagoResult
    {
        public string translatedText { get; set; }
        public string srcLangType { get; set; }
    }

    public class PapagoMessage
    {
        public string type { get; set; }
        public string service { get; set; }
        public string version { get; set; }
        public PapagoResult result { get; set; }
    }
}
