using System;
using System.Linq;
using Ensage;
using SharpDX;

namespace LastHitMarker
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (!Game.IsInGame || Game.GameTime > 1800)
                return;

            var screenX = Drawing.Width / (float)1600 * (float)0.8;

            var me = ObjectMgr.LocalHero;
            if (me == null || me.MinimumDamage > 100)
                return;

            var myDamage = me.DamageAverage;
            var creeps = ObjectMgr.GetEntities<Unit>().Where(creep => creep.IsAlive && creep.IsVisible && (creep.ClassID == ClassID.CDOTA_BaseNPC_Creep_Lane || creep.ClassID == ClassID.CDOTA_BaseNPC_Creep_Siege) && creep.IsSpawned).ToList();

            if (!creeps.Any())
                return;

            foreach (var creep in creeps)
            {
                Vector2 screenPos;
                var enemyPos = creep.Position + new Vector3(0, 0, creep.HealthBarOffset);
                if (!Drawing.WorldToScreen(enemyPos, out screenPos))
                    continue;

                var start = screenPos + new Vector2(-5, -30);

                if (creep.Health > 0 && creep.Health < myDamage * (1 - creep.DamageResist) + 1)
                {
                        Drawing.DrawRect(start, new Vector2((float)18 * screenX, (float)18 * screenX), Drawing.GetTexture("materials/vgui/hud/minimap_creep.vmat"));
                }
                else if (creep.Health < myDamage + 88)
                {
                        Drawing.DrawRect(start, new Vector2((float)18 * screenX, (float)18 * screenX), Drawing.GetTexture("materials/vgui/hud/minimap_glow.vmat"));
                }
            }
        }
    }
}
