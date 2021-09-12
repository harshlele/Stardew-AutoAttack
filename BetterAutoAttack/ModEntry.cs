using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Monsters;
using StardewValley.Tools;


namespace BetterAutoAttack
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        private IReflectionHelper reflection;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.Input.ButtonPressed += Util.ButtonPressTest;
            helper.Events.GameLoop.UpdateTicking += this.onUpdateTicking;
            reflection = this.Helper.Reflection;
        }



        private void onUpdateTicking(object sender, UpdateTickingEventArgs args)
        {
            if (!Context.IsWorldReady) return;

            Farmer player = Game1.player;
            GameLocation location = Game1.player.currentLocation;
            List<NPC> monsters = getMonstersWithinDistance(player.getTileLocation(), 1, location);

            if (!player.isActive() || player.CurrentTool == null || 
                !(player.CurrentTool is MeleeWeapon) || monsters.Count <= 0 || player.IsBusyDoingSomething())
            {
                return;
            }

            Monster toAttack = (Monster) monsters[0];
            bool blockAttack = false;
            foreach (Monster m in monsters)
            {
                if(m.health < toAttack.health)
                {
                    toAttack = m;
                }
                if(Vector2.Distance(m.getTileLocation(), player.getTileLocation()) < 1){
                    toAttack = m;
                    blockAttack = true;
                    break;
                }
            }
            
            int direction = player.getGeneralDirectionTowards(toAttack.Position);
            if (player.FacingDirection != direction)
            {
                player.faceDirection(direction);   
            }


            if (!blockAttack) player.BeginUsingTool();
            else {
                int newDir = (direction + 1) % 4;
                player.tryToMoveInDirection(newDir, true, 0, false);
                ((MeleeWeapon)player.CurrentTool).animateSpecialMove(player);
            }
            
        }
        

        private static List<NPC> getMonstersWithinDistance(Vector2 tileLocation, int tilesAway, GameLocation environment)
        {
            List<NPC> chars = new List<NPC>();
            foreach (NPC c in environment.characters)
            {
                if (Vector2.Distance(c.getTileLocation(), tileLocation) <= (float)tilesAway && c.IsMonster && !c.GetType().Equals(Game1.player))
                {
                    chars.Add(c);
                }
            }
            return chars;
        }



    }
}