using System;
using System.Linq;
using Ensage;
using Ensage.Common;
using Ensage.Common.Extensions;
using SharpDX;

namespace VenomancerWardControl
{
    internal class Program
    {
        private static readonly uint[] PlagueWardDamage = { 10, 19, 29, 38 };
        private static void Main()
        {
            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (!Game.IsInGame || !Utils.SleepCheck("VenomancerWardControl"))
                return;
            Utils.Sleep(125, "VenomancerWardControl");

            var me = ObjectMgr.LocalHero;
            if (me == null || me.ClassID != ClassID.CDOTA_Unit_Hero_Venomancer)
                return;

            var plagueWardLevel = me.FindSpell("venomancer_plague_ward").Level - 1;

            var enemies = ObjectMgr.GetEntities<Hero>().Where(hero => hero.IsAlive && !hero.IsIllusion && hero.IsVisible && hero.Team == me.GetEnemyTeam()).ToList();
            var creeps = ObjectMgr.GetEntities<Creep>().Where(creep => (creep.ClassID == ClassID.CDOTA_BaseNPC_Creep_Lane || creep.ClassID == ClassID.CDOTA_BaseNPC_Creep_Siege) && creep.IsAlive && creep.IsVisible && creep.IsSpawned).ToList();
            var plaguewards = ObjectMgr.GetEntities<Unit>().Where(unit => unit.ClassID == ClassID.CDOTA_BaseNPC_Venomancer_PlagueWard && unit.IsAlive && unit.IsVisible).ToList();

            if (!enemies.Any() || !creeps.Any() || !plaguewards.Any() || !(plagueWardLevel > 0))
                return;

            foreach (var enemy in enemies)
            {
                if (enemy.Modifiers.FirstOrDefault(modifier => modifier.Name == "modifier_venomancer_poison_sting_ward") == null && enemy.Health > 0)
                {
                    foreach (var plagueward in plaguewards)
                    {
                        if (GetDistance2D(enemy.Position, plagueward.Position) < plagueward.AttackRange && Utils.SleepCheck(plagueward.Handle.ToString()))
                        {
                            plagueward.Attack(enemy);
                            Utils.Sleep(1000, plagueward.Handle.ToString());
                        }
                    }
                }
            }

            foreach (var creep in creeps)
            {
                if (creep.Team == me.GetEnemyTeam() && creep.Health > 0 && creep.Health < (PlagueWardDamage[plagueWardLevel] * (1 - creep.DamageResist) + 20))
                    foreach (var plagueward in plaguewards)
                    {
                        if (GetDistance2D(creep.Position, plagueward.Position) < plagueward.AttackRange && Utils.SleepCheck(plagueward.Handle.ToString()))
                        {
                            plagueward.Attack(creep);
                            Utils.Sleep(1000, plagueward.Handle.ToString());
                        }
                    }
                else if (creep.Team == me.Team && creep.Health > (PlagueWardDamage[plagueWardLevel] * (1 - creep.DamageResist)) && creep.Health < (PlagueWardDamage[plagueWardLevel] * (1 - creep.DamageResist) + 88))
                    foreach (var plagueward in plaguewards)
                    {
                        if (GetDistance2D(creep.Position, plagueward.Position) < plagueward.AttackRange && Utils.SleepCheck(plagueward.Handle.ToString()))
                        {
                            plagueward.Attack(creep);
                            Utils.Sleep(1000, plagueward.Handle.ToString());
                        }
                    }
            }
        }
        private static float GetDistance2D(Vector3 p1, Vector3 p2)
        {
            return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
    }
}
