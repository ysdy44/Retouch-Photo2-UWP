using FanKit.Transformers;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    /// <summary>
    /// Convert to curves layer.
    /// </summary>
    public sealed partial class ConvertToCurvesButton : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        //@Construct
        public ConvertToCurvesButton()
        {
            this.InitializeComponent();
            this.Button.Click += (s, e) =>
            {
                if (this.SelectionViewModel.SelectionMode == ListViewSelectionMode.None) return;

                //History
                LayeragesArrangeHistory history = new LayeragesArrangeHistory("Convert to curves", this.ViewModel.LayerageCollection);
                this.ViewModel.HistoryPush(history);

                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer2 = layerage.Self;

                    //Turn to curve layer
                    ILayer curveLayer = this.CreateCurveLayer(layerage);

                    if (curveLayer != null)
                    {
                        Layerage curveLayerage = curveLayer.ToLayerage();
                        LayerBase.Instances.Add(curveLayer);

                        //set image brush
                        if (layer2.Type == LayerType.Image)
                        {
                            ImageLayer imageLayer = (ImageLayer)layer2;
                            curveLayer.Style.Fill = imageLayer.ToBrush();
                        }

                        this.ReplaceLayerage(curveLayerage, layerage);
                    }
                });

                LayerageCollection.ArrangeLayers(this.ViewModel.LayerageCollection);
                LayerageCollection.ArrangeLayersBackground(this.ViewModel.LayerageCollection);
                this.SelectionViewModel.SetMode(this.ViewModel.LayerageCollection);//Selection

                //Change tools group value.
                {
                    ITool tool = this.TipViewModel.Tools.First(t => t != null && t.Type == ToolType.Node);

                    this.TipViewModel.Tool = tool;
                    this.TipViewModel.ToolGroupType(ToolType.Node);
                    this.SelectionViewModel.ToolType = ToolType.Node;

                    this.ViewModel.TipTextBegin(tool.Title);
                    this.ViewModel.Invalidate();//Invalidate
                }
            };
        }
    }

    /// <summary>
    /// Convert to curves layer.
    /// </summary>
    public sealed partial class ConvertToCurvesButton : UserControl
    {

        //Create curve layer
        private ILayer CreateCurveLayer(Layerage layerage)
        {
            ILayer layer = layerage.Self;

            NodeCollection nodes = layer.ConvertToCurves(this.ViewModel.CanvasDevice);
            if (nodes == null) return null;
            
            if (nodes.Count >3)
            {
                CurveLayer curveLayer = new CurveLayer(this.ViewModel.CanvasDevice, nodes)
                {
                    IsSelected = true,
                };
                LayerBase.CopyWith(this.ViewModel.CanvasDevice, curveLayer, layer);
                return curveLayer;
            }

            return null;
        }


        //Replace curveLayerage to layerage
        private void ReplaceLayerage(Layerage curveLayerage, Layerage layerage)
        {
            IList<Layerage> parentsChildren = this.ViewModel.LayerageCollection.GetParentsChildren(layerage);
            int index = parentsChildren.IndexOf(layerage);
            parentsChildren[index] = curveLayerage;
        }


    }
}