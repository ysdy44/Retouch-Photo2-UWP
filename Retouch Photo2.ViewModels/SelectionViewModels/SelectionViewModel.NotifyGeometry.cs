using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionViewModel" />. 
    /// </summary>
    public partial class SelectionViewModel : INotifyPropertyChanged
    {

        /// <summary> Sets all IGeometryLayer. </summary>     
        private void SetIGeometryLayer(ILayer layer)
        {
            if (layer is IGeometryLayer geometryLayer)
            {

                if (layer is GeometryRoundRectLayer  roundRectLayer)
                {
                    this.GeometryRoundRectCorner = roundRectLayer.Corner;
                }

            }
        }


        /// <summary> GeometryRoundRectLayer's corner. </summary>     
        public float GeometryRoundRectCorner
        {
            get => this.geometryRoundRectCorner;
            set
            {
                if (this.geometryRoundRectCorner == value) return;
                this.geometryRoundRectCorner = value;
                this.OnPropertyChanged(nameof(this.GeometryRoundRectCorner));//Notify 
            }
        }
        private float geometryRoundRectCorner = 0.12f;


        /// <summary> GeometryTriangle's center-point. </summary>     
        public float GeometryTriangleCenter
        {
            get => this.geometryTriangleCenter;
            set
            {
                if (this.geometryTriangleCenter == value) return;
                this.geometryTriangleCenter = value;
                this.OnPropertyChanged(nameof(this.GeometryTriangleCenter));//Notify 
            }
        }
        private float geometryTriangleCenter = 0.5f;




    }
}