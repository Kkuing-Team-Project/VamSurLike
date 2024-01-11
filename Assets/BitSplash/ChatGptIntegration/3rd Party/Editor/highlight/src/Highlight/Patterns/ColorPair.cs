

using UnityEngine;

namespace Highlight.Patterns
{
    public class ColorPair
    {
        public Color ForeColor { get; set; }

        public ColorPair()
        {
        }

        public ColorPair(Color foreColor)
        {
            ForeColor = foreColor;
        }
    }
}
