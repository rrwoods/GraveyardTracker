using System.Text;

namespace GraveyardTracker
{
    class TauntTracker : TrackerBase
    {
        public TauntTracker(TrackerPlugin plugin) : base(plugin) { }

        protected override void Register()
        {
            Hearthstone_Deck_Tracker.API.GameEvents.OnPlayerPlayToGraveyard.Add(Death);
        }

        public override string[] TriggerCards => new string[] { "Hadronox" };
        public override string Text
        {
            get
            {
                if (this.trackedCards.Count == 0)
                    return "Dead taunts: none";

                StringBuilder display = new StringBuilder();
                display.Append("Dead taunts:");
                foreach (string name in trackedCards.Keys)
                    display.Append("\n" + trackedCards[name] + " " + name);

                return display.ToString();
            }
        }

        private void Death(Hearthstone_Deck_Tracker.Hearthstone.Card card)
        {
            if (!card.Type.Equals("Minion"))
                return;

            string cardText = card.Text.ToLower();
            // this condition is HARD to get right.  probably will get some false negatives and false positives until i nail it down.
            if (
                cardText.StartsWith("taunt") ||
                // cardText.Contains("taunt,") || // false positive on Spiked Hogrider; don't need it because ", taunt" covers everything
                cardText.Contains("taunt. ") ||
                cardText.Contains(", taunt") || // Al'Akir, Wickerflame
                cardText.Contains(". taunt") || // Tirion
                cardText.Contains("taunt\n")
            )
                this.IncrementCard(card.Name);

            // just to be clear: we can't just say "taunt" and be done with it; things like Lone Champion don't count, for example.
        }
    }
}
