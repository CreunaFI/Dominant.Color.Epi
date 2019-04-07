# Dominant.Color.Epi
Calculate dominant color for image in Episerver
## Installation
```
Install-Package Dominant.Color.Epi
```
## Usage
```
public class ImageFile : ImageData
{
   [DominantColor]
   public virtual string DominantColor { get; set; }
   
   [DominantColor(Quality = 15, Ignorewhite = false)]
   public virtual string BackgroundColor { get; set; }
  
   [UIHint("DominantColorPalette")]
   public virtual string Color { get; set; }
  
   [DominantColor(Quality = 5, Ignorewhite = false, PaletteColorCount = 10)]
   [UIHint("DominantColorPalette")]
   public virtual string Color2 { get; set; }
}
```
