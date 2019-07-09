using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using FanKit.Transformers;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Retouch_Photo2.ViewModels
{
    /// <summary>
    /// Retouch_Photo2 's Project
    /// </summary>
    public class Project
    {

        /// <summary> <see cref = "Project" />'s name. </summary>
        public string Name { set; get; }

        /// <summary> <see cref = "Project" />'s width. </summary>
        public int Width { set; get; }
        /// <summary> <see cref = "Project" />'s height. </summary>
        public int Height { set; get; }

        /// <summary> <see cref = "Project" />'s layers. </summary>
        public IEnumerable<Layer> Layers;


        //@Construct
        public Project()
        {
        }
        /// <summary>
        /// Construct a project from <see cref = "ImageLayer" />.
        /// </summary>
        /// <param name="imageLayer"> ImageLayer. </param>
        public Project(ImageLayer imageLayer)
        {
            CanvasBitmap bitmap = imageLayer.GetImage(imageLayer.ImageKey);

            int width = (int)bitmap.SizeInPixels.Width;
            int height = (int)bitmap.SizeInPixels.Height;

            this.Width = width;
            this.Height = height;
            this.Layers = new List<Layer>()
            {
                 imageLayer
            };
        }

        /// <summary>
        /// Construct a project.
        /// </summary>
        /// <param name="width"> width </param>
        /// <param name="height"> height </param>
        public Project(int width ,int height)
        {
            this.Width = width;
            this.Height = height;
            this.Layers = new List<Layer>();
        }
    }
}