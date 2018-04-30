using System.Collections.Generic;
using System.Text;

namespace GraveyardTracker
{
    class ClassCardsTracker : TrackerBase
    {
        public ClassCardsTracker(TrackerPlugin plugin) : base(plugin) { }

        protected override void Register()
        {
            Hearthstone_Deck_Tracker.API.GameEvents.OnPlayerPlay.Add(Cast);
        }

        public override string[] TriggerCards => new string[] { "Tess Greymane" };

        private Dictionary<string, Dictionary<string, int>> classCards = new Dictionary<string, Dictionary<string, int>>();

        public override void Reset()
        {
            classCards.Clear();
            ShouldDisplay = false;
        }

        public override string Text
        {
            get
            {
                string playerClass = Hearthstone_Deck_Tracker.API.Core.Game.Player.Class;

                StringBuilder display = new StringBuilder();
                display.Append("Other class's cards played:");
                foreach (string heroClass in classCards.Keys)
                    if (heroClass != playerClass)
                        foreach (string cardName in classCards[heroClass].Keys)
                            display.Append("\n" + classCards[heroClass][cardName] + " " + cardName);

                return display.ToString();
            }
        }

        private void Cast(Hearthstone_Deck_Tracker.Hearthstone.Card card)
        {
            // this class completely ignores trackedCards >.<
            // i wrote this after all the others.  i realized that TrackerBase should probably be composed instead of inherited
            // that would make this situation much better, since i wouldn't be inheriting a bunch of scaffolding i'm not using
            // alas, this is where i'm at now, and i'm not changing it for 10 other classes.  lesson learned for the future
            // remember kids:  composition > inheritance; inheritance is not for code reuse, composition is; etc etc etc.

            string cardClass = card.GetPlayerClass;
            if (!classCards.ContainsKey(cardClass))
                classCards[cardClass] = new Dictionary<string, int>();

            Dictionary<string, int> cardsForThatClass = classCards[cardClass];
            string cardName = card.Name;
            if (cardsForThatClass.ContainsKey(cardName))
                cardsForThatClass[cardName]++;
            else
                cardsForThatClass[cardName] = 1;
        }
    }
}
