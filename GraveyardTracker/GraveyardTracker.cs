using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace GraveyardTracker
{
    public class TrackerPlugin : Hearthstone_Deck_Tracker.Plugins.IPlugin
    {
        private const int INFO_LEFT = 1470;
        private const int STARTING_HEIGHT = 2160;
        private const int HEIGHT_REDUCE_PER_LINE = 25;

        public string Author => "rrwoods";
        public string ButtonText => "GraveyardTracker";
        public string Description => "A plugin to remember relevant \"this game\" effects (like N'Zoth).";
        public System.Windows.Controls.MenuItem MenuItem => null;
        public string Name => "GraveyardTracker";
        public Version Version => new Version(0, 0, 1);

        private Hearthstone_Deck_Tracker.HearthstoneTextBlock info = new Hearthstone_Deck_Tracker.HearthstoneTextBlock();
#if DEBUG
        public static Hearthstone_Deck_Tracker.HearthstoneTextBlock versionInfo = new Hearthstone_Deck_Tracker.HearthstoneTextBlock();
#endif

        private List<TrackerBase> trackers = new List<TrackerBase>();
        public bool textDirty = false;

        public void OnLoad()
        {
            var canvas = Hearthstone_Deck_Tracker.API.Core.OverlayCanvas; 

#if DEBUG
            Canvas.SetTop(versionInfo, 15);
            Canvas.SetLeft(versionInfo, 215);
            canvas.Children.Add(versionInfo);
            versionInfo.Height = 1080;
            versionInfo.TextWrapping = System.Windows.TextWrapping.Wrap;
            versionInfo.Text = "GraveyardTracker " + Version;
#endif

            Canvas.SetTop(info, 0);
            Canvas.SetLeft(info, INFO_LEFT);
            canvas.Children.Add(info);
            info.Text = "";
            info.FontSize = 18;
            info.Height = STARTING_HEIGHT;
            info.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            info.TextWrapping = System.Windows.TextWrapping.Wrap;
            
            Hearthstone_Deck_Tracker.API.GameEvents.OnGameStart.Add(CommenceGame);
            Hearthstone_Deck_Tracker.API.GameEvents.OnPlayerDraw.Add(Draw);
            Hearthstone_Deck_Tracker.API.GameEvents.OnGameEnd.Add(ConcludeGame);

            trackers.Add(new DeadMinionTracker(this));
            trackers.Add(new DeathrattleTracker(this));
            trackers.Add(new DiscardedCardCounter(this));
            trackers.Add(new DiscardedMinionTracker(this));
            trackers.Add(new HeroPowerCounter(this));
            trackers.Add(new TauntTracker(this));
            trackers.Add(new TotemCounter(this));
            trackers.Add(new DestroyedWeaponTracker(this));
            trackers.Add(new ClassCardsTracker(this));

            trackers.Add(new MurlocTracker(this));
            trackers.Add(new BeastTracker(this));
            trackers.Add(new DemonTracker(this));
        }

        public void OnButtonPress()
        {

        }

        public void OnUpdate()
        {
            if (textDirty)
                RefreshDisplay();
        }

        public void OnUnload()
        {

        }

        private void CommenceGame()
        {
            foreach (TrackerBase tracker in trackers)
                tracker.Reset();
        }

        private void ConcludeGame()
        {
            info.Text = "";
        }



        private void Draw(Hearthstone_Deck_Tracker.Hearthstone.Card card)
        {
            string cardName = card.Name;
            foreach (TrackerBase tracker in trackers)
            {
                if (tracker.ShouldDisplay)
                    continue;

                if(tracker.TriggerCards.Contains(cardName) || checkDeckAndHand(tracker))
                {
                    tracker.ShouldDisplay = true;
                    this.textDirty = true;
                }
            }
        }

        private bool checkDeckAndHand(TrackerBase tracker)
        {
            foreach (Hearthstone_Deck_Tracker.Hearthstone.Card deckCard in Hearthstone_Deck_Tracker.API.Core.Game.Player.PlayerCardList)
                if (tracker.TriggerCards.Contains(deckCard.Name))
                    return true;

            foreach (Hearthstone_Deck_Tracker.Hearthstone.Card handCard in Hearthstone_Deck_Tracker.API.Core.Game.Player.CreatedCardsInHand)
                if (tracker.TriggerCards.Contains(handCard.Name))
                    return true;

            if (!tracker.TriggerFromPlay)
                return false;

            foreach (Hearthstone_Deck_Tracker.Hearthstone.Entities.Entity minion in Hearthstone_Deck_Tracker.API.Core.Game.Player.Board)
                if (tracker.TriggerCards.Contains(minion.Card.Name))
                    return true;

            return false;
        }

        public void RefreshDisplay()
        {
            StringBuilder displayBuilder = new StringBuilder();
            foreach (TrackerBase tracker in trackers)
                if(tracker.ShouldDisplay)
                    displayBuilder.Append(tracker.Text + "\n\n");

            string display = displayBuilder.ToString();
            int newlines = display.Count(c => c == '\n');

            this.info.Height = STARTING_HEIGHT - (HEIGHT_REDUCE_PER_LINE * (1 + newlines));
            this.info.Text = display.ToString();
            
#if DEBUG
            versionInfo.Text += "\nInfo height: " + this.info.Height;
#endif

            this.textDirty = false;
        }
    }
}
