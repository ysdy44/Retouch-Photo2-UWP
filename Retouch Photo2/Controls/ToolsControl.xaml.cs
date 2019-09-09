using Retouch_Photo2.Tools;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Tips;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ToolsControl"/>. 
    /// </summary>
    public sealed partial class ToolsControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        //@Content
        /// <summary> MoreBorder's Child. </summary>
        public UIElement MoreBorderChild { get => this.MoreBorder.Child; set => this.MoreBorder.Child = value; }
        

        #region DependencyProperty

        /// <summary> 
        /// Type of <see cref = "ToolsControl" />. 
        /// </summary>
        public ToolType ToolType
        {
            get { return (ToolType)GetValue(ToolTypeProperty); }
            set { SetValue(ToolTypeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "ToolsControl.ToolType" /> dependency property. </summary>
        public static readonly DependencyProperty ToolTypeProperty = DependencyProperty.Register(nameof(ToolType), typeof(ToolType), typeof(ToolsControl), new PropertyMetadata(ToolType.None));

        #endregion


        //@Construct
        public ToolsControl()
        {
            this.InitializeComponent();
             
            //Cursor
            this.ConstructButton(this.CursorButton, this.TipViewModel.CursorTool);
            //View
            this.ConstructButton(this.ViewButton, this.TipViewModel.ViewTool);
            //Brush
            this.ConstructButton(this.BrushButton, this.TipViewModel.BrushTool);
            //Rectangle
            this.ConstructButton(this.RectangleButton, this.TipViewModel.RectangleTool);
            //Ellipse
            this.ConstructButton(this.EllipseButton, this.TipViewModel.EllipseTool);
            //Pen
            this.ConstructButton(this.PenButton, this.TipViewModel.PenTool);
            //Image
            this.ConstructButton(this.ImageButton, this.TipViewModel.ImageTool);
            //Acrylic
            this.ConstructButton(this.AcrylicButton, this.TipViewModel.AcrylicTool);
        }

        private void ConstructButton(Tools.Button button, ITool tool)
        {
            ToolType type = tool.Type;

            //Content
            button.Type = type;
            button.CenterContent = tool.Icon;

            //ItemClick
            button.RootGrid.Tapped += (s, e) =>
            {
                this.ToolType = type;
                this.TipViewModel.Tool = tool;

                this.TipViewModel.SetTouchbar(TouchbarType.None);//Touchbar

                this.ViewModel.Invalidate();//Invalidate
            };
        }
    }
}