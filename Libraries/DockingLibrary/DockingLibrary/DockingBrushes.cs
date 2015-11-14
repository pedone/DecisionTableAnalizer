using System;
using System.Windows;

namespace DockingLibrary
{
    public static class DockingBrushes
    {

        private enum BrushKey
        {
            MenuItemForeground,
            MenuItemBackground,
            MenuItemBorder,
            MenuItemMouseOverForeground,
            MenuItemMouseOverBackground,
            MenuItemMouseOverBorder,
            MenuItemPressedForeground,
            MenuItemPressedBackground,
            MenuItemPressedBorder,
            MenuItemSubmenuOpenForeground,
            MenuItemSubmenuOpenBackground,
            MenuItemSubmenuOpenBorder,
            MenuPopupBorder,
            MenuPopupBackground,
            MenuPopupIconColumnBackground,
            DockManagerBackground,
            ViewBackground,
            PinButtonBackground,
            PinButtonForeground,
            PinButtonBorder,
            PinButtonActiveBackground,
            PinButtonActiveForeground,
            PinButtonActiveBorder,
            PinButtonMouseOverBackground,
            PinButtonMouseOverForeground,
            PinButtonMouseOverBorder,
            PinButtonPressedBackground,
            PinButtonPressedForeground,
            PinButtonPressedBorder,
            DocumentPinButtonBackground,
            DocumentPinButtonForeground,
            DocumentPinButtonBorder,
            DocumentPinButtonActiveBackground,
            DocumentPinButtonActiveForeground,
            DocumentPinButtonActiveBorder,
            DocumentPinButtonMouseOverBackground,
            DocumentPinButtonMouseOverForeground,
            DocumentPinButtonMouseOverBorder,
            DocumentPinButtonSelectedBackground,
            DocumentPinButtonSelectedForeground,
            DocumentPinButtonSelectedBorder,
            DocumentPinButtonPressedBackground,
            DocumentPinButtonPressedForeground,
            DocumentPinButtonPressedBorder,
            DocumentPinButtonMouseOverActiveBackground,
            DocumentPinButtonMouseOverActiveForeground,
            DocumentPinButtonMouseOverActiveBorder,
            DocumentPinButtonPressedActiveBackground,
            DocumentPinButtonPressedActiveForeground,
            DocumentPinButtonPressedActiveBorder,
            DocumentGroupBackground,
            DocumentGroupContentBorder,
            DocumentGroupContentOuterBorderLine,
            DocumentGroupContentActiveBorder,
            DocumentGroupContentActiveOuterBorderLine,
            DocumentGroupTabItemBackground,
            DocumentGroupTabItemBorder,
            DocumentGroupTabItemForeground,
            DocumentGroupTabItemSelectedBorder,
            DocumentGroupTabItemSelectedBackground,
            DocumentGroupTabItemSelectedForeground,
            DocumentGroupTabItemActiveBorder,
            DocumentGroupTabItemActiveBackground,
            DocumentGroupTabItemActiveForeground,
            DocumentGroupTabItemMouseOverBackground,
            DocumentGroupTabItemMouseOverBorder,
            DocumentGroupTabItemMouseOverForeground,
            DocumentGroupContainerBackground,
            DockGroupSplitterBackground,
            TabGroupBackground,
            TabGroupContentBorder,
            TabGroupContentOuterBorderLine,
            TabGroupContentActiveBorder,
            TabGroupContentActiveOuterBorderLine,
            TabGroupTabItemBackground,
            TabGroupTabItemBorder,
            TabGroupTabItemForeground,
            TabGroupTabItemMouseOverBackground,
            TabGroupTabItemMouseOverBorder,
            TabGroupTabItemMouseOverForeground,
            TabGroupTabItemSelectedBackground,
            TabGroupTabItemSelectedBorder,
            TabGroupTabItemSelectedForeground,
            TabGroupTabItemActiveBackground,
            TabGroupTabItemActiveBorder,
            TabGroupTabItemActiveForeground,
            AutoHideChannelBackground,
            AutoHideChannelBorder,
            AutoHideChannelForeground,
            AutoHideChannelTabItemBackground,
            AutoHideChannelTabItemBorder,
            AutoHideChannelTabItemForeground,
            AutoHideChannelTabItemMouseOverBackground,
            AutoHideChannelTabItemMouseOverBorder,
            AutoHideChannelTabItemMouseOverForeground,
            AutoHideChannelTabItemSelectedBackground,
            AutoHideChannelTabItemSelectedBorder,
            AutoHideChannelTabItemSelectedForeground
        }

        public static ResourceKey MenuPopupBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.MenuPopupBackground);
            }
        }
        public static ResourceKey MenuPopupIconColumnBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.MenuPopupIconColumnBackground);
            }
        }
        public static ResourceKey MenuPopupBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.MenuPopupBorder);
            }
        }
        public static ResourceKey MenuItemForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.MenuItemForeground);
            }
        }
        public static ResourceKey MenuItemBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.MenuItemBackground);
            }
        }
        public static ResourceKey MenuItemBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.MenuItemBorder);
            }
        }
        public static ResourceKey MenuItemMouseOverForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.MenuItemMouseOverForeground);
            }
        }
        public static ResourceKey MenuItemMouseOverBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.MenuItemMouseOverBackground);
            }
        }
        public static ResourceKey MenuItemMouseOverBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.MenuItemMouseOverBorder);
            }
        }
        public static ResourceKey MenuItemPressedForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.MenuItemPressedForeground);
            }
        }
        public static ResourceKey MenuItemPressedBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.MenuItemPressedBackground);
            }
        }
        public static ResourceKey MenuItemPressedBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.MenuItemPressedBorder);
            }
        }
        public static ResourceKey MenuItemSubmenuOpenForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.MenuItemSubmenuOpenForeground);
            }
        }
        public static ResourceKey MenuItemSubmenuOpenBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.MenuItemSubmenuOpenBackground);
            }
        }
        public static ResourceKey MenuItemSubmenuOpenBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.MenuItemSubmenuOpenBorder);
            }
        }
        public static ResourceKey DockManagerBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DockManagerBackground);
            }
        }
        public static ResourceKey ViewBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.ViewBackground);
            }
        }
        public static ResourceKey PinButtonBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.PinButtonBackground);
            }
        }
        public static ResourceKey PinButtonForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.PinButtonForeground);
            }
        }
        public static ResourceKey PinButtonBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.PinButtonBorder);
            }
        }
        public static ResourceKey PinButtonActiveBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.PinButtonActiveBackground);
            }
        }
        public static ResourceKey PinButtonActiveForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.PinButtonActiveForeground);
            }
        }
        public static ResourceKey PinButtonActiveBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.PinButtonActiveBorder);
            }
        }
        public static ResourceKey PinButtonMouseOverBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.PinButtonMouseOverBackground);
            }
        }
        public static ResourceKey PinButtonMouseOverForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.PinButtonMouseOverForeground);
            }
        }
        public static ResourceKey PinButtonMouseOverBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.PinButtonMouseOverBorder);
            }
        }
        public static ResourceKey PinButtonPressedBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.PinButtonPressedBackground);
            }
        }
        public static ResourceKey PinButtonPressedForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.PinButtonPressedForeground);
            }
        }
        public static ResourceKey PinButtonPressedBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.PinButtonPressedBorder);
            }
        }
        public static ResourceKey DocumentPinButtonBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentPinButtonBackground);
            }
        }
        public static ResourceKey DocumentPinButtonForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentPinButtonForeground);
            }
        }
        public static ResourceKey DocumentPinButtonBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentPinButtonBorder);
            }
        }
        public static ResourceKey DocumentPinButtonActiveBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentPinButtonActiveBackground);
            }
        }
        public static ResourceKey DocumentPinButtonActiveForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentPinButtonActiveForeground);
            }
        }
        public static ResourceKey DocumentPinButtonActiveBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentPinButtonActiveBorder);
            }
        }
        public static ResourceKey DocumentPinButtonMouseOverBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentPinButtonMouseOverBackground);
            }
        }
        public static ResourceKey DocumentPinButtonMouseOverForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentPinButtonMouseOverForeground);
            }
        }
        public static ResourceKey DocumentPinButtonMouseOverBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentPinButtonMouseOverBorder);
            }
        }
        public static ResourceKey DocumentPinButtonSelectedBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentPinButtonSelectedBackground);
            }
        }
        public static ResourceKey DocumentPinButtonSelectedForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentPinButtonSelectedForeground);
            }
        }
        public static ResourceKey DocumentPinButtonSelectedBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentPinButtonSelectedBorder);
            }
        }
        public static ResourceKey DocumentPinButtonPressedBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentPinButtonPressedBackground);
            }
        }
        public static ResourceKey DocumentPinButtonPressedForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentPinButtonPressedForeground);
            }
        }
        public static ResourceKey DocumentPinButtonPressedBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentPinButtonPressedBorder);
            }
        }
        public static ResourceKey DocumentPinButtonMouseOverActiveBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentPinButtonMouseOverActiveBackground);
            }
        }
        public static ResourceKey DocumentPinButtonMouseOverActiveForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentPinButtonMouseOverActiveForeground);
            }
        }
        public static ResourceKey DocumentPinButtonMouseOverActiveBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentPinButtonMouseOverActiveBorder);
            }
        }
        public static ResourceKey DocumentPinButtonPressedActiveBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentPinButtonPressedActiveBackground);
            }
        }
        public static ResourceKey DocumentPinButtonPressedActiveForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentPinButtonPressedActiveForeground);
            }
        }
        public static ResourceKey DocumentPinButtonPressedActiveBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentPinButtonPressedActiveBorder);
            }
        }
        public static ResourceKey DocumentGroupBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentGroupBackground);
            }
        }
        public static ResourceKey DocumentGroupContentBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentGroupContentBorder);
            }
        }
        public static ResourceKey DocumentGroupContentOuterBorderLine
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentGroupContentOuterBorderLine);
            }
        }
        public static ResourceKey DocumentGroupContentActiveBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentGroupContentActiveBorder);
            }
        }
        public static ResourceKey DocumentGroupContentActiveOuterBorderLine
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentGroupContentActiveOuterBorderLine);
            }
        }
        public static ResourceKey DocumentGroupTabItemBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentGroupTabItemBackground);
            }
        }
        public static ResourceKey DocumentGroupTabItemBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentGroupTabItemBorder);
            }
        }
        public static ResourceKey DocumentGroupTabItemForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentGroupTabItemForeground);
            }
        }
        public static ResourceKey DocumentGroupTabItemSelectedBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentGroupTabItemSelectedBorder);
            }
        }
        public static ResourceKey DocumentGroupTabItemSelectedBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentGroupTabItemSelectedBackground);
            }
        }
        public static ResourceKey DocumentGroupTabItemSelectedForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentGroupTabItemSelectedForeground);
            }
        }
        public static ResourceKey DocumentGroupTabItemActiveBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentGroupTabItemActiveBorder);
            }
        }
        public static ResourceKey DocumentGroupTabItemActiveBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentGroupTabItemActiveBackground);
            }
        }
        public static ResourceKey DocumentGroupTabItemActiveForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentGroupTabItemActiveForeground);
            }
        }
        public static ResourceKey DocumentGroupTabItemMouseOverBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentGroupTabItemMouseOverBackground);
            }
        }
        public static ResourceKey DocumentGroupTabItemMouseOverBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentGroupTabItemMouseOverBorder);
            }
        }
        public static ResourceKey DocumentGroupTabItemMouseOverForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentGroupTabItemMouseOverForeground);
            }
        }
        public static ResourceKey DocumentGroupContainerBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DocumentGroupContainerBackground);
            }
        }
        public static ResourceKey DockGroupSplitterBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.DockGroupSplitterBackground);
            }
        }
        public static ResourceKey TabGroupBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.TabGroupBackground);
            }
        }
        public static ResourceKey TabGroupContentBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.TabGroupContentBorder);
            }
        }
        public static ResourceKey TabGroupContentOuterBorderLine
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.TabGroupContentOuterBorderLine);
            }
        }
        public static ResourceKey TabGroupContentActiveBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.TabGroupContentActiveBorder);
            }
        }
        public static ResourceKey TabGroupContentActiveOuterBorderLine
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.TabGroupContentActiveOuterBorderLine);
            }
        }
        public static ResourceKey TabGroupTabItemBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.TabGroupTabItemBackground);
            }
        }
        public static ResourceKey TabGroupTabItemBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.TabGroupTabItemBorder);
            }
        }
        public static ResourceKey TabGroupTabItemForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.TabGroupTabItemForeground);
            }
        }
        public static ResourceKey TabGroupTabItemMouseOverBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.TabGroupTabItemMouseOverBackground);
            }
        }
        public static ResourceKey TabGroupTabItemMouseOverBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.TabGroupTabItemMouseOverBorder);
            }
        }
        public static ResourceKey TabGroupTabItemMouseOverForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.TabGroupTabItemMouseOverForeground);
            }
        }
        public static ResourceKey TabGroupTabItemSelectedBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.TabGroupTabItemSelectedBackground);
            }
        }
        public static ResourceKey TabGroupTabItemSelectedBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.TabGroupTabItemSelectedBorder);
            }
        }
        public static ResourceKey TabGroupTabItemSelectedForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.TabGroupTabItemSelectedForeground);
            }
        }
        public static ResourceKey TabGroupTabItemActiveBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.TabGroupTabItemActiveBackground);
            }
        }
        public static ResourceKey TabGroupTabItemActiveBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.TabGroupTabItemActiveBorder);
            }
        }
        public static ResourceKey TabGroupTabItemActiveForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.TabGroupTabItemActiveForeground);
            }
        }
        public static ResourceKey AutoHideChannelBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.AutoHideChannelBackground);
            }
        }
        public static ResourceKey AutoHideChannelBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.AutoHideChannelBorder);
            }
        }
        public static ResourceKey AutoHideChannelForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.AutoHideChannelForeground);
            }
        }
        public static ResourceKey AutoHideChannelTabItemBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.AutoHideChannelTabItemBackground);
            }
        }
        public static ResourceKey AutoHideChannelTabItemBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.AutoHideChannelTabItemBorder);
            }
        }
        public static ResourceKey AutoHideChannelTabItemForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.AutoHideChannelTabItemForeground);
            }
        }
        public static ResourceKey AutoHideChannelTabItemMouseOverBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.AutoHideChannelTabItemMouseOverBackground);
            }
        }
        public static ResourceKey AutoHideChannelTabItemMouseOverBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.AutoHideChannelTabItemMouseOverBorder);
            }
        }
        public static ResourceKey AutoHideChannelTabItemMouseOverForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.AutoHideChannelTabItemMouseOverForeground);
            }
        }
        public static ResourceKey AutoHideChannelTabItemSelectedBackground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.AutoHideChannelTabItemSelectedBackground);
            }
        }
        public static ResourceKey AutoHideChannelTabItemSelectedBorder
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.AutoHideChannelTabItemSelectedBorder);
            }
        }
        public static ResourceKey AutoHideChannelTabItemSelectedForeground
        {
            get
            {
                return new ComponentResourceKey(typeof(DockingBrushes), BrushKey.AutoHideChannelTabItemSelectedForeground);
            }
        }

    }
}
