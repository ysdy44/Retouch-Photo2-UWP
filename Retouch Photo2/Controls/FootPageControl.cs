using Retouch_Photo2.Layers;
using Retouch_Photo2.Tools;
using Retouch_Photo2.Tools.Models;
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
        
        //@VisualState
        bool _isLoaded;


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
            if (con._isLoaded == false) return;

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
            if (con._isLoaded == false) return;

            if (e.NewValue is ListViewSelectionMode value)
            {
                con._vsMode = value;
                con.VisualState = con.VisualState;//State
            }
        }));


        /// <summary> Gets or sets <see cref = "FootPageControl" />'s layer type. </summary>
        public LayerType Type
        {
            get { return (LayerType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "FootPageControl.Type" /> dependency property. </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(LayerType), typeof(FootPageControl), new PropertyMetadata(LayerType.None, (sender, e) =>
        {
            FootPageControl con = (FootPageControl)sender;
            if (con._isLoaded == false) return;

            if (e.NewValue is LayerType value)
            {
                con._vsType = value;
                con.VisualState = con.VisualState;//State
            }
        }));


        #endregion


        //@VisualState
        ITool _vsTool;
        ListViewSelectionMode _vsMode;
        LayerType _vsType;
        public UIElement VisualState
        {
            get
            {
                if (this._vsTool == null) return this.TipViewModel.Tools.FirstOrDefault(tool => tool.Type == ToolType.Cursor).Page;

                return this._vsTool.Page;//@Debug:临时返回，建议删掉

                if (this._vsTool.Type == ToolType.Cursor)
                {
                    if (this._vsMode == ListViewSelectionMode.Single)
                    {
                        //Tool
                        ToolType toolType = this.LayerToTool(this._vsType);
                        ITool layerToTool = this.TipViewModel.Tools.FirstOrDefault(e => e != null && e.Type == toolType);
                        if (layerToTool != null) return layerToTool.Page;

                        //Cursor
                        ITool cursorTool = this.TipViewModel.Tools.FirstOrDefault();
                        if (layerToTool == null) return null;
                        return cursorTool.Page;
                    }
                }

                return this._vsTool.Page;
            }
            set
            {
                if (this.Content == value) return;

                this.Content = value;
            }
        }


        //@Construct
        public FootPageControl()
        {
            this.Loaded += (s, e) =>
            {
                this._isLoaded = true;

                this.VisualState = this.VisualState;//State
            };
        } 


        private ToolType LayerToTool(LayerType type)
        {
            switch (type)
            {
                //Geometry0
                case LayerType.GeometryRectangle: return ToolType.GeometryRectangle;
                case LayerType.GeometryEllipse: return ToolType.GeometryEllipse;
                case LayerType.Curve: return ToolType.Node;

                case LayerType.Image: return ToolType.Image;

                case LayerType.TextArtistic: return ToolType.TextArtistic;
                case LayerType.TextFrame: return ToolType.TextFrame;

                //Geometry1
                case LayerType.GeometryRoundRect: return ToolType.GeometryRoundRect;
                case LayerType.GeometryTriangle: return ToolType.GeometryTriangle;
                case LayerType.GeometryDiamond: return ToolType.GeometryDiamond;

                //Geometry2
                case LayerType.GeometryPentagon: return ToolType.GeometryPentagon;
                case LayerType.GeometryStar: return ToolType.GeometryStar;
                case LayerType.GeometryCog: return ToolType.GeometryCog;

                //Geometry3
                case LayerType.GeometryDount: return ToolType.GeometryDount;
                case LayerType.GeometryPie: return ToolType.GeometryPie;
                case LayerType.GeometryCookie: return ToolType.GeometryCookie;

                //Geometry4
                case LayerType.GeometryArrow: return ToolType.GeometryArrow;
                case LayerType.GeometryCapsule: return ToolType.GeometryCapsule;
                case LayerType.GeometryHeart: return ToolType.GeometryHeart;
            }
            return ToolType.None;
        }

    }
}