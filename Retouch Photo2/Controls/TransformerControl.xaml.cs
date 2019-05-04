using Retouch_Photo2.Controls.TransformerControls;
using Retouch_Photo2.Models;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static Retouch_Photo2.Library.HomographyController;

namespace Retouch_Photo2.Controls
{
    public sealed partial class TransformerControl : UserControl
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;
        IndicatorMode Mode = IndicatorMode.LeftTop;
        bool IsRatio =false;
        
        //RemoteOrIndicator
        private bool remoteOrIndicator;
        public bool RemoteOrIndicator
        {
            get => this.remoteOrIndicator;
            set
            {
                if (value)
                {
                    this.RemoteRootGrid.Visibility = Visibility.Visible;
                    this.IndicatorRootGrid.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.RemoteRootGrid.Visibility = Visibility.Collapsed;
                    this.IndicatorRootGrid.Visibility = Visibility.Visible;
                }

                if (this.remoteOrIndicator==true&& value==false)
                {
                    if (this.ViewModel.CurrentLayer is Layer layer)
                    {
                        this.Transformer = layer.Transformer;
                    }
                }

                this.remoteOrIndicator = value;
            }
        }
        
        #region DependencyProperty


        public Transformer? Transformer
        {
            get { return (Transformer?)GetValue(TransformerProperty); }
            set { SetValue(TransformerProperty, value); }
        }
        public static readonly DependencyProperty TransformerProperty = DependencyProperty.Register(nameof(Transformer), typeof(Transformer?), typeof(TransformerControl), new PropertyMetadata(null, (sender, e) =>
        {
            TransformerControl con = (TransformerControl)sender;

            if (e.NewValue is Transformer  value)
            {
                Vector2 horizontal = value.DstRight - value.DstLeft;
                Vector2 vertical = value.DstBottom - value.DstTop;
                //Radians
                float radians = con.GetRadians(horizontal);
                con.RPicker.Value = (int)radians;
                //Skew
                float skew = con.GetSkew(vertical, radians);
                con.SPicker.Value = (int)skew;
                //Width Height
                con.WPicker.Value = (int)horizontal.Length();
                con.HPicker.Value = (int)vertical.Length();
                //X Y
                Vector2 vector = con.GetVectorWithIndicatorMode(value, con.Mode);
                con.XPicker.Value = (int)vector.X;
                con.YPicker.Value = (int)vector.Y;
                //Indicator
                con.IndicatorControl.Radians = radians;

                /////////////////////////////////////

                con.RPicker.IsEnabled = true;
                con.SPicker.IsEnabled = true;

                con.WPicker.IsEnabled = true;
                con.HPicker.IsEnabled = true;

                con.XPicker.IsEnabled = true;
                con.YPicker.IsEnabled = true;

                /////////////////////////////////////

                con.RatioToggleControl.IsChecked = con.IsRatio;//IsRatio
                con.IndicatorControl.Mode = con.Mode;//IndicatorMode
            }
            else
            {
                con.RPicker.Value = 0;
                con.SPicker.Value = 0;

                con.WPicker.Value = 0;
                con.HPicker.Value = 0;

                con.XPicker.Value = 0;
                con.YPicker.Value = 0;

                /////////////////////////////////////

                con.RPicker.IsEnabled = false;
                con.SPicker.IsEnabled = false;

                con.WPicker.IsEnabled = false;
                con.HPicker.IsEnabled = false;

                con.XPicker.IsEnabled = false;
                con.YPicker.IsEnabled = false;

                /////////////////////////////////////

                con.RatioToggleControl.IsChecked = false;//IsRatio
                con.IndicatorControl.Mode = IndicatorMode.None;//IndicatorMode
            }
        }));

        #endregion

        public TransformerControl()
        {
            this.InitializeComponent();
                    
            //RemoteOrIndicator
            this.RemoteOrIndicator = false;
            this.RemoteOrIndicatorButton.Tapped += (s, e) => this.RemoteOrIndicator = !this.RemoteOrIndicator;

            this.RatioToggleControl.CheckedChanged += (IsChecked) => this.IsRatio = IsChecked;//Ratio
            this.IndicatorControl.ModeChanged += (mode) =>
            {                
                this.Mode = mode;//IndicatorMode

                if (this.Transformer is Transformer transformer)
                {
                    Vector2 vector = this.GetVectorWithIndicatorMode(transformer, this.Mode);
                    this.XPicker.Value = (int)vector.X;
                    this.YPicker.Value = (int)vector.Y;
                }
            };

            ///////////////////////////////////////////////

            //Remote
            this.RemoteControl.Moved += (s, value) =>
            {
                if (this.ViewModel.CurrentLayer is Layer layer)
                {
                    Transformer transformer = layer.Transformer;
                    Library.HomographyController.Transformer.Add(layer, transformer, value);

                    this.ViewModel.Invalidate();
                }
            };
            this.RemoteControl.ValueChangeStarted += (s, value) =>
            {
                if (this.ViewModel.CurrentLayer is Layer layer)
                {
                    layer.OldTransformer = layer.Transformer;
                }
            };
            this.RemoteControl.ValueChangeDelta += (s, value) =>
            {
                if (this.ViewModel.CurrentLayer is Layer layer)
                {
                    Transformer transformer = layer.OldTransformer;

                    if (System.Math.Abs(value.X) > System.Math.Abs(value.Y))
                        Library.HomographyController.Transformer.Add(layer, transformer, new Vector2(value.X, 0));
                    else
                        Library.HomographyController.Transformer.Add(layer, transformer, new Vector2(0, value.Y));

                    this.ViewModel.Invalidate();
                }
            };
            this.RemoteControl.ValueChangeCompleted += (s, value) => this.ViewModel.Invalidate();

            ///////////////////////////////////////////////

            this.WPicker.Minimum = 1;
            this.WPicker.Maximum = int.MaxValue;
            this.WPicker.ValueChange += (sender, value) =>
            {
                if (this.Transformer is Transformer transformer)
                {
                    if (this.ViewModel.CurrentLayer is Layer layer)
                    {
                        //Scale
                        Vector2 horizontal = transformer.DstRight - transformer.DstLeft;
                        float scale = value / horizontal.Length();

                        if (this.IsRatio)//Ratio
                        {
                            Vector2 dstVector = this.GetVectorWithIndicatorMode(transformer, this.Mode);
                            Matrix3x2 matrix = Matrix3x2.CreateScale(scale, dstVector);
                            Library.HomographyController.Transformer.Multiplies(layer, transformer, matrix);
                        }
                        else
                        {
                            Vector2 scaleHorizontal = scale * horizontal;
                            this.SetWidth(layer, transformer, scaleHorizontal, this.Mode);
                        }

                        this.Transformer = layer.Transformer;
                        this.ViewModel.Invalidate();
                    }
                }
            };

            this.HPicker.Minimum = 1;
            this.HPicker.Maximum = int.MaxValue;
            this.HPicker.ValueChange += (s, value) =>
            {
                if (this.Transformer is Transformer transformer)
                {
                    if (this.ViewModel.CurrentLayer is Layer layer)
                    {
                        //Scale
                        Vector2 vertical = transformer.DstRight - transformer.DstLeft;
                        float scale = value / vertical.Length();

                        if (this.IsRatio)//Ratio
                        {
                            Vector2 dstVector = this.GetVectorWithIndicatorMode(transformer, this.Mode);
                            Matrix3x2 matrix = Matrix3x2.CreateScale(scale, dstVector);
                            Library.HomographyController.Transformer.Multiplies(layer, transformer, matrix);
                        }
                        else
                        {
                            Vector2 scaleVertical = scale * vertical;
                            this.SetHeight(layer, transformer, scaleVertical, this.Mode);
                        }

                        this.Transformer = layer.Transformer;
                        this.ViewModel.Invalidate();
                    }
                }
            };

            ///////////////////////////////////////////////

            this.RPicker.Minimum = -180;
            this.RPicker.Maximum = 180;
            this.RPicker.ValueChange += (sender, value) =>
            {
                if (this.Transformer is Transformer transformer)
                {
                    if (this.ViewModel.CurrentLayer is Layer layer)
                    {
                        layer.OldTransformer = layer.Transformer;
                        
                        Vector2 horizontal = layer.Transformer.DstRight - layer.Transformer.DstLeft;
                        float radians = this.GetRadians(horizontal);

                        float radian = (value - radians) / 180.0f * Retouch_Photo2.Library.HomographyController.Transformer.PI;
                        Vector2 vector = this.GetVectorWithIndicatorMode(layer.Transformer, this.Mode);

                        Matrix3x2 matrix = Matrix3x2.CreateRotation(radian, vector);
                        Retouch_Photo2.Library.HomographyController.Transformer.Multiplies(layer, layer.OldTransformer, matrix);

                        this.Transformer = layer.Transformer;
                        this.ViewModel.Invalidate();
                    }
                }
            };

            this.SPicker.Minimum = -90;
            this.SPicker.Maximum = 90;
            this.SPicker.ValueChange += (s, value) =>
            {
                if (this.Transformer is Transformer transformer)
                {
                    if (this.ViewModel.CurrentLayer is Layer layer)
                    {
                        float horizontalHalf = Vector2.Distance( layer.Transformer.DstCenter , layer.Transformer.DstRight);
                        float verticalHalf = Vector2.Distance
                        (                       
                             layer.Transformer.DstCenter,
                             Retouch_Photo2.Library.HomographyController.Transformer.FootPoint
                             (
                                  layer.Transformer.DstCenter,
                                  layer.Transformer.DstLeftBottom,
                                  layer.Transformer.DstRightBottom
                             )
                        );                       

                        float radians = this.GetRadians(layer.Transformer.DstRight - layer.Transformer.DstLeft)
                         / 180.0f * Retouch_Photo2.Library.HomographyController.Transformer.PI;
                        float skew = -value / 180.0f * Retouch_Photo2.Library.HomographyController.Transformer.PI;


                        //Vector2
                        Vector2 postion;
                        Vector2 center;
                        switch (this.Mode)
                        {
                            case IndicatorMode.LeftTop:
                            case IndicatorMode.Top:
                            case IndicatorMode.RightTop:
                                postion = new Vector2(-horizontalHalf, 0);
                                center = layer.Transformer.DstTop;
                                break;

                            case IndicatorMode.LeftBottom:
                            case IndicatorMode.Bottom:
                            case IndicatorMode.RightBottom:
                                postion = new Vector2(-horizontalHalf, -verticalHalf * 2);
                                center = layer.Transformer.DstBottom;
                                break;

                            default:
                                postion = new Vector2(-horizontalHalf, -verticalHalf);
                                center = layer.Transformer.DstCenter;
                                break;
                        }


                        //Transformer
                        layer.OldTransformer = Retouch_Photo2.Library.HomographyController.Transformer.CreateFromSize
                        (
                            width: horizontalHalf * 2,
                            height: verticalHalf*2,
                            postion: postion
                         );
                        

                        //Matrix
                        Matrix3x2 matrix = 
                        Matrix3x2.CreateSkew(skew, 0) *
                        Matrix3x2.CreateRotation(radians) *
                        Matrix3x2.CreateTranslation(center);
                        

                        Retouch_Photo2.Library.HomographyController.Transformer.Multiplies(layer, layer.OldTransformer, matrix);
                        
                        this.Transformer = layer.Transformer;
                        this.ViewModel.Invalidate();
                    }
                }
            };

            ///////////////////////////////////////////////

            this.XPicker.Minimum = int.MinValue;
            this.XPicker.Maximum = int.MaxValue;
            this.XPicker.ValueChange += (s, value) =>
            {
                if (this.Transformer is Transformer transformer)
                {
                    if (this.ViewModel.CurrentLayer is Layer layer)
                    {
                        Vector2 vector = this.GetVectorWithIndicatorMode(layer.Transformer, this.Mode);
                        Vector2 offset = new Vector2(-vector.X, 0);
                        Library.HomographyController.Transformer.Add(layer, transformer, offset);

                        this.Transformer = layer.Transformer;
                        this.ViewModel.Invalidate();
                    }
                }
            };

            this.YPicker.Minimum = int.MinValue;
            this.YPicker.Maximum = int.MaxValue;
            this.YPicker.ValueChange += (s, value) =>
            {
                if (this.Transformer is Transformer transformer)
                {
                    if (this.ViewModel.CurrentLayer is Layer layer)
                    {
                        Vector2 vector = this.GetVectorWithIndicatorMode(layer.Transformer, this.Mode);
                        Vector2 offset = new Vector2(0, -vector.Y);
                        Library.HomographyController.Transformer.Add(layer, transformer, offset);

                        this.Transformer = layer.Transformer;
                        this.ViewModel.Invalidate();
                    }
                }
            };

            ///////////////////////////////////////////////

            this.RPicker.Minimum = -180;
            this.RPicker.Maximum = 180;
            this.RPicker.ValueChange += (s, value) =>
            {

            };

            this.SPicker.Minimum = -180;
            this.SPicker.Maximum = 180;
            this.SPicker.ValueChange += (s, value) =>
            {

            };
        }

        
        private void SetWidth(Layer layer, Transformer transformer, Vector2 scaleHorizontal, IndicatorMode mode)
        {
            switch (mode)
        {
            case IndicatorMode.LeftTop:
            case IndicatorMode.Left:
            case IndicatorMode.LeftBottom:
                layer.Transformer.DstRightTop = transformer.DstLeftTop + scaleHorizontal;
                layer.Transformer.DstRightBottom = transformer.DstLeftBottom + scaleHorizontal;
                    break;

            case IndicatorMode.Top:
            case IndicatorMode.Center:
            case IndicatorMode.Bottom:
                Vector2 scaleHorizontalHalf = scaleHorizontal / 2;
                layer.Transformer.DstRightTop = transformer.DstTop + scaleHorizontalHalf;
                layer.Transformer.DstRightBottom = transformer.DstBottom + scaleHorizontalHalf;
                layer.Transformer.DstLeftTop = transformer.DstTop - scaleHorizontalHalf;
                layer.Transformer.DstLeftBottom = transformer.DstBottom - scaleHorizontalHalf;
                     break;

            case IndicatorMode.RightTop:
            case IndicatorMode.Right:
            case IndicatorMode.RightBottom:
                layer.Transformer.DstLeftTop = transformer.DstRightTop - scaleHorizontal;
                layer.Transformer.DstLeftBottom = transformer.DstRightBottom - scaleHorizontal;
                break;
            }
        }

        private void SetHeight(Layer layer, Transformer transformer, Vector2 scaleVertical, IndicatorMode mode)
        {
            switch (mode)
            {
                case IndicatorMode.LeftTop:
                case IndicatorMode.Top:
                case IndicatorMode.RightTop:
                    layer.Transformer.DstLeftBottom = transformer.DstLeftTop + scaleVertical;
                    layer.Transformer.DstRightBottom = transformer.DstRightTop + scaleVertical;
                    this.ViewModel.Invalidate();
                    break;

                case IndicatorMode.Left:
                case IndicatorMode.Center:
                case IndicatorMode.Right:
                    Vector2 scaleVerticalHalf = scaleVertical / 2;
                    layer.Transformer.DstLeftTop = transformer.DstLeft + scaleVerticalHalf;
                    layer.Transformer.DstRightTop = transformer.DstRight + scaleVerticalHalf;
                    layer.Transformer.DstLeftBottom = transformer.DstLeft - scaleVerticalHalf;
                    layer.Transformer.DstRightBottom = transformer.DstRight - scaleVerticalHalf;
                    this.ViewModel.Invalidate();
                    break;

                case IndicatorMode.LeftBottom:
                case IndicatorMode.Bottom:
                case IndicatorMode.RightBottom:
                    layer.Transformer.DstLeftTop = transformer.DstLeftBottom + scaleVertical;
                    layer.Transformer.DstRightTop = transformer.DstRightBottom + scaleVertical;
                    break;
            }
        } 
        

        private Vector2 GetVectorWithIndicatorMode(Transformer value, IndicatorMode mode)
        {
            switch (this.Mode)
            {
                case IndicatorMode.LeftTop: return value.DstLeftTop;
                case IndicatorMode.RightTop: return value.DstRightTop;
                case IndicatorMode.RightBottom: return value.DstRightBottom;
                case IndicatorMode.LeftBottom: return value.DstLeftBottom;

                case IndicatorMode.Left: return value.DstLeft;
                case IndicatorMode.Top: return value.DstTop;
                case IndicatorMode.Right: return value.DstRight;
                case IndicatorMode.Bottom: return value.DstBottom;

                case IndicatorMode.Center: return value.DstCenter;
            }
            return value.DstLeftTop;
        }


        private float GetRadians(Vector2 vector)
        {
            float radians = Library.HomographyController.Transformer.VectorToRadians(vector);
            if (float.IsNaN(radians)) return 0.0f;

            radians = radians * 180.0f / Library.HomographyController.Transformer.PI;

            return radians % 180.0f;
        }

        private float GetSkew(Vector2 vector, float radians) 
        {
            float skew = Library.HomographyController.Transformer.VectorToRadians(vector);
            if (float.IsNaN(skew)) return 0;

            skew = skew * 180.0f / Library.HomographyController.Transformer.PI;
            skew = skew - radians - 90.0f;

            return skew % 180.0f;
        }
                
    }
}
