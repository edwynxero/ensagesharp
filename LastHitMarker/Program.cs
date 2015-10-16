using System;
using System.Collections.Generic;
using System.Linq;
using Ensage;
using Ensage.Common.Extensions;
using SharpDX;

namespace LastHitMarker
{
    internal class Program
    {
        private static readonly Dictionary<Unit, string> CreepsDictionary = new Dictionary<Unit, string>();
        private static void Main(string[] args)
        {
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (!Game.IsInGame || Game.GameTime > 1800 || Game.IsPaused)
                return;

            var player = ObjectMgr.LocalPlayer;
            var me = player.Hero;

            if (player == null || player.Team == Team.Observer || me == null || me.DamageAverage > 100)
                return;

            var quelling_blade = me.FindItem(" item_quelling_blade ");
            var damage = (quelling_blade != null) ? (me.DamageAverage*1.40 + me.BonusDamage) : (me.DamageAverage + me.BonusDamage);

            var creeps = ObjectMgr.GetEntities<Unit>().Where(creep => (creep.ClassID == ClassID.CDOTA_BaseNPC_Creep_Lane || creep.ClassID == ClassID.CDOTA_BaseNPC_Creep_Siege) && creep.IsSpawned).ToList();

            if (!creeps.Any())
                return;

            foreach (var creep in creeps) {
                string creepType;
                
                if (creep.IsAlive && creep.IsVisible) {
                    if (creep.Health > 0 && creep.Health < damage * (1 - creep.DamageResist) + 1) {
                        if (!CreepsDictionary.TryGetValue(creep, out creepType) || creepType != "passive") continue;
                        CreepsDictionary.Remove(creep);
                        creepType = "active";
                        CreepsDictionary.Add(creep, creepType);
                    }
                    else if (creep.Health < damage + 88) {
                        if (CreepsDictionary.TryGetValue(creep, out creepType)) continue;
                        creepType = "passive";
                        CreepsDictionary.Add(creep, creepType);
                    }
                }
                else
                    CreepsDictionary.Remove(creep);
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (!Game.IsInGame || Game.GameTime > 1800)
                return;

            var creeps = ObjectMgr.GetEntities<Unit>().Where(creep => (creep.ClassID == ClassID.CDOTA_BaseNPC_Creep_Lane || creep.ClassID == ClassID.CDOTA_BaseNPC_Creep_Siege) && creep.IsSpawned).ToList();
            foreach (var creep in creeps) {
                Vector2 screenPos;
                var enemyPos = creep.Position + new Vector3(0, 0, creep.HealthBarOffset);
                if (!Drawing.WorldToScreen(enemyPos, out screenPos)) continue;

                var start = screenPos + new Vector2(-5, -30);

                string creepType;
                if (!CreepsDictionary.TryGetValue(creep, out creepType)) continue;

                var screenX = Drawing.Width / (float)1600 * (float)0.8;
                switch (creepType) {
                    case "active": Drawing.DrawRect(start, new Vector2((float)18 * screenX, (float)18 * screenX), Drawing.GetTexture("materials/vgui/hud/minimap_creep.vmat")); break;
                    case "passive": Drawing.DrawRect(start, new Vector2((float)18 * screenX, (float)18 * screenX), Drawing.GetTexture("materials/vgui/hud/minimap_glow.vmat")); break;
                }
            }
        }
    }
}
