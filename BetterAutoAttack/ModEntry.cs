using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace BetterAutoAttack
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        private IReflectionHelper Reflection;
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.GameLoop.UpdateTicked += this.onUpdateTicked;
            Reflection = this.Helper.Reflection;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>
        /// Used for testing attack algorithm
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            //do attack action
            if(e.Button == SButton.Z)
            {
                log(Game1.player.FacingDirection.ToString());
                Game1.player.BeginUsingTool();
                
            }
            //do secondary attack function
            if (e.Button == SButton.P)
            { 
                ((StardewValley.Tools.MeleeWeapon)Game1.player.CurrentTool).animateSpecialMove(Game1.player);
            }

            //spawn a ground monster
            if (e.Button == SButton.V)
            {
                //Game1.getFarm().spawnGroundMonsterOffScreen();

                Rectangle r = Utility.getRectangleCenteredAt(Game1.player.getTileLocation(), 10);
                Vector2 spawnAt = Utility.getRandomPositionInThisRectangle(r, new Random());
                Game1.getFarm().characters.Add(new StardewValley.Monsters.GreenSlime(spawnAt * 64f, 41)
                {
                    focusedOnFarmers = true,
                    wildernessFarmMonster = true
                });
            }
            //spawn a flying bat
            if(e.Button == SButton.O)
            {
                Rectangle r = Utility.getRectangleCenteredAt(Game1.player.getTileLocation(), 10);
                Vector2 spawnAt = Utility.getRandomPositionInThisRectangle(r, new Random());
                Game1.getFarm().characters.Add(new StardewValley.Monsters.Bat(spawnAt * 64f, 41)
                {
                    focusedOnFarmers = true,
                    wildernessFarmMonster = true
                });
            }
        }

        private void onUpdateTicked(object sender, UpdateTickedEventArgs args)
        {

        }

        private void log(string s)
        {
            this.Monitor.Log(s, LogLevel.Debug);
        }
    }
}