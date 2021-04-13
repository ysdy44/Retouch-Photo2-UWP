// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using FanKit.Transformers;
using Retouch_Photo2.Tools;
using Retouch_Photo2.ViewModels;
using System;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Menu of <see cref = "FanKit.Transformers.Transformer"/>.
    /// </summary>
    public sealed partial class TransformerMenu : UserControl
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        bool IsRatio => this.SettingViewModel.IsRatio;
        Transformer SelectionTransformer { get => this.SelectionViewModel.Transformer; set => this.SelectionViewModel.Transformer = value; }


        private IndicatorMode IndicatorMode = IndicatorMode.LeftTop;


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "TransformerMenu" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "TransformerMenu.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(TransformerMenu), new PropertyMetadata(false));


        /// <summary> Gets or sets <see cref = "TransformerMenu" />'s tool type. </summary>
        public ToolType ToolType
        {
            get => (ToolType)base.GetValue(ToolTypeProperty);
            set => base.SetValue(ToolTypeProperty, value);
        }
        /// <summary> Identifies the <see cref = "TransformerMenu.ToolType" /> dependency property. </summary>
        public static readonly DependencyProperty ToolTypeProperty = DependencyProperty.Register(nameof(ToolType), typeof(ToolType), typeof(TransformerMenu), new PropertyMetadata(ToolType.Cursor, (sender, e) =>
        {
            TransformerMenu control = (TransformerMenu)sender;

            if (e.NewValue is ToolType value)
            {
                switch (value)
                {
                    case ToolType.Cursor:
                    case ToolType.View:

                    case ToolType.GeometryRectangle:
                    case ToolType.GeometryEllipse:

                    case ToolType.TextFrame:
                    case ToolType.TextArtistic:

                    case ToolType.Image:
                    case ToolType.Crop:


                    //Pattern
                    case ToolType.PatternGrid:
                    case ToolType.PatternDiagonal:
                    case ToolType.PatternSpotted:


                    //Geometry1
                    case ToolType.GeometryRoundRect:
                    case ToolType.GeometryTriangle:
                    case ToolType.GeometryDiamond:

                    //Geometry2
                    case ToolType.GeometryPentagon:
                    case ToolType.GeometryStar:
                    case ToolType.GeometryCog:

                    //Geometry3
                    case ToolType.GeometryDount:
                    case ToolType.GeometryPie:
                    case ToolType.GeometryCookie:

                    //Geometry4
                    case ToolType.GeometryArrow:
                    case ToolType.GeometryCapsule:
                    case ToolType.GeometryHeart:
                        {
                            control._vsDisabledTool = false;
                            control.VisualState = control.VisualState;//State
                            return;
                        }
                }
            }

            control._vsDisabledTool = true;
            control.VisualState = control.VisualState;//State
            return;
        }));


        /// <summary> Gets or sets <see cref = "TransformerMenu" />'s selection mode. </summary>
        public ListViewSelectionMode Mode
        {
            get => (ListViewSelectionMode)base.GetValue(ModeProperty);
            set => base.SetValue(ModeProperty, value);
        }
        /// <summary> Identifies the <see cref = "TransformerMenu.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(ListViewSelectionMode), typeof(TransformerMenu), new PropertyMetadata(ListViewSelectionMode.None, (sender, e) =>
        {
            TransformerMenu control = (TransformerMenu)sender;

            if (e.NewValue is ListViewSelectionMode value)
            {
                control._vsMode = value;
                control.VisualState = control.VisualState;//State
            }
        }));


        /// <summary> Gets or sets <see cref = "TransformerMenu" />'s transformer. </summary>
        public Transformer Transformer
        {
            get => (Transformer)base.GetValue(TransformerProperty);
            set => base.SetValue(TransformerProperty, value);
        }
        /// <summary> Identifies the <see cref = "TransformerMenu.Transformer" /> dependency property. </summary>
        public static readonly DependencyProperty TransformerProperty = DependencyProperty.Register(nameof(Transformer), typeof(Transformer), typeof(TransformerMenu), new PropertyMetadata(new Transformer(), (sender, e) =>
        {
            TransformerMenu control = (TransformerMenu)sender;

            if (e.NewValue is Transformer value)
            {
                control._vsTransformer = value;
                control.VisualState = control.VisualState;//State
            }
        }));

        #endregion


        //@VisualState
        bool _vsDisabledTool;
        Transformer _vsTransformer;
        ListViewSelectionMode _vsMode;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public ListViewReorderMode VisualState
        {
            get
            {
                if (this._vsDisabledTool) return ListViewReorderMode.Disabled;

                switch (this._vsMode)
                {
                    case ListViewSelectionMode.None: return ListViewReorderMode.Disabled;
                    case ListViewSelectionMode.Single:
                    case ListViewSelectionMode.Multiple:
                        return ListViewReorderMode.Enabled;
                }

                return ListViewReorderMode.Enabled;
            }
            set => this.SetTransformerState(value, this._vsTransformer);//asdsad
        }


        //TransformerMainPage
        private void SetTransformerState(ListViewReorderMode value, Transformer transformer)
        {
            switch (value)
            {
                case ListViewReorderMode.Enabled:
                    {
                        //Value
                        {
                            Vector2 horizontal = transformer.Horizontal;
                            Vector2 vertical = transformer.Vertical;

                            //Radians Skew
                            float radians = Transformer.GetRadians(horizontal);
                            float skew = Transformer.GetSkew(vertical, radians);
                            this.RPicker.Value = (int)radians;
                            this.SPicker.Value = (int)skew;

                            //@Release: case Debug
                            //Width Height
                            //float width = horizontal.Length();
                            //float height = vertical.Length();
                            //@Release: case Release
                            double width = Math.Sqrt(horizontal.X * horizontal.X + horizontal.Y * horizontal.Y);
                            double height = Math.Sqrt(vertical.X * vertical.X + vertical.Y * vertical.Y);
                            this.WPicker.Value = (int)width;
                            this.HPicker.Value = (int)height;

                            //X Y
                            Vector2 vector = transformer.GetIndicatorVector(this.IndicatorMode);
                            this.XPicker.Value = (int)vector.X;
                            this.YPicker.Value = (int)vector.Y;

                            //Indicator
                            this.IndicatorControl.Radians = radians;
                        }
                        //IsEnabled
                        {
                            this.RPicker.IsEnabled = true;
                            this.SPicker.IsEnabled = true;

                            this.WPicker.IsEnabled = true;
                            this.HPicker.IsEnabled = true;

                            this.XPicker.IsEnabled = true;
                            this.YPicker.IsEnabled = true;

                            this.RatioToggleControl.IsEnabled = true;
                            this.SnapToTickButton.IsEnabled = true;

                            this.IndicatorControl.Mode = this.IndicatorMode;
                            this.PositionRemoteButton.IsEnabled = true;
                        }
                    }
                    break;

                case ListViewReorderMode.Disabled:
                    {
                        //Value
                        {
                            //Radians Skew
                            this.RPicker.Value = 0;
                            this.SPicker.Value = 0;

                            //Width Height
                            this.WPicker.Value = 0;
                            this.HPicker.Value = 0;

                            //X Y
                            this.XPicker.Value = 0;
                            this.YPicker.Value = 0;

                            //Indicator
                            this.IndicatorControl.Radians = 0;
                        }
                        //IsEnabled
                        {
                            this.WPicker.IsEnabled = false;
                            this.HPicker.IsEnabled = false;

                            this.RPicker.IsEnabled = false;
                            this.SPicker.IsEnabled = false;

                            this.XPicker.IsEnabled = false;
                            this.YPicker.IsEnabled = false;

                            this.IndicatorControl.Mode = IndicatorMode.None;
                            this.PositionRemoteButton.IsEnabled = false;
                            this.RatioToggleControl.IsEnabled = false;
                            this.SnapToTickButton.IsEnabled = false;
                        }
                    }
                    break;

            }

        }


        //@Construct
        /// <summary>
        /// Initializes a TransformerMainPage. 
        /// </summary>
        public TransformerMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings(); 

            this.ConstructWidthHeight();
            this.ConstructRadianSkew();
            this.ConstructXY();

            this.ConstructPositionRemoteControl();
            this.ConstructIndicatorControl();


            base.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.SplitView.OpenPaneLength = e.NewSize.Width;
            };
            this.CloseButton.Click += (s, e) =>
            {
                this.SplitView.IsPaneOpen = true;
            };
        }

    }

    public sealed partial class TransformerMenu : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.WidthTextBlock.Text = resource.GetString("Menus_Transformer_Width");
            this.HeightTextBlock.Text = resource.GetString("Menus_Transformer_Height");
            this.RatioToolTip.Content = resource.GetString("Tools_More_Ratio");

            this.RotateTextBlock.Text = resource.GetString("Menus_Transformer_Rotate");
            this.SkewTextBlock.Text = resource.GetString("Menus_Transformer_Skew");
            this.SnapToTickToolTip.Content = resource.GetString("Tools_More_SnapToTick");

            this.XTextBlock.Text = resource.GetString("Menus_Transformer_X");
            this.YTextBlock.Text = resource.GetString("Menus_Transformer_Y");
            this.PositionRemoteToolTip.Content = resource.GetString("Menus_Transformer_PositionRemote");

            this.IndicatorToolTip.Content = resource.GetString("Menus_Transformer_Anchor");

            this.CloseButton.Content = resource.GetString("Menus_Close");
        }


        //RemoteControl
        private void ConstructPositionRemoteControl()
        {
            this.PositionRemoteControl.Background = this.RemoteBackground;
            this.PositionRemoteControl.BorderBrush = this.RemoteBorderBrush;
            this.PositionRemoteControl.Foreground = this.RemoteForeground;
            this.PositionRemoteButton.Click += (s, e) =>
            {
                this.SplitView.IsPaneOpen = false;
            };

            Vector2 remote(Vector2 value) =>
                (Math.Abs(value.X) > Math.Abs(value.Y)) ?
                new Vector2(value.X, 0) :
                new Vector2(0, value.Y);

            this.PositionRemoteControl.Moved += (s, value) => this.MethodViewModel.MethodTransformAdd(value);//Method
            this.PositionRemoteControl.ValueChangeStarted += (s, value) => this.MethodViewModel.MethodTransformAddStarted();//Method
            this.PositionRemoteControl.ValueChangeDelta += (s, value) => this.MethodViewModel.MethodTransformAddDelta(remote(value));//Method
            this.PositionRemoteControl.ValueChangeCompleted += (s, value) => this.MethodViewModel.MethodTransformAddComplete(remote(value));//Method
        }


        //IndicatorControl
        private void ConstructIndicatorControl()
        {
            this.IndicatorControl.ModeChanged += (s, mode) =>
            {
                this.IndicatorMode = mode;//IndicatorMode

                if (this.SelectionViewModel.SelectionMode == ListViewSelectionMode.None) return;

                Transformer transformer = this.SelectionTransformer;
                Vector2 vector = transformer.GetIndicatorVector(this.IndicatorMode);

                this.XPicker.Value = (int)vector.X;
                this.YPicker.Value = (int)vector.Y;
            };
        }


        //Width Height
        private void ConstructWidthHeight()
        {
            this.WPicker.Minimum = 1;
            this.WPicker.Maximum = int.MaxValue;
            this.WPicker.ValueChanged += (sender, value) =>
            {
                Transformer transformer = this.SelectionTransformer;
                Matrix3x2 matrix = transformer.TransformWidth(value, this.IndicatorMode, this.IsRatio);

                //Method
                this.MethodViewModel.MethodTransformMultiplies(matrix);
            };

            this.HPicker.Minimum = 1;
            this.HPicker.Maximum = int.MaxValue;
            this.HPicker.ValueChanged += (s, value) =>
            {
                Transformer transformer = this.SelectionTransformer;
                Matrix3x2 matrix = transformer.TransformHeight(value, this.IndicatorMode, this.IsRatio);

                //Method
                this.MethodViewModel.MethodTransformMultiplies(matrix);
            };
        }


        //Radian Skew
        private void ConstructRadianSkew()
        {
            this.RPicker.Minimum = -180;
            this.RPicker.Maximum = 180;
            this.RPicker.ValueChanged += (s, value) =>
            {
                Transformer transformer = this.SelectionTransformer;
                Matrix3x2 matrix = transformer.TransformRotate(value, this.IndicatorMode);

                //Method
                this.MethodViewModel.MethodTransformMultiplies(matrix);
            };

            this.SPicker.Minimum = -90;
            this.SPicker.Maximum = 90;
            this.SPicker.ValueChanged += (s, value) =>
            {
                Transformer transformer = this.SelectionTransformer;
                Matrix3x2 matrix = transformer.TransformSkew(value, this.IndicatorMode);

                //Method
                this.MethodViewModel.MethodTransformMultiplies(matrix);
            };
        }


        //X Y
        private void ConstructXY()
        {
            this.XPicker.Minimum = int.MinValue;
            this.XPicker.Maximum = int.MaxValue;
            this.XPicker.ValueChanged += (s, value) =>
            {
                Transformer transformer = this.SelectionTransformer;
                Vector2 vector = transformer.TransformX(value, this.IndicatorMode);

                //Method
                this.MethodViewModel.MethodTransformAdd(vector);
            };

            this.YPicker.Minimum = int.MinValue;
            this.YPicker.Maximum = int.MaxValue;
            this.YPicker.ValueChanged += (s, value) =>
            {
                Transformer transformer = this.SelectionTransformer;
                Vector2 vector = transformer.TransformY(value, this.IndicatorMode);

                //Method
                this.MethodViewModel.MethodTransformAdd(vector);
            };
        }

    }
}