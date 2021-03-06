// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Edits.CombineIcons;
using Retouch_Photo2.Edits.EditIcons;
using Retouch_Photo2.Edits.GroupIcons;
using Retouch_Photo2.Edits.SelectIcons;
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
            this.CutButton.Click += (s, e) => this.MethodViewModel.MethodEditCut();
            this.DuplicateButton.Click += (s, e) => this.MethodViewModel.MethodEditDuplicate();
            this.CopyButton.Click += (s, e) => this.MethodViewModel.MethodEditCopy();
            this.PasteButton.Click += (s, e) => this.MethodViewModel.MethodEditPaste();
            this.ClearButton.Click += (s, e) => this.MethodViewModel.MethodEditClear();

            //Select
            this.AllButton.Click += (s, e) => this.MethodViewModel.MethodSelectAll();
            this.DeselectButton.Click += (s, e) => this.MethodViewModel.MethodSelectDeselect();
            this.InvertButton.Click += (s, e) => this.MethodViewModel.MethodSelectInvert();

            //Group
            this.GroupButton.Click += (s, e) => this.MethodViewModel.MethodGroupGroup();
            this.UngroupButton.Click += (s, e) => this.MethodViewModel.MethodGroupUngroup();
            this.ReleaseButton.Click += (s, e) => this.MethodViewModel.MethodGroupRelease();

            //Combine
            this.UnionButton.Click += (s, e) => this.Combine(CanvasGeometryCombine.Union);
            this.ExcludeButton.Click += (s, e) => this.Combine(CanvasGeometryCombine.Exclude);
            this.XorButton.Click += (s, e) => this.Combine(CanvasGeometryCombine.Xor);
            this.IntersectButton.Click += (s, e) => this.Combine(CanvasGeometryCombine.Intersect);
            this.ExpandStrokeButton.Click += (s, e) => this.ExpandStroke();
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

            this.EditTextBlock.Text = resource.GetString("Edits_Edit");
            this.CutButton.Content = resource.GetString("Edits_Edit_Cut");
            this.CutButton.Tag = new CutIcon();
            this.DuplicateButton.Content = resource.GetString("Edits_Edit_Duplicate");
            this.DuplicateButton.Tag = new DuplicateIcon();
            this.CopyButton.Content = resource.GetString("Edits_Edit_Copy");
            this.CopyButton.Tag = new CopyIcon();
            this.PasteButton.Content = resource.GetString("Edits_Edit_Paste");
            this.PasteButton.Tag = new PasteIcon();
            this.ClearButton.Content = resource.GetString("Edits_Edit_Clear");
            this.ClearButton.Tag = new ClearIcon();

            this.GroupTextBlock.Text = resource.GetString("Edits_Group");
            this.GroupButton.Content = resource.GetString("Edits_Group_Group");
            this.GroupButton.Tag = new GroupIcon();
            this.UngroupButton.Content = resource.GetString("Edits_Group_Ungroup");
            this.UngroupButton.Tag = new UngroupIcon();
            this.ReleaseButton.Content = resource.GetString("Edits_Group_Release");
            this.ReleaseButton.Tag = new ReleaseIcon();

            this.SelectTextBlock.Text = resource.GetString("Edits_Select");
            this.AllButton.Content = resource.GetString("Edits_Select_All");
            this.AllButton.Tag = new AllIcon();
            this.DeselectButton.Content = resource.GetString("Edits_Select_Deselect");
            this.DeselectButton.Tag = new DeselectIcon();
            this.InvertButton.Content = resource.GetString("Edits_Select_Invert");
            this.InvertButton.Tag = new InvertIcon();

            this.CombineTextBlock.Text = resource.GetString("Edits_Combine");
            this.UnionButton.Content = resource.GetString("Edits_Combine_Union");
            this.UnionButton.Tag = new UnionIcon();
            this.ExcludeButton.Content = resource.GetString("Edits_Combine_Exclude");
            this.ExcludeButton.Tag = new ExcludeIcon();
            this.XorButton.Content = resource.GetString("Edits_Combine_Xor");
            this.XorButton.Tag = new XorIcon();
            this.IntersectButton.Content = resource.GetString("Edits_Combine_Intersect");
            this.IntersectButton.Tag = new IntersectIcon();
            this.ExpandStrokeButton.Content = resource.GetString("Edits_Combine_ExpandStroke");
            this.ExpandStrokeButton.Tag = new ExpandStrokeIcon();
        }


        /// <summary>
        /// Expand Stroke.
        /// </summary>
        private void ExpandStroke()
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
                        LayerBase.Instances.Add(curveLayer);

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
        private void Combine(CanvasGeometryCombine combine)
        {
            CanvasGeometry geometry = null;
            Styles.IStyle style = null;

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
                    LayerBase.Instances.Add(curveLayer);

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