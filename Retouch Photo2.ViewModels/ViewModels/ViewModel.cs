using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Retouch_Photo2.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using Windows.Storage;
using Windows.UI.Xaml;
using Retouch_Photo2.Elements.MainPages;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ViewModel" />.
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        /// <summary>
        /// Load from a project.
        /// </summary>
        /// <param name="project"> The project. </param>
        public void LoadFromProject(Project project)
        {
            if (project == null) return;

            //Name
            this.Name = project.Name;

            //Width Height
            this.CanvasTransformer.Width = project.Width;
            this.CanvasTransformer.Height = project.Height;
            
            //Layers
            this.Layers.RootLayers.Clear();
            if (project.Layers != null)
            {
                foreach (ILayer layer in project.Layers)
                {
                    if (layer != null)
                    {
                        this.Layers.RootLayers.Add(layer);
                    }
                }
            }

            //Arrange
            LayerCollection.ArrangeLayersControls(this.Layers);
            LayerCollection.ArrangeLayersBackgroundLayerCollection(this.Layers);
        }


        /// <summary> Retouch_Photo2's the current project name. </summary>
        public string Name = null;
        /// <summary> Update the <see cref="ProjectViewItem.RefreshImageSource"/> by <see cref="ViewModel.Name"/>.  </summary>
        public bool IsUpdateThumbnailByName = false;

        /// <summary> Retouch_Photo2's the only ILayers. </summary>
        public LayerCollection Layers { get; } = new LayerCollection();


        #region Invalidate


        /// <summary> Retouch_Photo2's the only <see cref = "Microsoft.Graphics.Canvas.UI.Xaml.CanvasDevice" />. </summary>
        public CanvasDevice CanvasDevice { get; } = new CanvasDevice();


        /// <summary>
        /// Indicates that the contents of the CanvasControl need to be redrawn.
        /// </summary>
        /// <param name="mode"> invalidate mode </param>
        public void Invalidate(InvalidateMode mode = InvalidateMode.None) => this.InvalidateAction?.Invoke(mode);
        /// <summary> <see cref = "Action" /> of the <see cref = "ViewModel.Invalidate" />. </summary>
        public Action<InvalidateMode> InvalidateAction { get; set; }


        #endregion


        //@Notify 
        /// <summary> Multicast event for property change notifications. </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName"> Name of the property used to notify listeners. </param>
        protected void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}