using Retouch_Photo2.Tools;
using Retouch_Photo2.Tools.Pages;
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
        private IToolPage toolPage = new NonePage();
        public IToolPage ToolPage 
        {
            get => this.toolPage ;
            set
            {
                if (value == null) return;

                //The current page becomes the active page.
                IToolPage oldToolPage = this.toolPage ;
                oldToolPage.OnNavigatedFrom();

                //The current page does not become an active page.
                IToolPage newToolPage = value;
                newToolPage.OnNavigatedTo();

                this.Content = null;
                this.Content = value.Self;
                this.toolPage  = value;
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
            if (con._isLoaded == false) return;

            if (e.NewValue is ITool value)
            {
                con.ToolPage = con.GetPage(value, con.Mode, con.Type);
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
                con.ToolPage = con.GetPage(con.Tool, value, con.Type);
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
            if (con._isLoaded == false) return;

            if (e.NewValue is string value)
            {
                con.ToolPage = con.GetPage(con.Tool, con.Mode, value);
            }
        }));


        #endregion
        
        //@Construct
        public FootPageControl()
        {
            this.Loaded += (s, e) =>
            {
                this._isLoaded = true;

                this.ToolPage = null;
                this.ToolPage = this.ToolPage;//State
            };
        }


        private IToolPage GetPage(ITool tool, ListViewSelectionMode mode, string type)
        {
            if (tool == null) return null;

            if (tool.Type == ToolType.Cursor)
            {
                if (mode == ListViewSelectionMode.Single)
                {
                    //Tool
                    ToolType toolType = this.LayerToTool(type);
                    ITool layerToTool = this.TipViewModel.Tools.FirstOrDefault(e => e != null && e.Type == toolType);
                    if (layerToTool != null) return layerToTool.Page;

                    //Cursor
                    ITool cursorTool = this.TipViewModel.Tools.FirstOrDefault();
                    if (layerToTool == null) return null;
                    return cursorTool.Page;
                }
            }

            return tool.Page;
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