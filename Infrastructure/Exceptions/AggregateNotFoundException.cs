using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class AggregateNotFoundException : ApplicationException
    {
        public AggregateNotFoundException(string message) : base(message) { }
    }
}
