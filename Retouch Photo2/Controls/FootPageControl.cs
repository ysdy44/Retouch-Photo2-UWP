using Retouch_Photo2.Tools;
using Retouch_Photo2.ViewModels;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "FootPageControl" />. 
    /// </summary>
    public sealed partial class FootPageControl : UserControl
    {
        //@ViewModel
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        
        //@VisualState
        bool _isLoaded;
        ITool _vsTool;
        ListViewSelectionMode _vsMode;
        string _vsType;
        public FrameworkElement VisualState
        {
            get
            {
                if (this._isLoaded == false) return null; 
                if (this._vsTool == null) return null;

                if (this._vsTool.Type == ToolType.Cursor)
                {
                    if (this._vsMode == ListViewSelectionMode.Single)
                    {
                        //Tool
                        ToolType toolType = this.LayerToTool(this._vsType);
                        ITool layerToTool = this.TipViewModel.Tools.FirstOrDefault(e => e != null && e.Type == toolType);
                        if (layerToTool != null) return layerToTool.Page.Self;

                        //Cursor
                        ITool cursorTool = this.TipViewModel.Tools.FirstOrDefault();
                        if (layerToTool == null) return null;
                        return cursorTool.Page.Self;
                    }
                }

                return this._vsTool.Page.Self;
            }
            set
            {
                if (this.Content != value)
                {
                    this.Content = value;
                }
            }
        }
        
        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "FootPageControl" />'s tool. </summary>
        public ITool Tool
        {
            get { return (ITool)GetValue(ShadowIToolProperty); }
            set { SetValue(ShadowIToolProperty, value); }
        }
        /// <summary> Identifies the <see cref = "FootPageControl.Tool" /> dependency property. </summary>
        public static readonly DependencyProperty ShadowIToolProperty = DependencyProperty.Register(nameof(Tool), typeof(ITool), typeof(FootPageControl), new PropertyMetadata(null, (sender, e) =>
        {
            FootPageControl con = (FootPageControl)sender;

            if (e.NewValue is ITool value)
            {
                con._vsTool = value;
                con.VisualState = con.VisualState;//State
            }
        }));


        /// <summary> Gets or sets <see cref = "FootPageControl" />'s selection mode. </summary>
        public ListViewSelectionMode Mode
        {
            get { return (ListViewSelectionMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "FootPageControl.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(ListViewSelectionMode), typeof(FootPageControl), new PropertyMetadata(ListViewSelectionMode.None, (sender, e) =>
        {
            FootPageControl con = (FootPageControl)sender;

            if (e.NewValue is ListViewSelectionMode value)
            {
                con._vsMode = value;
                con.VisualState = con.VisualState;//State
            }
        }));


        /// <summary> Gets or sets <see cref = "FootPageControl" />'s layer string. </summary>
        public string Type
        {
            get { return (string)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "FootPageControl.Type" /> dependency property. </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(string), typeof(FootPageControl), new PropertyMetadata(string.Empty, (sender, e) =>
        {
            FootPageControl con = (FootPageControl)sender;

            if (e.NewValue is string value)
            {
                con._vsType = value;
                con.VisualState = con.VisualState;//State
            }
        }));


        #endregion
        
        //@Construct
        public FootPageControl()
        {
            this.Loaded += (s, e) =>
            {
                this._isLoaded = true;

                this.VisualState = null;
                this.VisualState = this.VisualState;//State
            };
        }


        private ToolType LayerToTool(string type)
        {
            switch (type)
            {
                case "GeometryRectangleLayer": return ToolType.GeometryRectangle;
                case "GeometryEllipseLayer": return ToolType.GeometryEllipse;
                case "CurveLayer": return ToolType.Pen;

                case "ImageLayer": return ToolType.Image;
                case "AcrylicLayer": return ToolType.Acrylic;

                case "GeometryRoundRectLayer": return ToolType.GeometryRoundRect;
                case "GeometryTriangleLayer": return ToolType.GeometryTriangle;

                case "GeometryDiamondLayer": return ToolType.GeometryDiamond;
                case "GeometryPentagonLayer": return ToolType.GeometryPentagon;
                case "GeometryStarLayer": return ToolType.GeometryStar;
                case "GeometryPieLayer": return ToolType.GeometryPie;

                case "GeometryCogLayer": return ToolType.GeometryCog;
                case "GeometryArrowLayer": return ToolType.GeometryArrow;
                case "GeometryCapsuleLayer": return ToolType.GeometryCapsule;
                case "GeometryHeartLayer": return ToolType.GeometryHeart;
            }
            return ToolType.None;
        }
    }
}