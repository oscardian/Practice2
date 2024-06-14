using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class Result
    {
        public Boolean IsSuccess => Errors.Length == 0;
        public String[] Errors { get; set; }

        public Result(String error)
        {
            Errors = new String[] { error };
        }

        public Result()
        {
            Errors = Array.Empty<String>();
        }

        public static Result Success() => new Result();

        public static Result Fail(String error) => new Result(error);

    }
}
