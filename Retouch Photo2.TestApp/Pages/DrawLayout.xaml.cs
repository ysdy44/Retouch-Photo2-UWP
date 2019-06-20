using Retouch_Photo2.Tools;
using Windows.Devices.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.TestApp.Pages
{
    /// <summary> PhoneState of <see cref="DrawLayoutState"/>. </summary>
    public enum PhoneState
    {
        /// <summary> Hide left and right borders. </summary>
        Hided,
        /// <summary> Show left border. </summary>
        ShowLeft,
        /// <summary> Show right border. </summary>
        ShowRight,
    }

    /// <summary> State of <see cref="DrawLayout"/>. </summary>
    public enum DrawLayoutState
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Full-screen. </summary>
        FullScreen,

        /// <summary> Phone. </summary>
        Phone,
        /// <summary> Phone (Show left border). </summary>
        PhoneShowLeft,
        /// <summary> Phone (Show right border). </summary>
        PhoneShowRight,

        /// <summary> Pad. </summary>
        Pad,

        /// <summary> Person computer. </summary>
        PC,
    }

    /// <summary> <see cref = "DrawPage" />'s layout. </summary>
    public sealed partial class DrawLayout : UserControl
    {

        //@Content
        /// <summary> RightBorder's Child. </summary>
        public UIElement RightPane { get => this.RightBorder.Child; set => this.RightBorder.Child = value; }
        /// <summary> LeftBorder's Child. </summary>
        public UIElement LeftPane { get => this.LeftBorder.Child; set => this.LeftBorder.Child = value; }

        /// <summary> CenterBorder's Child. </summary>
        public UIElement CenterContent { get => this.CenterBorder.Child; set => this.CenterBorder.Child = value; }

        /// <summary> TopLeftBorder's Child. </summary>
        public UIElement TopLeftPane { get => this.TopLeftBorder.Child; set => this.TopLeftBorder.Child = value; }
        /// <summary> TopRightBorder's Child. </summary>
        public UIElement TopRightPane { get => this.TopRightBorder.Child; set => this.TopRightBorder.Child = value; }
        /// <summary> TopLeftStackPanel's Child. </summary>
        public UIElement TopLeftStackBar { get => this.TopLeftStackPanel.Child; set => this.TopLeftStackPanel.Child = value; }

        /// <summary> Backgroud's Color. </summary>
        public ElementTheme Theme
        {
            set
            {
                switch (value)
                {
                    case ElementTheme.Light:
                        this.LightStoryboard.Begin();//Storyboard
                        break;
                    case ElementTheme.Dark:
                        this.DarkStoryboard.Begin();//Storyboard
                        break;
                }
            }
        }


        #region DependencyProperty


        /// <summary> Sets or Gets the on state of the IsHitTestVisible on the canvas. </summary>
        public bool CanvasHitTestVisible
        {
            get { return (bool)GetValue(CanvasHitTestVisibleProperty); }
            set { SetValue(CanvasHitTestVisibleProperty, value); }
        }
        /// <summary> Identifies the <see cref = "DrawLayout.CanvasHitTestVisible" /> dependency property. </summary>
        public static readonly DependencyProperty CanvasHitTestVisibleProperty = DependencyProperty.Register(nameof(CanvasHitTestVisible), typeof(bool), typeof(DrawLayout), new PropertyMetadata(true));


        /// <summary> Sets or Gets the page layout is full screen. </summary>
        public bool IsFullScreen
        {
            get { return (bool)GetValue(IsFullScreenProperty); }
            set { SetValue(IsFullScreenProperty, value); }
        }
        /// <summary> Identifies the <see cref = "DrawLayout.IsFullScreen" /> dependency property. </summary>
        public static readonly DependencyProperty IsFullScreenProperty = DependencyProperty.Register(nameof(IsFullScreen), typeof(bool), typeof(DrawLayout), new PropertyMetadata(false, (sender, e) =>
        {
            DrawLayout con = (DrawLayout)sender;

            if (e.NewValue is bool value)
            {
                con.isFullScreen = value;
                con.State = con.GetState();//State
            }
        }));


        /// <summary> Gets or sets <see cref = "DrawLayout" />'s phone state. </summary>
        public PhoneState PhoneState
        {
            get { return (PhoneState)GetValue(PhoneStateProperty); }
            set { SetValue(PhoneStateProperty, value); }
        }
        /// <summary> Identifies the <see cref = "DrawLayout.Tool" /> dependency property. </summary>
        public static readonly DependencyProperty PhoneStateProperty = DependencyProperty.Register(nameof(PhoneState), typeof(PhoneState), typeof(DrawLayout), new PropertyMetadata(PhoneState.Hided, (sender, e) =>
        {
            DrawLayout con = (DrawLayout)sender;

            if (e.NewValue is PhoneState value)
            {
                con.phoneState = value;
                con.State = con.GetState();//State
            }
        }));


        /// <summary> Gets or sets <see cref = "DrawLayout" />'s tool. </summary>
        public Tool Tool
        {
            get { return (Tool)GetValue(ToolProperty); }
            set { SetValue(ToolProperty, value); }
        }
        /// <summary> Identifies the <see cref = "DrawLayout.Tool" /> dependency property. </summary>
        public static readonly DependencyProperty ToolProperty = DependencyProperty.Register(nameof(Tool), typeof(Tool), typeof(DrawLayout), new PropertyMetadata(null, (sender, e) =>
        {
            DrawLayout con = (DrawLayout)sender;

            if (e.NewValue is Tool newTool)
            {
                //Show
                con.IconLeftIcon.Content = newTool.ShowIcon;

                //Page
                con.ScrollViewer.Content = newTool.Page;

                //If you choose a different tool, PhoneState will hided.
                if (e.OldValue is Tool oldTool)
                {
                    if (newTool.Type != oldTool.Type)
                    {
                        if (con.phoneState != PhoneState.Hided)
                        {
                            con.phoneState = PhoneState.Hided;
                            con.State = con.GetState();//State
                        }
                    }
                }
            }
        }));


        #endregion


        /// <summary> State of <see cref="DrawLayout"/>. </summary>
        public DrawLayoutState State
        {
            get => this.state;
            set
            {
                switch (value)
                {
                    case DrawLayoutState.None:
                        VisualStateManager.GoToState(this, this.Normal.Name, false);
                        break;

                    case DrawLayoutState.FullScreen:
                        VisualStateManager.GoToState(this, this.FullScreen.Name, false);
                        break;

                    case DrawLayoutState.Phone:
                        VisualStateManager.GoToState(this, this.Phone.Name, false);
                        break;
                    case DrawLayoutState.PhoneShowLeft:
                        VisualStateManager.GoToState(this, this.PhoneShowLeft.Name, false);
                        break;
                    case DrawLayoutState.PhoneShowRight:
                        VisualStateManager.GoToState(this, this.PhoneShowRight.Name, false);
                        break;

                    case DrawLayoutState.Pad:
                        VisualStateManager.GoToState(this, this.Pad.Name, false);
                        break;

                    case DrawLayoutState.PC:
                        VisualStateManager.GoToState(this, this.PC.Name, false);
                        break;
                }
                this.state = value;
            }
        }
        private DrawLayoutState state;

        bool isFullScreen;
        double sizeWidth;
        PhoneState phoneState;

        private DrawLayoutState GetState()
        {
            if (this.isFullScreen) return DrawLayoutState.FullScreen;

            if (this.sizeWidth > 900.0) return DrawLayoutState.PC;
            if (this.sizeWidth > 600.0) return DrawLayoutState.Pad;

            switch (this.phoneState)
            {
                case PhoneState.Hided: return DrawLayoutState.Phone;
                case PhoneState.ShowLeft: return DrawLayoutState.PhoneShowLeft;
                case PhoneState.ShowRight: return DrawLayoutState.PhoneShowRight;
            }

            return DrawLayoutState.None;
        }


        //@Construct
        public DrawLayout()
        {
            this.InitializeComponent();

            this.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.sizeWidth = e.NewSize.Width;
                this.State = this.GetState(); //State
            };
            
            //FullScreen
            this.UnFullScreenButton.Tapped += (s, e) => this.IsFullScreen = false;
            this.FullScreenButton.Tapped += (s, e) => this.IsFullScreen = true;

            //DismissOverlay
            this.IconDismissOverlay.Tapped += (s, e) => this.PhoneState = PhoneState.Hided;
            
            //IconLeft
            this.IconLeftGrid.Tapped += (s, e) => this.PhoneState = PhoneState.ShowLeft;
            this.IconLeftGrid.PointerEntered += (s, e) =>
            {
                if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)                
                    this.PhoneState = PhoneState.ShowLeft;                
            };
            //IconRight
            this.IconRightGrid.Tapped += (s, e) => this.PhoneState = PhoneState.ShowRight;
            this.IconRightGrid.PointerEntered += (s, e) =>
            {
                if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)                
                    this.PhoneState = PhoneState.ShowRight;                
            };
        }
    }
}