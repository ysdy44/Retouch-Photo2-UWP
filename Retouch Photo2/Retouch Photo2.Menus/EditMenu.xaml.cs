// Core:              ★★★★
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

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Edits"/>.
    /// </summary>
    public sealed partial class EditMenu : Expander
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Construct
        /// <summary>
        /// Initializes a EditMenu. 
        /// </summary>
        public EditMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            //Edit
            this.Edit_CutItem.Tapped += (s, e) => this.MethodViewModel.MethodEditCut();
            this.Edit_DuplicateItem.Tapped += (s, e) => this.MethodViewModel.MethodEditDuplicate();
            this.Edit_CopyItem.Tapped += (s, e) => this.MethodViewModel.MethodEditCopy();
            this.Edit_PasteItem.Tapped += (s, e) => this.MethodViewModel.MethodEditPaste();
            this.Edit_ClearItem.Tapped += (s, e) => this.MethodViewModel.MethodEditClear();

            //Select
            this.Select_AllItem.Tapped += (s, e) => this.MethodViewModel.MethodSelectAll();
            this.Select_DeselectItem.Tapped += (s, e) => this.MethodViewModel.MethodSelectDeselect();
            this.Select_InvertItem.Tapped += (s, e) => this.MethodViewModel.MethodSelectInvert();

            //Group
            this.Group_GroupItem.Tapped += (s, e) => this.MethodViewModel.MethodGroupGroup();
            this.Group_UngroupItem.Tapped += (s, e) => this.MethodViewModel.MethodGroupUngroup();
            this.Group_ReleaseItem.Tapped += (s, e) => this.MethodViewModel.MethodGroupRelease();

            //Combine
            this.Combine_UnionItem.Tapped += (s, e) => this.GeometryCombine(CanvasGeometryCombine.Union);
            this.Combine_ExcludeItem.Tapped += (s, e) => this.GeometryCombine(CanvasGeometryCombine.Exclude);
            this.Combine_XorItem.Tapped += (s, e) => this.GeometryCombine(CanvasGeometryCombine.Xor);
            this.Combine_IntersectItem.Tapped += (s, e) => this.GeometryCombine(CanvasGeometryCombine.Intersect);
            this.Combine_ExpandStrokeItem.Tapped += (s, e) => this.ExpandStrokeCore();
        }

    }

    public sealed partial class EditMenu : Expander
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.ConstructStringsCore(resource, this.Edit);
            this.ConstructStringsCore(resource, this.Edit_Cut);
            this.ConstructStringsCore(resource, this.Edit_Duplicate);
            this.ConstructStringsCore(resource, this.Edit_Copy);
            this.ConstructStringsCore(resource, this.Edit_Paste);
            this.ConstructStringsCore(resource, this.Edit_Clear);

            this.ConstructStringsCore(resource, this.Group);
            this.ConstructStringsCore(resource, this.Group_Group);
            this.ConstructStringsCore(resource, this.Group_Ungroup);
            this.ConstructStringsCore(resource, this.Group_Release);

            this.ConstructStringsCore(resource, this.Select);
            this.ConstructStringsCore(resource, this.Select_All);
            this.ConstructStringsCore(resource, this.Select_Deselect);
            this.ConstructStringsCore(resource, this.Select_Invert);

            this.ConstructStringsCore(resource, this.Combine);
            this.ConstructStringsCore(resource, this.Combine_Union);
            this.ConstructStringsCore(resource, this.Combine_Exclude);
            this.ConstructStringsCore(resource, this.Combine_Xor);
            this.ConstructStringsCore(resource, this.Combine_Intersect);
            this.ConstructStringsCore(resource, this.Combine_ExpandStroke);
        }
        private void ConstructStringsCore(ResourceLoader resource, ContentControl control)
        {
            string key = control.Name;
            string title = resource.GetString($"Edits_{key}");

            control.Content = title;
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
                        Layerage curveLayerage = Layerage.CreateByGuid();
                        curveLayer.Id = curveLayerage.Id;
                        LayerBase.Instances.Add(curveLayerage.Id, curveLayer);

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
                    Layerage curveLayerage = Layerage.CreateByGuid();
                    curveLayer.Id = curveLayerage.Id;
                    LayerBase.Instances.Add(curveLayerage.Id, curveLayer);

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