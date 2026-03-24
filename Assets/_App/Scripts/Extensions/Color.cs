using UnityEngine;


namespace Bubbles
{
    public static class ColorExtension
    {
        public static Color ChangeAlpha(this Color color, float alpha)
        {
            Color newColor = new Color(color.r, color.g, color.b, alpha);
            return newColor;
        }

        static public Color ReturnNonOpacityColor(this Color color)
        {
            Color newColor = color;
            newColor.a = 1f;
            return newColor;
        }
    }
}
