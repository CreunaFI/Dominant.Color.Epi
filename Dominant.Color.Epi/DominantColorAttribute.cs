using System;

namespace Dominant.Color.Epi
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DominantColorAttribute : Attribute
    {
        public int Quality { get; set; } = 10;
        public bool Ignorewhite { get; set; } = true;
        public int PaletteColorCount { get; set; } = 5;
    }
}
