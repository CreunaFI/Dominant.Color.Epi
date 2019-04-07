using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ColorThiefDotNet;
using EPiServer.Core;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;

namespace Dominant.Color.Epi
{
    [EditorDescriptorRegistration(TargetType = typeof(string), UIHint = "DominantColorPalette")]
    public class DominantColorPaletteEditorDescriptor : EditorDescriptor
    {
        public DominantColorPaletteEditorDescriptor()
        {
            ClientEditingClass = "dominant-color/ColorPalette";
        }

        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            var image = metadata.Parent.Model as ImageData;
            var dominantColors = new List<QuantizedColor>();
            if (image != null)
            {
                var dcattr = attributes.FirstOrDefault(attr => attr is DominantColorAttribute) as DominantColorAttribute;
                dominantColors = GetDominantColors(dcattr, image);
            }
            var grid = CreateGrid(dominantColors);
            metadata.EditorConfiguration.Add("dominantColors", grid);
            base.ModifyMetadata(metadata, attributes);
        }

        private static List<QuantizedColor> GetDominantColors(DominantColorAttribute dominantColorAttribute, ImageData image)
        {
            using (var bitmap = new Bitmap(image.BinaryData.OpenRead()))
            {
                var colorThief = new ColorThief();
                var palette = colorThief.GetPalette(bitmap, dominantColorAttribute?.PaletteColorCount ?? 5,
                    dominantColorAttribute?.Quality ?? 10,
                    dominantColorAttribute?.Ignorewhite ?? true);
                if (!palette.Any())
                {
                    palette.Add(colorThief.GetColor(bitmap, dominantColorAttribute?.Quality ?? 10, dominantColorAttribute?.Ignorewhite ?? true));
                }
                return palette;
            }
        }

        private static List<IEnumerable<string>> CreateGrid(List<QuantizedColor> dominantColors)
        {
            var cols = (int)Math.Round(Math.Sqrt(dominantColors.Count));
            var grid = new List<IEnumerable<string>>();
            for (var i = 0; i < dominantColors.Count; i += cols)
            {
                grid.Add(dominantColors.Skip(i).Take(cols).Select(c => c.Color.ToHexString().ToLower()));
            }
            return grid;
        }
    }
}
