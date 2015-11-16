// --→ Assembly Namespace (Code)

using Ensage.Common.Extensions;

namespace ItemNotification
{
    // --→ References (Libraries)
    using System.Linq;
    using Ensage;
    using Ensage.Common;
    using SharpDX;
    using System.Collections.Generic;

    // --→ Assembly (Code)
    internal class ItemNotification
    {
        private static readonly Dictionary<uint, bool> Items = new Dictionary<uint, bool>();
        private static void Main()
        {
            //SideMessage.Intialize();
            Game.OnFireEvent += Game_OnFireEvent;
        }
        private static void Game_OnFireEvent(FireEventEventArgs args)
        {
            if (!Game.IsInGame || ObjectMgr.LocalHero == null) return;
            if (args.GameEvent.Name != "dota_inventory_changed") return;

            var itemsPurchased = ObjectMgr.GetEntities<Item>().Where(item => !Items.ContainsKey(item.Handle) && item.Owner.Team != ObjectMgr.LocalHero.Team && !item.Owner.IsIllusion() && item.Cost >= 1000 && !item.IsRecipe  && !item.Owner.Name.Equals("npc_dota_hero_roshan")).ToList();
            if (!itemsPurchased.Any())
                return;

            foreach (var item in itemsPurchased)
            {
                Items[item.Handle] = true;
                GenerateSideMessage(item.Owner.Name.Replace("npc_dota_hero_", ""), item.Name.Replace("item_", ""));
            }
        }

        private static void GenerateSideMessage(string hero, string item)
        {
            var sideMSG = new SideMessage(hero, new Vector2(200, 48));
            sideMSG.AddElement(new Vector2(006, 06), new Vector2(72, 36), Drawing.GetTexture("materials/ensage_ui/heroes_horizontal/" + hero + ".vmat"));
            sideMSG.AddElement(new Vector2(078, 12), new Vector2(64, 32), Drawing.GetTexture("materials/ensage_ui/other/arrow_item_bought.vmat"));
            sideMSG.AddElement(new Vector2(142, 06), new Vector2(72, 36), Drawing.GetTexture("materials/ensage_ui/items/" + item + ".vmat"));
            sideMSG.CreateMessage();
            
        }
    }
}
