using System.Drawing;

namespace Highlight.Patterns
{
    public class Style
    {
        public ColorPair Colors { get; private set; }

        public Style(ColorPair colors)
        {
            Colors = colors;
        }
    }
}
