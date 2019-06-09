using Retouch_Photo2.TestApp.Tools;
using System;
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


        #region DependencyProperty


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
                con.RadiusAnimaPanel.CenterContent = newTool.Page;

                //If you choose a different tool, PhoneState will hided.
                if (e.OldValue is Tool oldTool)
                {
                    if (newTool.Type != oldTool.Type)
                    {
                        if (con.PhoneState != PhoneState.Hided)
                        {
                            con.PhoneState = PhoneState.Hided;
                            con.State = con.GetState();
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

        bool IsFullScreen;
        double SizeWidth;
        PhoneState PhoneState;

        private DrawLayoutState GetState()
        {
            if (this.IsFullScreen) return DrawLayoutState.FullScreen;

            if (this.SizeWidth > 900.0) return DrawLayoutState.PC;
            if (this.SizeWidth > 600.0) return DrawLayoutState.Pad;

            switch (this.PhoneState)
            {
                case PhoneState.Hided: return DrawLayoutState.Phone;
                case PhoneState.ShowLeft: return DrawLayoutState.PhoneShowLeft;
                case PhoneState.ShowRight: return DrawLayoutState.PhoneShowRight;
            }

            return DrawLayoutState.None;
        }
        private void SetState() => this.State = GetState(); //State
        private void SetState(Action action)
        {
            action();
            this.State = GetState(); //State
        }


        //@Construct
        public DrawLayout()
        {
            this.InitializeComponent();

                this.SizeChanged += (s, e) =>
                {
                    if (e.NewSize == e.PreviousSize) return;
                    this.SizeWidth = e.NewSize.Width;
                    this.SetState(); //State
                };

                //FullScreen
                this.UnFullScreenButton.Tapped += (s, e) => this.SetState(() => this.IsFullScreen = false); //State
                this.FullScreenButton.Tapped += (s, e) => this.SetState(() => this.IsFullScreen = true); //State

                //DismissOverlay
                this.IconDismissOverlay.Tapped += (s, e) => this.SetState(() => this.PhoneState = PhoneState.Hided); //State

                //IconLeft
                this.IconLeftGrid.Tapped += (s, e) => this.SetState(() => this.PhoneState = PhoneState.ShowLeft); //State
                this.IconLeftGrid.PointerEntered += (s, e) =>
                {
                    if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
                    {
                        this.PhoneState = PhoneState.ShowLeft;
                        this.SetState(); //State
                    }
                };
                //IconRight
                this.IconRightGrid.Tapped += (s, e) => this.SetState(() => this.PhoneState = PhoneState.ShowRight); //State
                this.IconRightGrid.PointerEntered += (s, e) =>
                {
                    if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
                    {
                        this.PhoneState = PhoneState.ShowRight;
                        this.SetState(); //State
                    }
                };
        }
    }
}