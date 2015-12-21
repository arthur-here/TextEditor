using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace TextEditor.Utilities
{
    /// <summary>
    /// Provides utility methods
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Enumeration needed to scan virtual key.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
        private enum MapType : uint
        {
            [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
            MAPVK_VK_TO_VSC = 0x0,
            [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
            MAPVK_VSC_TO_VK = 0x1,
            [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
            MAPVK_VK_TO_CHAR = 0x2,
            [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
            MAPVK_VSC_TO_VK_EX = 0x3,
        }

        /// <summary>
        /// Join list of strings by \n character.
        /// </summary>
        /// <param name="list">List of strings.</param>
        /// <returns>Joined strings.</returns>
        public static string Text(this List<string> list)
        {
            return string.Join("\n", list);
        }

        /// <summary>
        /// Searches for visual child of provided type.
        /// </summary>
        /// <typeparam name="T">Type to search for.</typeparam>
        /// <param name="dependencyObject">First object to search.</param>
        /// <returns>Found object.</returns>
        public static T FindVisualChild<T>(this DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }

                    T childItem = FindVisualChild<T>(child);
                    if (childItem != null)
                    {
                        return childItem;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Transforms <see cref="Key"/> to <see cref="char"/>.
        /// </summary>
        /// <param name="key">Key to transform.</param>
        /// <returns>Char transformed from key.</returns>
        public static char GetChar(this Key key)
        {
            char ch = ' ';

            int virtualKey = KeyInterop.VirtualKeyFromKey(key);
            byte[] keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            uint scanCode = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
            StringBuilder stringBuilder = new StringBuilder(2);

            int result = ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
            switch (result)
            {
                case -1:
                    break;
                case 0:
                    break;
                case 1:
                    {
                        ch = stringBuilder[0];
                        break;
                    }

                default:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
            }

            return ch;
        }

        [DllImport("user32.dll")]
        private static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)]
            StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1414:MarkBooleanPInvokeArgumentsWithMarshalAs")]
        [DllImport("user32.dll")]
        private static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, MapType uMapType);
    }
}
