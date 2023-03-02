using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawCorpus.Dto
{
    public class BleuParam
    {
        public List<string> Candidate { get; set; }
        public List<string> Reference { get; set; }
        public int SplitType { get; set; }
    }
}
