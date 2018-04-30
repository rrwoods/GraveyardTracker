using System.Linq;

namespace GraveyardTracker
{
    class DiscardedCardCounter : TrackerBase
    {
        public DiscardedCardCounter(TrackerPlugin plugin) : base(plugin) { }

        protected override void Register()
        {
            Hearthstone_Deck_Tracker.API.GameEvents.OnPlayerHandDiscard.Add(Discard);
        }

        public override string[] TriggerCards => new string[] { "Blood-Queen Lana'thel" };

        public override string Text => "Discarded cards: " + this.trackedCards.Values.Sum();

        private void Discard(Hearthstone_Deck_Tracker.Hearthstone.Card card)
        {
            this.IncrementCard(card.Name);
        }
    }
}
