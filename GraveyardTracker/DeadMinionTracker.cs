using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraveyardTracker
{
    class DeadMinionTracker : TrackerBase
    {
        public DeadMinionTracker(TrackerPlugin plugin) : base(plugin) { }

        protected override void Register()
        {
            Hearthstone_Deck_Tracker.API.GameEvents.OnPlayerPlayToGraveyard.Add(Death);
        }

        public override string[] TriggerCards => new string[] {
            "Resurrect",
            "Onyx Bishop",
            "Kazakus",
            "Eternal Servitude",
            "Lesser Diamond Spellstone",
            "Diamond Spellstone",
            "Greater Diamond Spellstone",
        };

        public override string Text
        {
            get
            {
                if (this.trackedCards.Count == 0)
                    return "Dead minions: none";

                StringBuilder display = new StringBuilder();
                display.Append("Dead minions:");
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
