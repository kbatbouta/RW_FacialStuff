using System.Reflection;
using RimWorld;
using UnityEngine;
using Verse;

namespace FacialStuff.Detouring
{
    public static class _PawnSkinColors
    {
        public struct SkinColorData
        {
            public float melanin;

            public float selector;

            public Color color;

            public SkinColorData(float melanin, float selector, Color color)
            {
                this.melanin = melanin;
                this.selector = selector;
                this.color = color;
            }
        }

        public static readonly SkinColorData[] _SkinColors = {
            new SkinColorData(0f, 0f, new Color32(101, 56, 41, 255)),
            new SkinColorData(0.1f, 0.085f, new Color32(127,71,51,255)),
            new SkinColorData(0.25f, 0.175f, new Color32(165,93,41,255)),
            new SkinColorData(0.35f, 0.225f, new Color32(211, 114, 76, 255)),
            new SkinColorData(0.45f, 0.285f, new Color32(220, 138, 88, 255)),
            new SkinColorData(0.5f, 0.35f, new Color32(255,231,179,255)),
            new SkinColorData(0.55f, 0.4f, new Color32(255, 220, 179, 255)),
            new SkinColorData(0.6f, 0.45f, new Color32(255,186,153,255)),
            new SkinColorData(0.65f, 0.6f, new Color32(240,187,140,255)),
            new SkinColorData(0.75f, 0.75f, new Color32(245,210,178,255)),
            new SkinColorData(0.85f, 0.85f, new Color32(248, 193, 189, 255)),
            new SkinColorData(0.9f, 0.9f, new Color32(252,215,202,255)),
            new SkinColorData(1f, 1f, new Color32(255,231,213,255))
  
                // Vanilla
//          new SkinColorData(0f, 0f, new Color(0.3882353f, 0.274509817f, 0.141176477f)),
//          new SkinColorData(0.1f, 0.05f, new Color(0.509803951f, 0.356862754f, 0.1882353f)),
//          new SkinColorData(0.25f, 0.2f, new Color(0.894117653f, 0.619607866f, 0.3529412f)),
//          new SkinColorData(0.5f, 0.285f, new Color(1f, 0.9372549f, 0.7411765f)),
//          new SkinColorData(0.75f, 0.785f, new Color(1f, 0.9372549f, 0.8352941f)),
//          new SkinColorData(1f, 1f, new Color(0.9490196f, 0.929411769f, 0.8784314f))
        };

        [Detour(typeof(PawnSkinColors), bindingFlags = (BindingFlags.Static | BindingFlags.Public))]
        public static bool IsDarkSkin(Color color)
        {
            Color skinColor = GetSkinColor(0.5f);
            return color.r + color.g + color.b <= skinColor.r + skinColor.g + skinColor.b + 0.01f;
        }

        [Detour(typeof(PawnSkinColors), bindingFlags = (BindingFlags.Static | BindingFlags.Public))]
        public static Color GetSkinColor(float melanin)
        {
            int skinDataLeftIndexByMelanin = GetSkinDataIndexOfMelanin(melanin);
            if (skinDataLeftIndexByMelanin == _SkinColors.Length - 1)
            {
                return _SkinColors[skinDataLeftIndexByMelanin].color;
            }
            float t = Mathf.InverseLerp(_SkinColors[skinDataLeftIndexByMelanin].melanin, _SkinColors[skinDataLeftIndexByMelanin + 1].melanin, melanin);
            return Color.Lerp(_SkinColors[skinDataLeftIndexByMelanin].color, _SkinColors[skinDataLeftIndexByMelanin + 1].color, t);
        }

        [Detour(typeof(PawnSkinColors), bindingFlags = (BindingFlags.Static | BindingFlags.NonPublic))]
        public static int GetSkinDataIndexOfMelanin(float melanin)
        {
            int result = 0;
            for (int i = 0; i < _SkinColors.Length; i++)
            {
                if (melanin < _SkinColors[i].melanin)
                {
                    break;
                }
                result = i;
            }
            return result;
        }

        [Detour(typeof(PawnSkinColors), bindingFlags = (BindingFlags.Static | BindingFlags.Public))]
        public static float RandomMelanin()
        {
            float value = Rand.Value;
            int num = 0;
            for (int i = 0; i < _SkinColors.Length; i++)
            {
                if (value < _SkinColors[i].selector)
                {
                    break;
                }
                num = i;
            }
            if (num == _SkinColors.Length - 1)
            {
                return _SkinColors[num].melanin;
            }
            float t = Mathf.InverseLerp(_SkinColors[num].selector, _SkinColors[num + 1].selector, value);
            return Mathf.Lerp(_SkinColors[num].melanin, _SkinColors[num + 1].melanin, t);
        }

        [Detour(typeof(PawnSkinColors), bindingFlags = (BindingFlags.Static | BindingFlags.Public))]
        public static float GetMelaninCommonalityFactor(float melanin)
        {
            int skinDataLeftIndexByWhiteness = GetSkinDataIndexOfMelanin(melanin);
            if (skinDataLeftIndexByWhiteness == _SkinColors.Length - 1)
            {
                return GetSkinCommonalityFactor(skinDataLeftIndexByWhiteness);
            }
            float t = Mathf.InverseLerp(_SkinColors[skinDataLeftIndexByWhiteness].melanin, _SkinColors[skinDataLeftIndexByWhiteness + 1].melanin, melanin);
            return Mathf.Lerp(GetSkinCommonalityFactor(skinDataLeftIndexByWhiteness), GetSkinCommonalityFactor(skinDataLeftIndexByWhiteness + 1), t);
        }

        private static float GetSkinCommonalityFactor(int skinDataIndex)
        {
            float num = 0f;
            for (int i = 0; i < _SkinColors.Length; i++)
            {
                num = Mathf.Max(num, GetTotalAreaWhereClosestToSelector(i));
            }
            return GetTotalAreaWhereClosestToSelector(skinDataIndex) / num;
        }

        private static float GetTotalAreaWhereClosestToSelector(int skinDataIndex)
        {
            float num = 0f;
            if (skinDataIndex == 0)
            {
                num += _SkinColors[skinDataIndex].selector;
            }
            else if (_SkinColors.Length > 1)
            {
                num += (_SkinColors[skinDataIndex].selector - _SkinColors[skinDataIndex - 1].selector) / 2f;
            }
            if (skinDataIndex == _SkinColors.Length - 1)
            {
                num += 1f - _SkinColors[skinDataIndex].selector;
            }
            else if (_SkinColors.Length > 1)
            {
                num += (_SkinColors[skinDataIndex + 1].selector - _SkinColors[skinDataIndex].selector) / 2f;
            }
            return num;
        }

        // FS bench
        public static float GetRelativeLerpValue(float value)
        {
            int leftIndexForValue = GetSkinDataIndexOfMelanin(value);
            if (leftIndexForValue == _SkinColors.Length - 1)
            {
                return 0f;
            }
            int num = leftIndexForValue + 1;
            return Mathf.InverseLerp(_SkinColors[leftIndexForValue].melanin, _SkinColors[num].melanin, value);
        }

        public static float GetValueFromRelativeLerp(int leftIndex, float lerp)
        {
            if (leftIndex >= _SkinColors.Length - 1)
            {
                return 1f;
            }
            if (leftIndex < 0)
            {
                return 0f;
            }
            int num = leftIndex + 1;
            return Mathf.Lerp(_SkinColors[leftIndex].melanin, _SkinColors[num].melanin, lerp);
        }
    }
}