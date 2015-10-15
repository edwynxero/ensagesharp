using System;
using System.Collections.Generic;
using System.Linq;
using Ensage;
using Ensage.Common;
using Ensage.Common.Extensions;
using SharpDX;


namespace VisibilityEnhanced
{
    class Program
    {
        private static readonly bool Visibility_Self = true;
        private static readonly bool Visibility_Ally = true;
        private static readonly bool Visibility_Ward = true;
        private static readonly bool Visibility_Creep = true;
        private static readonly bool Visibility_Mines = true;
        private static readonly bool Visibility_Roshan = true;
        private static readonly bool Visibility_Netural = true;
        private static readonly bool Visibility_Courier = true;
        private static readonly bool Visibility_Summons = true;
        private static readonly bool Visibility_Building = true;
        
        private static readonly Dictionary<Unit, ParticleEffect> VisibleUnit = new Dictionary<Unit, ParticleEffect>();
        
        private static Hero _me;

        static void Main(string[] args)
        {
            Game.OnUpdate += Visibility_OnUpdate;
        }

        private static void Visibility_OnUpdate(EventArgs args)
        {
            if (!Game.IsInGame || !Utils.SleepCheck("VisibilitySleep"))
                return;
            Utils.Sleep(250, "VisibilitySleep");

            _me = ObjectMgr.LocalHero;
            if (_me == null)
                return;

            if (Visibility_Self)
                HandleEffect(_me, "items_fx/aura_shivas");
            if (Visibility_Ally)
            {
                var allies = ObjectMgr.GetEntities<Hero>().Where(hero => hero.IsAlive && hero.Team == _me.Team).ToList();
                foreach (var ally in allies)
                    HandleEffect(ally, "items_fx/aura_shivas");
            }
            if (Visibility_Ward)
            {
                var wards = ObjectMgr.GetEntities<Unit>().Where(ward => (ward.ClassID == ClassID.CDOTA_NPC_Observer_Ward || ward.ClassID == ClassID.CDOTA_NPC_Observer_Ward_TrueSight) && ward.IsAlive && ward.Team == _me.Team).ToList();
                foreach (var ward in wards)
                    HandleEffect(ward, "items_fx/aura_shivas");
            }
            if (Visibility_Creep)
            {
                var creeps = ObjectMgr.GetEntities<Creep>().Where(creep => creep.IsAlive && creep.IsSpawned && creep.Team == _me.Team).ToList();
                foreach (var creep in creeps)
                    HandleEffect(creep, "items_fx/aura_shivas");
            }
            if (Visibility_Mines)
            {
                var mines = ObjectMgr.GetEntities<Unit>().Where(mine => mine.IsAlive && mine.ClassID == ClassID.CDOTA_NPC_TechiesMines && mine.Team == _me.Team).ToList();
                foreach (var mine in mines)
                    HandleEffect(mine, "items_fx/aura_shivas");
            }
            if (Visibility_Roshan)
            {
                var roshans = ObjectMgr.GetEntities<Unit>().Where(roshan => roshan.IsAlive && roshan.ClassID == ClassID.CDOTA_Unit_Roshan).ToList();
                foreach (var roshan in roshans)
                    HandleEffect(roshan, "items_fx/aura_shivas");
            }
            if (Visibility_Netural)
            {
                var neturals = ObjectMgr.GetEntities<Unit>().Where(netural => netural.IsAlive && netural.ClassID == ClassID.CDOTA_BaseNPC_Creep_Neutral).ToList();
                foreach (var netural in neturals)
                    HandleEffect(netural, "items_fx/aura_shivas");
            }
            if (Visibility_Courier)
            {
                var couriers = ObjectMgr.GetEntities<Unit>().Where(courier => courier.IsAlive && courier.ClassID == ClassID.CDOTA_Unit_Courier && courier.Team == _me.Team).ToList();
                foreach (var courier in couriers)
                    HandleEffect(courier, "items_fx/aura_shivas");
            }
            if (Visibility_Building)
            {
                var buildings = ObjectMgr.GetEntities<Building>().Where(building => building.IsAlive && building.Team == _me.Team).ToList();
                foreach (var building in buildings)
                    HandleEffect(building, "items2_fx/mjollnir_shield");
            }
            if (Visibility_Summons)
            {
                var broodSpiderlings = ObjectMgr.GetEntities<Unit>().Where(spiderling => spiderling.IsAlive && spiderling.ClassID == ClassID.CDOTA_Unit_Broodmother_Spiderling && spiderling.Team == _me.Team).ToList();
                foreach (var spiderling in broodSpiderlings)
                    HandleEffect(spiderling, "items_fx/aura_shivas");

                var beastmasterBoar = ObjectMgr.GetEntities<Unit>().Where(boar => boar.IsAlive && boar.ClassID == ClassID.CDOTA_Unit_Hero_Beastmaster_Boar && boar.Team == _me.Team).ToList();
                foreach (var boar in beastmasterBoar)
                    HandleEffect(boar, "items_fx/aura_shivas");

                var beastmasterHawk = ObjectMgr.GetEntities<Unit>().Where(hawk => hawk.IsAlive && hawk.ClassID == ClassID.CDOTA_Unit_Hero_Beastmaster_Hawk && hawk.Team == _me.Team).ToList();
                foreach (var hawk in beastmasterBoar)
                    HandleEffect(hawk, "items_fx/aura_shivas");

                var serpentWards = ObjectMgr.GetEntities<Unit>().Where(serpentWard => serpentWard.IsAlive && serpentWard.ClassID == ClassID.CDOTA_BaseNPC_ShadowShaman_SerpentWard && serpentWard.Team == _me.Team).ToList();
                foreach (var serpentWard in serpentWards)
                    HandleEffect(serpentWard, "items_fx/aura_shivas");

                var plagueWards = ObjectMgr.GetEntities<Unit>().Where(plagueWard => plagueWard.IsAlive && plagueWard.ClassID == ClassID.CDOTA_BaseNPC_Venomancer_PlagueWard && plagueWard.Team == _me.Team).ToList();
                foreach (var plagueWard in plagueWards)
                    HandleEffect(plagueWard, "items_fx/aura_shivas");

                var forgedSpirits = ObjectMgr.GetEntities<Unit>().Where(forgedSpirit => forgedSpirit.IsAlive && forgedSpirit.ClassID == ClassID.CDOTA_BaseNPC_Invoker_Forged_Spirit && forgedSpirit.Team == _me.Team).ToList();
                foreach (var forgedSpirit in forgedSpirits)
                    HandleEffect(forgedSpirit, "items_fx/aura_shivas");

                var misc = ObjectMgr.GetEntities<Unit>().Where(unit => unit.IsAlive && unit.ClassID == ClassID.CDOTA_BaseNPC && unit.Team == _me.Team).ToList();
                foreach (var unit in misc)
                    HandleEffect(unit, "items_fx/aura_shivas");

                var miscExtra = ObjectMgr.GetEntities<Unit>().Where(unit => unit.IsAlive && unit.ClassID == ClassID.CDOTA_BaseNPC_Additive && unit.Team == _me.Team).ToList();
                foreach (var unit in miscExtra)
                    HandleEffect(unit, "items_fx/aura_shivas");
            }
        }

        private static void HandleEffect(Unit unit, string effectName)
        {
            if (unit == null) return;
            Vector2 screenPos;

            ParticleEffect effect;

            var enemyPos = unit.Position + new Vector3(0, 0, unit.HealthBarOffset);
            if (Drawing.WorldToScreen(enemyPos, out screenPos) && unit.IsVisibleToEnemies && unit.IsAlive)
            {
                if (VisibleUnit.TryGetValue(unit, out effect)) return;
                effect = unit.AddParticleEffect("particles/" + effectName + ".vpcf");
                VisibleUnit.Add(unit, effect);
            }
            else
            {
                if (!VisibleUnit.TryGetValue(unit, out effect)) return;
                effect.Dispose();
                VisibleUnit.Remove(unit);
            }
        }
    }
}
