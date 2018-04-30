using System;
using System.Linq;

namespace GraveyardTracker
{
    class TotemCounter : TrackerBase
    {
        public TotemCounter(TrackerPlugin plugin) : base(plugin) { }

        protected override void Register()
        {
            Hearthstone_Deck_Tracker.API.GameEvents.OnPlayerPlay.Add(Summon);
            Hearthstone_Deck_Tracker.API.GameEvents.OnPlayerDeckToPlay.Add(Summon);
            Hearthstone_Deck_Tracker.API.GameEvents.OnPlayerCreateInPlay.Add(Summon);
        }

        public override string[] TriggerCards => new string[] { "Thing from Below" };
        public override string Text
        {
            get
            {
                int cost = Math.Max(6 - this.trackedCards.Values.Sum(), 0);
                return "Thing from Below costs (" + cost + ")";
            }
        }

        private void Summon(Hearthstone_Deck_Tracker.Hearthstone.Card card)
        {
            if (card.Race != null && card.Race.Equals("Totem"))
                this.IncrementCard(card.Name);
        }
    }
}
