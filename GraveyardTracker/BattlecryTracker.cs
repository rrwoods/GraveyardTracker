using System.Text;

namespace GraveyardTracker
{
    class BattlecryTracker : TrackerBase
    {
        public BattlecryTracker(TrackerPlugin plugin) : base(plugin) { }

        protected override void Register()
        {
            Hearthstone_Deck_Tracker.API.GameEvents.OnPlayerPlay.Add(Cast);
        }

        public override string[] TriggerCards => new string[] { "Shudderwock" };

        public override string Text
        {
            get
            {
                if (trackedCards.Count == 0)
                    return "Your battlecries: none";

                StringBuilder display = new StringBuilder();
                display.Append("Your battlecries:");
                foreach (string name in trackedCards.Keys)
                    display.Append("\n" + trackedCards[name] + " " + name);


                return display.ToString();
            }
        }

        private void Cast(Hearthstone_Deck_Tracker.Hearthstone.Card card)
        {
            if (card.Text.Contains("Battlecry:") ||
                card.Text.Contains("Battlecry and "))
                this.IncrementCard(card.Name);
        }
    }
}
