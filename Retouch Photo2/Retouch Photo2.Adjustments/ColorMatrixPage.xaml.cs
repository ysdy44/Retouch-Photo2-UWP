// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              
// Complete:      ★★★
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Models;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "ColorMatrixAdjustment"/>.
    /// </summary>
    public sealed partial class ColorMatrixPage : IAdjustmentPage
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Content
        /// <summary> Gets the type. </summary>
        public AdjustmentType Type => AdjustmentType.ColorMatrix;
        /// <summary> Gets the icon. </summary>
        public ControlTemplate Icon => this.IconContentControl.Template;
        /// <summary> Gets the self. </summary>
        public FrameworkElement Self => this;
        /// <summary> Gets the text. </summary>
        public string Title => this.TextBlock.Text;

        /// <summary> Gets the adjustment index. </summary>
        public int Index { get; set; }

        private Matrix5x4 ColorMatrix
        {
            get => new Matrix5x4
            {
                // Red
                M11 = this.M11Picker.Value / 100.0f,
                M12 = this.M12Picker.Value / 100.0f,
                M13 = this.M13Picker.Value / 100.0f,
                M14 = this.M14Picker.Value / 100.0f,
                // Green
                M21 = this.M21Picker.Value / 100.0f,
                M22 = this.M22Picker.Value / 100.0f,
                M23 = this.M23Picker.Value / 100.0f,
                M24 = this.M24Picker.Value / 100.0f,
                // Blue
                M31 = this.M31Picker.Value / 100.0f,
                M32 = this.M32Picker.Value / 100.0f,
                M33 = this.M33Picker.Value / 100.0f,
                M34 = this.M34Picker.Value / 100.0f,
                // Alpha
                M41 = this.M41Picker.Value / 100.0f,
                M42 = this.M42Picker.Value / 100.0f,
                M43 = this.M43Picker.Value / 100.0f,
                M44 = this.M44Picker.Value / 100.0f,
                // Offset
                M51 = this.M51Picker.Value / 100.0f,
                M52 = this.M52Picker.Value / 100.0f,
                M53 = this.M53Picker.Value / 100.0f,
                M54 = this.M54Picker.Value / 100.0f,
            };
            set
            {
                // Red
                this.M11Picker.Value = (int)(value.M11 * 100.0f);
                this.M12Picker.Value = (int)(value.M12 * 100.0f);
                this.M13Picker.Value = (int)(value.M13 * 100.0f);
                this.M14Picker.Value = (int)(value.M14 * 100.0f);
                // Green
                this.M21Picker.Value = (int)(value.M21 * 100.0f);
                this.M22Picker.Value = (int)(value.M22 * 100.0f);
                this.M23Picker.Value = (int)(value.M23 * 100.0f);
                this.M24Picker.Value = (int)(value.M24 * 100.0f);
                // Blue
                this.M31Picker.Value = (int)(value.M31 * 100.0f);
                this.M32Picker.Value = (int)(value.M32 * 100.0f);
                this.M33Picker.Value = (int)(value.M33 * 100.0f);
                this.M34Picker.Value = (int)(value.M34 * 100.0f);
                // Alpha
                this.M41Picker.Value = (int)(value.M41 * 100.0f);
                this.M42Picker.Value = (int)(value.M42 * 100.0f);
                this.M43Picker.Value = (int)(value.M43 * 100.0f);
                this.M44Picker.Value = (int)(value.M44 * 100.0f);
                // Offset
                this.M51Picker.Value = (int)(value.M51 * 100.0f);
                this.M52Picker.Value = (int)(value.M52 * 100.0f);
                this.M53Picker.Value = (int)(value.M53 * 100.0f);
                this.M54Picker.Value = (int)(value.M54 * 100.0f);
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a ColorMatrixPage. 
        /// </summary>
        public ColorMatrixPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructRed();
            this.ConstructGreen();
            this.ConstructBlue();
            this.ConstructAlpha();
            this.ConstructOffset();
        }
    }

    public sealed partial class ColorMatrixPage : IAdjustmentPage
    {

        // Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.TextBlock.Text = resource.GetString("Adjustments_ColorMatrix");

            this.RedTextBlock.Text = this.ColorRedTextBlock.Text = resource.GetString("Adjustments_ColorMatrix_Red");
            this.GreenTextBlock.Text = this.ColorGreenTextBlock.Text = resource.GetString("Adjustments_ColorMatrix_Green");
            this.BlueTextBlock.Text = this.ColorBlueTextBlock.Text = resource.GetString("Adjustments_ColorMatrix_Blue");
            this.AlphaTextBlock.Text = this.ColorAlphaTextBlock.Text = resource.GetString("Adjustments_ColorMatrix_Alpha");
            this.OffsetTextBlock.Text = resource.GetString("Adjustments_ColorMatrix_Offset");
        }

        /// <summary>
        /// Reset the <see cref="IAdjustmentPage"/>'s data.
        /// </summary>
        public void Reset()
        {
            this.ColorMatrix = AdjustmentExtensions.One;

            this.MethodViewModel.TAdjustmentChanged<Matrix5x4, ColorMatrixAdjustment>
            (
                index: this.Index,
                set: (tAdjustment) => tAdjustment.ColorMatrix = AdjustmentExtensions.One,

                type: HistoryType.LayersProperty_ResetAdjustment_ColorMatrix,
                getUndo: (tAdjustment) => tAdjustment.ColorMatrix,
                setUndo: (tAdjustment, previous) => tAdjustment.ColorMatrix = previous
            );
        }
        /// <summary>
        /// <see cref="IAdjustmentPage"/>'s value follows the <see cref="IAdjustment"/>.
        /// </summary>
        public void Follow()
        {
            if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                if (layer.Filter.Adjustments[this.Index] is ColorMatrixAdjustment adjustment)
                {
                    this.ColorMatrix = adjustment.ColorMatrix;
                }
            }
        }

    }

    public sealed partial class ColorMatrixPage : IAdjustmentPage
    {

        public void ConstructRed()
        {
            this.M11Picker.Unit = "%";
            this.M11Picker.ValueChanged += (s, value) =>
            {
                Matrix5x4 colorMatrix = this.ColorMatrix;
                colorMatrix.M11 = value / 100f;
                this.TAdjustmentChanged(colorMatrix);
            };
            this.M12Picker.Unit = "%";
            this.M12Picker.ValueChanged += (s, value) =>
            {
                Matrix5x4 colorMatrix = this.ColorMatrix;
                colorMatrix.M12 = value / 100f;
                this.TAdjustmentChanged(colorMatrix);
            };
            this.M13Picker.Unit = "%";
            this.M13Picker.ValueChanged += (s, value) =>
            {
                Matrix5x4 colorMatrix = this.ColorMatrix;
                colorMatrix.M13 = value / 100f;
                this.TAdjustmentChanged(colorMatrix);
            };
            this.M14Picker.Unit = "%";
            this.M14Picker.ValueChanged += (s, value) =>
            {
                Matrix5x4 colorMatrix = this.ColorMatrix;
                colorMatrix.M14 = value / 100f;
                this.TAdjustmentChanged(colorMatrix);
            };
        }

        public void ConstructGreen()
        {
            this.M21Picker.Unit = "%";
            this.M21Picker.ValueChanged += (s, value) =>
            {
                Matrix5x4 colorMatrix = this.ColorMatrix;
                colorMatrix.M21 = value / 100f;
                this.TAdjustmentChanged(colorMatrix);
            };
            this.M22Picker.Unit = "%";
            this.M22Picker.ValueChanged += (s, value) =>
            {
                Matrix5x4 colorMatrix = this.ColorMatrix;
                colorMatrix.M22 = value / 100f;
                this.TAdjustmentChanged(colorMatrix);
            };
            this.M23Picker.Unit = "%";
            this.M23Picker.ValueChanged += (s, value) =>
            {
                Matrix5x4 colorMatrix = this.ColorMatrix;
                colorMatrix.M23 = value / 100f;
                this.TAdjustmentChanged(colorMatrix);
            };
            this.M24Picker.Unit = "%";
            this.M24Picker.ValueChanged += (s, value) =>
            {
                Matrix5x4 colorMatrix = this.ColorMatrix;
                colorMatrix.M24 = value / 100f;
                this.TAdjustmentChanged(colorMatrix);
            };
        }

        public void ConstructBlue()
        {
            this.M31Picker.Unit = "%";
            this.M31Picker.ValueChanged += (s, value) =>
            {
                Matrix5x4 colorMatrix = this.ColorMatrix;
                colorMatrix.M31 = value / 100f;
                this.TAdjustmentChanged(colorMatrix);
            };
            this.M32Picker.Unit = "%";
            this.M32Picker.ValueChanged += (s, value) =>
            {
                Matrix5x4 colorMatrix = this.ColorMatrix;
                colorMatrix.M32 = value / 100f;
                this.TAdjustmentChanged(colorMatrix);
            };
            this.M33Picker.Unit = "%";
            this.M33Picker.ValueChanged += (s, value) =>
            {
                Matrix5x4 colorMatrix = this.ColorMatrix;
                colorMatrix.M33 = value / 100f;
                this.TAdjustmentChanged(colorMatrix);
            };
            this.M34Picker.Unit = "%";
            this.M34Picker.ValueChanged += (s, value) =>
            {
                Matrix5x4 colorMatrix = this.ColorMatrix;
                colorMatrix.M34 = value / 100f;
                this.TAdjustmentChanged(colorMatrix);
            };
        }

        public void ConstructAlpha()
        {
            this.M41Picker.Unit = "%";
            this.M41Picker.ValueChanged += (s, value) =>
            {
                Matrix5x4 colorMatrix = this.ColorMatrix;
                colorMatrix.M41 = value / 100f;
                this.TAdjustmentChanged(colorMatrix);
            };
            this.M42Picker.Unit = "%";
            this.M42Picker.ValueChanged += (s, value) =>
            {
                Matrix5x4 colorMatrix = this.ColorMatrix;
                colorMatrix.M42 = value / 100f;
                this.TAdjustmentChanged(colorMatrix);
            };
            this.M43Picker.Unit = "%";
            this.M43Picker.ValueChanged += (s, value) =>
            {
                Matrix5x4 colorMatrix = this.ColorMatrix;
                colorMatrix.M43 = value / 100f;
                this.TAdjustmentChanged(colorMatrix);
            };
            this.M44Picker.Unit = "%";
            this.M44Picker.ValueChanged += (s, value) =>
            {
                Matrix5x4 colorMatrix = this.ColorMatrix;
                colorMatrix.M44 = value / 100f;
                this.TAdjustmentChanged(colorMatrix);
            };
        }

        private void ConstructOffset()
        {
            this.M51Picker.Unit = "%";
            this.M51Picker.ValueChanged += (s, value) =>
            {
                Matrix5x4 colorMatrix = this.ColorMatrix;
                colorMatrix.M51 = value / 100f;
                this.TAdjustmentChanged(colorMatrix);
            };
            this.M52Picker.Unit = "%";
            this.M52Picker.ValueChanged += (s, value) =>
            {
                Matrix5x4 colorMatrix = this.ColorMatrix;
                colorMatrix.M52 = value / 100f;
                this.TAdjustmentChanged(colorMatrix);
            };
            this.M53Picker.Unit = "%";
            this.M53Picker.ValueChanged += (s, value) =>
            {
                Matrix5x4 colorMatrix = this.ColorMatrix;
                colorMatrix.M53 = value / 100f;
                this.TAdjustmentChanged(colorMatrix);
            };
            this.M54Picker.Unit = "%";
            this.M54Picker.ValueChanged += (s, value) =>
            {
                Matrix5x4 colorMatrix = this.ColorMatrix;
                colorMatrix.M54 = value / 100f;
                this.TAdjustmentChanged(colorMatrix);
            };
        }

        private void TAdjustmentChanged(Matrix5x4 colorMatrix)
        {
            this.MethodViewModel.TAdjustmentChanged<Matrix5x4, ColorMatrixAdjustment>
            (
                index: this.Index,
                set: (tAdjustment) => tAdjustment.ColorMatrix = colorMatrix,

                type: HistoryType.LayersProperty_SetAdjustment_ColorMatrix_ColorMatrix,
                getUndo: (tAdjustment) => tAdjustment.ColorMatrix,
                setUndo: (tAdjustment, previous) => tAdjustment.ColorMatrix = previous
            );
        }

    }
}