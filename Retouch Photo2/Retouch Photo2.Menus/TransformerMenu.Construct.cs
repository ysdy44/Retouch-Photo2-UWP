using FanKit.Transformers;
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

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.WidthTextBlock.Text = resource.GetString("Menus_Transformer_Width");
            this.HeightTextBlock.Text = resource.GetString("Menus_Transformer_Height");
            this.RatioScalingToolTip.Content = resource.GetString("Menus_Transformer_RatioScaling");

            this.RotateTextBlock.Text = resource.GetString("Menus_Transformer_Rotate");
            this.SkewTextBlock.Text = resource.GetString("Menus_Transformer_Skew");
            this.StepFrequencyToolTip.Content = resource.GetString("Menus_Transformer_StepFrequency");

            this.XTextBlock.Text = resource.GetString("Menus_Transformer_X");
            this.YTextBlock.Text = resource.GetString("Menus_Transformer_Y");
            this.PositionRemoteToolTip.Content = resource.GetString("Menus_Transformer_PositionRemote");

            this.IndicatorToolTip.Content = resource.GetString("Menus_Transformer_Anchor");
        }


        //RemoteControl
        private void ConstructPositionRemoteControl()
        {
            this.PositionRemoteControl.Background = this.RemoteBackground;
            this.PositionRemoteControl.BorderBrush = this.RemoteBorderBrush;
            this.PositionRemoteControl.Foreground = this.RemoteForeground;
            this.PositionRemoteButton.Click += (s, e) =>
            {
                object title = this.PositionRemoteToolTip.Content;
                UIElement secondPage = this.PositionRemoteControl;
                this.SecondPageChanged?.Invoke(title, secondPage);//Delegate
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