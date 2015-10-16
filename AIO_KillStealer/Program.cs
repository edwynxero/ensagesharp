using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Ensage;
using Ensage.Common;
using Ensage.Common.Extensions;
using SharpDX;

namespace AIO_KillStealer
{
    internal class Program
    {
        private static bool _killstealEnabled = true;
        private static readonly Dictionary<Hero, float> HeroDamageDictionary = new Dictionary<Hero, float>();
        private static readonly Dictionary<Hero, Ability> HeroSpellDictionary = new Dictionary<Hero, Ability>();
        private static void Main()
        {
            //Game.OnWndProc += KillStealer_OnWndProc;
            Game.OnUpdate += KillStealer_OnUpdate;
            Drawing.OnDraw += KillStealer_OnDraw;
        }

        private static void KillStealer_OnUpdate(EventArgs args)
        {
            if (!Game.IsInGame || !Utils.SleepCheck("AIO_KillStealer") || Game.IsPaused)
                return;
            Utils.Sleep(250, "AIO_KillStealer");

            var me = ObjectMgr.LocalHero;
            if (me == null)
                return;

            switch (me.ClassID)
            {
                case ClassID.CDOTA_Unit_Hero_Abaddon:
                    Kill(me, me.Spellbook.Spell1, new double[] { 100, 150, 200, 250 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Alchemist:
                    Kill(me, me.Spellbook.Spell2, new double[] { 24, 33, 42, 52.5 }, 1, 800, false, false, true);
                    break;
                case ClassID.CDOTA_Unit_Hero_AntiMage:
                    Kill(me, me.Spellbook.Spell4, new double[] { .6, .85, 1.1 }, 1, null, false, true, false, true);
                    break;
                case ClassID.CDOTA_Unit_Hero_Axe:
                    Kill(me, me.Spellbook.Spell4, new double[] { 250, 325, 400 }, 1, null, true, true, false, false, new double[] { 300, 425, 550 });
                    break;
                case ClassID.CDOTA_Unit_Hero_Bane:
                    Kill(me, me.Spellbook.Spell2, new double[] { 90, 160, 230, 300 }, 1, null, true, true);
                    break;
                case ClassID.CDOTA_Unit_Hero_BountyHunter:
                    Kill(me, me.Spellbook.Spell1, new double[] { 100, 200, 250, 325 }, 1, 700);
                    break;
                case ClassID.CDOTA_Unit_Hero_Broodmother:
                    Kill(me, me.Spellbook.Spell1, new double[] { 75, 150, 225, 300 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Centaur:
                    Kill(me, me.Spellbook.Spell2, new double[] { 175, 250, 325, 400 }, 1, 300);
                    break;
                case ClassID.CDOTA_Unit_Hero_Chen:
                    Kill(me, me.Spellbook.Spell2, new double[] { 50, 100, 150, 200 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_CrystalMaiden:
                    Kill(me, me.Spellbook.Spell1, new double[] { 100, 150, 200, 250 }, 2, 700, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_DeathProphet:
                    Kill(me, me.Spellbook.Spell1, new double[] { 75, 150, 225, 300 }, 1, null, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_DoomBringer:
                    Kill(me, me.Spellbook.Spell3, new double[] { 1, 1, 1, 1 }, 1, null, true, false, false, true);
                    break;
                case ClassID.CDOTA_Unit_Hero_DragonKnight:
                    Kill(me, me.Spellbook.Spell1, new double[] { 90, 170, 240, 300 }, 1, null, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Earthshaker:
                    Kill(me, me.Spellbook.Spell1, new double[] { 125, 175, 225, 275 }, 2, null, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_EarthSpirit:
                    Kill(me, me.Spellbook.Spell1, new double[] { 125, 125, 125, 125 }, 1, 250);
                    break;
                case ClassID.CDOTA_Unit_Hero_Elder_Titan:
                    Kill(me, me.Spellbook.Spell2, new double[] { 60, 90, 120, 150 }, 2, 250, false, true, false, true);
                    break;
                case ClassID.CDOTA_Unit_Hero_Furion:
                    break;
                case ClassID.CDOTA_Unit_Hero_Leshrac:
                    Kill(me, me.Spellbook.Spell3, new double[] { 80, 140, 200, 260 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Legion_Commander:
                    Kill(me, me.Spellbook.Spell1, new double[] { 40, 80, 120, 160 }, 2, null, false, false, false, true);
                    break;
                case ClassID.CDOTA_Unit_Hero_Lich:
                    Kill(me, me.Spellbook.Spell1, new double[] { 115, 200, 275, 350 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Lion:
                    Kill(me, me.Spellbook.Spell4, new double[] { 600, 725, 850 }, 1, null, true, false, false, false, new double[] { 725, 875, 1025 });
                    break;
                case ClassID.CDOTA_Unit_Hero_Lina:
                    Kill(me, me.Spellbook.Spell4, new double[] { 450, 675, 950 }, 1, null, true, me.AghanimState());
                    break;
                case ClassID.CDOTA_Unit_Hero_Luna:
                    Kill(me, me.Spellbook.Spell1, new double[] { 75, 150, 210, 260 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Magnataur:
                    break;
                case ClassID.CDOTA_Unit_Hero_Mirana:
                    Kill(me, me.Spellbook.Spell1, new double[] { 75, 150, 225, 300 }, 3, 625, false, false, false, true);
                    break;
                case ClassID.CDOTA_Unit_Hero_Morphling:
                    Kill(me, me.Spellbook.Spell2, new double[] { 20, 40, 60, 80 }, 1, null, true, false, true);
                    break;
                case ClassID.CDOTA_Unit_Hero_Necrolyte:
                    Kill(me, me.Spellbook.Spell4, new double[] { 0.4, 0.6, 0.9 }, 1, null, true, true, false, true, new double[] { 0.6, 0.9, 1.2 });
                    break;
                case ClassID.CDOTA_Unit_Hero_NightStalker:
                    Kill(me, me.Spellbook.Spell1, new double[] { 90, 160, 225, 335 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Nyx_Assassin:
                    Kill(me, me.Spellbook.Spell2, new double[] { 3.5, 4, 4.5, 5 }, 1, null, true, false, false, true);
                    break;
                case ClassID.CDOTA_Unit_Hero_Obsidian_Destroyer:
                    Kill(me, me.Spellbook.Spell4, new double[] { 8, 9, 10 }, 2, null, false, false, false, true, new double[] { 9, 10, 11 });
                    break;
                case ClassID.CDOTA_Unit_Hero_Oracle:
                    Kill(me, me.Spellbook.Spell3, new double[] { 90, 180, 270, 360 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_PhantomLancer:
                    Kill(me, me.Spellbook.Spell1, new double[] { 100, 150, 200, 250 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Puck:
                    Kill(me, me.Spellbook.Spell2, new double[] { 70, 140, 210, 280 }, 3, 400, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_PhantomAssassin:
                    Kill(me, me.Spellbook.Spell1, new double[] { 30, 50, 70, 90 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_QueenOfPain:
                    Kill(me, me.Spellbook.Spell3, new double[] { 85, 165, 225, 300 }, 3, 475, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Rattletrap:
                    Kill(me, me.Spellbook.Spell3, new double[] { 80, 120, 160, 200 }, 2, 1000, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Riki:
                    Riki_Kill(me, me.Spellbook.Spell4, new double[] { 40, 70, 100 });
                    break;
                case ClassID.CDOTA_Unit_Hero_Rubick:
                    Kill(me, me.Spellbook.Spell2, new double[] { 70, 140, 210, 280 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_SkeletonKing:
                    Kill(me, me.Spellbook.Spell1, new double[] { 80, 160, 230, 300 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Shredder:
                    Kill(me, me.Spellbook.Spell1, new double[] { 100, 150, 200, 250 }, 3, 300, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Spectre:
                    Kill(me, me.Spellbook.Spell1, new double[] { 50, 100, 150, 200 }, 1, 2000);
                    break;
                case ClassID.CDOTA_Unit_Hero_Shadow_Demon:
                    Kill(me, me.Spellbook.Spell3, new double[] { 20, 35, 60, 65 }, 2, null, false, false, false, true);
                    break;
                case ClassID.CDOTA_Unit_Hero_ShadowShaman:
                    Kill(me, me.Spellbook.Spell1, new double[] { 140, 200, 260, 320 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Sniper:
                    Kill(me, me.Spellbook.Spell4, new double[] { 350, 500, 650 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Sven:
                    Kill(me, me.Spellbook.Spell1, new double[] { 100, 175, 250, 325 }, 1, 650);
                    break;
                case ClassID.CDOTA_Unit_Hero_Techies:
                    Kill(me, me.Spellbook.Spell3, new double[] { 500, 650, 850, 1150 }, 1, 250, false, true);
                    break;
                case ClassID.CDOTA_Unit_Hero_Tidehunter:
                    Kill(me, me.Spellbook.Spell1, new double[] { 110, 160, 210, 260 }, 1, 750);
                    break;
                case ClassID.CDOTA_Unit_Hero_Tinker:
                    if (me.FindSpell("tinker_laser").AbilityState == AbilityState.Ready)
                        Kill(me, me.Spellbook.Spell1, new double[] { 80, 160, 240, 320 }, 1);
                    else if (me.FindSpell("tinker_heat_seeking_missile").AbilityState == AbilityState.Ready)
                        Kill(me, me.Spellbook.Spell2, new double[] { 125, 200, 275, 350 }, 3, 2500);
                    break;
                case ClassID.CDOTA_Unit_Hero_Tusk:
                    var tuskDamage = (me.MinimumDamage + me.BonusDamage)*3.5;
                    Kill(me, me.Spellbook.Spell6, new double[] { tuskDamage, tuskDamage, tuskDamage }, 1, 300, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Undying:
                    Kill(me, me.Spellbook.Spell2, new double[] { 10, 12, 14, 16 }, 1, null, true, false, true);
                    break;
                case ClassID.CDOTA_Unit_Hero_VengefulSpirit:
                    Kill(me, me.Spellbook.Spell1, new double[] { 100, 175, 250, 325 }, 1);
                    break;
                case ClassID.CDOTA_Unit_Hero_Visage:
                    Kill(me, me.Spellbook.Spell2, new double[] { 20, 20, 20, 20 }, 1, null, true, false, true);
                    break;
                case ClassID.CDOTA_Unit_Hero_Zuus:
                    break;
            }
        }

        private static void Kill(Hero me, Ability ability, IReadOnlyList<double> damage, uint spellTargetType, uint? range = null, bool lsblock = true, bool piercesSpellImmunity = false, bool smartKill = false, bool complexKill = false, IReadOnlyList<double> adamage = null)
        {
            if (ability.Level > 0)
            {
                double normalDamage;
                if (adamage == null)
                    normalDamage = damage[(int) ability.Level];
                else
                    normalDamage = me.AghanimState() ? damage[(int) ability.Level] : adamage[(int) ability.Level];

                var spellDamageType = ability.DamageType;
                var spellRange = range ?? (ability.CastRange + 50);
                var spellCastPoint = (float)(ability.GetCastPoint() + Game.AvgPing / 1000);

                var enemies = ObjectMgr.GetEntities<Hero>().Where(enemy => enemy.Team == me.GetEnemyTeam() && !enemy.IsIllusion() && enemy.IsVisible && enemy.IsAlive && enemy.Health > 0).ToList();
                foreach (var enemy in enemies)
                {
                    double spellDamage;
                    if (complexKill)
                        spellDamage = GetComplexDamage(ability.Level, enemy, me, normalDamage);
                    else if (smartKill)
                        spellDamage = GetSmartDamage(ability.Level, me, damage);
                    else
                        spellDamage = normalDamage;

                    var damageDone = enemy.DamageTaken((float) spellDamage, spellDamageType, me, piercesSpellImmunity);
                    float damageNeeded;

                    if (!HeroDamageDictionary.TryGetValue(enemy, out damageNeeded))
                    {
                        damageNeeded = enemy.Health - damageDone + spellCastPoint * enemy.HealthRegeneration + MorphMustDie(enemy, spellCastPoint);
                        HeroDamageDictionary.Add(enemy, damageNeeded);
                        HeroSpellDictionary.Add(enemy, ability);
                    }
                    else
                    {
                        HeroDamageDictionary.Remove(enemy);
                        HeroSpellDictionary.Remove(enemy);
                        damageNeeded = enemy.Health - damageDone + spellCastPoint * enemy.HealthRegeneration + MorphMustDie(enemy, spellCastPoint);
                        HeroDamageDictionary.Add(enemy, damageNeeded);
                        HeroSpellDictionary.Add(enemy, ability);
                    }

                    if (_killstealEnabled && !me.IsChanneling())
                    {
                        if (damageNeeded < 0 && me.Distance2D(enemy) < spellRange && MeCanSurvive(enemy, me, ability, damageDone))
                        {
                            switch (spellTargetType)
                            {
                                case 1:
                                    CastSpell(ability, enemy, me, lsblock);
                                    break;
                                case 2:
                                    CastSpell(ability, enemy.Position, me, lsblock);
                                    break;
                                case 3:
                                    if (ability.CanBeCasted() && me.CanCast())
                                        ability.UseAbility();
                                    break;
                            }
                            break;
                        }
                    }
                }
            }
        }

        private static void Riki_Kill(Hero me, Ability blinkstrike, IReadOnlyList<double> damage)
        {
            var bs = new[] { 0.5, 0.75, 1, 1.25 };
            var backstab = me.Spellbook.Spell3;

            if (blinkstrike.Level <= 0 || backstab.Level <= 0) return;

            var spellDamage = damage[(int) blinkstrike.Level];
            var backstabDamage = (float)bs[backstab.Level] * me.TotalAgility + me.MinimumDamage + me.BonusDamage;
            var spellRange = blinkstrike.CastRange + 50;
            var spellCastPoint = (float)(blinkstrike.GetCastPoint() + Game.AvgPing / 1000);
            var enemies = ObjectMgr.GetEntities<Hero>().Where(enemy => enemy.Team == me.GetEnemyTeam() && !enemy.IsIllusion() && enemy.IsVisible && enemy.IsAlive && enemy.Health > 0).ToList();
            foreach (var enemy in enemies)
            {
                var damageBlinkstrike = enemy.DamageTaken((float) spellDamage, blinkstrike.DamageType, me, true);
                var damageBackstab = enemy.DamageTaken(backstabDamage, backstab.DamageType, me, true);
                float damageNeeded;

                if (!HeroDamageDictionary.TryGetValue(enemy, out damageNeeded))
                {
                    damageNeeded = enemy.Health - damageBlinkstrike - damageBackstab + spellCastPoint * enemy.HealthRegeneration + MorphMustDie(enemy, spellCastPoint);
                    HeroDamageDictionary.Add(enemy, damageNeeded);
                    HeroSpellDictionary.Add(enemy, blinkstrike);
                }
                else
                {
                    HeroDamageDictionary.Remove(enemy);
                    HeroSpellDictionary.Remove(enemy);
                    damageNeeded = enemy.Health - damageBlinkstrike - damageBackstab + spellCastPoint * enemy.HealthRegeneration + MorphMustDie(enemy, spellCastPoint);
                    HeroDamageDictionary.Add(enemy, damageNeeded);
                    HeroSpellDictionary.Add(enemy, blinkstrike);
                }
                if (_killstealEnabled && !me.IsChanneling())
                {
                    if (damageNeeded < 0 && me.Distance2D(enemy) < spellRange && MeCanSurvive(enemy, me, blinkstrike, damageBlinkstrike + damageBackstab))
                    {
                        CastSpell(blinkstrike, enemy, me, true);
                        break;
                    }
                }
            }
        }

        private static double GetComplexDamage(uint level, Hero enemy, Hero me, double damage)
        {
            switch (me.ClassID)
            {
                case ClassID.CDOTA_Unit_Hero_AntiMage:
                    return Math.Floor((enemy.MaximumMana - enemy.Mana)*damage);
                case ClassID.CDOTA_Unit_Hero_DoomBringer:
                    var lvldeath = new[] {new[] { 6, 125 }, new[] { 5, 175 }, new[] { 4, 225 }, new[] { 3, 275 }};
                    return Math.Floor((enemy.Level == 25 || enemy.Level % lvldeath[level][0] == 0) ? (enemy.MaximumHealth * 0.20 + lvldeath[level][1]) : (lvldeath[level][1]));
                case ClassID.CDOTA_Unit_Hero_Mirana:
                    if (me.Distance2D(enemy) < 200)
                        return (damage * 1.75);
                    return damage;
                case ClassID.CDOTA_Unit_Hero_Necrolyte:
                    return Math.Floor((enemy.MaximumHealth - enemy.Health) * damage);
                case ClassID.CDOTA_Unit_Hero_Nyx_Assassin:
                    var tempBurn = damage*Math.Floor(enemy.TotalIntelligence);
                    return enemy.Mana < tempBurn ? enemy.Mana : tempBurn;
                case ClassID.CDOTA_Unit_Hero_Obsidian_Destroyer:
                    if (me.TotalIntelligence > enemy.TotalIntelligence)
                        return ((Math.Floor(me.TotalIntelligence) - Math.Floor(enemy.TotalIntelligence))*damage);
                    return 0;
                case ClassID.CDOTA_Unit_Hero_Elder_Titan:
                    var pasDmg = new[] {1.12, 1.19, 1.25, 1.25};
                    var pas = me.Spellbook.Spell3.Level;
                    if (pas != 0)
                    { 
                        if (enemy.Modifiers.FirstOrDefault(modifier => modifier.Name == "modifier_elder_titan_natural_order") == null)
                            return (pasDmg[pas] * damage);
                        return damage;
                    }
                    return damage;
                case ClassID.CDOTA_Unit_Hero_Shadow_Demon:
                    var actDmg = new[] {1, 2, 4, 8, 16};
                    var poison = enemy.Modifiers.FirstOrDefault(modifier => modifier.Name == "modifier_shadow_demon_shadow_poison");
                    if (poison != null)
                    {
                        var Mod = poison.StackCount;
                        if (Mod != 0 && Mod < 6)
                            return (actDmg[Mod])*damage;
                        else if (Mod > 5)
                            return (damage * 16) + ((Mod - 5) * 50);
                    }
                    return 0;
                case ClassID.CDOTA_Unit_Hero_Legion_Commander:
                    var bonusCreep = new[] {14, 16, 18, 20};
                    var bonusHero = new[] {20, 35, 50, 65};
                    var heroDmg = ObjectMgr.GetEntities<Hero>().Where(hero => hero.IsAlive && hero.Team == me.GetEnemyTeam() && hero.IsVisible && hero.Health > 0 && enemy.Distance2D(hero) < 330).ToList().Count * bonusHero[level];
                    var creepDmg = ObjectMgr.GetEntities<Unit>().Where(unit => ((unit.ClassID == ClassID.CDOTA_BaseNPC_Creep && unit.ClassID != ClassID.CDOTA_BaseNPC_Creature && !unit.IsAncient) || unit.ClassID == ClassID.CDOTA_Unit_VisageFamiliar || unit.ClassID == ClassID.CDOTA_Unit_Undying_Zombie || unit.ClassID == ClassID.CDOTA_Unit_SpiritBear || unit.ClassID == ClassID.CDOTA_Unit_Broodmother_Spiderling || unit.ClassID == ClassID.CDOTA_Unit_Hero_Beastmaster_Boar || unit.ClassID == ClassID.CDOTA_Unit_Hero_Beastmaster_Hawk || unit.ClassID == ClassID.CDOTA_BaseNPC_Invoker_Forged_Spirit) && unit.Team != me.Team && unit.IsAlive && unit.IsVisible && unit.Health > 0 && enemy.Distance2D(unit) < 350).ToList().Count * bonusCreep[level];
                    return  Math.Floor((damage + heroDmg + creepDmg));
                case ClassID.CDOTA_Unit_Hero_Zuus:
                    var hp = new[] {.05, .07, .09, .11};
                    if (me.Spellbook.Spell3.Level > 0 && me.Distance2D(enemy) < 1000)
                        damage = (damage + ((hp[me.Spellbook.Spell3.Level]) * enemy.Health));
                    return damage;
            }
            return damage;
        }

        private static double GetSmartDamage(uint level, Hero me, IReadOnlyList<double> damage)
        {
            var baseDmg = damage[(int)level];
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

        private static bool MeCanSurvive(Hero enemy, Hero me, Ability spell, float damageDone)
        {
            if (me.IsMagicImmune() || (NotDieFromSpell(spell, enemy, me) && enemy.Modifiers.FirstOrDefault(modifier => modifier.Name == "modifier_nyx_assassin_spiked_carapace") == null && NotDieFromBladeMail(enemy, me, damageDone)))
                return true;
            return false;
        }

        private static bool NotDieFromBladeMail(Hero enemy, Hero me, float damageDone)
        {
            if (enemy.Modifiers.FirstOrDefault(modifier => modifier.Name == "modifier_item_blade_mail_reflect") != null && me.Health < me.DamageTaken(damageDone, DamageType.Magical, enemy, false))
                return false;
            return true;
        }

        private static bool NotDieFromSpell(Ability spell, Hero enemy, Hero me)
        {
            if (me.Modifiers.FirstOrDefault(modifier => modifier.Name == "modifier_pugna_nether_ward_aura") != null)
                if (me.Health < me.DamageTaken((spell.ManaCost * (float)1.75), DamageType.Magical, enemy, false))
                    return false;
            return true;
        }

        private static void CastSpell(Ability spell, Unit target, Unit me, bool lsblock)
        {
            Console.Write("casting!");
            if (spell.CanBeCasted() && me.CanCast() && (target.Modifiers.FirstOrDefault(modifier => modifier.Name == "modifier_item_sphere") == null || target.FindItem("item_sphere").Cooldown > 0) || lsblock == false)
                spell.UseAbility(target);
        }
        private static void CastSpell(Ability spell, Vector3 targetPos, Unit me, bool lsblock)
        {
            Console.Write("casting!");
            if (spell.CanBeCasted() && me.CanCast() || lsblock == false)
                spell.UseAbility(targetPos);
        }

        private static float MorphMustDie(Hero target, float value)
        {
            if (target.ClassID != ClassID.CDOTA_Unit_Hero_Morphling) return 0;

            uint[] hp = { 38, 76, 114, 190 };
            if (target.Spellbook.Spell3.Level > 0)
                if (target.Modifiers.FirstOrDefault(modifier => modifier.Name == "modifier_morphling_morph_agi") != null && target.Strength > 1)
                    return value * (0 - hp[target.Spellbook.Spell3.Level] + 1);
                else if (target.Modifiers.FirstOrDefault(modifier => modifier.Name == "modifier_morphling_morph_str") != null && target.Agility > 1)
                    return value * hp[target.Spellbook.Spell3.Level];
                else
                    return 0;
            return 0;
        }

        private static void KillStealer_OnWndProc(WndEventArgs args)
        {
            if (args.Msg != (ulong)Utils.WindowsMessages.WM_KEYUP || args.WParam != 'Z' || Game.IsChatOpen)
                return;
            _killstealEnabled = !_killstealEnabled;
        }

        private static void KillStealer_OnDraw(EventArgs args)
        {
            if (!Game.IsInGame || Game.IsPaused)
                return;
            var me = ObjectMgr.LocalHero;
            if (me == null)
                return;

            var enemies = ObjectMgr.GetEntities<Hero>().Where(enemy => enemy.Team == me.GetEnemyTeam() && !enemy.IsIllusion()).ToList();
            foreach (var enemy in enemies)
            {
                Vector2 screenPos;
                var enemyPos = enemy.Position + new Vector3(0, 0, enemy.HealthBarOffset);

                if (Drawing.WorldToScreen(enemyPos, out screenPos))
                {
                    var start = screenPos + new Vector2(-51, -40);
                    float damageNeeded;
                    Ability spell;
                    if (HeroDamageDictionary.TryGetValue(enemy, out damageNeeded) && HeroSpellDictionary.TryGetValue(enemy, out spell))
                    {
                        var text = "D a m a g e  f o r  K S: " + Math.Floor(damageNeeded).ToString(CultureInfo.InvariantCulture);
                        var textSize = Drawing.MeasureText(text, "Arial", new Vector2(10, 150), FontFlags.None);
                        var textPos = start + new Vector2(51 - textSize.X / 2, -textSize.Y / 2 + 2);
                        //Drawing.DrawRect(textPos - new Vector2(-10,0), new Vector2(24,24), Drawing.GetTexture("materials/NyanUI/spellicons/" + spell.Name + ".vmt"));
                        if(damageNeeded < 0)
                            Drawing.DrawText(text, "Arial", textPos, new Vector2(10, 150), new Color(0x99FFFF99), FontFlags.AntiAlias | FontFlags.DropShadow);
                        else
                            Drawing.DrawText(text, "Arial", textPos, new Vector2(10, 150), new Color(0xFFFFFF99), FontFlags.AntiAlias | FontFlags.DropShadow);

                    }
                }
            }
        }
    }
}
