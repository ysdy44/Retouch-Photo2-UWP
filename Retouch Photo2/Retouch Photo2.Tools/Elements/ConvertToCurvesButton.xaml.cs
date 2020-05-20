using FanKit.Transformers;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    public sealed partial class ConvertToCurvesButton : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
               
        //@Construct
        public ConvertToCurvesButton()
        {
            this.InitializeComponent();
            this.Button.Click += (s, e) =>
            {
                if (this.SelectionViewModel.SelectionMode == ListViewSelectionMode.None) return;
            

                this.SelectionViewModel.SetValue((layer)=>
                {
                    //Turn to curve layer
                    ILayer curveLayer = this.GetCurveLayer(layer);
                    if (curveLayer == null) return;

                    //set image brush
                    if (layer.Type == LayerType.Image)
                    {
                        ImageLayer imageLayer = (ImageLayer)layer;
                        curveLayer.Style.Fill = imageLayer.ToBrush();
                    }

                    this.Replace(curveLayer, layer);
                    curveLayer.IsSelected = true;
                });


                //Change tools group value.
                this.TipViewModel.Tool = this.TipViewModel.Tools.First(t => t != null && t.Type == ToolType.Node);
                this.TipViewModel.ToolGroupType(ToolType.Node);

                LayerCollection.ArrangeLayersControls(this.ViewModel.LayerCollection);

                this.SelectionViewModel.SetMode(this.ViewModel.LayerCollection);//Selection

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        //Get curve layer
        private ILayer GetCurveLayer(ILayer layer)
        {
            IEnumerable<IEnumerable<Node>> nodess = layer.ConvertToCurves();
            if (nodess == null) return null;
            
            if (nodess.Count() == 1)
            {
                CurveLayer curveLayer = new CurveLayer(nodess.Single());
                LayerBase.CopyWith(this.ViewModel.CanvasDevice, curveLayer, layer);
                return curveLayer;
            }

            CurveMultiLayer curveMultiLayer = new CurveMultiLayer(nodess);
            LayerBase.CopyWith(this.ViewModel.CanvasDevice, curveMultiLayer, layer);
            return curveMultiLayer;
        }
         
        
        //Replace curveLayer to layer
        private void Replace(ILayer curveLayer, ILayer layer)
        {
            IList<ILayer> parentsChildren = this.ViewModel.LayerCollection.GetParentsChildren(layer);
            int index = parentsChildren.IndexOf(layer);
            parentsChildren[index] = curveLayer;
        }


    }
}