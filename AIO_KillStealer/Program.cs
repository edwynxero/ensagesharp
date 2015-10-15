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
            Game.OnWndProc += KillStealer_OnWndProc;
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
                    Kill(true, me, me.Spellbook.Spell1, new uint[] {100, 150, 200, 250}, null, null, 1, true);
                    break;
                case ClassID.CDOTA_Unit_Hero_Bane:
                    Kill(true, me, me.Spellbook.Spell2,new uint[] {90, 160, 230, 300},null,null,1, true);
                    break;
                case ClassID.CDOTA_Unit_Hero_BountyHunter:
                    Kill(true, me, me.Spellbook.Spell1, new uint[] {100, 200, 250, 325},null,700,1, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Broodmother:
                    Kill(true, me, me.Spellbook.Spell1, new uint[] {75, 150, 225, 300}, null, null, 1, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Centaur:
                    Kill(true, me, me.Spellbook.Spell2, new uint[] {175, 250, 325, 400}, null, 300, 1, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Chen:
                    Kill(true, me, me.Spellbook.Spell2, new uint[] {50, 100, 150, 200}, null, null, 1, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_CrystalMaiden:
                    Kill(false, me, me.Spellbook.Spell1, new uint[] {100, 150, 200, 250}, null, 700, 2, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_DeathProphet:
                    Kill(false, me, me.Spellbook.Spell1, new uint[] {75, 150, 225, 300}, null, null, 1, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_DragonKnight:
                    Kill(false, me, me.Spellbook.Spell1, new uint[] {90, 170, 240, 300}, null, null, 1, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_EarthSpirit:
                    Kill(true, me, me.Spellbook.Spell1, new uint[] {125, 125, 125, 125}, null, 250, 1, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Earthshaker:
                    Kill(false, me, me.Spellbook.Spell1, new uint[] {125, 175, 225, 275}, null, null, 2, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Leshrac:
                    Kill(true, me, me.Spellbook.Spell3, new uint[] {80, 140, 200, 260}, null, null, 1, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Lich:
                    Kill(true, me, me.Spellbook.Spell1, new uint[] {115, 200, 275, 350}, null, null, 1, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Lion:
                    Kill(true, me, me.Spellbook.Spell4, new uint[] {600, 725, 850}, new uint[] {725, 875, 1025}, null, 1, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Luna:
                    Kill(true, me, me.Spellbook.Spell1, new uint[] {75, 150, 210, 260}, null, null, 1, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_NightStalker:
                    Kill(true, me, me.Spellbook.Spell1, new uint[] {90, 160, 225, 335}, null, null, 1, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Oracle:
                    Kill(true, me, me.Spellbook.Spell3, new uint[] {90, 180, 270, 360}, null, null, 1, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_PhantomLancer:
                    Kill(true, me, me.Spellbook.Spell1, new uint[] {100, 150, 200, 250}, null, null, 1, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Puck:
                    Kill(false, me, me.Spellbook.Spell2, new uint[] {70, 140, 210, 280}, null, 400, 3, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_PhantomAssassin:
                    Kill(true, me, me.Spellbook.Spell1, new uint[] {30, 50, 70, 90}, null, null, 1, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_QueenOfPain:
                    Kill(false, me, me.Spellbook.Spell3, new uint[] {85, 165, 225, 300}, null, 475, 3, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Rattletrap:
                    Kill(false, me, me.Spellbook.Spell3, new uint[] {80, 120, 160, 200}, null, 1000, 2, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Rubick:
                    Kill(true, me, me.Spellbook.Spell2, new uint[] {70, 140, 210, 280}, null, null, 1, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_SkeletonKing:
                    Kill(true, me, me.Spellbook.Spell1, new uint[] {80, 160, 230, 300}, null, null, 1, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Shredder:
                    Kill(false, me, me.Spellbook.Spell1, new uint[] {100, 150, 200, 250}, null, 300, 3, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Spectre:
                    Kill(true, me, me.Spellbook.Spell1, new uint[] {50, 100, 150, 200}, null, 2000, 1, true);
                    break;
                case ClassID.CDOTA_Unit_Hero_ShadowShaman:
                    Kill(true, me, me.Spellbook.Spell1, new uint[] {140, 200, 260, 320}, null, null, 1, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Sniper:
                    Kill(true, me, me.Spellbook.Spell4, new uint[] {350, 500, 650}, null, null, 1, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Sven:
                    Kill(true, me, me.Spellbook.Spell1, new uint[] {100, 175, 250, 325}, null, 650, 1, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Tidehunter:
                    Kill(true, me, me.Spellbook.Spell1, new uint[] {110, 160, 210, 260}, null, 750, 1, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Tinker:
                    if (me.FindSpell("tinker_laser").AbilityState == AbilityState.Ready)
                        Kill(true, me, me.Spellbook.Spell1, new uint[] {80, 160, 240, 320}, null, null, 1, false);
                    else if (me.FindSpell("tinker_heat_seeking_missile").AbilityState == AbilityState.Ready)
                        Kill(false, me, me.Spellbook.Spell2, new uint[] {125, 200, 275, 350}, null, 2500, 3, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_VengefulSpirit:
                    Kill(true, me, me.Spellbook.Spell1, new uint[] {100, 175, 250, 325}, null, null, 1, false);
                    break;
                case ClassID.CDOTA_Unit_Hero_Lina:
                    Kill(true, me, me.Spellbook.Spell4, new uint[] {450, 675, 950}, null, null, 1, me.AghanimState());
                    break;
            }
        }

        private static void Kill(bool lsblock, Hero me, Ability ability, IReadOnlyList<uint> damage, IReadOnlyList<uint> adamage, uint? range, uint spellTargetType, bool piercesSpellImmunity)
        {
            if (ability.Level > 0)
            {
                var spellDamage = me.AghanimState() ? damage[(int) ability.Level] : adamage[(int) ability.Level];
                var spellDamageType = ability.DamageType;
                var spellRange = range ?? (ability.CastRange + 50);
                var spellCastPoint = (float) (ability.GetCastPoint() + Game.AvgPing / 1000);

                var enemies = ObjectMgr.GetEntities<Hero>().Where(enemy => enemy.Team == me.GetEnemyTeam() && !enemy.IsIllusion() && enemy.IsVisible && enemy.IsAlive && enemy.Health > 0).ToList();
                foreach (var enemy in enemies)
                {
                    var damageDone = enemy.DamageTaken(spellDamage, spellDamageType, me, piercesSpellImmunity);
                    float damageNeeded;
                    
                    if (!HeroDamageDictionary.TryGetValue(enemy, out damageNeeded))
                    {
                        damageNeeded = enemy.Health - damageDone + spellCastPoint*enemy.HealthRegeneration + MorphMustDie(enemy, spellCastPoint);
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

            uint[] hp = {38, 76, 114, 190};
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
                        Drawing.DrawText(text, "Arial", textPos, new Vector2(10, 150), Color.White, FontFlags.AntiAlias | FontFlags.DropShadow);
                    }
                }
            }
        }
    }
}
