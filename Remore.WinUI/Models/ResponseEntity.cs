using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remore.WinUI.Models
{
    public class ResponseEntity<T>
    {
        public bool Ok { get; init; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }
    }
}
