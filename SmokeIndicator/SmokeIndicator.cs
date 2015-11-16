namespace SmokeIndicator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ensage;
    using Ensage.Common;
    using SharpDX;
    internal class SmokeIndicator
    {
        private static Hero mHero;
        private static List<Dictionary<int,string>> last = new List<Dictionary<int, string>>(); 
        static void Main()
        {
            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (!Game.IsInGame || !Utils.SleepCheck("SmokeIndicator")) return;

            if (mHero == null) mHero = ObjectMgr.LocalHero;
            if (mHero == null) return; Utils.Sleep(1000, "SmokeIndicator");

            var smoke = ObjectMgr.GetEntities<Item>().Where(item => item.Name == "item_smoke_of_deceit" && item.Team != mHero.Team).ToList();
            for (var i = 0; i < smoke.Count; i++)
            {
                if(last.ElementAt(i) == null)
                    last.ElementAt(i).Add(0,"");
                if (smoke.Count == last.ElementAt(i).Count) continue;
                if(smoke.ElementAt(i) != null)
                    last.ElementAt(i)[smoke.Count] = smoke.ElementAt(i).Owner.Name.Replace("npc_dota_hero_", "");
                else
                    GenerateSideMessage(last.ElementAt(i)[0], "smoke_of_deceit");
            }
        }

        private static void GenerateSideMessage(string hero, string item)
        {
            var sideMessage = new SideMessage(hero, new Vector2(200, 50));
            sideMessage.AddElement(new Vector2(10, 10), new Vector2(72, 40), Drawing.GetTexture("materials/ensage_ui/heroes_horizontal" + hero + ".vmat"));
            sideMessage.AddElement(new Vector2(85, 1), new Vector2(62, 61), Drawing.GetTexture("materials/ensage_ui/statpop_question.vmat"));
            sideMessage.AddElement(new Vector2(140, 13), new Vector2(70, 35), Drawing.GetTexture("materials/ensage_ui/items" + item + ".vmat"));
        }
    }
}
