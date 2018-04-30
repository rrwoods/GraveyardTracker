using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraveyardTracker
{
    class DestroyedWeaponTracker : TrackerBase
    {
        public DestroyedWeaponTracker(TrackerPlugin plugin) : base(plugin) { }

        protected override void Register()
        {
            Hearthstone_Deck_Tracker.API.GameEvents.OnPlayerPlayToGraveyard.Add(Death);
        }

        public override string[] TriggerCards => new string[] { "Rummaging Kobold" };

        public override string Text
        {
            get
            {
                if (this.trackedCards.Count == 0)
                    return "Destroyed weapons: none";

                StringBuilder display = new StringBuilder();
                display.Append("Destroyed weapons:");
                foreach (string name in trackedCards.Keys)
                    display.Append("\n" + trackedCards[name] + " " + name);

                return display.ToString();
            }
        }

        private void Death(Hearthstone_Deck_Tracker.Hearthstone.Card card)
        {
            if (card.Type.Equals("Weapon"))
                this.IncrementCard(card.Name);
        }
    }
}
