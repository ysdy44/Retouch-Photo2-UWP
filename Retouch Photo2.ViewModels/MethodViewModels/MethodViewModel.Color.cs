using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Effects;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ViewModel" />. 
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {


        //History
        LayersPropertyHistory _historyMethodFillColor = null;

        public void MethodFillColorChanged(Color value)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set fill");

            //Selection
            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.Color = value;
                    break;
            }
            this.Fill = BrushBase.ColorBrush(value);
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Style.Fill.Clone();
                history.UndoActions.Push(() =>
                {
                    ILayer layer2 = layerage.Self;

                    layer2.Style.Fill = previous.Clone();
                });

                layer.Style.Fill = BrushBase.ColorBrush(value);
                this.StyleLayerage = layerage;
            });

            //History
            this.Historys.Add(history);

            this.Invalidate();//Invalidate
        }

        public void MethodFillColorChangeStarted(Color value)
        {
            this._historyMethodFillColor = new LayersPropertyHistory("Set fill");

            //Selection
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;
                layer.Style.CacheFill();
            });

            this.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }

        public void MethodFillColorChangeDelta(Color value)
        {
            //Selection
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                layer.Style.Fill = BrushBase.ColorBrush(value);
            });

            this.Invalidate();//Invalidate         
        }

        public void MethodFillColorChangeCompleted(Color value)
        {
            //Selection
            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.Color = value;
                    break;
            }
            this.Fill = BrushBase.ColorBrush(value);
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Style.StartingFill.Clone();
                this._historyMethodFillColor.UndoActions.Push(() =>
                {
                    ILayer layer2 = layerage.Self;

                    layer2.Style.Fill = previous.Clone();
                });

                layer.Style.Fill = BrushBase.ColorBrush(value);
                this.StyleLayerage = layerage;
            });

            //History
            this.HistoryPush(this._historyMethodFillColor);

            this.Invalidate(InvalidateMode.HD);//Invalidate 
        }

               


        //History
        LayersPropertyHistory _historyMethodStrokeColor = null;

        public void MethodStrokeColorChanged(Color value)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set stroke");

            //Selection
            switch (this.FillOrStroke)
            {
                case FillOrStroke.Stroke:
                    this.Color = value;
                    break;
            }
            this.Stroke = BrushBase.ColorBrush(value);
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Style.Stroke.Clone();
                history.UndoActions.Push(() =>
                {
                    ILayer layer2 = layerage.Self;

                    layer2.Style.Stroke = previous.Clone();
                });

                layer.Style.Stroke = BrushBase.ColorBrush(value);
                this.StyleLayerage = layerage;
            });

            //History
            this.Historys.Add(history);

            this.Invalidate();//Invalidate
        }

        public void MethodStrokeColorChangeStarted(Color value)
        {
            this._historyMethodStrokeColor = new LayersPropertyHistory("Set stroke");

            //Selection
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;
                layer.Style.CacheStroke();
            });

            this.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }

        public void MethodStrokeColorChangeDelta(Color value)
        {
            //Selection
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                layer.Style.Stroke = BrushBase.ColorBrush(value);
            });

            this.Invalidate();//Invalidate         
        }

        public void MethodStrokeColorChangeCompleted(Color value)
        {
            //Selection
            switch (this.FillOrStroke)
            {
                case FillOrStroke.Stroke:
                    this.Color = value;
                    break;
            }
            this.Stroke = BrushBase.ColorBrush(value);
            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Style.StartingStroke.Clone();
                this._historyMethodStrokeColor.UndoActions.Push(() =>
                {
                    ILayer layer2 = layerage.Self;

                    layer2.Style.Stroke = previous.Clone();
                });

                layer.Style.Stroke = BrushBase.ColorBrush(value);
                this.StyleLayerage = layerage;
            });

            //History
            this.HistoryPush(this._historyMethodStrokeColor);

            this.Invalidate(InvalidateMode.HD);//Invalidate 
        }


    }
}
