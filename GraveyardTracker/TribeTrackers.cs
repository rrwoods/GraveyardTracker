using System;
using System.Text;

namespace GraveyardTracker
{
    abstract class TribeTracker : TrackerBase
    {
        private String tribe;

        private bool bothPlayers;

        public TribeTracker(TrackerPlugin plugin, String tribe, bool bothPlayers) : base(plugin)
        {
            this.tribe = tribe;
            this.bothPlayers = bothPlayers;
        }

        protected override void Register()
        {
            Hearthstone_Deck_Tracker.API.GameEvents.OnPlayerPlayToGraveyard.Add(Death);
            if (this.bothPlayers)
                Hearthstone_Deck_Tracker.API.GameEvents.OnOpponentPlayToGraveyard.Add(Death);
        }

        public override string Text
        {
            get
            {
                String lowercaseTribe = tribe.ToLower();
                if (this.trackedCards.Count == 0)
                    return "Dead " + lowercaseTribe + "s: none";

                StringBuilder display = new StringBuilder();
                display.Append("Dead " + lowercaseTribe + "s:");
                foreach (string name in trackedCards.Keys)
                    display.Append("\n" + trackedCards[name] + " " + name);

                return display.ToString();
            }
        }

        private void Death(Hearthstone_Deck_Tracker.Hearthstone.Card card)
        {
            if (card.Race != null && card.Race.Equals(this.tribe))
                this.IncrementCard(card.Name);
        }
    }

    class MurlocTracker : TribeTracker
    {
        public MurlocTracker(TrackerPlugin plugin) : base(plugin, "Murloc", true) { }

        public override string[] TriggerCards => new string[] { "Anyfin Can Happen" };
    }

    class BeastTracker : TribeTracker
    {
        public BeastTracker(TrackerPlugin plugin) : base(plugin, "Beast", false) { }

        public override string[] TriggerCards => new string[] { "Abominable Bowman", "Witching Hour" };

        public override bool TriggerFromPlay => true;
    }

    class DemonTracker : TribeTracker
    {
        public DemonTracker(TrackerPlugin plugin) : base(plugin, "Demon", false) { }

        public override string[] TriggerCards => new string[] { "Bloodreaver Gul'dan" };
    }
}
