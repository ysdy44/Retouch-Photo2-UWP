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

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionViewModel" />. 
    /// </summary>
    public partial class SelectionViewModel : INotifyPropertyChanged
    {

        /// <summary>
        /// Sets <see cref = "SelectionViewModel.SelectionMode" /> to None.
        /// </summary>
        public void SetModeNone()
        {
            this.Transformer = new Transformer();
            this.DsabledRadian = false;//DisabledRadian

            this.Layer = null;
            this.Layers = null;

            this.SelectionMode = ListViewSelectionMode.None;

            //////////////////////////

            this.SetOpacity(1.0f);
            this.BlendType = BlendType.None;
            this.Visibility = Visibility.Collapsed;

            //////////////////////////

            this.IsCrop = false;
            this.Children = null;
            this.EffectManager = null;
            this.AdjustmentManager = null;

            //////////////////////////

            this.SetGroupLayer(null);
            this.SetAcrylicLayer(null);
            this.SetImageLayer(null);
            this.SetGeometryLayer(null);
            this.SetCurveLayer(null);
        }

        /// <summary>
        /// Sets <see cref = "SelectionViewModel.SelectionMode" /> to Single.
        /// </summary>
        /// <param name="layer"> The selection layer. </param>
        public void SetModeSingle(ILayer layer)
        {
            this.Transformer = layer.TransformManager.ActualDestination;
            this.DsabledRadian = layer.TransformManager.DisabledRadian;//DisabledRadian

            this.Layer = layer;
            this.Layers = null;

            this.SelectionMode = ListViewSelectionMode.Single;

            //////////////////////////

            this.SetOpacity(layer.Opacity);
            this.BlendType = layer.BlendType;
            this.Visibility = layer.Visibility;

            //////////////////////////

            this.IsCrop = (layer.TransformManager.IsCrop && (layer.TransformManager.DisabledRadian == false));
            this.Children = layer.Children;
            this.EffectManager = layer.EffectManager;
            this.AdjustmentManager = layer.AdjustmentManager;

            //////////////////////////

            this.SetGroupLayer(layer);
            this.SetAcrylicLayer(layer);
            this.SetImageLayer(layer);
            this.SetGeometryLayer(layer);
            this.SetCurveLayer(layer);

            //////////////////////////

            if (layer.FillColor is Color color)
            {
                switch (this.FillOrStroke)
                {
                    case Brushs.FillOrStroke.Fill:
                        this.FillColor = color;
                        break;
                    case Brushs.FillOrStroke.Stroke:
                        this.StrokeColor = color;
                        break;
                }
            }
        }

        /// <summary>
        /// Sets <see cref = "SelectionViewModel.SelectionMode" /> to Multiple.
        /// </summary>
        /// <param name="layers"> All selection layers. </param>
        public void SetModeMultiple(IEnumerable<ILayer> layers)
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
                foreach (ILayer layer in layers)
                {
                    Transformer transformer = layer.TransformManager.ActualDestination;
                    aaa(transformer.LeftTop);
                    aaa(transformer.RightTop);
                    aaa(transformer.RightTop);
                    aaa(transformer.LeftBottom);

                    if (layer.TransformManager.DisabledRadian)
                    {
                        disabledRadian = true;//DisabledRadian
                    }
                }
            }

            {
                Transformer transformer = new Transformer(left, top, right, bottom);
                this.SetModeMultiple(layers, transformer, disabledRadian);
            }
        }

        /// <summary>
        /// Sets <see cref = "SelectionViewModel.SelectionMode" /> to Multiple.
        /// </summary>
        /// <param name="layers"> All selection layers. </param>
        /// <param name="transformer"> transformer </param>
        public void SetModeMultiple(IEnumerable<ILayer> layers, Transformer transformer, bool disabledRadian)
        {
            this.Transformer = transformer;
            this.DsabledRadian = disabledRadian;

            this.Layer = null;
            this.Layers = layers;

            this.SelectionMode = ListViewSelectionMode.Multiple;//Transformer      

            //////////////////////////

            //this.SetOpacity(0);
            //this.SetBlendType( BlendType.Normal);
            //this.Visibility = Visibility.Collapsed;

            //////////////////////////

            this.IsCrop = layers.Any(layer=>(layer.TransformManager.IsCrop && (layer.TransformManager.DisabledRadian == false)));
            this.Children = null;
            //this.EffectManager = layer.EffectManager;
            this.AdjustmentManager = null;

            //////////////////////////

            this.SetGroupLayer(null);
            this.SetAcrylicLayer(null);
            //this.SetImageLayer(layer);
            //this.SetGeometryLayer(null);
            this.SetCurveLayer(null);
        }



        /// <summary>
        ///  Sets <see cref = "SelectionViewModel.SelectionMode" />.
        /// </summary>
        /// <param name="layers"> Layers </param>
        public void SetMode(Collection<ILayer> layers)
        {
            IEnumerable<ILayer> checkedLayers = from item in layers where item.IsChecked select item;
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