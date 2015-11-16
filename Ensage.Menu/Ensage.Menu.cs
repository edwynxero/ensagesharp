/* <copyright file = "Ensage.Menu.cs" company="EnsageSharp">
 *    Copyright (c) 2015 edwynxero @ EnsageSharp.
 *
 *    This program is free software: you can redistribute it and/or modify
 *    it under the terms of the GNU General Public License as published by
 *    the Free Software Foundation, either version 3 of the License, or
 *    (at your option) any later version.
 *
 *    This program is distributed in the hope that it will be useful,
 *    but WITHOUT ANY WARRANTY; without even the implied warranty of
 *    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *    GNU General Public License for more details.
 *
 *    You should have received a copy of the GNU General Public License
 *    along with this program.  If not, see http://www.gnu.org/licenses/
 * </copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Ensage.Common;
using SharpDX;
using SharpDX.Direct3D9;

namespace Ensage.Menu
{
    public class EnsageMenu
    {
        #region Fields
        private static Vector2 mScreenSize;

        private static bool mMenuIntialized, mScreenVariablesSet;
        private static bool mOpenMenuClicked, mIsMenuCreated, mMenuShowText, mMenuTitlesCreated;
        private static float mMenuPosition, mButtonHeight;
        private static Vector2 mMenuPos, mMenuButtonPos, mMenuSize, mMenuButtonSize, mItemPosition;

        private static List<EnsageMenu> mEnsageMenus;
        private static Font mEnsageMenusTitle;
        private static int mIndex;

        private string mMenuTitle, mMenuFooter;
        private Font mMenuHeaderFont, mMenuFooterFont;
        private readonly List<EnsageMenuItem> mMenuItems;
        #endregion
        static EnsageMenu() {
            Initialize();
        }

        public EnsageMenu()
        {
            mMenuItems = new List<EnsageMenuItem>();
            mEnsageMenusTitle = mMenuHeaderFont = new Font(Drawing.Direct3DDevice9, new FontDescription
            {
                FaceName = "Century Gothic",
                Height = 18,
                OutputPrecision = FontPrecision.Outline,
                Quality = FontQuality.Proof
            });
            mMenuFooterFont = new Font(Drawing.Direct3DDevice9, new FontDescription
            {
                FaceName = "Comic Sans MS",
                Height = 24,
                OutputPrecision = FontPrecision.Outline,
                Quality = FontQuality.Proof
            });
        }
        public void SetHeaderFont(Font menuHeaderFont)
        {
            mMenuHeaderFont = menuHeaderFont;
        }
        public void SetFooterFont(Font menuFooterFont)
        {
            mMenuFooterFont = menuFooterFont;
        }

        public static void Initialize()
        {
            if (mMenuIntialized) return;
            mEnsageMenus = new List<EnsageMenu>();
            mMenuIntialized = true;

            Game.OnWndProc += MenuClickEvent;
            Drawing.OnPreReset += OnPreReset;
            Drawing.OnPostReset += OnPostReset;
            Drawing.OnEndScene += OnEndScene;
            Drawing.OnDraw += OnDraw;
        }

        public void SetTitle(string title)
        {
            mMenuTitle = title;
        }
        public void SetFooter(string footer)
        {
            mMenuFooter = footer;
        }
        public static void AddMenu(EnsageMenu ensageMenu)
        {
            mEnsageMenus.Add(ensageMenu);
        }

        public void AddItem(EnsageMenuItem ensageMenuItem)
        {
            mMenuItems.Add(ensageMenuItem);
        }
        public void AddItem(string type, string content, bool isActive = false)
        {
            mMenuItems.Add(new EnsageMenuItem(type, content, isActive));
        }
        public EnsageMenuItem CreateItem(string type, string content, bool isActive = false)
        {
            return new EnsageMenuItem(type, content, isActive);
        }

        public class EnsageMenuItem
        {
            private string mItemType;
            private string mItemTextureActive, mItemTexturePassive;
            private string mItemHoverTextureActive, mItemHoverTexturePassive;
            private string mItemContent;
            private Font mMenuTextFont, mItemTextFont;
            private bool mItemHover;
            public bool ItemActive;
            public Vector2 ElementPosition, ElementTextPosition, ElementSize;
            public EnsageMenuItem(string type, string content, bool isActive = false)
            {
                mItemType = type.ToLower();
                switch (mItemType)
                {
                    case "checkbox":
                        SetTexture("materials/ensage_ui/ensagemenu/menu_checkbox_active.vmat", true);
                        SetTexture("materials/ensage_ui/ensagemenu/menu_checkbox_passive.vmat");
                        SetHoverTexture("materials/ensage_ui/ensagemenu/menu_checkbox_active_hover.vmat", true);
                        SetHoverTexture("materials/ensage_ui/ensagemenu/menu_checkbox_passive_hover.vmat");
                        mItemContent = content;
                        break;
                    case "button":
                        SetTexture("materials/ensage_ui/ensagemenu/menu_button.vmat");
                        SetHoverTexture("materials/ensage_ui/ensagemenu/menu_button_hover.vmat");
                        mItemContent = content;
                        break;
                    case "text":
                        mItemContent = content.ToUpper();
                        break;
                }
                mMenuTextFont = new Font(Drawing.Direct3DDevice9, new FontDescription { FaceName = "Copperplate Gothic Bold", Height = 20, OutputPrecision = FontPrecision.Outline, Quality = FontQuality.Proof });
                mItemTextFont = new Font(Drawing.Direct3DDevice9, new FontDescription { FaceName = "Tahoma", Height = 16, OutputPrecision = FontPrecision.Outline, Weight = FontWeight.Thin, Quality = FontQuality.Proof });
                ItemActive = isActive;
            }
            public void SetItemHover(bool isHover)
            {
                mItemHover = isHover;
            }
            public void SetMenuTextFont(Font menuTextFont)
            {
                mMenuTextFont = menuTextFont;
            }
            public Font GetMenuTextFont()
            {
                return mMenuTextFont;
            }
            public void SetItemTextFont(Font itemTextFont)
            {
                mItemTextFont = itemTextFont;
            }
            public Font GetItemTextFont()
            {
                return mItemTextFont;
            }

            public void SetTexture(string texture, bool active = false)
            {
                if (active)
                    mItemTextureActive = texture;
                else
                    mItemTexturePassive = texture;
            }
            public void SetHoverTexture(string texture, bool active = false)
            {
                if (active)
                    mItemHoverTextureActive = texture;
                else
                    mItemHoverTexturePassive = texture;
            }
            public string GetTexture()
            {
                switch (mItemType)
                {
                    case "checkbox":
                        return ItemActive ? mItemHover ? mItemHoverTextureActive : mItemTextureActive : mItemHover ? mItemHoverTexturePassive : mItemTexturePassive;
                    case "button":
                        return mItemHover ? mItemHoverTexturePassive : mItemTexturePassive;
                    default:
                        return ItemActive ? mItemTextureActive : mItemTexturePassive;
                }
            }
            public void SetItemType(string type)
            {
                mItemType = type;
            }
            public string GetItemType()
            {
                return mItemType;
            }
            public void SetContent(string content)
            {
                mItemContent = content;
            }
            public string GetContent()
            {
                return mItemContent;
            }
        }

        public string GetTitle()
        {
            return mMenuTitle;
        }
        public string GetFooter()
        {
            return mMenuFooter;
        }

        public List<EnsageMenuItem> GetMenuItems()
        {
            return mMenuItems;
        }

        private static void OnEndScene(EventArgs args)
        {
            if (Drawing.Direct3DDevice9 == null || Drawing.Direct3DDevice9.IsDisposed || !Game.IsInGame || !mScreenVariablesSet) return;

            if (!mMenuShowText) return;
            mEnsageMenus.ElementAt(mIndex).mMenuHeaderFont.DrawText(null, mEnsageMenus.ElementAt(mIndex).GetTitle(), (int)(mMenuPosition + 6), (int)(mMenuPos.Y + mButtonHeight / 2 - 9), new ColorBGRA(0x88, 0x88, 0x88, 0xFF));
            mEnsageMenus.ElementAt(mIndex).mMenuFooterFont.DrawText(null, mEnsageMenus.ElementAt(mIndex).GetFooter(), (int)(mMenuPosition + 18), (int)(mMenuPos.Y + mMenuSize.Y - (mButtonHeight / 2) - 22), new ColorBGRA(0x00, 0x00, 0x00, 0xFF));

            if (mMenuTitlesCreated)
            {
                var itemPosition = new Vector2(mMenuPos.X - (mMenuSize.X * 3) - 10, mMenuPos.Y);
                for (var i = 1; i <= mEnsageMenus.Count; i++)
                {
                    mEnsageMenusTitle.DrawText(null, mEnsageMenus.ElementAt(i - 1).GetTitle(), (int)itemPosition.X + 6, (int)(itemPosition.Y + (mButtonHeight / 2) - 9), new ColorBGRA(0x88, 0x88, 0x88, 0xFF));
                    if (i % 3 == 0)
                        itemPosition = new Vector2(mMenuPos.X - (mMenuSize.X * 3) - 10, itemPosition.Y + mButtonHeight + 3);
                    else
                        itemPosition += new Vector2(mMenuSize.X + 3, 0);
                }
            }

            if (!mEnsageMenus.ElementAt(mIndex).GetMenuItems().Any()) return;
            foreach (var item in mEnsageMenus.ElementAt(mIndex).GetMenuItems())
            {
                if (item.GetItemType().Equals("text"))
                    item.GetMenuTextFont().DrawText(null, item.GetContent(), (int)item.ElementTextPosition.X, (int)item.ElementTextPosition.Y, new ColorBGRA(0x99, 0x99, 0x99, 0xFF));
                else
                    item.GetItemTextFont().DrawText(null, item.GetContent(), (int)item.ElementTextPosition.X, (int)item.ElementTextPosition.Y, new ColorBGRA(0xCC, 0xCC, 0xCC, 0xFF));
            }
        }
        private static void OnPostReset(EventArgs args)
        {
            mEnsageMenusTitle.OnResetDevice();
            mEnsageMenus.ElementAt(mIndex).mMenuHeaderFont.OnResetDevice();
            foreach (var item in mEnsageMenus.ElementAt(mIndex).GetMenuItems())
            {
                item.GetMenuTextFont().OnResetDevice();
                item.GetMenuTextFont().OnResetDevice();
            }
            mEnsageMenus.ElementAt(mIndex).mMenuFooterFont.OnResetDevice();
        }
        private static void OnPreReset(EventArgs args)
        {
            mEnsageMenusTitle.OnLostDevice();
            mEnsageMenus.ElementAt(mIndex).mMenuHeaderFont.OnLostDevice();
            foreach (var item in mEnsageMenus.ElementAt(mIndex).GetMenuItems())
            {
                item.GetMenuTextFont().OnLostDevice();
                item.GetMenuTextFont().OnLostDevice();
            }
            mEnsageMenus.ElementAt(mIndex).mMenuFooterFont.OnLostDevice();
        }
        private static void OnDraw(EventArgs args)
        {
            if (!Game.IsInGame) return;

            if (!mScreenVariablesSet)
            {
                SetScreenVariables();
                return;
            }

            if (mOpenMenuClicked)
            {
                foreach (var item in mEnsageMenus.ElementAt(mIndex).GetMenuItems())
                    item.SetItemHover(Utils.IsUnderRectangle(Game.MouseScreenPosition, item.ElementPosition.X, item.ElementPosition.Y, item.ElementSize.X, item.ElementSize.Y));

                mMenuShowText = true;
                if (!Equals((int)mMenuPosition, (int)mMenuPos.X))
                {
                    CreateEnsageMenu(mMenuPosition--);
                    Drawing.DrawRect(new Vector2(mMenuPosition - mMenuButtonSize.X, mMenuButtonPos.Y), new Vector2(mMenuButtonSize.X, mMenuButtonSize.Y), Drawing.GetTexture("materials/ensage_ui/ensagemenu/menu_close_passive.vmat"));
                }
                else
                {
                    Drawing.DrawRect(new Vector2(mMenuButtonPos.X - mMenuSize.X, mMenuButtonPos.Y), new Vector2(mMenuButtonSize.X, mMenuButtonSize.Y), Drawing.GetTexture("materials/ensage_ui/ensagemenu/menu_close_passive.vmat"));
                    CreateEnsageMenu(mMenuPos.X);
                    mIsMenuCreated = true;

                    if (mMenuTitlesCreated)
                        CreateMenuTitles();
                }
            }
            else
            {
                if (mIsMenuCreated && !Equals((int)mMenuPosition, (int)mScreenSize.X))
                {
                    CreateEnsageMenu(mMenuPosition++);
                    Drawing.DrawRect(new Vector2(mMenuPosition - mMenuButtonSize.X, mMenuButtonPos.Y), new Vector2(mMenuButtonSize.X, mMenuButtonSize.Y), Drawing.GetTexture("materials/ensage_ui/ensagemenu/menu_open_passive.vmat"));
                }
                else
                {
                    Drawing.DrawRect(new Vector2(mMenuButtonPos.X, mMenuButtonPos.Y), new Vector2(mMenuButtonSize.X, mMenuButtonSize.Y), Drawing.GetTexture("materials/ensage_ui/ensagemenu/menu_open_passive.vmat"));
                    mMenuShowText = false;
                    mIsMenuCreated = false;
                }
            }
        }

        private static void CreateMenuTitles()
        {
            Drawing.DrawRect(new Vector2(mMenuPos.X - (mMenuSize.X * 3) - 13, mMenuPos.Y), new Vector2(mMenuSize.X * 3 + 12, mScreenSize.Y / 4), new Color(0x00, 0x00, 0x00, 0xAB));
            var itemPosition = new Vector2(mMenuPos.X - (mMenuSize.X * 3) - 10, mMenuPos.Y);
            for (var i = 1; i <= mEnsageMenus.Count; i++)
            {
                Drawing.DrawRect(itemPosition, new Vector2(mMenuSize.X, mButtonHeight), new Color(0x18, 0x18, 0x18, 0xFF));
                if (i % 3 == 0)
                    itemPosition = new Vector2(mMenuPos.X - (mMenuSize.X * 3) - 10, itemPosition.Y + mButtonHeight + 3);
                else
                    itemPosition += new Vector2(mMenuSize.X + 3, 0);
            }
        }

        public static void MenuClickEvent(WndEventArgs args)
        {
            if (!Game.IsInGame || args.Msg != (ulong)Utils.WindowsMessages.WM_LBUTTONDOWN) return;

            if (!mOpenMenuClicked && Utils.IsUnderRectangle(Game.MouseScreenPosition, mMenuButtonPos.X, mMenuButtonPos.Y, mMenuButtonSize.X, mMenuButtonSize.Y))
            {
                mMenuPosition = mScreenSize.X;
                mOpenMenuClicked = true;
            }
            else if (mOpenMenuClicked && Utils.IsUnderRectangle(Game.MouseScreenPosition, mMenuButtonPos.X - mMenuSize.X, mMenuButtonPos.Y, mMenuButtonSize.X, mMenuButtonSize.Y))
            {
                mMenuPosition = mMenuPos.X;
                mOpenMenuClicked = false;
                mMenuTitlesCreated = false;
            }
            if (!mIsMenuCreated) return;
            if (Utils.IsUnderRectangle(Game.MouseScreenPosition, mMenuPos.X, mMenuPos.Y, mMenuSize.X - 1, mButtonHeight))
                mMenuTitlesCreated = !mMenuTitlesCreated;

            foreach (var item in mEnsageMenus.ElementAt(mIndex).GetMenuItems().Where(item => !item.GetItemType().Equals("text") && Utils.IsUnderRectangle(Game.MouseScreenPosition, item.ElementPosition.X, item.ElementPosition.Y, item.ElementSize.X, item.ElementSize.Y)))
                item.ItemActive = !item.ItemActive;

            if (!mMenuTitlesCreated) return;
            var itemPosition = new Vector2(mMenuPos.X - (mMenuSize.X * 3) - 10, mMenuPos.Y);
            for (var i = 1; i <= mEnsageMenus.Count; i++)
            {
                if (Utils.IsUnderRectangle(Game.MouseScreenPosition, itemPosition.X, itemPosition.Y, mMenuSize.X, mButtonHeight))
                    mIndex = i - 1;
                if (i % 3 == 0)
                    itemPosition = new Vector2(mMenuPos.X - (mMenuSize.X * 3) - 10, itemPosition.Y + mButtonHeight + 3);
                else
                    itemPosition += new Vector2(mMenuSize.X + 3, 0);
            }
        }

        private static void SetScreenVariables()
        {
            mScreenSize = new Vector2(HUDInfo.ScreenSizeX(), HUDInfo.ScreenSizeY());

            mMenuPosition = mScreenSize.X;
            mButtonHeight = mScreenSize.Y / 30;

            // Open - Close Menu Button
            mMenuButtonSize = new Vector2(mScreenSize.X / 60, mScreenSize.Y / 12);
            mMenuButtonPos = new Vector2(mScreenSize.X - mMenuButtonSize.X, (float)((mScreenSize.Y - (mScreenSize.Y / 5.5)) / 2 - (mMenuButtonSize.Y / 2)));

            // Menu Background
            mMenuSize = new Vector2(mScreenSize.X / 12, (float)(mScreenSize.Y / 1.56));
            mMenuPos = new Vector2(mScreenSize.X - mMenuSize.X, (float)((mScreenSize.Y - (mScreenSize.Y / 5.5)) / 2 - (mMenuSize.Y / 2)));
            mScreenVariablesSet = true;
        }

        private static void CreateEnsageMenu(float menuPosX)
        {
            var menuPosition = new Vector2(menuPosX, mMenuPos.Y);

            // Menu Background
            Drawing.DrawRect(menuPosition, mMenuSize, new Color(0x00, 0x00, 0x00, 0xAB));
            Drawing.DrawRect(menuPosition - 1, mMenuSize + 1, new Color(0x00, 0x00, 0x00), true);

            // Header
            Drawing.DrawRect(menuPosition, new Vector2(mMenuSize.X - 1, mButtonHeight), new Color(0x18, 0x18, 0x18, 0xFF));

            // Footer
            Drawing.DrawRect(menuPosition + new Vector2(10, mMenuSize.Y - mButtonHeight - 10), new Vector2(mMenuSize.X - 20, mButtonHeight), new Color(0x66, 0x22, 0x22));

            // Menu Start!
            mItemPosition = menuPosition + new Vector2(6, mButtonHeight + 10);
            for (var i = 0; i < mEnsageMenus.ElementAt(mIndex).GetMenuItems().Count; i++)
            {
                var item = mEnsageMenus.ElementAt(mIndex).GetMenuItems().ElementAt(i);
                item.ElementSize = new Vector2(mMenuSize.X - 24, mButtonHeight);
                if (item.GetItemType().Equals("text"))
                {
                    item.ElementPosition = mItemPosition;
                    item.ElementTextPosition = mItemPosition;
                    mItemPosition += new Vector2(0, mButtonHeight - 5);
                }
                else if (item.GetItemType().Equals("button"))
                {
                    mItemPosition += new Vector2(6, 0);
                    item.ElementPosition = mItemPosition;
                    Drawing.DrawRect(item.ElementPosition, item.ElementSize, Drawing.GetTexture(item.GetTexture()));
                    item.ElementTextPosition = mItemPosition + new Vector2(6, mButtonHeight / 2 - 8);
                    mItemPosition += new Vector2(-6, mButtonHeight + 10);
                }
                else if (item.GetItemType().Equals("checkbox"))
                {
                    mItemPosition += new Vector2(6, 0);
                    item.ElementPosition = mItemPosition;
                    Drawing.DrawRect(item.ElementPosition, item.ElementSize, Drawing.GetTexture(item.GetTexture()));
                    item.ElementTextPosition = mItemPosition + new Vector2(30, mButtonHeight / 2 - 8);
                    mItemPosition += new Vector2(-6, mButtonHeight + 10);
                }
            }
        }
    }
}
