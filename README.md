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
}
```
