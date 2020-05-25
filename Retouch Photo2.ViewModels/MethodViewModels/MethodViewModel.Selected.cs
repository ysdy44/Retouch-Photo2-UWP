using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Effects;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ViewModel" />. 
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {


        public void MethodSelectedNone()
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set select mode");

            //Selection
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.IsSelected == true)
                {
                    //History
                    var previous = layer.IsSelected;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.IsSelected = previous;
                    });
                    
                    layer.IsSelected = false;
                }
            });

            //History
            this.HistoryPush(history);

            this.SetModeNone();//Selection
            LayerageCollection.ArrangeLayersBackgroundLayerCollection(this.LayerageCollection);
            this.Invalidate();//Invalidate     
        }

        public void MethodSelectedNot(Layerage selectedLayerage)
        {
            ILayer layer = selectedLayerage.Self;

            //History 
            LayersPropertyHistory history = new LayersPropertyHistory("Set is selected");
            var previous = layer.IsSelected;
            history.UndoActions.Push(() =>
            {
                ILayer layer2 = selectedLayerage.Self;

                layer2.IsSelected = previous;
            });

            bool isSelected = !layer.IsSelected;
            layer.IsSelected = isSelected;

            this.SetMode(this.LayerageCollection);//Selection
            //LayerageCollection.ArrangeLayersBackgroundItemClick(selectedLayerage);
            LayerageCollection.ArrangeLayersBackgroundLayerCollection(this.LayerageCollection);
            this.Invalidate();//Invalidate
        }



        public void MethodSelectedNew(Layerage selectedLayerage)
        {
            ILayer selectedLayer = selectedLayerage.Self;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set select mode");

            if (selectedLayer.IsSelected == false)
            {
                var previous = selectedLayer.IsSelected;
                history.UndoActions.Push(() =>
                {
                    ILayer selectedLayer2 = selectedLayer;

                    selectedLayer2.IsSelected = previous;
                });

                selectedLayer.IsSelected = true;
            }

            //Selection
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer != selectedLayer)
                {
                    if (layer.IsSelected == true)
                    {
                        //History
                        var previous = layer.IsSelected;
                        history.UndoActions.Push(() =>
                        {
                            ILayer layer2 = layerage.Self;

                            layer2.IsSelected = previous;
                        });

                        layer.IsSelected = false;
                    }
                }
            });

            //History
            this.HistoryPush(history);

            this.SetModeSingle(selectedLayerage);//Selection
            LayerageCollection.ArrangeLayersBackgroundLayerCollection(this.LayerageCollection);
            this.Invalidate();//Invalidate     
        }

        public void MethodSelectedAdd(Layerage selectedLayerage)
        {
            ILayer selectedLayer = selectedLayerage.Self;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set select mode");

            if (selectedLayer.IsSelected == false)
            {
                //History
                var previous = selectedLayer.IsSelected;
                history.UndoActions.Push(() =>
                {
                    ILayer selectedLayer2 = selectedLayerage.Self;

                    selectedLayer2.IsSelected = previous;
                });
                
                selectedLayer.IsSelected = true;
            }

            //History
            this.HistoryPush(history);

            this.SetMode(this.LayerageCollection);//Selection
            LayerageCollection.ArrangeLayersBackgroundLayerCollection(this.LayerageCollection);
            this.Invalidate();//Invalidate
        }

        public void MethodSelectedSubtract(Layerage selectedLayerage)
        {
            ILayer selectedLayer = selectedLayerage.Self;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set select mode");

            if (selectedLayerage.Self.IsSelected == true)
            {
                //History
                var previous = selectedLayer.IsSelected;
                history.UndoActions.Push(() =>
                {
                    ILayer selectedLayer2 = selectedLayerage.Self;

                    selectedLayer2.IsSelected = previous;
                });

                selectedLayer.IsSelected = false;
            }

            //History
            this.HistoryPush(history);

            this.SetMode(this.LayerageCollection);//Selection
            LayerageCollection.ArrangeLayersBackgroundLayerCollection(this.LayerageCollection);
            this.Invalidate();//Invalidate
        }

        public void MethodSelectedIntersect(Layerage selectedLayerage)
        {
            ILayer selectedLayer = selectedLayerage.Self;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set select mode");

            //Selection
            this.SetValue((layerage) =>
            {
                if (layerage != selectedLayerage)
                {
                    ILayer layer = layerage.Self;

                    if (layer.IsSelected == true)
                    {
                        //History
                        var previous = selectedLayer.IsSelected;
                        history.UndoActions.Push(() =>
                        {
                            ILayer selectedLayer2 = selectedLayer;

                            selectedLayer2.IsSelected = previous;
                        });

                        layer.IsSelected = false;
                    }
                }
            });

            //History
            this.HistoryPush(history);

            this.SetModeSingle(selectedLayerage);//Selection

            LayerageCollection.ArrangeLayersBackgroundLayerCollection(this.LayerageCollection);

            this.Invalidate();//Invalidate
        }


    }
}