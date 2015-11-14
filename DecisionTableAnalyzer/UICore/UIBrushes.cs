using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace UICore
{
    public static class UIBrushes
    {
        private enum BrushKey
        {
            RecentProjectButtonForeground,
            RecentProjectButtonBackground,
            RecentProjectButtonBorderBrush,
            RecentProjectButtonMouseOverForeground,
            RecentProjectButtonMouseOverBackground,
            RecentProjectButtonMouseOverBorderBrush,
            LinkButtonForeground,
            LinkButtonMouseOverForeground,
            StartViewBackground,
            StartViewSeperatorBackground,
            ToolButtonBackground,
            ToolButtonBorder,
            ToolButtonMouseOverBackground,
            ToolButtonMouseOverBorder,
            ToolButtonForeground,
            ToolButtonMouseOverForeground,
            ToolButtonPressedForeground,
            ToolButtonPressedBackground,
            ToolButtonPressedBorder,
            AboutDialogBackground
        }


        public static ResourceKey AboutDialogBackground
        {
            get { return new ComponentResourceKey(typeof(UIBrushes), BrushKey.AboutDialogBackground); }
        }

        public static ResourceKey ToolButtonPressedBorder
        {
            get { return new ComponentResourceKey(typeof(UIBrushes), BrushKey.ToolButtonPressedBorder); }
        }

        public static ResourceKey ToolButtonMouseOverForeground
        {
            get { return new ComponentResourceKey(typeof(UIBrushes), BrushKey.ToolButtonMouseOverForeground); }
        }

        public static ResourceKey ToolButtonPressedForeground
        {
            get { return new ComponentResourceKey(typeof(UIBrushes), BrushKey.ToolButtonPressedForeground); }
        }

        public static ResourceKey ToolButtonForeground
        {
            get { return new ComponentResourceKey(typeof(UIBrushes), BrushKey.ToolButtonForeground); }
        }

        public static ResourceKey ToolButtonBackground
        {
            get { return new ComponentResourceKey(typeof(UIBrushes), BrushKey.ToolButtonBackground); }
        }

        public static ResourceKey ToolButtonBorder
        {
            get { return new ComponentResourceKey(typeof(UIBrushes), BrushKey.ToolButtonBorder); }
        }

        public static ResourceKey ToolButtonMouseOverBackground
        {
            get { return new ComponentResourceKey(typeof(UIBrushes), BrushKey.ToolButtonMouseOverBackground); }
        }

        public static ResourceKey ToolButtonMouseOverBorder
        {
            get { return new ComponentResourceKey(typeof(UIBrushes), BrushKey.ToolButtonMouseOverBorder); }
        }

        public static ResourceKey ToolButtonPressedBackground
        {
            get { return new ComponentResourceKey(typeof(UIBrushes), BrushKey.ToolButtonPressedBackground); }
        }

        public static ResourceKey StartViewSeperatorBackground
        {
            get { return new ComponentResourceKey(typeof(UIBrushes), BrushKey.StartViewSeperatorBackground); }
        }

        public static ResourceKey StartViewBackground
        {
            get { return new ComponentResourceKey(typeof(UIBrushes), BrushKey.StartViewBackground); }
        }

        public static ResourceKey RecentProjectButtonForeground
        {
            get { return new ComponentResourceKey(typeof(UIBrushes), BrushKey.RecentProjectButtonForeground); }
        }

        public static ResourceKey RecentProjectButtonBackground
        {
            get { return new ComponentResourceKey(typeof(UIBrushes), BrushKey.RecentProjectButtonBackground); }
        }

        public static ResourceKey RecentProjectButtonBorderBrush
        {
            get { return new ComponentResourceKey(typeof(UIBrushes), BrushKey.RecentProjectButtonBorderBrush); }
        }

        public static ResourceKey RecentProjectButtonMouseOverForeground
        {
            get { return new ComponentResourceKey(typeof(UIBrushes), BrushKey.RecentProjectButtonMouseOverForeground); }
        }

        public static ResourceKey RecentProjectButtonMouseOverBackground
        {
            get { return new ComponentResourceKey(typeof(UIBrushes), BrushKey.RecentProjectButtonMouseOverBackground); }
        }

        public static ResourceKey RecentProjectButtonMouseOverBorderBrush
        {
            get { return new ComponentResourceKey(typeof(UIBrushes), BrushKey.RecentProjectButtonMouseOverBorderBrush); }
        }

        public static ResourceKey LinkButtonForeground
        {
            get { return new ComponentResourceKey(typeof(UIBrushes), BrushKey.LinkButtonForeground); }
        }

        public static ResourceKey LinkButtonMouseOverForeground
        {
            get { return new ComponentResourceKey(typeof(UIBrushes), BrushKey.LinkButtonMouseOverForeground); }
        }

    }
}
