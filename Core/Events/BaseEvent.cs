using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Events
{
    public class BaseEvent{
        public BaseEvent(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; protected set; }
        public int Version { get; set; }
    }
}
