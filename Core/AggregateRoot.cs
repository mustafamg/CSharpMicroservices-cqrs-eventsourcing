using Core.Events;
using System.Reflection;

namespace Cqrs.Core
{
    public abstract class AggregateRoot
    {
        public Guid Id { get; protected set; }
        public int Version { get; set; }

        private readonly List<BaseEvent> _changes = new();
        //   private readonly Logger logger = Logger.getLogger(AggregateRoot.class.getName());

        public List<BaseEvent> GetUncommittedChanges()
        {
            return this._changes;
        }

        public void CommitChanges()
        {
            this._changes.Clear();
        }

        protected void ApplyChange(BaseEvent evnt, Boolean isNewEvent)
        {
            try
            {
                var method = evnt.GetType().GetMethod("Apply",
                         BindingFlags.NonPublic | BindingFlags.Instance);
                method.Invoke(this, new[] { evnt });
            }
            catch (MissingMemberException e)
            {
                //logger.log(Level.WARNING, MessageFormat.format("'Apply' method was not found in the aggregate:{0}", evnt.ToString()));
            }
            catch (Exception e)
            {
                //logger.log(Level.SEVERE, "Error applying event to aggregate", e);
            }
            finally
            {
                if (isNewEvent)
                {
                    _changes.Add(evnt);
                }
            }
        }

        public void RaiseEvent(BaseEvent evnt)
        {
            ApplyChange(evnt, true);
        }

        public void ReplayEvents(IEnumerable<BaseEvent> events)
        {
            foreach (var evnt in events)
            {
                ApplyChange(evnt, false);
            }
        }
    }
}