using FanKit.Transformers;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Layers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels.Selections
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionViewModel" />. 
    /// </summary>
    public partial class SelectionViewModel : INotifyPropertyChanged
    {

        /// <summary>
        /// Sets <see cref = "SelectionViewModel.Mode" /> to None.
        /// </summary>
        public void SetModeNone()
        {
            this.Transformer = new Transformer();
            this.DsabledRadian = false;//DisabledRadian

            this.Layer = null;
            this.Layers = null;

            this.Mode = ListViewSelectionMode.None;


            //////////////////////////


            this.SetOpacity(1.0f);
            this.SetBlendType(BlendType.Normal);
            this.Visibility = Visibility.Collapsed;


            this.Children = null;
            this.EffectManager = null;
            this.AdjustmentManager = null;


            this.SetGroupLayer(null);
            this.SetAcrylicLayer(null);
            this.SetGeometryLayer(null);
            
        }

        /// <summary>
        /// Sets <see cref = "SelectionViewModel.Mode" /> to Single.
        /// </summary>
        /// <param name="layer"> The selection layer. </param>
        public void SetModeSingle(Layer layer)
        {
            this.Transformer = layer.TransformerMatrix.Destination;
            this.DsabledRadian = layer.TransformerMatrix.DisabledRadian;//DisabledRadian

            this.Layer = layer;
            this.Layers = null;

            this.Mode = ListViewSelectionMode.Single;


            //////////////////////////


            this.SetOpacity(layer.Opacity);
            this.SetBlendType(layer.BlendType);
            this.Visibility = layer.Visibility;


            this.Children = layer.Children;
            this.EffectManager = layer.EffectManager;
            this.AdjustmentManager = layer.AdjustmentManager;
            

            this.SetGroupLayer(layer);
            this.SetAcrylicLayer(layer);
            this.SetGeometryLayer(layer);


            if (layer.GetFillColor() is Color color)
            {
                 //App.ViewModel.FillColor = color;
            }
        }

        /// <summary>
        /// Sets <see cref = "SelectionViewModel.Mode" /> to Multiple.
        /// </summary>
        /// <param name="layers"> All selection layers. </param>
        public void SetModeMultiple(IEnumerable<Layer> layers)
        {
            float left = float.MaxValue;
            float top = float.MaxValue;
            float right = float.MinValue;
            float bottom = float.MinValue;
            bool disabledRadian = false;//DisabledRadian

            //Foreach
            {
                void aaa(Vector2 vector)
                {
                    if (left > vector.X) left = vector.X;
                    if (top > vector.Y) top = vector.Y;
                    if (right < vector.X) right = vector.X;
                    if (bottom < vector.Y) bottom = vector.Y;
                }

                //Foreach
                foreach (Layer item in layers)
                {
                    aaa(item.TransformerMatrix.Destination.LeftTop);
                    aaa(item.TransformerMatrix.Destination.RightTop);
                    aaa(item.TransformerMatrix.Destination.RightTop);
                    aaa(item.TransformerMatrix.Destination.LeftBottom);

                    if (item.TransformerMatrix.DisabledRadian) disabledRadian = true;//DisabledRadian
                }
            }

            Transformer transformer = new Transformer(left, top, right, bottom);
            this.SetModeMultiple(layers, transformer, disabledRadian);
        }

        /// <summary>
        /// Sets <see cref = "SelectionViewModel.Mode" /> to Multiple.
        /// </summary>
        /// <param name="layers"> All selection layers. </param>
        /// <param name="transformer"> transformer </param>
        public void SetModeMultiple(IEnumerable<Layer> layers, Transformer transformer, bool disabledRadian)
        {
            this.Transformer = transformer;
            this.DsabledRadian = disabledRadian;

            this.Layer = null;
            this.Layers = layers;

            this.Mode = ListViewSelectionMode.Multiple;//Transformer      


            //////////////////////////


            //this.SetOpacity(0);
            //this.SetBlendType( BlendType.Normal);
            //this.Visibility = Visibility.Collapsed;
            

            this.Children = null;
            //this.EffectManager = layer.EffectManager;
            this.AdjustmentManager = null;


            this.SetGroupLayer(null);
            this.SetAcrylicLayer(null);
            this.SetGeometryLayer(null);
        }



        /// <summary>
        ///  Sets <see cref = "SelectionViewModel.Mode" />.
        /// </summary>
        /// <param name="layers"> Layers </param>
        public void SetMode(Collection<Layer> layers)
        {
            IEnumerable<Layer> checkedLayers = from item in layers where item.IsChecked select item;
            int count = checkedLayers.Count();

            if (count == 0)
                this.SetModeNone();//None

            else if (count == 1)
                this.SetModeSingle(checkedLayers.Single());//Single

            else if (count >= 2)
                this.SetModeMultiple(checkedLayers);//Multiple
        }

    }
}