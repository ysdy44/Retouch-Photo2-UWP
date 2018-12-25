using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models.Layers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Retouch_Photo.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using Windows.Foundation;
using Windows.System;
using System.Linq;
using System.Xml.Linq;
using Windows.Graphics.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Microsoft.Graphics.Canvas;

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
        
        public static Project CreateFromSize(ICanvasResourceCreatorWithDpi creator, BitmapSize pixels)
        {
            int width = (int)pixels.Width;
            int height = (int)pixels.Height;

            return new Project()
            {
                Width = width,
                Height = height,
                Layers = new List<Layer>()
            };
        }

        public static async Task<Project> CreateFromFileAsync(ICanvasResourceCreatorWithDpi creator, StorageFile file)
        {
            using (IRandomAccessStream stream = await file.OpenReadAsync())
            {
                try
                {
                    CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(creator, stream, 96);

                    int width = (int)bitmap.SizeInPixels.Width;
                    int height = (int)bitmap.SizeInPixels.Height;

                    return new Project()
                    {
                        Width = width,
                        Height = height,
                        Layers = new List<Layer>()
                        {
                            ImageLayer.CreateFromBitmap(creator,bitmap,width,height)
                        }
                    };
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }




    }
}
