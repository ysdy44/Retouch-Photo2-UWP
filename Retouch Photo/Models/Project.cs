using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models.Layers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Windows.Graphics.Imaging;

namespace Retouch_Photo.Models
{
    /// <summary>项目</summary>
    public class Project
    {
       public string Name;

        public int Width;
        public int Height;

        public int Tool;
        public int Index;

        public IEnumerable<Layer> Layers;


        public static Project CreateFromSize(ICanvasResourceCreatorWithDpi creator, BitmapSize pixels)
        {
            int width = (int)pixels.Width;
            int height = (int)pixels.Height;

            return new Project()
            {
                Width = width,
                Height = height,
                Layers = new List<Layer>()
                {
                   PixelLayer.CreateFromSize(creator,width,height)
                }
            };
        }

        public static Project CreateFromXDocument(ICanvasResourceCreatorWithDpi creator, XDocument xdoc)
        {
            return new Project()
            {
                Width = (int)xdoc.Descendants("Width").Single(),
                Height = (int)xdoc.Descendants("Height").Single(),

                Tool = (int)xdoc.Descendants("Tool").Single(),
                Index = (int)xdoc.Descendants("Index").Single(),

                Layers = from element in xdoc.Descendants("Layer") select Layer.CreateFromXElement(creator, element)
            };
        }


    }
}
