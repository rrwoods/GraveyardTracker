using System.Text;

namespace GraveyardTracker
{
    class DiscardedMinionTracker : TrackerBase
    {
        public DiscardedMinionTracker(TrackerPlugin plugin) : base(plugin) { }

        protected override void Register()
        {
            Hearthstone_Deck_Tracker.API.GameEvents.OnPlayerHandDiscard.Add(Death);
        }

        public override string[] TriggerCards => new string[] { "Cruel Dinomancer" };

        public override string Text
        {
            get
            {
                if (this.trackedCards.Count == 0)
                    return "Discarded minions: none";

                StringBuilder display = new StringBuilder();
                display.Append("Discarded minions:");
                foreach (string name in trackedCards.Keys)
                    display.Append("\n" + trackedCards[name] + " " + name);

                return display.ToString();
            }
        }

        private void Death(Hearthstone_Deck_Tracker.Hearthstone.Card card)
        {
            if (card.Type.Equals("Minion"))
                this.IncrementCard(card.Name);
        }
    }
}
