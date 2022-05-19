using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eService.DTO.JSONSearializer
{
    public class ProfileDTO
    {
        public IDNo IDNo { get; set; }
    }

    public class IDNo
    {
        public string Identification { get; set; }

        public string CheckDigit { get; set; }
    }
}
