using System.Text;
using System.Windows.Input;
using NativeAPI.Win.User;

namespace UILib.WPF
{
    public static class WpfKeyboardUtils
    {
        /// <summary>
        ///     Converts a WPF <see cref="Key"/> to a <see cref="char"/>.
        /// </summary>
        /// <param name="key">
        ///     A WPF <c>Key</c> value.
        /// </param>
        /// <returns>
        ///     The <c>char</c> value of <paramref name="key"/>.
        /// </returns>
        /// <seealso cref="http://stackoverflow.com/a/5826175/467582"/>
        public static char GetCharFromKey(Key key)
        {
            char ch = ' ';

            int virtualKey = KeyInterop.VirtualKeyFromKey(key);
            byte[] keyboardState = new byte[256];
            KeyboardAPI.GetKeyboardState(keyboardState);

            uint scanCode = KeyboardAPI.MapVirtualKey((uint)virtualKey, KeyboardAPI.MapType.MAPVK_VK_TO_VSC);
            StringBuilder stringBuilder = new StringBuilder(2);

            int result = KeyboardAPI.ToUnicode((uint) virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
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
    }
}
