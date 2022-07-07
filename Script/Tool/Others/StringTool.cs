using System.Text;
using UnityEngine;

namespace Tool.Others
{
    public static class StringTool
    {
        public static string Formatting(string str)
        {
            return str.Replace("\\n","\n");
        }

        public static Color ToColor(this string str)
        {
            int v = int.Parse(str, System.Globalization.NumberStyles.HexNumber);
            //转换颜色
            return new Color(
                //int>>移位 去低位
                //&按位与 去高位
                ((float)(((v >> 16) & 255))) / 255,
                ((float)((v >> 8) & 255)) / 255,
                ((float)((v >> 0) & 255)) / 255
            );
        }
        
        public static Vector3 ToVector3(this string str)
        {
            Vector3 vector3 = Vector3.zero;
            int startOffset = 0;
            int endOffset = str.Length;
            int index = 0;
            if (str[0] == '[')
            {
                startOffset = 1;
                endOffset = str.Length - 1;
            }
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = startOffset; i < endOffset; i++)
            {
                if (str[i] == ',')
                {
                    string s = stringBuilder.ToString();
                    vector3[index] = float.Parse(s);
                    index++;
                    stringBuilder.Clear();
                }
                else
                {
                    stringBuilder.Append(str[i]);
                }
            }
            string s2 = stringBuilder.ToString();
            vector3[index] = float.Parse(s2);
            return vector3;
        }
        
    }
}