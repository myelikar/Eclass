using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.User
{
    public record GetUserList
    {
        public int pageSize { get; set; }
        public int pageNo { get; set; }
    }
}
