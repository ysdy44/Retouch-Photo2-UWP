﻿// Core:              ★
// Referenced:   ★★★
// Difficult:         ★★★
// Only:              ★
// Complete:      ★★★★
using FanKit.Transformers;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    /// <summary> 
    /// Represents a control that convert layer to curves layer.
    /// </summary>
    public sealed partial class ConvertToCurvesButton : UserControl
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        private static string title = null;

        #region DependencyProperty


        /// <summary> Gets or sets the title of <see cref = "ConvertToCurvesButton" />. </summary>
        public string Title
        {
            get => (string)base.GetValue(TitleProperty);
            set => base.SetValue(TitleProperty, value);
        }
        /// <summary> Identifies the <see cref = "ConvertToCurvesButton.Title" /> dependency property. </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(ConvertToCurvesButton), new PropertyMetadata(null));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a ConvertToCurvesButton. 
        /// </summary>
        public ConvertToCurvesButton()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.Button.Click += (s, e) =>
            {
                if (this.SelectionViewModel.SelectionMode == ListViewSelectionMode.None) return;

                //History
                LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_AddLayer_ConvertToCurves);
                this.ViewModel.HistoryPush(history);

                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //Turn to curve layer
                    ILayer curveLayer = this.CreateCurveLayer(layerage);

                    if (curveLayer != null)
                    {
                        Layerage curveLayerage = curveLayer.ToLayerage();
                        LayerBase.Instances.Add(curveLayer);

                        //set image brush
                        if (layer.Type == LayerType.Image)
                        {
                            ImageLayer imageLayer = (ImageLayer)layer;
                            curveLayer.Style.Fill = imageLayer.ToBrush();
                        }

                        this.ReplaceLayerage(curveLayerage, layerage);
                    }
                });

                LayerManager.ArrangeLayers();
                LayerManager.ArrangeLayersBackground();
                this.SelectionViewModel.SetMode();//Selection

                //Change tools group value.
                this.ItemClick(this.TipViewModel.Tools.First(t => t != null && t.Type == ToolType.Node));
            };
        }

        private void ItemClick(ITool tool)
        {
            //Change tools group value.
            ToolManager.Instance = tool;
            this.SelectionViewModel.ToolType = tool.Type;

            this.ViewModel.TipTextBegin(tool.Title);
            this.ViewModel.Invalidate();//Invalidate
        }

    }

    /// <summary>
    /// Convert to curves layer.
    /// </summary>
    public sealed partial class ConvertToCurvesButton : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            if (ConvertToCurvesButton.title == null)
            {
                ResourceLoader resource = ResourceLoader.GetForCurrentView();

                ConvertToCurvesButton.title = resource.GetString("Tools_ConvertToCurves");
            }
            this.Title = ConvertToCurvesButton.title;
        }

        //Create curve layer
        private ILayer CreateCurveLayer(Layerage layerage)
        {
            ILayer layer = layerage.Self;

            NodeCollection nodes = layer.ConvertToCurves(LayerManager.CanvasDevice);
            if (nodes == null) return null;
            
            if (nodes.Count >3)
            {
                CurveLayer curveLayer = new CurveLayer(nodes)
                {
                    IsSelected = true,
                };
                LayerBase.CopyWith(curveLayer, layer);
                return curveLayer;
            }

            return null;
        }


        //Replace curveLayerage to layerage
        private void ReplaceLayerage(Layerage curveLayerage, Layerage layerage)
        {
            Layerage parents = LayerManager.GetParentsChildren(layerage);
            int index = parents.Children.IndexOf(layerage);
            parents.Children[index] = curveLayerage;
        }


    }
}