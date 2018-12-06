using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
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
            if (image != null)
            {
                foreach (var property in GetColorProperties(image))
                {
                    var attribute = property.GetCustomAttribute<DominantColorAttribute>();
                    var colorHex = GetDominantColor(image, attribute.Quality, attribute.Ignorewhite);
                    image.Property[property.Name].Value = colorHex;
                }
            }
        }

        private static IEnumerable<PropertyInfo> GetColorProperties(IContentData image)
        {
            return image.GetType().GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(DominantColorAttribute)));
        }

        private static string GetDominantColor(IBinaryStorable image, int quality, bool ignoreWhite)
        {
            using (var bitmap = new Bitmap(image.BinaryData.OpenRead()))
            {
                var colorThief = new ColorThief();
                var dominantColor = colorThief.GetColor(bitmap, quality, ignoreWhite);
                return dominantColor.Color.ToHexString();
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
