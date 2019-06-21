using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Transformers;
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

        //@Static
        /// <summary>
        /// Create a project from <see cref = "StorageFile" />.
        /// </summary>
        /// <param name="creator"> ICanvasResourceCreator. </param>
        /// <param name="file"> StorageFile. </param>
        public static async Task<Project> CreateFromFileAsync(ICanvasResourceCreator creator, StorageFile file)
        {
            using (IRandomAccessStream stream = await file.OpenReadAsync())
            {
                CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(creator, stream, 96);

                int width = (int)bitmap.SizeInPixels.Width;
                int height = (int)bitmap.SizeInPixels.Height;
                TransformerMatrix transformerMatrix = new TransformerMatrix(width, height, Vector2.Zero);
                
                return new Project
                {
                    Width = width,
                    Height = height,
                    Layers = new List<Layer>()
                    {
                        new ImageLayer()
                        {
                            Bitmap=bitmap
                        }
                    }
                };
            }
        }
    }
}