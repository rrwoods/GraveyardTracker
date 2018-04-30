using System.Collections.Generic;
using System.Linq;

namespace GraveyardTracker
{
    abstract class TrackerBase
    {
        private TrackerPlugin plugin;

        public TrackerBase(TrackerPlugin plugin)
        {
            Register();
            this.plugin = plugin;
            ShouldDisplay = false;
        }

        public virtual void Reset()
        {
            trackedCards.Clear();
            ShouldDisplay = false;
        }

        protected abstract void Register();

        public abstract string[] TriggerCards { get; }

        public virtual bool TriggerFromPlay => false;
        public virtual bool ShouldDisplay { get; set; }

        public abstract string Text { get; }

        protected SortedDictionary<string, int> trackedCards = new SortedDictionary<string,int>();

        protected void IncrementCard(string cardName)
        {
            if (trackedCards.Keys.Contains(cardName))
                trackedCards[cardName]++;
            else
                trackedCards[cardName] = 1;
            plugin.textDirty = true;
        }
    }
}
