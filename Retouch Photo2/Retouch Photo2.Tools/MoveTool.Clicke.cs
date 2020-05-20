using FanKit.Transformers;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// <see cref="IMoveTool"/>'s MoveTool.
    /// </summary>
    public partial class MoveTool : IMoveTool
    {

        public bool Clicke(Vector2 point)
        {
            //SelectedLayer
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);
            ILayer selectedLayer = this.GetSelectedLayer(canvasPoint);

            if (selectedLayer == null)
            {
                this.ClickeNone();
                return false;
            }

            switch (this.MarqueeCompositeMode)
            {
                case MarqueeCompositeMode.New: this.ClickeNew(selectedLayer); return true;
                case MarqueeCompositeMode.Add: this.ClickeAdd(selectedLayer); return true;
                case MarqueeCompositeMode.Subtract: this.ClickeSubtract(selectedLayer); return true;
                case MarqueeCompositeMode.Intersect: this.ClickeIntersect(selectedLayer); return true;
                default: return false;
            }
        }


        #region Clicke

        ///TODO: <see cref="LayersControl.ItemClick"/> replace
        public void ClickeNone()
        {
            //History
            IHistoryBase history = new IHistoryBase("Set select mode");

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer.IsSelected == true)
                {
                    //History
                    var previous = layer.IsSelected;
                    history.Undos.Push(() => layer.IsSelected = previous);

                    layer.IsSelected = false;
                }
            });

            //History
            this.ViewModel.Push(history);

            this.SelectionViewModel.SetModeNone();//Selection

            LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.Layers);

            this.ViewModel.Invalidate();//Invalidate     
        }

        public void ClickeNew(ILayer selectedLayer)
        {
            //History
            IHistoryBase history = new IHistoryBase("Set select mode");

            if (selectedLayer.IsSelected == false)
            {
                //History
                var previous = selectedLayer.IsSelected;
                history.Undos.Push(() => selectedLayer.IsSelected = previous);

                selectedLayer.IsSelected = true;
            }

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer != selectedLayer)
                {
                    if (layer.IsSelected == true)
                    {
                        //History
                        var previous = layer.IsSelected;
                        history.Undos.Push(() => layer.IsSelected = previous);

                        layer.IsSelected = false;
                    }
                }
            });

            //History
            this.ViewModel.Push(history);

            this.SelectionViewModel.SetModeSingle(selectedLayer);//Selection

            LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.Layers);

            this.ViewModel.Invalidate();//Invalidate     
        }

        public void ClickeAdd(ILayer selectedLayer)
        {
            //History
            IHistoryBase history = new IHistoryBase("Set select mode");

            if (selectedLayer.IsSelected == false)
            {
                //History
                var previous = selectedLayer.IsSelected;
                history.Undos.Push(() => selectedLayer.IsSelected = previous);

                selectedLayer.IsSelected = true;
            }

            //History
            this.ViewModel.Push(history);
            
            this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection

            LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.Layers);

            this.ViewModel.Invalidate();//Invalidate
        }

        public void ClickeSubtract(ILayer selectedLayer)
        {
            //History
            IHistoryBase history = new IHistoryBase("Set select mode");

            if (selectedLayer.IsSelected == true)
            {
                //History
                var previous = selectedLayer.IsSelected;
                history.Undos.Push(() => selectedLayer.IsSelected = previous);

                selectedLayer.IsSelected = false;
            }

            //History
            this.ViewModel.Push(history);

            this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection

            LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.Layers);

            this.ViewModel.Invalidate();//Invalidate
        }

        public void ClickeIntersect(ILayer selectedLayer)
        {
            //History
            IHistoryBase history = new IHistoryBase("Set select mode");

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer != selectedLayer)
                {
                    if (layer.IsSelected == true)
                    {
                        //History
                        var previous = layer.IsSelected;
                        history.Undos.Push(() => layer.IsSelected = previous);

                        layer.IsSelected = false;
                    }
                }
            });

            //History
            this.ViewModel.Push(history);

            this.SelectionViewModel.SetModeSingle(selectedLayer);//Selection

            LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.Layers);

            this.ViewModel.Invalidate();//Invalidate
        }


        #endregion


        private ILayer GetSelectedLayer(Vector2 canvasPoint)
        {
            //Select a layer of the same depth
            ILayer firstLayer = this.SelectionViewModel.GetFirstLayer();
            IList<ILayer> parentsChildren = this.ViewModel.Layers.GetParentsChildren(firstLayer);
            ILayer selectedLayer = parentsChildren.FirstOrDefault((layer) => layer.FillContainsPoint(canvasPoint));
            return selectedLayer;
        }

    }
}