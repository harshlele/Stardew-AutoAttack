using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using Microsoft.Xna.Framework;

namespace AutoAttack
{
    static class Util
    {
        /// <summary>
        /// Used for generating monsters, doing attack/defense actions etc
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        public static void ButtonPressTest(object sender, ButtonPressedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            //do attack action
            if (e.Button == SButton.Z)
            {
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
                Game1.getFarm().characters.Add(new StardewValley.Monsters.GreenSlime(spawnAt * 64f,41)
                {
                    focusedOnFarmers = true,
                    wildernessFarmMonster = true
                });
            }
            //spawn a flying bat
            if (e.Button == SButton.O)
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

        public static void Log(ModEntry e,string s)
        {
            e.Monitor.Log(s, LogLevel.Debug);
        }

    }
}
