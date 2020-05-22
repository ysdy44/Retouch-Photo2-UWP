using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Effects;
using Retouch_Photo2.Layers.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Xml.Linq;
using Windows.UI;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer that can have render properties. Provides a rendering method.
    /// </summary>
    public abstract partial class Layer
    {

        //@Static
        /// <summary> Collection <see cref="Layerage"/>s instances. </summary>
        public static ObservableCollection<ILayer> Instances = new ObservableCollection<ILayer>();


        /// <summary>
        /// Find the first <see cref="Layer"/> by <see cref="Layer"/>.
        /// </summary>
        /// <param name="layerage"> The source layerage</param>
        /// <returns> The product layer. </returns>
        public static ILayer FindFirstLayer(Layerage layerage)
        {
            string id = layerage.Id;
            return Layer.Instances.FirstOrDefault(i => i.Id == id);
        }


        static int IIIII;
        public string Id
        {
            get
            {
                if (this.id == null)
                {
                    //  TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    //  this.id = Convert.ToInt64(ts.TotalSeconds).ToString();
                   
                    do
                    {
                        this.id = IIIII.ToString();
                        IIIII++;

                    } while (Layer.Instances.All(l => l.Id != this.id)==false);

                }
                return this.id;
            }
            set => this.id = value;
        }
        private string id = null;


        /// <summary>
        /// To <see cref="Layerage"/>.
        /// </summary>
        /// <returns> The producted layerage. </returns>
        public Layerage ToLayerage()
        {
            return new Layerage
            {
                Id = this.Id,
            };
        }


        /// <summary>
        /// Returns a boolean indicating whether the given <see cref="Layerage"/> is equal to this <see cref="Layer"/> instance.
        /// </summary>
        /// <param name="other"> The <see cref="Layerage"/> to compare this instance to. </param>
        /// <returns> True if the other <see cref="Layerage"/> is equal to this instance; False otherwise. </returns>
        public bool Equals(Layerage other)
        {
            if (this.Id != other.Id) return false;

            return true;
        }
        /// <summary>
        /// Returns a boolean indicating whether the given <see cref="Layer"/> is equal to this <see cref="Layer"/> instance.
        /// </summary>
        /// <param name="other"> The <see cref="Layer"/> to compare this instance to. </param>
        /// <returns> True if the other <see cref="Layer"/> is equal to this instance; False otherwise. </returns>
        public bool Equals(Layer other)
        {
            if (this.Id != other.Id) return false;

            return true;
        }

    }
}