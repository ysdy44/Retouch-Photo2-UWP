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
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        
        //@VisualState
        bool _isLoaded;
        private ITool toolPage = new NoneTool();
        public ITool ToolPage 
        {
            get => this.toolPage ;
            set
            {
                if (value == null) return;

                //The current page becomes the active page.
                ITool oldToolPage = this.toolPage ;
                oldToolPage.OnNavigatedFrom();

                //The current page does not become an active page.
                ITool newToolPage = value;
                newToolPage.OnNavigatedTo();

                this.Content = null;
                this.Content = value.Page;
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
                
                this.ToolPage = this.GetPage(this.Tool, this.Mode, this.Type);
            };
        }


        private ITool GetPage(ITool tool, ListViewSelectionMode mode, string type)
        {
            return tool;//@Debug:临时返回，建议删掉
            
            if (tool == null) return null;

            if (tool.Type == ToolType.Cursor)
            {
                if (mode == ListViewSelectionMode.Single)
                {
                    //Tool
                    ToolType toolType = this.LayerToTool(type);
                    ITool layerToTool = this.TipViewModel.Tools.FirstOrDefault(e => e != null && e.Type == toolType);
                    if (layerToTool != null) return layerToTool;

                    //Cursor
                    ITool cursorTool = this.TipViewModel.Tools.FirstOrDefault();
                    if (layerToTool == null) return null;
                    return cursorTool;
                }
            }

            return tool;
        }


        private ToolType LayerToTool(string type)
        {
            switch (type)
            {
                //Geometry0
                case "GeometryRectangle": return ToolType.GeometryRectangle;
                case "GeometryEllipse": return ToolType.GeometryEllipse;
                case "GeometryCurve": return ToolType.Pen;

                case "Image": return ToolType.Image;
                case "Acrylic": return ToolType.Acrylic;

                case "TextArtistic": return ToolType.TextArtistic;
                case "TextFrame": return ToolType.TextFrame;

                //Geometry1
                case "GeometryRoundRect": return ToolType.GeometryRoundRect;
                case "GeometryTriangle": return ToolType.GeometryTriangle;
                case "GeometryDiamond": return ToolType.GeometryDiamond;

                //Geometry2
                case "GeometryPentagon": return ToolType.GeometryPentagon;
                case "GeometryStar": return ToolType.GeometryStar;
                case "GeometryCog": return ToolType.GeometryCog;

                //Geometry3
                case "GeometryDount": return ToolType.GeometryDount;
                case "GeometryPie": return ToolType.GeometryPie;
                case "GeometryCookie": return ToolType.GeometryCookie;

                //Geometry4
                case "GeometryArrow": return ToolType.GeometryArrow;
                case "GeometryCapsule": return ToolType.GeometryCapsule;
                case "GeometryHeart": return ToolType.GeometryHeart;
            }
            return ToolType.None;
        }
    }
}