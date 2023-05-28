using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remore.WinUI.Models
{
    public class SigninResponse
    {
        public string AccessToken { get; set; }
        public User User { get; set; }
    }
}
