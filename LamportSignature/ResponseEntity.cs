using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LamportSignature
{
    public class random
    {
        public List<int> data { get; set; }
        public string completionTime { get; set; }
    }

    public class result
    {
        public random random { get; set; }
        public int bitsUsed { get; set; }
        public int bitsLeft { get; set; }
        public int requestsLeft { get; set; }
        public int advisoryDelay { get; set; }
    }

    public class ResponseEntity
    {
        public string jsonrpc { get; set; }
        public result result { get; set; }
        public int id { get; set; }
    }
}
