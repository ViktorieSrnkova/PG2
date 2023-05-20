using OpenTK.Mathematics;

namespace CSharp_PG2.Utils
{
    public class ColorUtils
    {
        public static Vector3 ColorFromHSV(float hue, float saturation, float value)
        {
            hue = hue % 1.0f;
            var hi = (int)(hue * 6);
            var f = hue * 6 - hi;
            var p = value * (1 - saturation);
            var q = value * (1 - f * saturation);
            var t = value * (1 - (1 - f) * saturation);

            Vector3 color = hi switch
            {
                0 => new Vector3(value, t, p),
                1 => new Vector3(q, value, p),
                2 => new Vector3(p, value, t),
                3 => new Vector3(p, q, value),
                4 => new Vector3(t, p, value),
                _ => new Vector3(value, p, q)
            };

            return color;
        }
    }
}