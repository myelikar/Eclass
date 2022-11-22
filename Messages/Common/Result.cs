using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Common
{
    public class Result
    {
        public Result()
        {

        }
        internal Result(bool succeeded, IEnumerable<string> messages)
        {
            Succeeded = succeeded;
            Messages = messages.ToArray();
        }

        public bool Succeeded { get; set; }
        public int Id { get; set; }
        public string[] Messages { get; set; }

        public static Result Success()
        {
            return new Result(true, Array.Empty<string>());
        }

        public static Result Success(int id)
        {
            return new Result(true, Array.Empty<string>()) { Id = id };
        }

        public static Result Failure(IEnumerable<string> messages)
        {
            return new Result(false, messages);
        }
    }
}
