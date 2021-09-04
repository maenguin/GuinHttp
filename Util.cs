using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuinHttp
{
    class Util
    {
        public static string Join<T>(string delim, IEnumerable<T> cols)
        {
            StringBuilder buffer = new StringBuilder();
            IEnumerator<T> @enum = cols.GetEnumerator();

            if (@enum.MoveNext())
                buffer.Append(@enum.Current);

            while (@enum.MoveNext())
            {
                buffer.Append(delim);
                buffer.Append(@enum.Current);
            }

            return buffer.ToString();
        }

        public static string JoinAdvanced<T>(string AMidDelim, IEnumerable<T> ACols, Func<T, string> AFunc)
        {
            return JoinAdvanced<T>(string.Empty, AMidDelim, string.Empty, ACols, AFunc);
        }
        public static string JoinAdvanced<T>(string APreDelim, string AMidDelim, IEnumerable<T> ACols, Func<T, string> AFunc)
        {
            return JoinAdvanced<T>(APreDelim, AMidDelim, string.Empty, ACols, AFunc);
        }
        public static string JoinAdvanced<T>(string APreDelim, string AMidDelim, string APostDelim, IEnumerable<T> ACols, Func<T, string> AFunc)
        {
            StringBuilder buffer = new StringBuilder();
            IEnumerator<T> @enum = ACols.GetEnumerator();

            if (@enum.MoveNext())
                buffer.Append($"{APreDelim}{AFunc(@enum.Current)}");

            while (@enum.MoveNext())
            {
                buffer.Append(AMidDelim);
                buffer.Append(AFunc(@enum.Current));
            }
            buffer.Append($"{APostDelim}");
            return buffer.ToString();
        }

        public static byte[] CombineByteArray(byte[] origin, byte[] target)
        {
            byte[] result = new byte[origin.Length + target.Length];
            Array.Copy(origin, 0, result, 0, origin.Length);
            Array.Copy(target, 0, result, target.Length, target.Length);
            return result;
        }


    }
}
