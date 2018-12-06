using System.Drawing;
using ColorThiefDotNet;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace Dominant.Color.Epi
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class DominantColorInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var contentEvents = ServiceLocator.Current.GetInstance<IContentEvents>();
            contentEvents.CreatingContent += ContentEventsOnCreatingContent;
            contentEvents.PublishingContent += ContentEventsOnCreatingContent;
        }

        private void ContentEventsOnCreatingContent(object sender, ContentEventArgs contentEventArgs)
        {
            var image = contentEventArgs.Content as ImageData;
            if (image?.Property["DominantColor"] != null)
            {
                using (var bitmap = new Bitmap(image.BinaryData.OpenRead()))
                {
                    var colorThief = new ColorThief();
                    var dominantColor = colorThief.GetColor(bitmap);
                    image.Property["DominantColor"].Value = dominantColor.Color.ToHexString();
                }
            }
        }

        public void Uninitialize(InitializationEngine context)
        {
            var contentEvents = ServiceLocator.Current.GetInstance<IContentEvents>();
            contentEvents.CreatingContent -= ContentEventsOnCreatingContent;
            contentEvents.PublishingContent -= ContentEventsOnCreatingContent;

        }
    }
}
