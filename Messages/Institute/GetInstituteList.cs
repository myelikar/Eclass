using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Institute
{
    public record GetInstituteList
    {
        public int pageSize { get; set; }
        public int pageNo { get; set; }
    }
}
