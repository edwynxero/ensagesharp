using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Ensage;
using Ensage.Common;
using Ensage.Common.Extensions;
using SharpDX;

namespace OverlayInformation
{
    internal class Program
    {
        public static bool OverlayManaBar = true;
        public static bool OverlaySpells = true;
        public static bool OverlayItems = true;
        public static bool OverlayGlyph = true;
        public static bool OverlayRune = true;
        public static bool OverlayCourier = true;
        public static bool OverlayHeroInformation = true;

        private static int _screenX = Drawing.Width;
        private static int _screenY = Drawing.Height;

        private static float _testX;
        private static float _testY;

        // Hero Panel Variables
        private static double _panelSize;
        private static double _panelHealth;
        private static double _panelUltimate;

        // Mana Bar Variables
        private static float _manaBarX;
        private static float _manaBarY;
        private static float _manaBarSize;

        // Glyph Variables
        private static double _glyphX;
        private static double _glyphY;

        private static double _txxB;
        private static double _txxG;

        private static Player _player = ObjectMgr.LocalPlayer;

        private static void Main()
        {
            Game.OnUpdate += OverlayInformation_OnUpdate;
            Drawing.OnDraw += OverlayInformation_OnDraw;
        }

        private static void OverlayInformation_OnUpdate(EventArgs args)
        {
            if (!Game.IsInGame) return;
            Utils.Sleep(250, "OverlayInformation");

            _player = ObjectMgr.LocalPlayer;
            var me = _player.Hero;

            if (_player == null || me == null) return;

            var screenRatio = _screenX / _screenY;

            switch ((int)Math.Floor((decimal) (screenRatio * 100)))
            {
                case 177:
                    _testX = 1600;
                    _testY = 900;

                    _panelSize = 55;
                    _panelHealth = 25.714;
                    _panelUltimate = 20;

                    _manaBarX = 42;
                    _manaBarY = 18;
                    _manaBarSize = 83;

                    _glyphX = 1.0158;
                    _glyphY = 1.03448;

                    _txxB = 2.535;
                    _txxG = 3.485;
                    break;
                case 166:
                    _testX = 1280;
                    _testY = 768;

                    _panelSize = 47.1;
                    _panelHealth = 25.714;
                    _panelUltimate = 18;

                    _manaBarX = 36;
                    _manaBarY = 15;
                    _manaBarSize = 70;

                    _glyphX = 1.0180;
                    _glyphY = 1.03448;

                    _txxB = 2.59;
                    _txxG = 3.66;
                    break;
                case 160:
                    _testX = 1280;
                    _testY = 800;

                    _panelSize = 48.5;
                    _panelHealth = 25.714;
                    _panelUltimate = 20;

                    _manaBarX = 38;
                    _manaBarY = 16;
                    _manaBarSize = 74;

                    _glyphX = 1.0180;
                    _glyphY = 1.03448;

                    _txxB = 2.579;
                    _txxG = 3.74;
                    break;
                case 133:
                    _testX = 1024;
                    _testY = 768;

                    _panelSize = 47;
                    _panelHealth = 25.714;
                    _panelUltimate = 18;

                    _manaBarX = 37;
                    _manaBarY = 14;
                    _manaBarSize = 72;

                    _glyphX = 1.021;
                    _glyphY = 1.03448;

                    _txxB = 2.78;
                    _txxG = 4.63;
                    break;
                case 125:
                    _testX = 1280;
                    _testY = 1024;

                    _panelSize = 58;
                    _panelHealth = 25.714;
                    _panelUltimate = 23;

                    _manaBarX = 48;
                    _manaBarY = 21;
                    _manaBarSize = 97;

                    _glyphX = 1.021;
                    _glyphY = 1.03448;

                    _txxB = 2.747;
                    _txxG = 4.54;
                    break;
                default:
                    _testX = 1600;
                    _testY = 900;

                    _panelSize = 55;
                    _panelHealth = 25.714;
                    _panelUltimate = 20;

                    _manaBarX = 42;
                    _manaBarY = 18;
                    _manaBarSize = 83;

                    _glyphX = 1.0158;
                    _glyphY = 1.03448;

                    _txxB = 2.535;
                    _txxG = 3.485;
                    break;
            }

            if (OverlayRune)
            {
                //OverlayRune();
            }

            if (OverlayCourier)
            {
                var couriers = ObjectMgr.GetEntities<Courier>().Where(courier => courier.Team == _player.Hero.GetEnemyTeam()).ToList();
                //OverlayCourier(couriers);
            }
        }

        private static void OverlayInformation_OnDraw(EventArgs args)
        {
            if (!Game.IsInGame || _player == null) return;

           _screenX = Drawing.Width;
           _screenY = Drawing.Height;

            var rate = _screenX/ _testX;
            var con = rate;
            if (rate < 1) rate = 1;

            var players = ObjectMgr.GetEntities<Player>().Where(player => !player.Hero.IsIllusion).ToList();
            if (!players.Any()) return;
            
            foreach (var enemy in players.Where(player => player.Hero.Team != _player.Hero.Team).Select(player => player.Hero).Where(hero => hero.IsVisible && hero.IsAlive && hero.IsVisible))
            {
                Vector2 screenPos;
                var enemyPos = enemy.Position + new Vector3(0, 0, enemy.HealthBarOffset);
                if (!Drawing.WorldToScreen(enemyPos, out screenPos)) continue;

                if (OverlayManaBar) {
                    var manaX = con * _manaBarX;
                    var manaY = _screenY / _testY * _manaBarY;
                    var manaSizeW = con * _manaBarSize;
                    var manaPercent = enemy.Mana/enemy.MaximumMana;

                    Drawing.DrawRect(screenPos + new Vector2(-manaX - 1, -manaY), new Vector2(manaSizeW + 2, 6), new Color(0x01, 0x01, 0x02, 0xFF));
                    Drawing.DrawRect(screenPos + new Vector2(-manaX, -manaY + 1), new Vector2(manaSizeW*manaPercent, 4), new Color(0x52, 0x79, 0xFF, 0xFF));
                    Drawing.DrawRect(screenPos + new Vector2(-manaX + manaSizeW*manaPercent, -manaY + 1), new Vector2(manaSizeW*(1 - manaPercent), 4), new Color(0x00, 0x17, 0x5F, 0xFF), true);

                    for (var i = 0; i < enemy.MaximumMana/250; i++)
                        Drawing.DrawRect(screenPos + new Vector2(-manaX + manaSizeW/enemy.MaximumMana*250*i, -manaY + 1), new Vector2(2, 5), new Color(0x0D, 0x14, 0x53, 0x90));
                }

                if (OverlaySpells)
                {
                    var spells = new Ability[7];
                    try { spells[1] = enemy.Spellbook.Spell1;} catch {/*ignored*/}
                    try { spells[2] = enemy.Spellbook.Spell2;} catch {/*ignored*/}
                    try { spells[3] = enemy.Spellbook.Spell3;} catch {/*ignored*/}
                    try { spells[4] = enemy.Spellbook.Spell4;} catch {/*ignored*/}
                    try { spells[5] = enemy.Spellbook.Spell5;} catch {/*ignored*/}
                    try { spells[6] = enemy.Spellbook.Spell6;} catch {/*ignored*/}
                    for (var index = 1; index < 7; index++)
                    {
                        if (spells[index] == null ) continue;

                        var cooldown = Math.Ceiling(spells[index].Cooldown);

                        Drawing.DrawRect(screenPos + new Vector2(index * 30 * rate - 70 * rate, 80 * rate), new Vector2(24 * rate,  6 * rate), new Color(0x00, 0x00, 0x00, 0x90));

                        if (spells[index].Level.Equals(0)) continue;
                        for (var level = 1; level <= spells[index].Level; level++)
                            Drawing.DrawRect(screenPos + new Vector2(index * 30 * rate - (74 - (4 * level)) * rate + 3 * rate, 82 * rate), new Vector2(3 * rate, 3 * rate), new Color(0xFF, 0xFF, 0x00, 0xFF));

                        if (cooldown.Equals(0)) continue;
                        Drawing.DrawRect(screenPos + new Vector2(index * 30 * rate - 70 * rate, 86 * rate), new Vector2(24 * rate, 16 * rate), new Color(0x00, 0x00, 0x00, 0x75));
                        var shift = 3;
                        if (cooldown > 99) cooldown = 99;
                        if (cooldown < 10) shift = 5;
                        Drawing.DrawText(string.Format("{0}", (int) cooldown), screenPos + new Vector2(index * 30 * rate - 68 * rate + shift * rate + 2 * rate, 80 * rate + 8 * rate), new Color(0xFF, 0xFF, 0xFF, 0xAA), FontFlags.AntiAlias | FontFlags.DropShadow);
                    }
                }

                if (OverlayItems)
                {
                    var itemX = con * _manaBarX;
                    var itemY = _screenY / _testY * _manaBarY;

                    var itemtab = new Dictionary<Hero, float>();
                    itemtab[enemy] = 0;
                    if (enemy.FindItem("item_gem") != null) {
                        itemtab[enemy] = itemtab[enemy] + 20 * rate;
                        //Drawing.DrawRect(screenPos + new Vector2(itemtab[enemy] - itemX - 18 * rate, -itemY + 7), new Vector2(18 * rate, 16 * rate), Drawing.GetTexture("materials/NyanUI/other/O_gem"));
                    }
                    if (enemy.FindItem("item_dust") != null) {
                        itemtab[enemy] = itemtab[enemy] + 20 * rate;
                        //Drawing.DrawRect(screenPos + new Vector2(itemtab[enemy] - itemX - 18 * rate, -itemY + 7), new Vector2(18 * rate, 16 * rate), Drawing.GetTexture("materials/NyanUI/other/O_dust"));
                    }
                    if (enemy.FindItem("item_dust") != null) {
                        itemtab[enemy] = itemtab[enemy] + 20 * rate;
                        //Drawing.DrawRect(screenPos + new Vector2(itemtab[enemy] - itemX - 18 * rate, -itemY + 7), new Vector2(18 * rate, 16 * rate), Drawing.GetTexture("materials/NyanUI/other/O_sentry"));
                    }
                    if (enemy.FindItem("item_sphere") != null) {
                        itemtab[enemy] = itemtab[enemy] + 20 * rate;
                        //Drawing.DrawRect(screenPos + new Vector2(itemtab[enemy] - itemX - 18 * rate, -itemY + 7), new Vector2(18 * rate, 16 * rate), Drawing.GetTexture("materials/NyanUI/other/O_sphere"));
                    }
                }
            }

            if (OverlayGlyph)
            {
                //Game.FindKeyValues()
            }

            if (OverlayHeroInformation) {
                var panelX = (float) (_panelSize * (con));
                var panelY = (float) (_screenY / _panelHealth);
                var panelUltimate = (float) _panelUltimate * (con);

                foreach (var player in players) {
                    var hero = player.Hero;
                    if (hero.Equals(_player.Hero)) continue;
                    float xx = 0;
                    switch (hero.Team)
                    {
                        case Team.Radiant: xx = (float) (_screenX/_txxG + 1); break;
                        case Team.Dire: xx = (float) (_screenX/_txxB + 1); break;
                    }
                    var color = (hero.Team == _player.Team) ? new Color(0x00, 0x80, 0x00, 0xFF) : new Color(0x96, 0x00, 0x18, 0xFF);
                    var handId = (float) GetId(hero.Player.ID, GetCount(), hero.Team);
                    
                    //var heroResources = ObjectMgr.GetEntities<Unit>().First(resource => resource.ClassID == ClassID.CDOTA_PlayerResource);
                    //const string lasthits = "5";
                    //const string denies = "10";
                    
                    var healtPercentage = (float) (hero.IsAlive? (decimal)hero.Health / hero.MaximumHealth:0);
                    Drawing.DrawRect(new Vector2(xx - panelUltimate + panelX * handId, panelY + 2), new Vector2(panelX - 1, 8 * rate), new Color(0x00, 0x00, 0x00, 0xD0));
                    Drawing.DrawRect(new Vector2(xx - panelUltimate + panelX * handId, panelY + 2), new Vector2((panelX - 2) * healtPercentage, 8 * rate), color);
                    Drawing.DrawRect(new Vector2(xx - panelUltimate + panelX * handId, panelY + 1), new Vector2(panelX - 1, 8 * rate + 1), new Color(0x00, 0x00, 0x00, 0xFF), true);

                    //Drawing.DrawText(" " + lasthits + " / " + denies, new Vector2(xx - panelUltimate + panelX * handId, panelY - 30 * con), new Color(0xFF, 0xFF, 0xFF, 0x99), FontFlags.None);

                    var ult = hero.Spellbook.Spells.First(spell => spell.AbilityType == AbilityType.Ultimate);
                    if (ult.Cooldown > 0)
                    {
                        var ultCoolDown = Math.Ceiling(ult.Cooldown);
                        var shift = 5;
                        if (ultCoolDown > 99)
                            shift = 2;
                        else if (ultCoolDown < 10)
                            shift = 7;
                        Drawing.DrawText(ultCoolDown.ToString(CultureInfo.InvariantCulture), new Vector2(xx + panelX * handId + shift, panelY + 5), new Color(0xFF, 0xFF, 0xFF, 0x99), FontFlags.None);
                    }

                    if (hero.Team == _player.Team) continue;

                    switch (ult.AbilityState)
                    {
                        case AbilityState.Ready:
                        case AbilityState.NoAbility:
                            Drawing.DrawRect(new Vector2((float)(xx + panelX * (handId + 0.01)), panelY - 5), new Vector2(14 * rate, 15 * rate), Drawing.GetTexture("materials/vgui/hud/minimap_shield.vmat"));
                            break;
                        case AbilityState.NotEnoughMana:
                            Drawing.DrawRect(new Vector2((float)(xx + panelX * (handId + 0.01)), panelY - 5), new Vector2(14 * rate, 15 * rate), Drawing.GetTexture("materials/vgui/hud/mana_drop.vmt"));
                            break;
                    }
                }
            }
        }

        private static int GetCount()
        {
            return ObjectMgr.GetEntities<Player>().Where(player => player.Team == Team.Radiant).ToList().Count();
        }

        private static int GetId(int id, int count, Team team)
        {
            if (team == Team.Dire)
                return 5 - count + id;
            return id;
        }
    }
}