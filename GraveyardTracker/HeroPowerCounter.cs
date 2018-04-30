using System;

namespace GraveyardTracker
{
    class HeroPowerCounter : TrackerBase
    {
        private const string HERO_POWER = "_hero_power";

        public HeroPowerCounter(TrackerPlugin plugin) : base(plugin) { }

        protected override void Register()
        {
            Hearthstone_Deck_Tracker.API.GameEvents.OnPlayerHeroPower.Add(HeroPower);
        }

        public override string[] TriggerCards => new string[] { "Frost Giant" };

        public override string Text
        {
            get
            {
                int cost = Math.Max(10 - this.trackedCards[HERO_POWER], 0);
                return "Frost Giant costs (" + cost + ")";
            }
        }

        private void HeroPower()
        {
            IncrementCard(HERO_POWER);
        }
    }
}
