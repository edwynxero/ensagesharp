using System;
using System.Collections.Generic;
using System.Linq;
using Ensage;
using Ensage.Common;
using Ensage.Common.Extensions;
using SharpDX;

namespace AIO_KillStealer
{
    internal class Program
    {
        private static readonly Dictionary<Hero, double> HeroDamageDictionary = new Dictionary<Hero, double>();
        private static readonly Dictionary<Hero, string> HeroSpellDictionary = new Dictionary<Hero, string>();

        private static bool _killError = true;
        private static bool _killStealEnabled;
        private static bool _activ = true;
        
        private static Player _player;
        private static Hero _me;
        private static void Main()
        {
            Game.OnWndProc += Game_OnWndProc;
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Game_OnDraw;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (!_killStealEnabled) {
                if (!Game.IsInGame) return;

                _player = ObjectMgr.LocalPlayer;
                _me = ObjectMgr.LocalHero;

                if (_player == null || _me == null) return;

                _killStealEnabled = true;
                Console.Write("#AIO Kill-Stealer: Loaded!");
            } else if (!Game.IsInGame || _player == null || _me == null) {
                _killStealEnabled = false;
                Console.Write("#AIO Kill-Stealer: UnLoaded!");
                return;
            }

            if (!Utils.SleepCheck("AIO_KillStealer") || Game.IsPaused || !_activ) return;
            Utils.Sleep(100, "AIO_KillStealer");
            
            if (_me == null) return;
            switch (_me.ClassID) {
                case ClassID.CDOTA_Unit_Hero_Abaddon:
                    Kill(_me.Spellbook.Spell1, new double[] { 100, 150, 200, 250 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Alchemist:
                    Kill(_me.Spellbook.Spell2, new[] { 24, 33, 42, 52.5 }, 1, 800, "smart");
                    break;
                case ClassID.CDOTA_Unit_Hero_AntiMage:
                    Kill(_me.Spellbook.Spell4, new[] { .6, .85, 1.1 }, 1, null, "complex", false, true);
                    break;
                case ClassID.CDOTA_Unit_Hero_Axe:
                    Kill(_me.Spellbook.Spell4, new double[] { 250, 325, 400 }, 1, 400, "normal", true, true, new double[] { 300, 425, 550 });
                    break;
                case ClassID.CDOTA_Unit_Hero_Bane:
                    Kill(_me.Spellbook.Spell2, new double[] { 90, 160, 230, 300 }, 1, null, "normal", true, true);
                    break;
                case ClassID.CDOTA_Unit_Hero_BountyHunter:
                    Kill(_me.Spellbook.Spell1, new double[] { 100, 200, 250, 325 }, 1, 700);
                    break;
                case ClassID.CDOTA_Unit_Hero_Broodmother:
                    Kill(_me.Spellbook.Spell1, new double[] { 75, 150, 225, 300 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Centaur:
                    Kill(_me.Spellbook.Spell2, new double[] { 175, 250, 325, 400 }, 1, 300);
                    break;
                case ClassID.CDOTA_Unit_Hero_Chen:
                    Kill(_me.Spellbook.Spell2, new double[] { 50, 100, 150, 200 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_CrystalMaiden:
                    Kill(_me.Spellbook.Spell1, new double[] { 100, 150, 200, 250 }, 2, 700, "normal", false);
                    break;
                case ClassID.CDOTA_Unit_Hero_DeathProphet:
                    Kill(_me.Spellbook.Spell1, new double[] { 75, 150, 225, 300 }, 1, null, "normal", false);
                    break;
                case ClassID.CDOTA_Unit_Hero_DoomBringer:
                    Kill(_me.Spellbook.Spell3, new double[] { 1, 1, 1, 1 }, 1, null, "complex");
                    break;
                case ClassID.CDOTA_Unit_Hero_DragonKnight:
                    Kill(_me.Spellbook.Spell1, new double[] { 90, 170, 240, 300 }, 1, null, "normal", false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Earthshaker:
                    Kill(_me.Spellbook.Spell1, new double[] { 125, 175, 225, 275 }, 2, null, "normal", false);
                    break;
                case ClassID.CDOTA_Unit_Hero_EarthSpirit:
                    Kill(_me.Spellbook.Spell1, new double[] { 125, 125, 125, 125 }, 1, 250);
                    break;
                case ClassID.CDOTA_Unit_Hero_Elder_Titan:
                    Kill(_me.Spellbook.Spell2, new double[] { 60, 90, 120, 150 }, 2, 250, "complex", false, true);
                    break;
                case ClassID.CDOTA_Unit_Hero_Furion:
                    //Kill(Hero, Ability, damage, spellTargetType, range = null, abilityType, lsblock, piercesSpellImmunity, adamage)
                    break;
                case ClassID.CDOTA_Unit_Hero_Leshrac:
                    Kill(_me.Spellbook.Spell3, new double[] { 80, 140, 200, 260 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Legion_Commander:
                    Kill(_me.Spellbook.Spell1, new double[] { 40, 80, 120, 160 }, 2, null, "complex", false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Lich:
                    Kill(_me.Spellbook.Spell1, new double[] { 115, 200, 275, 350 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Lion:
                    Kill(_me.Spellbook.Spell4, new double[] { 600, 725, 850 }, 1, null, "normal", true, false, new double[] { 725, 875, 1025 });
                    break;
                case ClassID.CDOTA_Unit_Hero_Lina:
                    Kill(_me.Spellbook.Spell4, new double[] { 450, 675, 950 }, 1, null, "normal", true, _me.AghanimState());
                    break;
                case ClassID.CDOTA_Unit_Hero_Luna:
                    Kill(_me.Spellbook.Spell1, new double[] { 75, 150, 210, 260 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Magnataur:
                    break;
                case ClassID.CDOTA_Unit_Hero_Mirana:
                    Kill(_me.Spellbook.Spell1, new double[] { 75, 150, 225, 300 }, 3, 625, "complex", false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Morphling:
                    Kill(_me.Spellbook.Spell2, new double[] { 20, 40, 60, 80 }, 1, null, "smart");
                    break;
                case ClassID.CDOTA_Unit_Hero_Necrolyte:
                    Kill(_me.Spellbook.Spell4, new[] { 0.4, 0.6, 0.9 }, 1, null, "complex", true, true, new[] { 0.6, 0.9, 1.2 });
                    break;
                case ClassID.CDOTA_Unit_Hero_NightStalker:
                    Kill(_me.Spellbook.Spell1, new double[] { 90, 160, 225, 335 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Nyx_Assassin:
                    Kill(_me.Spellbook.Spell2, new[] { 3.5, 4, 4.5, 5 }, 1, null, "complex");
                    break;
                case ClassID.CDOTA_Unit_Hero_Obsidian_Destroyer:
                    Kill(_me.Spellbook.Spell4, new double[] { 8, 9, 10 }, 2, null, "complex", false, false, new double[] { 9, 10, 11 });
                    break;
                case ClassID.CDOTA_Unit_Hero_Oracle:
                    Kill(_me.Spellbook.Spell3, new double[] { 90, 180, 270, 360 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_PhantomLancer:
                    Kill(_me.Spellbook.Spell1, new double[] { 100, 150, 200, 250 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Puck:
                    Kill(_me.Spellbook.Spell2, new double[] { 70, 140, 210, 280 }, 3, 400, "normal", false);
                    break;
                case ClassID.CDOTA_Unit_Hero_PhantomAssassin:
                    Kill(_me.Spellbook.Spell1, new double[] { 30, 50, 70, 90 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_QueenOfPain:
                    Kill(_me.Spellbook.Spell3, new double[] { 85, 165, 225, 300 }, 3, 475, "normal", false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Rattletrap:
                    Kill(_me.Spellbook.Spell3, new double[] { 80, 120, 160, 200 }, 2, 1000, "normal", false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Riki:
                    Riki_Kill(_me, _me.Spellbook.Spell4, new double[] { 40, 70, 100 });
                    break;
                case ClassID.CDOTA_Unit_Hero_Rubick:
                    Kill(_me.Spellbook.Spell2, new double[] { 70, 140, 210, 280 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_SkeletonKing:
                    Kill(_me.Spellbook.Spell1, new double[] { 80, 160, 230, 300 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Shredder:
                    Kill(_me.Spellbook.Spell1, new double[] { 100, 150, 200, 250 }, 3, 300, "normal", false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Spectre:
                    Kill(_me.Spellbook.Spell1, new double[] { 50, 100, 150, 200 }, 1, 2000);
                    break;
                case ClassID.CDOTA_Unit_Hero_Shadow_Demon:
                    Kill(_me.Spellbook.Spell3, new double[] { 20, 35, 60, 65 }, 2, null, "complex", false);
                    break;
                case ClassID.CDOTA_Unit_Hero_ShadowShaman:
                    Kill(_me.Spellbook.Spell1, new double[] { 140, 200, 260, 320 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Sniper:
                    Kill(_me.Spellbook.Spell4, new double[] { 350, 500, 650 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Sven:
                    Kill(_me.Spellbook.Spell1, new double[] { 100, 175, 250, 325 }, 1, 650);
                    break;
                case ClassID.CDOTA_Unit_Hero_Techies:
                    Kill(_me.Spellbook.Spell3, new double[] { 500, 650, 850, 1150 }, 1, 250, "normal", false, true);
                    break;
                case ClassID.CDOTA_Unit_Hero_Tidehunter:
                    Kill(_me.Spellbook.Spell1, new double[] { 110, 160, 210, 260 }, 1, 750);
                    break;
                case ClassID.CDOTA_Unit_Hero_Tinker:
                    if (CanBeCasted(_me.Spellbook.Spell2))
                        Kill(_me.Spellbook.Spell2, new double[] { 125, 200, 275, 350 }, 3, 2500);
                    else
                        Kill(_me.Spellbook.Spell1, new double[] { 80, 160, 240, 320 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Tusk:
                    var tuskDamage = (_me.MinimumDamage + _me.BonusDamage) * 3.5;
                    Kill(_me.Spellbook.Spell6, new[] { tuskDamage, tuskDamage, tuskDamage }, 1, 300, "normal", false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Undying:
                    Kill(_me.Spellbook.Spell2, new double[] { 10, 12, 14, 16 }, 1, null, "smart");
                    break;
                case ClassID.CDOTA_Unit_Hero_VengefulSpirit:
                    Kill(_me.Spellbook.Spell1, new double[] { 100, 175, 250, 325 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Visage:
                    Kill(_me.Spellbook.Spell2, new double[] { 20, 20, 20, 20 }, 1, null, "smart");
                    break;
                case ClassID.CDOTA_Unit_Hero_Zuus:
                    if(CanBeCasted(_me.Spellbook.Spell4))
                        Kill(_me.Spellbook.Spell4, new double[] { 225, 350, 475 }, 3, null, "global", true, false, new double[] { 440, 540, 640 });
                    else
                        Kill(_me.Spellbook.Spell2, new double[] { 100, 175, 275, 350 }, 1, null, "complex");
                    break;
            }
        }

        private static void Kill(Ability ability, IReadOnlyList<double> damage, uint spellTargetType, uint? range = null, string abilityType = "normal", bool lsblock = true, bool throughSpellImmunity = false, IReadOnlyList<double> adamage = null)
        {
            var spellLevel = (int) ability.Level - 1;
            if (ability.Level <= 0) return;

            Item refresher = null;
            var refresh = false;
            if (abilityType.Equals("global")) {
                refresher = _me.FindItem("item_refresher");
                if (refresher != null)
                    refresh = CanBeCasted(refresher) && (_me.Mana > ability.ManaCost*2 + refresher.ManaCost);
            }

            double normalDamage;
            if (adamage == null)
                normalDamage = damage[spellLevel];
            else
                normalDamage = _me.AghanimState() ? adamage[spellLevel] : damage[spellLevel];

            var spellDamageType = (_me.ClassID == ClassID.CDOTA_Unit_Hero_Alchemist) ? DamageType.Physical : ability.DamageType;
            var spellRange = range ?? (ability.CastRange + 50);
            var spellCastPoint = (float)(((_killError? 0 : ability.GetCastPoint(ability.Level)) + Game.Ping) / 1000);

            var enemies = ObjectMgr.GetEntities<Hero>().Where(enemy => enemy.Team == _me.GetEnemyTeam() && !enemy.IsIllusion() && enemy.IsVisible && enemy.IsAlive && enemy.Health > 0).ToList();
            foreach (var enemy in enemies) {
                double spellDamage = 0;
                if (abilityType.Equals("complex"))
                    spellDamage = GetComplexDamage(spellLevel, enemy, _me, normalDamage);
                else if (abilityType.Equals("smart"))
                    spellDamage = GetSmartDamage(spellLevel, _me, damage);
                else if(abilityType.Equals("normal") || abilityType.Equals("global"))
                    spellDamage = normalDamage;
                var damageDone = enemy.DamageTaken((float)spellDamage, spellDamageType, _me, throughSpellImmunity);

                if (refresh)
                    damageDone = damageDone * 2;
                if (_me.ClassID == ClassID.CDOTA_Unit_Hero_Axe)
                    damageDone = (float)spellDamage;

                double damageNeeded;

                if (!HeroDamageDictionary.TryGetValue(enemy, out damageNeeded)) {
                    damageNeeded = enemy.Health - damageDone + spellCastPoint * enemy.HealthRegeneration + MorphMustDie(enemy, spellCastPoint);
                    HeroDamageDictionary.Add(enemy, damageNeeded);
                    HeroSpellDictionary.Add(enemy, ability.Name);
                } else {
                    HeroDamageDictionary.Remove(enemy);
                    HeroSpellDictionary.Remove(enemy);

                    damageNeeded = enemy.Health - damageDone + spellCastPoint * enemy.HealthRegeneration + MorphMustDie(enemy, spellCastPoint);
                    
                    HeroDamageDictionary.Add(enemy, damageNeeded);
                    HeroSpellDictionary.Add(enemy, ability.Name);
                }
                if (_me.IsChanneling()) return;

                if (!(damageNeeded < 0) || !(_me.Distance2D(enemy) < spellRange || abilityType.Equals("global")) || !MeCanSurvive(enemy, _me, ability, damageDone)) continue;

                switch (spellTargetType) {
                    case 1:
                        CastSpell(ability, enemy, _me, lsblock);
                        if (abilityType.Equals("global") && refresh) {
                            refresher.UseAbility();
                            CastSpell(ability, enemy, _me, lsblock);
                        }
                        break;
                    case 2:
                        CastSpell(ability, enemy.Position, _me, lsblock);
                        if (abilityType.Equals("global") && refresh) {
                            refresher.UseAbility();
                            CastSpell(ability, enemy.Position, _me, lsblock);
                        }
                        break;
                    case 3:
                        if (CanBeCasted(ability) && _me.CanCast())
                            ability.UseAbility();
                        if (abilityType.Equals("global") && refresh) {
                            refresher.UseAbility();
                            ability.UseAbility();
                        }
                        break;
                }
                break;
            }
        }

        private static void Riki_Kill(Hero me, Ability blinkstrike, IReadOnlyList<double> damage)
        {
            var bs = new[] { 0.5, 0.75, 1, 1.25 };
            var backstab = me.Spellbook.Spell3;
            if (blinkstrike.Level <= 0 || backstab.Level <= 0) return;

            var spellDamage = damage[(int)blinkstrike.Level - 1];
            var backstabDamage = (float)bs[backstab.Level - 1] * me.TotalAgility + me.MinimumDamage + me.BonusDamage;
            var spellRange = blinkstrike.CastRange + 50;
            var spellCastPoint = (float)((_killError ? 0 : blinkstrike.GetCastPoint(blinkstrike.Level)) + Game.Ping / 1000);
            var enemies = ObjectMgr.GetEntities<Hero>().Where(enemy => enemy.Team == me.GetEnemyTeam() && !enemy.IsIllusion() && enemy.IsVisible && enemy.IsAlive && enemy.Health > 0).ToList();
            foreach (var enemy in enemies)
            {
                var damageBlinkstrike = Math.Floor(enemy.DamageTaken((float)spellDamage, blinkstrike.DamageType, me, true));
                var damageBackstab = Math.Floor(enemy.DamageTaken(backstabDamage, backstab.DamageType, me, true));
                double damageNeeded;

                if (!HeroDamageDictionary.TryGetValue(enemy, out damageNeeded)) {
                    damageNeeded = Math.Floor(enemy.Health - damageBlinkstrike - damageBackstab + spellCastPoint * enemy.HealthRegeneration + MorphMustDie(enemy, spellCastPoint));
                    HeroDamageDictionary.Add(enemy, damageNeeded);
                    HeroSpellDictionary.Add(enemy, blinkstrike.Name);
                } else {
                    HeroDamageDictionary.Remove(enemy);
                    HeroSpellDictionary.Remove(enemy);

                    damageNeeded = Math.Floor(enemy.Health - damageBlinkstrike - damageBackstab + spellCastPoint * enemy.HealthRegeneration + MorphMustDie(enemy, spellCastPoint));

                    HeroDamageDictionary.Add(enemy, damageNeeded);
                    HeroSpellDictionary.Add(enemy, blinkstrike.Name);
                }

                if (me.IsChanneling()) continue;

                if (!(damageNeeded < 0) || !(me.Distance2D(enemy) < spellRange) || !MeCanSurvive(enemy, me, blinkstrike, damageBlinkstrike + damageBackstab)) continue;

                CastSpell(blinkstrike, enemy, me, true);
                break;
            }
        }

        private static double GetComplexDamage(int level, Hero enemy, Hero me, double damage)
        {
            switch (me.ClassID)
            {
                case ClassID.CDOTA_Unit_Hero_AntiMage:
                    return Math.Floor((enemy.MaximumMana - enemy.Mana) * damage);
                case ClassID.CDOTA_Unit_Hero_DoomBringer:
                    var lvldeath = new[] { new[] { 6, 125 }, new[] { 5, 175 }, new[] { 4, 225 }, new[] { 3, 275 } };
                    return Math.Floor((enemy.Level == 25 || enemy.Level % lvldeath[level][0] == 0) ? (enemy.MaximumHealth * 0.20 + lvldeath[level][1]) : (lvldeath[level][1]));
                case ClassID.CDOTA_Unit_Hero_Mirana:
                    if (me.Distance2D(enemy) < 200)
                        return (damage * 1.75);
                    return damage;
                case ClassID.CDOTA_Unit_Hero_Necrolyte:
                    return Math.Floor((enemy.MaximumHealth - enemy.Health) * damage);
                case ClassID.CDOTA_Unit_Hero_Nyx_Assassin:
                    var tempBurn = damage * Math.Floor(enemy.TotalIntelligence);
                    return enemy.Mana < tempBurn ? enemy.Mana : tempBurn;
                case ClassID.CDOTA_Unit_Hero_Obsidian_Destroyer:
                    if (me.TotalIntelligence > enemy.TotalIntelligence)
                        return ((Math.Floor(me.TotalIntelligence) - Math.Floor(enemy.TotalIntelligence)) * damage);
                    return 0;
                case ClassID.CDOTA_Unit_Hero_Elder_Titan:
                    var pasDmg = new[] { 1.12, 1.19, 1.25, 1.25 };
                    var pas = me.Spellbook.Spell3.Level;
                    if (pas != 0) {
                        if (enemy.Modifiers.FirstOrDefault(modifier => modifier.Name == "modifier_elder_titan_natural_order") == null)
                            return (pasDmg[pas] * damage);
                        return damage;
                    }
                    return damage;
                case ClassID.CDOTA_Unit_Hero_Shadow_Demon:
                    var actDmg = new[] { 1, 2, 4, 8, 16 };
                    var poison = enemy.Modifiers.FirstOrDefault(modifier => modifier.Name == "modifier_shadow_demon_shadow_poison");
                    if (poison != null) {
                        var poisonStack = poison.StackCount;
                        if (poisonStack != 0 && poisonStack < 6)
                            return (actDmg[poisonStack]) * damage;
                        if (poisonStack > 5)
                            return (damage * 16) + ((poisonStack - 5) * 50);
                    }
                    return 0;
                case ClassID.CDOTA_Unit_Hero_Legion_Commander:
                    var bonusCreep = new[] { 14, 16, 18, 20 };
                    var bonusHero = new[] { 20, 35, 50, 65 };
                    var heroDmg = ObjectMgr.GetEntities<Hero>().Where(hero => hero.IsAlive && hero.Team == me.GetEnemyTeam() && hero.IsVisible && hero.Health > 0 && enemy.Distance2D(hero) < 330).ToList().Count * bonusHero[level];
                    var creepDmg = ObjectMgr.GetEntities<Unit>().Where(unit => ((unit.ClassID == ClassID.CDOTA_BaseNPC_Creep && unit.ClassID != ClassID.CDOTA_BaseNPC_Creature && !unit.IsAncient) || unit.ClassID == ClassID.CDOTA_Unit_VisageFamiliar || unit.ClassID == ClassID.CDOTA_Unit_Undying_Zombie || unit.ClassID == ClassID.CDOTA_Unit_SpiritBear || unit.ClassID == ClassID.CDOTA_Unit_Broodmother_Spiderling || unit.ClassID == ClassID.CDOTA_Unit_Hero_Beastmaster_Boar || unit.ClassID == ClassID.CDOTA_Unit_Hero_Beastmaster_Hawk || unit.ClassID == ClassID.CDOTA_BaseNPC_Invoker_Forged_Spirit) && unit.Team != me.Team && unit.IsAlive && unit.IsVisible && unit.Health > 0 && enemy.Distance2D(unit) < 350).ToList().Count * bonusCreep[level];
                    return Math.Floor((damage + heroDmg + creepDmg));
                case ClassID.CDOTA_Unit_Hero_Zuus:
                    var hp = new[] { .05, .07, .09, .11 };
                    if (me.Spellbook.Spell3.Level > 0 && me.Distance2D(enemy) < 1000)
                        damage = (damage + ((hp[me.Spellbook.Spell3.Level]) * enemy.Health));
                    return damage;
            }
            return damage;
        }

        private static double GetSmartDamage(int level, Hero me, IReadOnlyList<double> damage)
        {
            var baseDmg = damage[level];
            switch (me.ClassID)
            {
                case ClassID.CDOTA_Unit_Hero_Alchemist:
                    var stun = me.Modifiers.FirstOrDefault(modifier => modifier.Name == "modifier_alchemist_unstable_concoction");
                    if (stun == null) return 0;
                    if (stun.ElapsedTime < 4.8)
                        return Math.Floor(stun.ElapsedTime * baseDmg);
                    return (float)Math.Floor(4.8 * baseDmg);
                case ClassID.CDOTA_Unit_Hero_Morphling:
                    var agi = Math.Floor(me.TotalAgility);
                    var dmg = agi / Math.Floor(me.TotalStrength);
                    if (dmg > 1.5)
                        return Math.Floor(0.5 * level * agi + baseDmg);
                    if (dmg < 0.5)
                        return Math.Floor(0.25 * agi + baseDmg);
                    if (dmg >= 0.5 && dmg <= 1.5)
                        return Math.Floor(0.25 + ((dmg - 0.5) * (0.5 * level - 0.25)) * agi + baseDmg);
                    break;
                case ClassID.CDOTA_Unit_Hero_Visage:
                    var soul = me.Modifiers.FirstOrDefault(modifier => modifier.Name == "modifier_visage_soul_assumption");
                    if (soul != null)
                        return 20 + 65 * soul.StackCount;
                    return 20;
                case ClassID.CDOTA_Unit_Hero_Undying:
                    var count = ObjectMgr.GetEntities<Unit>().Where(unit => ((unit.ClassID == ClassID.CDOTA_BaseNPC_Creep && unit.ClassID != ClassID.CDOTA_BaseNPC_Creature && !unit.IsAncient) || unit.ClassID == ClassID.CDOTA_Unit_Undying_Zombie || unit.ClassID == ClassID.CDOTA_Unit_SpiritBear || unit.ClassID == ClassID.CDOTA_Unit_Broodmother_Spiderling || unit.ClassID == ClassID.CDOTA_Unit_Hero_Beastmaster_Boar || unit.ClassID == ClassID.CDOTA_Unit_Hero_Beastmaster_Hawk || unit.ClassID == ClassID.CDOTA_BaseNPC_Invoker_Forged_Spirit || unit.ClassID == ClassID.CDOTA_Unit_Courier || unit.ClassID == ClassID.CDOTA_BaseNPC_Hero) && unit.IsAlive && unit.IsVisible && unit.Health > 0 && me.Distance2D(unit) < 1300);
                    var num = count.Count() - 2;
                    var bonus = new uint[] { 18, 22, 26, 30 };
                    if (num < baseDmg)
                        return num * bonus[level];
                    return baseDmg * bonus[level];
            }
            return baseDmg;
        }

        private static bool MeCanSurvive(Hero enemy, Hero me, Ability spell, double damageDone)
        {
            return (me.IsMagicImmune() || (NotDieFromSpell(spell, enemy, me) && enemy.Modifiers.FirstOrDefault(modifier => modifier.Name == "modifier_nyx_assassin_spiked_carapace") == null && NotDieFromBladeMail(enemy, me, damageDone)));
        }

        private static bool NotDieFromBladeMail(Unit enemy, Unit me, double damageDone)
        {
            return !(enemy.Modifiers.FirstOrDefault(modifier => modifier.Name == "modifier_item_blade_mail_reflect") != null && me.Health < me.DamageTaken((float)damageDone, DamageType.Magical, enemy));
        }

        private static bool NotDieFromSpell(Ability spell, Hero enemy, Hero me)
        {
            if (me.Modifiers.FirstOrDefault(modifier => modifier.Name == "modifier_pugna_nether_ward_aura") == null)
                return true;
            return !(me.Health < me.DamageTaken((spell.ManaCost * (float)1.75), DamageType.Magical, enemy));
        }

        private static void CastSpell(Ability spell, Unit target, Unit me, bool lsblock)
        {
            if (spell.Cooldown > 0) return;
            if (CanBeCasted(spell) && me.CanCast() && (target.Modifiers.FirstOrDefault(modifier => modifier.Name == "modifier_item_sphere") == null || target.FindItem("item_sphere").Cooldown > 0) || lsblock == false)
                spell.UseAbility(target);
        }

        private static bool CanBeCasted(Ability ability)
        {
            return ability != null && ability.Cooldown.Equals(0) && ability.Level > 0 && _me.Mana > ability.ManaCost;
        }

        private static void CastSpell(Ability spell, Vector3 targetPos, Unit me, bool lsblock)
        {
            if (spell.Cooldown > 0) return;
            if (CanBeCasted(spell) && me.CanCast() && !lsblock)
                spell.UseAbility(targetPos);
        }

        private static float MorphMustDie(Hero target, float value)
        {
            if (target.ClassID != ClassID.CDOTA_Unit_Hero_Morphling) return 0;

            var morphLevel = target.Spellbook.Spell3.Level;
            if (morphLevel <= 0) return 0;

            uint[] morphGain = { 38, 76, 114, 190 };
            if (target.Modifiers.FirstOrDefault(modifier => modifier.Name == "modifier_morphling_morph_agi") != null && target.Strength > 1)
                return value * (0 - morphGain[morphLevel - 1] + 1);
            if (target.Modifiers.FirstOrDefault(modifier => modifier.Name == "modifier_morphling_morph_str") != null && target.Agility > 1)
                return value * morphGain[morphLevel - 1];
            return 0;
        }

        
        private static void Game_OnWndProc(WndEventArgs args)
        {
            if (!Game.IsInGame || args.Msg != (ulong)Utils.WindowsMessages.WM_KEYUP || args.WParam != 'D' || Game.IsChatOpen)
                return;
            _activ = !_activ;
        }
        
        private static void Game_OnDraw(EventArgs args)
        {
            if (!Game.IsInGame || _player == null || _me == null || !_activ)
                return;

            var enemies = ObjectMgr.GetEntities<Hero>().Where(hero => hero.IsVisible && hero.IsAlive && hero.Team != _player.Team && !hero.IsIllusion()).ToList();
            foreach (var enemy in enemies)
            {
                Vector2 screenPos;
                var enemyPos = enemy.Position + new Vector3(0, 0, enemy.HealthBarOffset);
                if (!Drawing.WorldToScreen(enemyPos, out screenPos)) continue;

                var start = screenPos + new Vector2(-51, -40);
                double damageNeeded;
                string spell;
                if (!HeroDamageDictionary.TryGetValue(enemy, out damageNeeded) || !HeroSpellDictionary.TryGetValue(enemy, out spell)) continue;

                var text = "D a m a g e  f o r  K S: " + string.Format("{0}", (int)damageNeeded);
                var textSize = Drawing.MeasureText(text, "Arial", new Vector2(10, 150), FontFlags.None);
                var textPos = start + new Vector2(51 - textSize.X / 2, -textSize.Y / 2 + 2);
                //Drawing.DrawRect(textPos - new Vector2(15, 0), new Vector2(10, 10), Drawing.GetTexture("materials/NyanUI/spellicons/" + spell + ".vmt"));
                Drawing.DrawText(text, "Arial", textPos, new Vector2(10, 150), damageNeeded < 0 ? Color.Red : Color.White, FontFlags.AntiAlias | FontFlags.DropShadow);
            }
        }
    }
}
