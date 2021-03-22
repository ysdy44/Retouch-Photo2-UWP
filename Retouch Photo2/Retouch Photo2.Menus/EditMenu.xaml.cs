﻿// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Styles;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Edits;
using System;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Edits"/>.
    /// </summary>
    public sealed partial class EditMenu : Expander, IMenu
    {

        //@Content       
        public bool IsOpen { set { } }
        public override UIElement MainPage => this.EditMainPage;

        readonly EditMainPage EditMainPage = new EditMainPage();


        //@Construct
        /// <summary>
        /// Initializes a EditMenu. 
        /// </summary>
        public EditMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
        }

    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Edits"/>.
    /// </summary>
    public sealed partial class EditMenu : Expander, IMenu
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title =
            this.Title = resource.GetString("Menus_Edit");
        }

        //Menu
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Edit;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            Content = new Retouch_Photo2.Edits.Icon()
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset() { }

    }

    /// <summary>
    /// MainPage of <see cref = "EditMenu"/>.
    /// </summary>
    public sealed partial class EditMainPage : UserControl
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Construct
        /// <summary>
        /// Initializes a EditMainPage. 
        /// </summary>
        public EditMainPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            //Edit
            this.Edit_Cut.Click += (s, e) => this.MethodViewModel.MethodEditCut();
            this.Edit_Duplicate.Click += (s, e) => this.MethodViewModel.MethodEditDuplicate();
            this.Edit_Copy.Click += (s, e) => this.MethodViewModel.MethodEditCopy();
            this.Edit_Paste.Click += (s, e) => this.MethodViewModel.MethodEditPaste();
            this.Edit_Clear.Click += (s, e) => this.MethodViewModel.MethodEditClear();

            //Select
            this.Select_All.Click += (s, e) => this.MethodViewModel.MethodSelectAll();
            this.Select_Deselect.Click += (s, e) => this.MethodViewModel.MethodSelectDeselect();
            this.Select_Invert.Click += (s, e) => this.MethodViewModel.MethodSelectInvert();

            //Group
            this.Group_Group.Click += (s, e) => this.MethodViewModel.MethodGroupGroup();
            this.Group_Ungroup.Click += (s, e) => this.MethodViewModel.MethodGroupUngroup();
            this.Group_Release.Click += (s, e) => this.MethodViewModel.MethodGroupRelease();

            //Combine
            this.Combine_Union.Click += (s, e) => this.GeometryCombine(CanvasGeometryCombine.Union);
            this.Combine_Exclude.Click += (s, e) => this.GeometryCombine(CanvasGeometryCombine.Exclude);
            this.Combine_Xor.Click += (s, e) => this.GeometryCombine(CanvasGeometryCombine.Xor);
            this.Combine_Intersect.Click += (s, e) => this.GeometryCombine(CanvasGeometryCombine.Intersect);
            this.Combine_ExpandStroke.Click += (s, e) => this.ExpandStrokeCore();
        }

    }

    /// <summary>
    /// MainPage of <see cref = "EditMenu"/>.
    /// </summary>
    public sealed partial class EditMainPage : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            
            foreach (UIElement childPanel in this.LayoutRoot.Children)
            {
                if (childPanel is StackPanel stackPanel)
                {
                    foreach (UIElement child in stackPanel.Children)
                    {
                        if (child is Button button)
                        {
                            button.Content = resource.GetString($"Edits_{button.Name}");
                        }
                        if (child is TextBlock textBlock)
                        {
                            textBlock.Text = resource.GetString($"Edits_{textBlock.Name}");
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Expand Stroke.
        /// </summary>
        private void ExpandStrokeCore()
        {
            IList<Layerage> layerages = new List<Layerage>();

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                IBrush stroke = layer.Style.Stroke;
                float strokeWidth = layer.Style.StrokeWidth;
                CanvasStrokeStyle strokeStyle = layer.Style.StrokeStyle;


                if (stroke.Type == BrushType.None) return;
                if (strokeWidth == 0) return;

                if (layer.CreateGeometry(LayerManager.CanvasDevice) is CanvasGeometry geometry2)
                {
                    CanvasGeometry strokeGeometry = geometry2.Stroke(strokeWidth, strokeStyle);
                    IStyle strokeStyleClone = new Retouch_Photo2.Styles.Style
                    {
                        Fill = stroke.Clone()
                    };


                    //Turn to curve layer
                    ILayer curveLayer = this.CreateCurveLayer(strokeGeometry, strokeStyleClone);
                    if (curveLayer != null)
                    {
                        Layerage curveLayerage = curveLayer.ToLayerage();
                        string id = curveLayerage.Id;
                        LayerBase.Instances.Add(id, curveLayer);

                        layerages.Add(curveLayerage);
                    }
                }
            });


            if (layerages.Count == 0) return;

            if (layerages.Count == 1)
            {
                Layerage curveLayerage = layerages.Single();

                //History
                LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_AddLayer_ExpandStroke);
                this.ViewModel.HistoryPush(history);

                //Mezzanine
                LayerManager.Mezzanine(curveLayerage);

                //History
                this.ViewModel.MethodSelectedNone();

                LayerManager.ArrangeLayers();
                LayerManager.ArrangeLayersBackground();
                this.SelectionViewModel.SetModeSingle(curveLayerage);//Selection
                this.ViewModel.Invalidate();//Invalidate
            }


            if (layerages.Count > 1)
            {
                //History
                LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_AddLayers_ExpandStroke);
                this.ViewModel.HistoryPush(history);

                //Mezzanine
                LayerManager.MezzanineRange(layerages);

                //History
                this.ViewModel.MethodSelectedNone();

                LayerManager.ArrangeLayers();
                LayerManager.ArrangeLayersBackground();
                this.SelectionViewModel.SetModeMultiple(layerages);//Selection
                this.ViewModel.Invalidate();//Invalidate
            }
        }


        /// <summary>
        /// Combine.
        /// </summary>
        /// <param name="combine"> The combine mode. </param>
        private void GeometryCombine(CanvasGeometryCombine combine)
        {
            CanvasGeometry geometry = null;
            IStyle style = null;

            CanvasGeometry other = null;

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.CreateGeometry(LayerManager.CanvasDevice) is CanvasGeometry geometry2)
                {
                    if (geometry == null)
                    {
                        geometry = geometry2;
                        style = layer.Style.Clone();
                    }
                    else
                    {
                        if (other == null)
                            other = geometry2;
                        else
                            other = other.CombineWith(geometry2, Matrix3x2.CreateTranslation(Vector2.Zero), CanvasGeometryCombine.Union);
                    }
                }
            });


            if (geometry != null && other != null)
            {
                CanvasGeometry combineGeometry = geometry.CombineWith(other, Matrix3x2.CreateTranslation(Vector2.Zero), combine);

                //Turn to curve layer
                ILayer curveLayer = this.CreateCurveLayer(combineGeometry, style);
                if (curveLayer != null)
                {
                    Layerage curveLayerage = curveLayer.ToLayerage();
                    string id = curveLayerage.Id;
                    LayerBase.Instances.Add(id, curveLayer);

                    //History
                    LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_AddLayer_Combine);
                    this.ViewModel.HistoryPush(history);

                    //Mezzanine
                    LayerManager.Mezzanine(curveLayerage);

                    //History
                    this.ViewModel.MethodSelectedNone();

                    LayerManager.ArrangeLayers();
                    LayerManager.ArrangeLayersBackground();
                    this.SelectionViewModel.SetModeSingle(curveLayerage);//Selection
                    this.ViewModel.Invalidate();//Invalidate
                }
            }
        }


        //Create curve layer
        private ILayer CreateCurveLayer(CanvasGeometry geometry, IStyle style)
        {
            NodeCollection nodes = new NodeCollection(geometry);
            if (nodes == null) return null;

            if (nodes.Count > 3)
            {
                CurveLayer curveLayer = new CurveLayer(nodes)
                {
                    Style = style,
                    IsSelected = true,
                };
                return curveLayer;
            }

            return null;
        }

    }
}