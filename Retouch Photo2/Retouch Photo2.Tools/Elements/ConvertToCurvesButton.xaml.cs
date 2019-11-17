﻿using FanKit.Transformers;
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
            this.Button.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.SelectionMode == ListViewSelectionMode.Single)
                {
                    ILayer destination = this.SelectionViewModel.Layer;

                    IEnumerable<IEnumerable<Node>> nodess = destination.ConvertToCurves();
                    if (nodess == null) return;
                    {
                        IList<ILayer> parentsChildren = (destination.Parents == null) ?
                            this.ViewModel.Layers.RootLayers :
                            destination.Parents.Children;

                        int index = parentsChildren.IndexOf(destination);

                        //Turn to curve
                        {
                            ILayer curveLayer =
                                nodess.Count() == 1 ?
                                (ILayer)new GeometryCurveLayer(nodess.Single()) :
                                (ILayer)new GeometryCurveMultiLayer(nodess);

                            LayerBase.CopyWith(this.ViewModel.CanvasDevice, curveLayer, destination);
                            parentsChildren[index] = curveLayer;

                            curveLayer.SelectMode = SelectMode.Selected;
                            this.SelectionViewModel.SetModeSingle(curveLayer);//Selection
                        }

                        //Change tools group value.
                        {
                            ITool penTool = this.TipViewModel.Tools.First(t => t != null && t.Type == ToolType.Pen);
                            this.TipViewModel.Tool = penTool;
                            this.TipViewModel.ToolGroupType(ToolType.Pen);
                        }

                        this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();
                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
        }


    }
}