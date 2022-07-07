using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class BoldTextEffect : BaseMeshEffect
{
    [Range(0, 1)] public float Alpha;
    [Range(1, 5)] public int Strength;

    public string RichText = "";

    private Text m_Text = null;

    private Text TextComp
    {
        get
        {
            if (m_Text == null)
            {
                m_Text = GetComponent<Text>();
            }

            return m_Text;
        }
    }

    private Color effectColor
    {
        get
        {
            if (TextComp == null)
            {
                return Color.black;
            }

            return TextComp.color;
        }
    }

    protected void ApplyShadowZeroAlloc(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
    {
        int num = verts.Count + end - start;
        if (verts.Capacity < num)
            verts.Capacity = num;
        for (int index = start; index < end; ++index)
        {
            UIVertex vert = verts[index];
            verts.Add(vert);
            Vector3 position = vert.position;
            position.x += x;
            position.y += y;
            vert.position = position;
            Color32 color32 = color;
            color32.a = (byte)((int)color32.a * (int)verts[index].color.a / (int)byte.MaxValue);
            color32.a = (byte)(Alpha * color32.a);
            vert.color = color32;
            verts[index] = vert;
        }
    }

    private static readonly Regex s_BoldBeginRegex = new Regex("<b>", RegexOptions.Singleline);
    private static readonly Regex s_BoldEndRegex = new Regex("</b>", RegexOptions.Singleline);

    private MatchCollection begin = null;
    private MatchCollection end = null;


    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
        {
            return;
        }

        List<UIVertex> verts = new List<UIVertex>();
        vh.GetUIVertexStream(verts);


        if (!string.IsNullOrEmpty(RichText) && begin != null && end != null)
        {
            int offset = 0;
            for (int i = 0; i < begin.Count && i < end.Count; ++i)
            {
                for (int j = 0; j < Strength; ++j)
                {
                    ApplyShadowZeroAlloc(verts, effectColor, (begin[i].Index - offset) * 6, (end[i].Index - offset - 3) * 6, 0, 0);
                }
                offset += 7;
            }

        }
        else
        {
            for (int i = 0; i < Strength; ++i)
            {
                ApplyShadowZeroAlloc(verts, effectColor, 0, verts.Count, 0, 0);
            }
        }


        vh.Clear();
        vh.AddUIVertexTriangleStream(verts);
    }


    public void SetText(string text)
    {
        this.RichText = text;
        begin = s_BoldBeginRegex.Matches(RichText);
        end = s_BoldEndRegex.Matches(RichText);

        text = text.Replace("<b>", "");
        text = text.Replace("</b>", "");

        if (m_Text != null)
        {
            m_Text.text = text;
        }
    }

}
