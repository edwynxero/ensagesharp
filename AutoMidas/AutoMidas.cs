// --→ Namespace: Auto Midas
namespace AutoMidas
{
    // --→ Libraries
    using Ensage;
    using Ensage.Common;
    using Ensage.Common.Extensions;
    using SharpDX;
    using System;
    using System.Linq;

    // --→ Class: Auto Midas
    internal class AutoMidas
    {
        // --→ Variables
        private static Hero mHero;
        private static bool heroCreateSideMessage, bearCreateSideMessage;
        static void Main()
        {
            Game.OnUpdate += Game_OnUpdate;
        }

        // --→ Function: Game_OnUpdate
        private static void Game_OnUpdate(EventArgs args)
        {
            if (!Game.IsInGame || Game.IsPaused || !Utils.SleepCheck("Auto Midas")) return;

            if (mHero == null) mHero = ObjectMgr.LocalHero;
            if (mHero == null) return;

            var midas = FindMidas(mHero);
            var midasOwner =  midas != null ? (Unit) midas.Owner : null;
            if (midasOwner != null && !midasOwner.IsChanneling() && !midasOwner.IsInvisible()) UseMidas(midas, midasOwner);
            Utils.Sleep(500, "Auto Midas");
        }

        // --→ Function: Find Midas
        private static Item FindMidas(Unit entity)
        {
            if (entity.ClassID.Equals(ClassID.CDOTA_Unit_Hero_LoneDruid))
            {
                var bear = ObjectMgr.GetEntities<Unit>().Where(unit => unit.ClassID.Equals(ClassID.CDOTA_Unit_SpiritBear) && unit.IsAlive && unit.Team.Equals(mHero.Team) && unit.IsControllable).ToList();
                var heroMidas = entity.FindItem("item_hand_of_midas");
                if (heroMidas.CanBeCasted()) return heroMidas;
                return bear.Any() ? bear.First().FindItem("item_hand_of_midas") : null;
            }
            else
            {
                var heroMidas = entity.FindItem("item_hand_of_midas");
                return heroMidas;
            }
        }

        // --→ Function: Use Midas
        private static void UseMidas(Ability midas, Unit owner)
        {
            if (midas.CanBeCasted() && owner.CanUseItems() && owner.Equals(mHero))
            {
                if (!heroCreateSideMessage)
                {
                    heroCreateSideMessage = true;
                    GenerateSideMessage(GetOwnerName(owner));
                }
                var creeps = ObjectMgr.GetEntities<Creep>().Where(creep => creep.Team != owner.Team && creep.IsAlive && creep.IsVisible && creep.IsSpawned && !creep.IsAncient && creep.Health > 0 && creep.Distance2D(owner) < midas.CastRange + 25).ToList();
                if (!creeps.Any()) return;
                midas.UseAbility(creeps.First());
            }
            else if(heroCreateSideMessage) heroCreateSideMessage = false;

            if (midas.CanBeCasted() && owner.CanUseItems() && owner.ClassID.Equals(ClassID.CDOTA_Unit_SpiritBear))
            {
                if (!bearCreateSideMessage)
                {
                    bearCreateSideMessage = true;
                    GenerateSideMessage(GetOwnerName(owner));
                }
                var creeps = ObjectMgr.GetEntities<Creep>().Where(creep => creep.Team != owner.Team && creep.IsAlive && creep.IsVisible && creep.IsSpawned && !creep.IsAncient && creep.Health > 0 && creep.Distance2D(owner) < midas.CastRange + 25).ToList();
                if (!creeps.Any()) return;
                midas.UseAbility(creeps.First());
            }
            else if(bearCreateSideMessage) bearCreateSideMessage = false;
        }

        // --→ Function: Get Midas Owner Name
        private static string GetOwnerName(Entity owner)
        {
            return owner.Equals(mHero) ? owner.Name.Replace("npc_dota_hero_", "") : "spirit_bear";
        }

        // --→ Function: Create Side Message
        private static void GenerateSideMessage(string unit)
        {
            var sideMessage = new SideMessage(unit, new Vector2(200, 60));
            sideMessage.AddElement(new Vector2(10, 10), new Vector2(72, 40), Drawing.GetTexture("materials/ensage_ui/heroes_horizontal/" + unit + ".vmat"));
            sideMessage.AddElement(new Vector2(85, 16), new Vector2(62, 31), Drawing.GetTexture("materials/ensage_ui/other/arrow_usual.vmat"));
            sideMessage.AddElement(new Vector2(145, 10), new Vector2(70, 40), Drawing.GetTexture("materials/ensage_ui/items/hand_of_midas.vmat"));
            sideMessage.CreateMessage();
        }
    }
}
