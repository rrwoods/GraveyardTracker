using System.Text;

namespace GraveyardTracker
{
    class BuffSpellTracker : TrackerBase
    {
        public BuffSpellTracker(TrackerPlugin plugin) : base(plugin) { }

        protected override void Register()
        {
            Hearthstone_Deck_Tracker.API.GameEvents.OnPlayerPlay.Add(Cast);
        }

        public override string[] TriggerCards => new string[] { "Lynessa Sunsorrow" };

        public override string Text
        {
            get
            {
                if (this.trackedCards.Count == 0)
                    return "Spells cast: none";

                StringBuilder display = new StringBuilder();
                display.Append("Spells cast:");
                foreach (string name in trackedCards.Keys)
                    display.Append("\n" + trackedCards[name] + " " + name);

                return display.ToString();
            }
        }

        private void Cast(Hearthstone_Deck_Tracker.Hearthstone.Card card)
        {
            // HDT doesn't let me figure out who you're targeting :(
            if (card.Type.Equals("Spell"))
                this.IncrementCard(card.Name);
        }
    }
}
