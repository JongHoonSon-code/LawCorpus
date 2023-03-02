using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawCorpus.Dto
{
    public class GoogleDto
    {
        public GoogleData data { get; set; }
    }

    public class GoogleTranslation
    {
        public string translatedText { get; set; }
    }

    public class GoogleData
    {
        public IList<GoogleTranslation> translations { get; set; }
    }
}
