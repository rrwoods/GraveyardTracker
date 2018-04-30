using System.Text;

namespace GraveyardTracker
{
    class DeathrattleTracker : TrackerBase
    {
        public DeathrattleTracker(TrackerPlugin plugin) : base(plugin) { }

        protected override void Register()
        {
            Hearthstone_Deck_Tracker.API.GameEvents.OnPlayerPlayToGraveyard.Add(Death);
        }

        public override string[] TriggerCards => new string[] { "N'Zoth, the Corruptor", "Tomb Lurker", "Twilight's Call" };

        public override string Text
        {
            get
            {
                if (this.trackedCards.Count == 0)
                    return "Dead deathrattles: none";

                StringBuilder display = new StringBuilder();
                display.Append("Dead deathrattles:");
                foreach (string name in trackedCards.Keys)
                    display.Append("\n" + trackedCards[name] + " " + name);

                return display.ToString();
            }
        }

        private void Death(Hearthstone_Deck_Tracker.Hearthstone.Card card)
        {
            if (card.Type.Equals("Minion") && card.Text.ToLower().Contains("deathrattle:"))
                this.IncrementCard(card.Name);
        }
    }
}
