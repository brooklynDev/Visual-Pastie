using System.Text;

namespace PastieAPI.Internal
{
    internal static class CustomUrlEncoder
    {
        private static bool IsSafe(char ch)
        {
            if ((int)ch >= 97 && (int)ch <= 122 || (int)ch >= 65 && (int)ch <= 90 || (int)ch >= 48 && (int)ch <= 57)
                return true;
            switch (ch)
            {
                case '!':
                case '\'':
                case '(':
                case ')':
                case '*':
                case '-':
                case '.':
                case '_':
                    return true;
                default:
                    return false;
            }
        }

        private static char IntToHex(int n)
        {
            if (n <= 9)
                return (char)(n + 48);
            else
                return (char)(n - 10 + 97);
        }

        private static byte[] UrlEncodeBytesToBytesInternal(byte[] bytes, int offset, int count, bool alwaysCreateReturnValue)
        {
            int num1 = 0;
            int num2 = 0;
            for (int index = 0; index < count; ++index)
            {
                char ch = (char)bytes[offset + index];
                if ((int)ch == 32)
                    ++num1;
                else if (!IsSafe(ch))
                    ++num2;
            }
            if (!alwaysCreateReturnValue && num1 == 0 && num2 == 0)
                return bytes;
            byte[] numArray1 = new byte[count + num2 * 2];
            int num3 = 0;
            for (int index1 = 0; index1 < count; ++index1)
            {
                byte num4 = bytes[offset + index1];
                char ch = (char)num4;
                if (IsSafe(ch))
                    numArray1[num3++] = num4;
                else if ((int)ch == 32)
                {
                    numArray1[num3++] = (byte)43;
                }
                else
                {
                    byte[] numArray2 = numArray1;
                    int index2 = num3;
                    int num5 = 1;
                    int num6 = index2 + num5;
                    int num7 = 37;
                    numArray2[index2] = (byte)num7;
                    byte[] numArray3 = numArray1;
                    int index3 = num6;
                    int num8 = 1;
                    int num9 = index3 + num8;
                    int num10 = (int)(byte)IntToHex((int)num4 >> 4 & 15);
                    numArray3[index3] = (byte)num10;
                    byte[] numArray4 = numArray1;
                    int index4 = num9;
                    int num11 = 1;
                    num3 = index4 + num11;
                    int num12 = (int)(byte)IntToHex((int)num4 & 15);
                    numArray4[index4] = (byte)num12;
                }
            }
            return numArray1;
        }

        private static byte[] UrlEncodeToBytes(string str, Encoding e)
        {
            if (str == null)
                return (byte[])null;
            byte[] bytes = e.GetBytes(str);
            return UrlEncodeBytesToBytesInternal(bytes, 0, bytes.Length, false);
        }

        private static string UrlEncode(string str, Encoding e)
        {
            if (str == null)
                return (string)null;
            else
                return Encoding.ASCII.GetString(UrlEncodeToBytes(str, e));
        }

        public static string UrlEncode(string str)
        {
            return UrlEncode(str, Encoding.UTF8);
        }
    }
}