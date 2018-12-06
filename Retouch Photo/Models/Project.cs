using Microsoft.Graphics.Canvas;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Windows.Graphics.Imaging;

namespace Retouch_Photo.Models
{
    public class Project
    {
       public string Name;

        public int Width;
        public int Height;

        public int Tool;
        public int Index;

        public IEnumerable<Layer> Layers;


        public static Project CreateFromSize(ICanvasResourceCreatorWithDpi resourceCreator, BitmapSize pixels)
        {
            int width = (int)pixels.Width;
            int height = (int)pixels.Height;

            return new Project()
            {
                Width = width,
                Height = height,
                Layers = new List<Layer>()
                {
                    Layer.CreateFromSize(resourceCreator,width,height)
                }
            };
        }

        public static Project CreateFromXDocument(ICanvasResourceCreatorWithDpi resourceCreator, XDocument xdoc)
        {
            return new Project()
            {
                Width = (int)xdoc.Descendants("Width").Single(),
                Height = (int)xdoc.Descendants("Height").Single(),

                Tool = (int)xdoc.Descendants("Tool").Single(),
                Index = (int)xdoc.Descendants("Index").Single(),

                Layers = from element in xdoc.Descendants("Layer") select Layer.CreateFromXElement(resourceCreator, element)
            };
        }


    }
}
