using FanKit.Transformers;
using Retouch_Photo2.Tools;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Keyboards;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using System;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// State of <see cref="TransformerControl"/>. 
    /// </summary>
    public enum TransformerControlState
    {
        /// <summary> Enabled. </summary>
        Enabled,
        /// <summary> Disabled radian. </summary>
        EnabledWithoutRadian,
        /// <summary> Disabled. </summary>
        Disabled
    }


    /// <summary> 
    /// Manager of <see cref="TransformerControlState"/>. 
    /// </summary>
    public class TransformerControlStateManager
    {
        /// <summary> <see cref = "TransformerControl.Tool" />/ </summary>
        public bool DisabledTool;
        /// <summary> <see cref = "TransformerControl.DisabledRadian" />/ </summary>
        public bool DisabledRadian;
        /// <summary> <see cref = "TransformerControl.Mode" />/ </summary>
        public ListViewSelectionMode Mode;

        /// <summary>
        /// Return status based on propertys.
        /// </summary>
        /// <returns> state </returns>
        public TransformerControlState GetState()
        {
            if (this.DisabledTool) return TransformerControlState.Disabled;

            switch (this.Mode)
            {
                case ListViewSelectionMode.None: return TransformerControlState.Disabled;
                case ListViewSelectionMode.Single:
                case ListViewSelectionMode.Multiple:
                    {
                        if (this.DisabledRadian)
                            return TransformerControlState.EnabledWithoutRadian;
                        else
                            return TransformerControlState.Enabled;
                    }
            }

            return TransformerControlState.Enabled;
        }
    }


    /// <summary>
    /// Retouch_Photo2's the only <see cref = "TransformerControl" />. 
    /// </summary>
    public sealed partial class TransformerControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        Transformer SelectionTransformer { get => this.SelectionViewModel.Transformer; set => this.SelectionViewModel.Transformer = value; }


        Transformer oldTransformer;
        IndicatorMode IndicatorMode = IndicatorMode.LeftTop;
        
        //RemoteOrIndicator
        private bool remoteOrIndicator;
        public bool RemoteOrIndicator
        {
            get => this.remoteOrIndicator;
            set
            {
                this.RemoteRootGrid.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                this.IndicatorRootGrid.Visibility = value ? Visibility.Collapsed : Visibility.Visible;

                this.remoteOrIndicator = value;
            }
        }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "TransformerControl" />'s tool. </summary>
        public ITool Tool
        {
            get { return (ITool)GetValue(ToolProperty); }
            set { SetValue(ToolProperty, value); }
        }
        /// <summary> Identifies the <see cref = "TransformerControl.Tool" /> dependency property. </summary>
        public static readonly DependencyProperty ToolProperty = DependencyProperty.Register(nameof(Tool), typeof(ITool), typeof(TransformerControl), new PropertyMetadata(null, (sender, e) =>
        {
            TransformerControl con = (TransformerControl)sender;

            if (e.NewValue is ITool value)
            {
                switch (value.Type)
                {
                    case ToolType.Cursor:
                    case ToolType.View:
                    case ToolType.Rectangle:
                    case ToolType.Ellipse:
                    case ToolType.Acrylic:
                        {
                            con.Manager.DisabledTool = false;
                            con.State = con.Manager.GetState();
                            return;
                        }
                }
            }

            con.Manager.DisabledTool = true;
            con.State = con.Manager.GetState();
            return;
        }));


        /// <summary> Gets or sets <see cref = "TransformerControl" />'s IsRatio. </summary>
        public bool IsRatio
        {
            get { return (bool)GetValue(IsRatioProperty); }
            set { SetValue(IsRatioProperty, value); }
        }
        /// <summary> Identifies the <see cref = "TransformerControl.IsRatio" /> dependency property. </summary>
        public static readonly DependencyProperty IsRatioProperty = DependencyProperty.Register(nameof(IsRatio), typeof(bool), typeof(TransformerControl), new PropertyMetadata(false));


        /// <summary> Gets or sets <see cref = "TransformerControl" />'s selection mode. </summary>
        public ListViewSelectionMode Mode
        {
            get { return (ListViewSelectionMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "TransformerControl.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(ListViewSelectionMode), typeof(TransformerControl), new PropertyMetadata(ListViewSelectionMode.None, (sender, e) =>
        {
            TransformerControl con = (TransformerControl)sender;

            if (e.NewValue is ListViewSelectionMode value)
            {
                con.Manager.Mode = value;
                con.State = con.Manager.GetState();
            }
        }));


        /// <summary> Gets or sets <see cref = "TransformerControl" />'s transformer. </summary>
        public Transformer Transformer
        {
            get { return (Transformer)GetValue(TransformerProperty); }
            set { SetValue(TransformerProperty, value); }
        }
        /// <summary> Identifies the <see cref = "TransformerControl.Transformer" /> dependency property. </summary>
        public static readonly DependencyProperty TransformerProperty = DependencyProperty.Register(nameof(Transformer), typeof(Transformer), typeof(TransformerControl), new PropertyMetadata(new Transformer(), (sender, e) =>
        {
            TransformerControl con = (TransformerControl)sender;

            con.State = con.Manager.GetState();
        }));



        public bool DisabledRadian
        {
            get { return (bool)GetValue(DisabledRadianProperty); }
            set { SetValue(DisabledRadianProperty, value); }
        }
        /// <summary> Identifies the <see cref = "TransformerControl.DisabledRadian" /> dependency property. </summary>
        public static readonly DependencyProperty DisabledRadianProperty = DependencyProperty.Register(nameof(DisabledRadian), typeof(bool), typeof(TransformerControl), new PropertyMetadata(false, (sender, e) =>
        {
            TransformerControl con = (TransformerControl)sender;

            if (e.NewValue is bool value)
            {
                con.Manager.DisabledRadian = value;
                con.State = con.Manager.GetState();
            }
        }));


        /// <summary> Gets or sets <see cref = "TransformerControl" />'s ToolTip IsOpen. </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        /// <summary> Identifies the <see cref = "TransformerControl.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(TransformerControl), new PropertyMetadata(false));


        #endregion


        /// <summary> Manager of <see cref="TransformerControlState"/>. </summary>
        TransformerControlStateManager Manager = new TransformerControlStateManager();
        /// <summary> State of <see cref="TransformerControl"/>. </summary>
        TransformerControlState State
        {
            set
            {
                switch (value)
                {
                    case TransformerControlState.Enabled:
                        {
                            //Value
                            {
                                Vector2 horizontal = this.Transformer.Horizontal;
                                Vector2 vertical = this.Transformer.Vertical;

                                //Radians
                                float radians = this.GetRadians(horizontal);
                                this.RPicker.Value = (int)radians;

                                //Skew
                                float skew = this.GetSkew(vertical, radians);
                                this.SPicker.Value = (int)skew;

                                //Width Height
                                this.WPicker.Value = (int)horizontal.Length();
                                this.HPicker.Value = (int)vertical.Length();

                                //X Y
                                Vector2 vector = this.GetVectorWithIndicatorMode(this.Transformer, this.IndicatorMode);
                                this.XPicker.Value = (int)vector.X;
                                this.YPicker.Value = (int)vector.Y;

                                //Indicator
                                this.IndicatorControl.Radians = radians;
                            }
                            //IsEnabled
                            {
                                this.WPicker.IsEnabled = true;
                                this.HPicker.IsEnabled = true;

                                this.RPicker.IsEnabled = true;
                                this.SPicker.IsEnabled = true;

                                this.XPicker.IsEnabled = true;
                                this.YPicker.IsEnabled = true;

                                this.RatioToggleControl.IsEnabled = true;//IsRatio
                                this.IndicatorControl.Mode = this.IndicatorMode;//IndicatorMode
                            }
                        }
                        break;

                    case TransformerControlState.EnabledWithoutRadian:
                        {
                            //Value
                            {
                                Vector2 horizontal = this.Transformer.Horizontal;
                                Vector2 vertical = this.Transformer.Vertical;

                                //Radians
                                this.RPicker.Value = 0;

                                //Skew
                                this.SPicker.Value = 0;

                                //Width Height
                                this.WPicker.Value = (int)horizontal.Length();
                                this.HPicker.Value = (int)vertical.Length();

                                //X Y
                                Vector2 vector = this.GetVectorWithIndicatorMode(this.Transformer, this.IndicatorMode);
                                this.XPicker.Value = (int)vector.X;
                                this.YPicker.Value = (int)vector.Y;

                                //Indicator
                                this.IndicatorControl.Radians = 0;
                            }
                            //IsEnabled
                            {
                                this.WPicker.IsEnabled = true;
                                this.HPicker.IsEnabled = true;

                                this.RPicker.IsEnabled = false;
                                this.SPicker.IsEnabled = false;

                                this.XPicker.IsEnabled = true;
                                this.YPicker.IsEnabled = true;

                                this.RatioToggleControl.IsEnabled = true;//IsRatio
                                this.IndicatorControl.Mode = this.IndicatorMode;//IndicatorMode
                            }
                        }
                        break;

                    case TransformerControlState.Disabled:
                        {
                            //Value
                            {
                                //Radians
                                this.RPicker.Value = 0;

                                //Skew
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

                                this.RatioToggleControl.IsEnabled = false;//IsRatio
                                this.IndicatorControl.Mode = IndicatorMode.None;//IndicatorMode
                            }
                        }
                        break;

                }
            }
        }


        //@Construct
        public TransformerControl()
        {
            this.InitializeComponent();

            #region Remote


            //RemoteOrIndicator
            this.RemoteOrIndicator = false;
            this.RemoteOrIndicatorButton.Tapped += (s, e) => this.RemoteOrIndicator = !this.RemoteOrIndicator;
                       
            //Remote
            this.RemoteControl.Moved += (s, value) =>
            {
                Transformer transformer = this.SelectionTransformer;
                Vector2 vector = value;

                //Selection
                this.SelectionTransformer = transformer + vector;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.CacheTransform();
                    layer.TransformAdd(vector);
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.RemoteControl.ValueChangeStarted += (s, value) =>
            {
                //Selection
                this.oldTransformer = this.SelectionTransformer;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.CacheTransform();
                }, true);

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.RemoteControl.ValueChangeDelta += (s, value) =>
            {
                Vector2 vector =
                   (Math.Abs(value.X) > Math.Abs(value.Y)) ?
                   new Vector2(value.X, 0) :
                   new Vector2(0, value.Y);

                //Selection
                this.SelectionTransformer = this.oldTransformer + vector;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TransformAdd(vector);
                }, true);

                this.ViewModel.Invalidate();//Invalidate
            };
            this.RemoteControl.ValueChangeCompleted += (s, value) => this.ViewModel.Invalidate(InvalidateMode.HD);


            #endregion


            #region Indicator

            this.IndicatorControl.ModeChanged += (s, mode) =>
            {
                {
                    IndicatorMode newMode = mode;
                    IndicatorMode oldMode = this.IndicatorMode;

                    HorizontalAlignment newHorizontal = this.GetHorizontalAlignmentFormIndicatorMode(newMode);
                    HorizontalAlignment oldHorizontal = this.GetHorizontalAlignmentFormIndicatorMode(oldMode);

                    VerticalAlignment newVertical = this.GetVerticalAlignmentFormIndicatorMode(newMode);
                    VerticalAlignment oldVertical = this.GetVerticalAlignmentFormIndicatorMode(oldMode);

                    if (newHorizontal != oldHorizontal) this.XEaseStoryboard.Begin();//Storyboard
                    if (newVertical != oldVertical) this.YEaseStoryboard.Begin();//Storyboard
                }

                this.IndicatorMode = mode;//IndicatorMode

                if (this.SelectionViewModel.Mode == ListViewSelectionMode.None) return;

                Transformer transformer = this.SelectionTransformer;
                Vector2 vector = this.GetVectorWithIndicatorMode(transformer, this.IndicatorMode);

                this.XPicker.Value = (int)vector.X;
                this.YPicker.Value = (int)vector.Y;
            };

            #endregion


            #region Width Height


            this.WPicker.Minimum = 1;
            this.WPicker.Maximum = int.MaxValue;
            this.WPicker.ValueChange += (sender, value) =>
            {
                Transformer transformer = this.SelectionTransformer;
                Vector2 horizontal = transformer.Horizontal;
                Vector2 vector = this.GetVectorWithIndicatorMode(transformer, this.IndicatorMode);

                float canvasStartingRadian = FanKit.Math.VectorToRadians(transformer.CenterTop - transformer.Center);
                float canvasStartingWidth = horizontal.Length();
                float scale = value / canvasStartingWidth;

                Matrix3x2 matrix =
                Matrix3x2.CreateRotation(-canvasStartingRadian, vector) *
                Matrix3x2.CreateScale(this.IsRatio ? scale : 1, scale, vector) *
                Matrix3x2.CreateRotation(canvasStartingRadian, vector);

                //Selection
                this.SelectionTransformer = transformer * matrix;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.CacheTransform();
                    layer.TransformMultiplies(matrix);
                });

                this.ViewModel.Invalidate();//Invalidate
            };


            this.HPicker.Minimum = 1;
            this.HPicker.Maximum = int.MaxValue;
            this.HPicker.ValueChange += (s, value) =>
            {
                Transformer transformer = this.SelectionTransformer;
                Vector2 vertical = transformer.Vertical;
                Vector2 vector = this.GetVectorWithIndicatorMode(transformer, this.IndicatorMode);

                float canvasStartingRadian = FanKit.Math.VectorToRadians(transformer.CenterTop - transformer.Center);
                float canvasStartingWidth = vertical.Length();
                float scale = value / canvasStartingWidth;

                Matrix3x2 matrix =
                Matrix3x2.CreateRotation(-canvasStartingRadian, vector) *
                Matrix3x2.CreateScale(scale, this.IsRatio ? scale : 1, vector) *
                Matrix3x2.CreateRotation(canvasStartingRadian, vector);

                //Selection
                this.SelectionTransformer = transformer * matrix;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.CacheTransform();
                    layer.TransformMultiplies(matrix);
                });

                this.ViewModel.Invalidate();//Invalidate
            };


            #endregion


            #region Radian Skew


            this.RPicker.Minimum = -180;
            this.RPicker.Maximum = 180;
            this.RPicker.ValueChange += (s, value) =>
            {
                Transformer transformer = this.SelectionTransformer;
                Vector2 vector = this.GetVectorWithIndicatorMode(transformer, this.IndicatorMode);

                float canvasRadian = value / 180.0f * FanKit.Math.Pi;
                float canvasStartingRadian = FanKit.Math.VectorToRadians(transformer.CenterTop - transformer.Center);

                float radian = canvasRadian - canvasStartingRadian - FanKit.Math.PiOver2;
                Matrix3x2 matrix = Matrix3x2.CreateRotation(radian, vector);

                //Selection
                this.SelectionTransformer = transformer * matrix;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.CacheTransform();
                    layer.TransformMultiplies(matrix);
                });

                this.ViewModel.Invalidate();
            };

            this.SPicker.Minimum = -90;
            this.SPicker.Maximum = 90;
            this.SPicker.ValueChange += (s, value) =>
            {
                Transformer transformer = this.SelectionTransformer;
                float horizontalHalf = Vector2.Distance(transformer.Center, transformer.CenterRight);

                Vector2 footPoint = FanKit.Math.FootPoint(transformer.Center, transformer.LeftBottom, transformer.RightBottom);
                float verticalHalf = Vector2.Distance(transformer.Center, footPoint);

                Vector2 horizontal = transformer.Horizontal;
                float radians = this.GetRadians(horizontal) / 180.0f * FanKit.Math.Pi;
                float skew = -value / 180.0f * FanKit.Math.Pi;

                //Vector2
                Vector2 postion;
                Vector2 center;
                switch (this.IndicatorMode)
                {
                    case IndicatorMode.LeftTop:
                    case IndicatorMode.Top:
                    case IndicatorMode.RightTop:
                        {
                            postion = new Vector2(-horizontalHalf, 0);
                            center = transformer.CenterTop;
                        }
                        break;
                    case IndicatorMode.LeftBottom:
                    case IndicatorMode.Bottom:
                    case IndicatorMode.RightBottom:
                        {
                            postion = new Vector2(-horizontalHalf, -verticalHalf * 2);
                            center = transformer.CenterBottom;
                        }
                        break;
                    default:
                        {
                            postion = new Vector2(-horizontalHalf, -verticalHalf);
                            center = transformer.Center;
                        }
                        break;
                }

                //Matrix
                Matrix3x2 matrix =
                Matrix3x2.CreateSkew(skew, 0) *
                Matrix3x2.CreateRotation(radians) *
                Matrix3x2.CreateTranslation(center);
                Transformer zeroTransformer = new Transformer(horizontalHalf * 2, verticalHalf * 2, postion);

                //Selection
                this.SelectionTransformer = zeroTransformer * matrix;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Destination = zeroTransformer * matrix;
                });

                this.ViewModel.Invalidate();
            };


            #endregion


            #region X Y


            this.XPicker.Minimum = int.MinValue;
            this.XPicker.Maximum = int.MaxValue;
            this.XPicker.ValueChange += (s, value) =>
            {
                Transformer transformer = this.SelectionTransformer;
                Vector2 indicator = this.GetVectorWithIndicatorMode(transformer, this.IndicatorMode);
                Vector2 vector = new Vector2(value - indicator.X, 0);

                //Selection
                this.SelectionTransformer = transformer + vector;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.CacheTransform();
                    layer.TransformAdd(vector);
                });

                this.ViewModel.Invalidate();
            };

            this.YPicker.Minimum = int.MinValue;
            this.YPicker.Maximum = int.MaxValue;
            this.YPicker.ValueChange += (s, value) =>
            {
                Transformer transformer = this.SelectionTransformer;
                Vector2 indicator = this.GetVectorWithIndicatorMode(transformer, this.IndicatorMode);
                Vector2 vector = new Vector2(0, value - indicator.Y);

                //Selection
                this.SelectionTransformer = transformer + vector;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.CacheTransform();
                    layer.TransformAdd(vector);
                });

                this.ViewModel.Invalidate();
            };


            #endregion
        }


        /// <summary>
        /// Gets vector by left, right, top, bottom.
        /// </summary>
        /// <param name="value"> Transformer </param>
        /// <param name="mode"> IndicatorMode </param>
        /// <returns></returns>
        private Vector2 GetVectorWithIndicatorMode(Transformer value, IndicatorMode mode)
        {
            switch (this.IndicatorMode)
            {
                case IndicatorMode.LeftTop: return value.LeftTop;
                case IndicatorMode.RightTop: return value.RightTop;
                case IndicatorMode.RightBottom: return value.RightBottom;
                case IndicatorMode.LeftBottom: return value.LeftBottom;

                case IndicatorMode.Left: return value.CenterLeft;
                case IndicatorMode.Top: return value.CenterTop;
                case IndicatorMode.Right: return value.CenterRight;
                case IndicatorMode.Bottom: return value.CenterBottom;

                case IndicatorMode.Center: return value.Center;
            }
            return value.LeftTop;
        }


        private float GetRadians(Vector2 vector)
        {
            float radians = FanKit.Math.VectorToRadians(vector);
            if (float.IsNaN(radians)) return 0.0f;

            float value = radians * 180.0f / FanKit.Math.Pi;
            return value % 180.0f;
        }

        private float GetSkew(Vector2 vector, float radians)
        {
            float skew = FanKit.Math.VectorToRadians(vector);
            if (float.IsNaN(skew)) return 0;

            skew = skew * 180.0f / FanKit.Math.Pi;
            skew = skew - radians - 90.0f;

            return skew % 180.0f;
        }


        //@Debug
        //Indicator
        private HorizontalAlignment GetHorizontalAlignmentFormIndicatorMode(IndicatorMode mode)
        {
            switch (mode)
            {
                case IndicatorMode.None:  return HorizontalAlignment.Center;
                case IndicatorMode.LeftTop: return HorizontalAlignment.Left;
                case IndicatorMode.RightTop: return HorizontalAlignment.Right;
                case IndicatorMode.RightBottom: return HorizontalAlignment.Right;
                case IndicatorMode.LeftBottom: return HorizontalAlignment.Left;
                case IndicatorMode.Left: return HorizontalAlignment.Left;
                case IndicatorMode.Top: return HorizontalAlignment.Center;
                case IndicatorMode.Right: return HorizontalAlignment.Right;
                case IndicatorMode.Bottom: return HorizontalAlignment.Center;
                case IndicatorMode.Center: return HorizontalAlignment.Center;
                default: return HorizontalAlignment.Center;
            }
        }
        private VerticalAlignment GetVerticalAlignmentFormIndicatorMode(IndicatorMode mode )
        {
            switch (mode)
            {
                case IndicatorMode.None: return VerticalAlignment.Center;
                case IndicatorMode.LeftTop: return VerticalAlignment.Top;
                case IndicatorMode.RightTop: return VerticalAlignment.Top;
                case IndicatorMode.RightBottom: return VerticalAlignment.Bottom;
                case IndicatorMode.LeftBottom: return VerticalAlignment.Bottom;
                case IndicatorMode.Left: return VerticalAlignment.Center;
                case IndicatorMode.Top: return VerticalAlignment.Top;
                case IndicatorMode.Right: return VerticalAlignment.Center;
                case IndicatorMode.Bottom: return VerticalAlignment.Bottom;
                case IndicatorMode.Center: return VerticalAlignment.Center;
                default: return VerticalAlignment.Center;
            }
        }
   

    }
}