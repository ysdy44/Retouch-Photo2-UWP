using FanKit.Transformers;
using Retouch_Photo2.Historys.Models;
using Retouch_Photo2.Layers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// <see cref="ITransformerTool"/>'s TransformerTool.
    /// </summary>
    public partial class TransformerTool : ITransformerTool
    {

        public bool Clicke(Vector2 point)
        {
            //SelectedLayer
            ILayer selectedLayer = this._getSelectedLayer(point);

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


        public void ClickeNone()
        {
            //History
            SelectModeHistory history = new SelectModeHistory();
            this.ViewModel.Push(history);

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer.SelectMode.ToBool())
                {
                    //History
                    history.Add(layer, layer.SelectMode, SelectMode.UnSelected);

                    layer.SelectMode = SelectMode.UnSelected;
                }
            });

            this.SelectionViewModel.SetModeNone();//Selection
            this.ViewModel.Invalidate();//Invalidate     
        }

        public void ClickeNew(ILayer selectedLayer)
        {
            //History
            SelectModeHistory history = new SelectModeHistory();
            this.ViewModel.Push(history);

            if (selectedLayer.SelectMode.ToBool() == false)
            {
                //History
                history.Add(selectedLayer, selectedLayer.SelectMode, SelectMode.Selected);

                selectedLayer.SelectMode = SelectMode.Selected;
            }

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer != selectedLayer)
                {
                    if (layer.SelectMode.ToBool())
                    {
                        //History
                        history.Add(layer, layer.SelectMode, SelectMode.UnSelected);

                        layer.SelectMode = SelectMode.UnSelected;
                    }
                }
            });

            this.SelectionViewModel.SetModeSingle(selectedLayer);//Selection
            this.ViewModel.Invalidate();//Invalidate     
        }

        public void ClickeAdd(ILayer selectedLayer)
        {
            //History
            SelectModeHistory history = new SelectModeHistory();
            this.ViewModel.Push(history);

            if (selectedLayer.SelectMode.ToBool() == false)
            {
                //History
                history.Add(selectedLayer, selectedLayer.SelectMode, SelectMode.Selected);

                selectedLayer.SelectMode = SelectMode.Selected;
            }

            this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
            this.ViewModel.Invalidate();//Invalidate
        }

        public void ClickeSubtract(ILayer selectedLayer)
        {
            //History
            SelectModeHistory history = new SelectModeHistory();
            this.ViewModel.Push(history);

            if (selectedLayer.SelectMode != SelectMode.UnSelected)
            {
                //History
                history.Add(selectedLayer, selectedLayer.SelectMode, SelectMode.UnSelected);

                selectedLayer.SelectMode = SelectMode.UnSelected;
            }

            this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
            this.ViewModel.Invalidate();//Invalidate
        }

        public void ClickeIntersect(ILayer selectedLayer)
        {
            //History
            SelectModeHistory history = new SelectModeHistory();
            this.ViewModel.Push(history);

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer != selectedLayer)
                {
                    if (layer.SelectMode != SelectMode.UnSelected)
                    {
                        //History
                        history.Add(layer, layer.SelectMode, SelectMode.UnSelected);

                        layer.SelectMode = SelectMode.UnSelected;
                    }
                }
            });

            this.SelectionViewModel.SetModeSingle(selectedLayer);//Selection
            this.ViewModel.Invalidate();//Invalidate
        }


        #endregion


        ILayer _getSelectedLayer(Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, matrix);

            //Select a layer of the same depth
            ILayer _firstLayer = this.SelectionViewModel.GetFirstLayer();
            IList<ILayer> _parentsChildren = this.ViewModel.Layers.GetParentsChildren(_firstLayer);
            ILayer selectedLayer = _parentsChildren.FirstOrDefault((layer) => layer.FillContainsPoint(canvasPoint));
            return selectedLayer;
        }


    }
}