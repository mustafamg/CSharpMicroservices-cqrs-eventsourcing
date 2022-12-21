using Core.Events;
using System.Reflection;

namespace Cqrs.Core
{
    public class AggregateRoot
    {
        public Guid Id { get; protected set; }
        public int Version { get; set; }

        private readonly List<BaseEvent> changes = new();
        //private readonly ILogger logger;

        public AggregateRoot() { 
        //    this.logger =;
        }

        public List<BaseEvent> GetUncommittedChanges()
        {
            return changes;
        }

        public void CommitChanges()
        {
            changes.Clear();
        }

        protected void ApplyChange(BaseEvent evnt, Boolean isNewEvent)
        {
            try
            {
                var method = this.GetType().GetMethod("Apply",
                         BindingFlags.NonPublic | BindingFlags.Instance,
                         new Type[]{evnt.GetType()});
                method.Invoke(this, new[] { evnt });
            }
            catch (MissingMemberException)
            {
                //logger.LogWarning("'Apply' method was not found in the aggregate:{0}", evnt.ToString());
            }
            catch (Exception ex)
            {
                //logger.LogCritical("Error applying event to aggregate", ex);
            }
            finally
            {
                if (isNewEvent)
                {
                    changes.Add(evnt);
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