using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Keyboards;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using System;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FanKit.Transformers;

namespace Retouch_Photo2.Controls
{
    public sealed partial class TransformerControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


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
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Transformer), typeof(ListViewSelectionMode), typeof(TransformerControl), new PropertyMetadata(ListViewSelectionMode.None, (sender, e) =>
        {
            TransformerControl con = (TransformerControl)sender;

            if (e.NewValue is ListViewSelectionMode value)
            {
                if (con.mode == value) return;

                con.mode = value;
                con.SetTransformer();
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

            if (e.NewValue is Transformer value)
            {
                con.transformer = value;
                con.SetTransformer();
            }
        }));



        public bool DisabledRadian
        {
            get { return (bool)GetValue(DisabledRadianProperty); }
            set { SetValue(DisabledRadianProperty, value); }
        }
        /// <summary> Identifies the <see cref = "TransformerControl.DisabledRadian" /> dependency property. </summary>
        public static readonly DependencyProperty DisabledRadianProperty = DependencyProperty.Register(nameof(Transformer), typeof(Transformer), typeof(TransformerControl), new PropertyMetadata(false, (sender, e) =>
        {
        TransformerControl con = (TransformerControl)sender;

            if (e.NewValue is bool value)
            {
                if (con.disabledRadian == value) return;

                con.disabledRadian = value;
                con.SetTransformer();
            }
        }));


        #endregion




        ListViewSelectionMode mode;
        Transformer transformer;
        bool disabledRadian;

        void SetTransformer()
        {
            switch (this.mode)
            {
                case ListViewSelectionMode.None:
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
                    break;

                case ListViewSelectionMode.Single:
                case ListViewSelectionMode.Multiple:
                    {
                        this.WPicker.IsEnabled = true;
                        this.HPicker.IsEnabled = true;

                        if (this.disabledRadian==false)
                        {
                            this.RPicker.IsEnabled = true;
                            this.SPicker.IsEnabled = true;
                        }
                        else
                        {
                            this.RPicker.IsEnabled = false;
                            this.SPicker.IsEnabled = false;
                        }

                        this.XPicker.IsEnabled = true;
                        this.YPicker.IsEnabled = true;

                        this.RatioToggleControl.IsEnabled = true;//IsRatio
                        this.IndicatorControl.Mode = this.IndicatorMode;//IndicatorMode
                    }
                    break;

            }



            if (disabledRadian == false)
            {
                Vector2 horizontal = this.transformer.Horizontal;
                Vector2 vertical = this.transformer.Vertical;

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
                Vector2 vector = this.GetVectorWithIndicatorMode(this.transformer, this.IndicatorMode);
                this.XPicker.Value = (int)vector.X;
                this.YPicker.Value = (int)vector.Y;

                //Indicator
                this.IndicatorControl.Radians = radians;
            }
            else
            {
                Vector2 horizontal = this.transformer.Horizontal;
                Vector2 vertical = this.transformer.Vertical;

                //Radians
                this.RPicker.Value = 0;

                //Skew
                this.SPicker.Value = 0;

                //Width Height
                this.WPicker.Value = (int)horizontal.Length();
                this.HPicker.Value = (int)vertical.Length();

                //X Y
                Vector2 vector = this.GetVectorWithIndicatorMode(this.transformer, this.IndicatorMode);
                this.XPicker.Value = (int)vector.X;
                this.YPicker.Value = (int)vector.Y;

                //Indicator
                this.IndicatorControl.Radians = 0;
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

            this.IndicatorControl.ModeChanged += (mode) =>
            {
                this.IndicatorMode = mode;//IndicatorMode

                if (this.SelectionViewModel.Mode == ListViewSelectionMode.None) return;

                Transformer transformer = this.SelectionViewModel.GetTransformer();
                Vector2 vector = this.GetVectorWithIndicatorMode(transformer, this.IndicatorMode);

                this.XPicker.Value = (int)vector.X;
                this.YPicker.Value = (int)vector.Y;
            };



            //Remote
            this.RemoteControl.Moved += (s, value) =>
            {
                //Transformer
                Transformer transformer = this.SelectionViewModel.GetTransformer();

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TransformerMatrix.OldDestination = layer.TransformerMatrix.Destination;
                    layer.TransformerMatrix.Destination = Transformer.Add(layer.TransformerMatrix.OldDestination, value);
                });
                this.SelectionViewModel.Transformer = Transformer.Add(transformer, value);

                this.ViewModel.Invalidate();//Invalidate
            };
            this.RemoteControl.ValueChangeStarted += (s, value) =>
            {
                //Transformer
                this.oldTransformer = this.SelectionViewModel.GetTransformer();

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TransformerMatrix.OldDestination = layer.TransformerMatrix.Destination;
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.RemoteControl.ValueChangeDelta += (s, value) =>
            {
                Vector2 vector =
                   (Math.Abs(value.X) > Math.Abs(value.Y)) ?
                   new Vector2(value.X, 0) :
                   new Vector2(0, value.Y);

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TransformerMatrix.Destination = Transformer.Add(layer.TransformerMatrix.OldDestination, vector);
                });
                this.SelectionViewModel.Transformer = Transformer.Add(this.oldTransformer, vector);

                this.ViewModel.Invalidate();//Invalidate
            };
            this.RemoteControl.ValueChangeCompleted += (s, value) => this.ViewModel.Invalidate(InvalidateMode.HD);


            #endregion


            #region Width Height


            this.WPicker.Minimum = 1;
            this.WPicker.Maximum = int.MaxValue;
            this.WPicker.ValueChange += (sender, value) =>
            {
                //Transformer
                Transformer transformer = this.SelectionViewModel.GetTransformer();
                Vector2 horizontal = transformer.Horizontal;
                Vector2 vector = this.GetVectorWithIndicatorMode(transformer, this.IndicatorMode);

                float canvasStartingRadian = TransformerMath.VectorToRadians(transformer.CenterTop - transformer.Center);
                float canvasStartingWidth = horizontal.Length();
                float scale = value/canvasStartingWidth;
                
                Matrix3x2 scaleMatrix =
                Matrix3x2.CreateRotation(-canvasStartingRadian, vector) *
                Matrix3x2.CreateScale(this.IsRatio ? scale : 1, scale, vector) *
                Matrix3x2.CreateRotation(canvasStartingRadian, vector);

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TransformerMatrix.OldDestination = layer.TransformerMatrix.Destination;
                    layer.TransformerMatrix.Destination = Transformer.Multiplies(layer.TransformerMatrix.OldDestination, scaleMatrix);
                });
                this.SelectionViewModel.Transformer = Transformer.Multiplies(transformer, scaleMatrix);

                this.ViewModel.Invalidate();//Invalidate
            };


            this.HPicker.Minimum = 1;
            this.HPicker.Maximum = int.MaxValue;
            this.HPicker.ValueChange += (s, value) =>
            {                
                //Transformer
                Transformer transformer = this.SelectionViewModel.GetTransformer();
                Vector2 vertical = transformer.Vertical;
                Vector2 vector = this.GetVectorWithIndicatorMode(transformer, this.IndicatorMode);

                float canvasStartingRadian = TransformerMath.VectorToRadians(transformer.CenterTop - transformer.Center);
                float canvasStartingWidth = vertical.Length();
                float scale = value / canvasStartingWidth;

                Matrix3x2 scaleMatrix =
                Matrix3x2.CreateRotation(-canvasStartingRadian, vector) *
                Matrix3x2.CreateScale(scale, this.IsRatio ? scale : 1, vector) *
                Matrix3x2.CreateRotation(canvasStartingRadian, vector);

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TransformerMatrix.OldDestination = layer.TransformerMatrix.Destination;
                    layer.TransformerMatrix.Destination = Transformer.Multiplies(layer.TransformerMatrix.OldDestination, scaleMatrix);
                });
                this.SelectionViewModel.Transformer = Transformer.Multiplies(transformer, scaleMatrix);

                this.ViewModel.Invalidate();//Invalidate
            };


            #endregion


            #region Radian Skew


            this.RPicker.Minimum = -180;
            this.RPicker.Maximum = 180;
            this.RPicker.ValueChange += (s, value) =>
            { 
                //Transformer
                Transformer transformer = this.SelectionViewModel.GetTransformer();
                Vector2 vector = this.GetVectorWithIndicatorMode(transformer, this.IndicatorMode);

                float canvasRadian = value / 180.0f * TransformerMath.Pi;              
                float canvasStartingRadian = TransformerMath.VectorToRadians(transformer.CenterTop - transformer.Center);

                float radian = canvasRadian - canvasStartingRadian - TransformerMath.PiOver2;
                Matrix3x2 rotationMatrix = Matrix3x2.CreateRotation(radian, vector);

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TransformerMatrix.OldDestination = layer.TransformerMatrix.Destination;
                    layer.TransformerMatrix.Destination = Transformer.Multiplies(layer.TransformerMatrix.OldDestination, rotationMatrix);
                });
                this.SelectionViewModel.Transformer = Transformer.Multiplies(transformer, rotationMatrix);

                this.ViewModel.Invalidate();
            };

            this.SPicker.Minimum = -90;
            this.SPicker.Maximum = 90;
            this.SPicker.ValueChange += (s, value) =>
            {
                //Transformer
                Transformer transformer = this.SelectionViewModel.GetTransformer();
                float horizontalHalf = Vector2.Distance(transformer.Center, transformer.CenterRight);

                Vector2 footPoint = TransformerMath.FootPoint(transformer.Center, transformer.LeftBottom, transformer.RightBottom);
                float verticalHalf = Vector2.Distance(transformer.Center, footPoint);

                Vector2 horizontal = transformer.Horizontal;
                float radians = this.GetRadians(horizontal) / 180.0f * TransformerMath.Pi;
                float skew = -value / 180.0f * TransformerMath.Pi;
                
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
                Matrix3x2 skewMatrix =
                Matrix3x2.CreateSkew(skew, 0) * 
                Matrix3x2.CreateRotation(radians) * 
                Matrix3x2.CreateTranslation(center);
                Transformer zeroTransformer = new Transformer(horizontalHalf * 2, verticalHalf * 2, postion);

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TransformerMatrix.OldDestination = layer.TransformerMatrix.Destination;
                    layer.TransformerMatrix.Destination=Transformer.Multiplies(zeroTransformer, skewMatrix);
                });
                this.SelectionViewModel.Transformer = Transformer.Multiplies(zeroTransformer, skewMatrix);

                this.ViewModel.Invalidate();
            };


            #endregion


            #region X Y


            this.XPicker.Minimum = int.MinValue;
            this.XPicker.Maximum = int.MaxValue;
            this.XPicker.ValueChange += (s, value) =>
            {
                //Transformer
                Transformer transformer = this.SelectionViewModel.GetTransformer();
                Vector2 vector = this.GetVectorWithIndicatorMode(transformer, this.IndicatorMode);
                Vector2 offset = new Vector2(value - vector.X, 0);

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TransformerMatrix.OldDestination = layer.TransformerMatrix.Destination;
                    layer.TransformerMatrix.Destination = Transformer.Add(layer.TransformerMatrix.OldDestination, offset);
                });
                this.SelectionViewModel.Transformer = Transformer.Add(transformer, offset);

                this.ViewModel.Invalidate();
            };

            this.YPicker.Minimum = int.MinValue;
            this.YPicker.Maximum = int.MaxValue;
            this.YPicker.ValueChange += (s, value) =>
            {
                //Transformer
                Transformer transformer = this.SelectionViewModel.GetTransformer();
                Vector2 vector = this.GetVectorWithIndicatorMode(transformer, this.IndicatorMode);
                Vector2 offset = new Vector2(0, value - vector.Y);

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TransformerMatrix.OldDestination = layer.TransformerMatrix.Destination;
                    layer.TransformerMatrix.Destination = Transformer.Add(layer.TransformerMatrix.OldDestination, offset);
                });
                this.SelectionViewModel.Transformer = Transformer.Add(transformer, offset);

                this.ViewModel.Invalidate();
            };


            #endregion
        }


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
            float radians = TransformerMath.VectorToRadians(vector);
            if (float.IsNaN(radians)) return 0.0f;

            float value = radians * 180.0f / TransformerMath.Pi;
            return value % 180.0f;
        }

        private float GetSkew(Vector2 vector, float radians)
        {
            float skew = TransformerMath.VectorToRadians(vector);
            if (float.IsNaN(skew)) return 0;

            skew = skew * 180.0f / TransformerMath.Pi;
            skew = skew - radians - 90.0f;

            return skew % 180.0f;
        }

    }
}