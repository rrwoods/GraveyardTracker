using System.Linq;

namespace GraveyardTracker
{
    class BigSpellTracker : TrackerBase
    {
        public BigSpellTracker(TrackerPlugin plugin) : base(plugin) { }

        protected override void Register()
        {
            Hearthstone_Deck_Tracker.API.GameEvents.OnPlayerPlay.Add(Cast);
        }

        public override string[] TriggerCards => new string[] { "Dragoncaller Alanna" };

        public override string Text => "Alanna summons " + this.trackedCards.Values.Sum() + " dragons";

        private void Cast(Hearthstone_Deck_Tracker.Hearthstone.Card card)
        {
            if (card.Type.Equals("Spell") && card.Cost >= 5)
                this.IncrementCard(card.Name);
        }
    }
}
