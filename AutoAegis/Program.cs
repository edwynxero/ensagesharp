using System;
using System.Linq;
using Ensage;
using Ensage.Common.Extensions;
using SharpDX;

namespace AutoAegis
{
    internal class Program
    {
        public static bool AutoAegisEnabled = true;
        private static void Main()
        {
            Game.OnUpdate += Game_OnUpdate;
            Game.OnFireEvent += Game_OnFireEvent;
        }

        private static void Game_OnFireEvent(FireEventEventArgs args)
        {
            var player = ObjectMgr.LocalPlayer;
            if (player == null || !AutoAegisEnabled || player.Hero == null)
                return;

            if (args.GameEvent.Name == "dota_roshan_kill")
                AutoAegis();
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            var hero = ObjectMgr.LocalHero;
            if (hero == null || !AutoAegisEnabled)
                return;
            
            var roshanItems = ObjectMgr.GetEntities<PhysicalItem>().Where(x => x.IsVisible && GetDistance2D(x.NetworkPosition, hero.NetworkPosition) < 400 && (x.Item.Name == "item_aegis" || x.Item.Name == "item_cheese")).ToList();

            if (!roshanItems.Any() && !hero.Inventory.FreeSlots.Any()) return;
            hero.PickUpItem(roshanItems.First());
        }

        private static void AutoAegis()
        {
            var hero = ObjectMgr.LocalHero;
            if (hero == null || !AutoAegisEnabled)
                return;
            var heroId = hero.ClassID;

            var blinkDagger = hero.FindItem("item_blink");
            var aegisLoc = new Vector3(4150, -1880, 0);
            var spellLoc = new Vector3(4077, -2143, 0);

            var aegisDistance = GetDistance2D(aegisLoc, hero.NetworkPosition);
            if (aegisDistance < 400)
                return;

            if (aegisDistance <= 1200 && blinkDagger != null && blinkDagger.CanBeCasted())
                CastSpell(blinkDagger, aegisLoc);
            else if (heroId == ClassID.CDOTA_Unit_Hero_EmberSpirit && GetDistance2D(spellLoc, hero.NetworkPosition) <= 700)
                CastSpell(hero.Spellbook.Spell2, spellLoc);
            else if (heroId == ClassID.CDOTA_Unit_Hero_AntiMage && aegisDistance <= 1150)
                CastSpell(hero.Spellbook.Spell2, aegisLoc);
            else if (heroId == ClassID.CDOTA_Unit_Hero_Rattletrap && aegisDistance <= 2000)
                CastSpell(hero.Spellbook.Spell4, aegisLoc);
            else if (heroId == ClassID.CDOTA_Unit_Hero_FacelessVoid && aegisDistance <= 1300)
                CastSpell(hero.Spellbook.Spell1, aegisLoc);
            else if (heroId == ClassID.CDOTA_Unit_Hero_Magnataur && aegisDistance <= 1200)
                CastSpell(hero.Spellbook.Spell3, aegisLoc);
            else if (heroId == ClassID.CDOTA_Unit_Hero_SandKing && aegisDistance <= 650)
                CastSpell(hero.Spellbook.Spell1, aegisLoc);
            else if (heroId == ClassID.CDOTA_Unit_Hero_QueenOfPain && aegisDistance <= 1150)
                CastSpell(hero.Spellbook.Spell2, aegisLoc);
            else if (heroId == ClassID.CDOTA_Unit_Hero_Morphling && aegisDistance <= 1000)
                CastSpell(hero.Spellbook.Spell1, aegisLoc);
            else if (heroId == ClassID.CDOTA_Unit_Hero_Naga_Siren && aegisDistance <= 1250)
                CastSpell(hero.Spellbook.Spell4, aegisLoc);
            else if (heroId == ClassID.CDOTA_Unit_Hero_StormSpirit && aegisDistance <= 1000)
                CastSpell(hero.Spellbook.Spell4, aegisLoc);
        }


        private static float GetDistance2D(Vector3 p1, Vector3 p2)
        {
            return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        private static void CastSpell(Ability spell, Vector3 aegisLocation)
        {
            if (spell.CanBeCasted())
            {
                spell.UseAbility(aegisLocation);
            }
        }
    }
}
