using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutBuddy.Common.Exceptions
{
    public class ForbiddenErrorException : Exception
    {
        public ForbiddenErrorException(string message) : base(message)
        {
        }

        public ForbiddenErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ForbiddenErrorException()
        {
        }
    }
}
