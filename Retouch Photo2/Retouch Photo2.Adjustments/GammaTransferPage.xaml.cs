// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              
// Complete:      ★★★
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Adjustments.Models;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "GammaTransferAdjustment"/>.
    /// </summary>       
    public sealed partial class GammaTransferPage : IAdjustmentPage
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Converter
        private bool ReverseBoolConverter(bool value) => !value;


        //@Content
        /// <summary> Gets the type. </summary>
        public AdjustmentType Type => AdjustmentType.GammaTransfer;
        /// <summary> Gets the icon. </summary>
        public ControlTemplate Icon => this.IconContentControl.Template;
        /// <summary> Gets the self. </summary>
        public FrameworkElement Self => this;
        /// <summary> Gets the text. </summary>
        public string Title { get; private set; }

        /// <summary> Gets the adjustment index. </summary>
        public int Index { get; set; }


        //@Construct
        /// <summary>
        /// Initializes a GammaTransferPage. 
        /// </summary>
        public GammaTransferPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();


            //Alpha
            this.ConstructAlphaDisable();

            this.ConstructAlphaOffset1();
            this.ConstructAlphaOffset2();

            this.ConstructAlphaExponent1();
            this.ConstructAlphaExponent2();

            this.ConstructAlphaAmplitude1();
            this.ConstructAlphaAmplitude2();


            //Red
            this.ConstructRedDisable();

            this.ConstructRedOffset1();
            this.ConstructRedOffset2();

            this.ConstructRedExponent1();
            this.ConstructRedExponent2();

            this.ConstructRedAmplitude1();
            this.ConstructRedAmplitude2();

            
            //Green
            this.ConstructGreenDisable();

            this.ConstructGreenOffset1();
            this.ConstructGreenOffset2();

            this.ConstructGreenExponent1();
            this.ConstructGreenExponent2();

            this.ConstructGreenAmplitude1();
            this.ConstructGreenAmplitude2();


            //Blue
            this.ConstructBlueDisable();

            this.ConstructBlueOffset1();
            this.ConstructBlueOffset2();

            this.ConstructBlueExponent1();
            this.ConstructBlueExponent2();

            this.ConstructBlueAmplitude1();
            this.ConstructBlueAmplitude2();
        }
    }

    public sealed partial class GammaTransferPage : IAdjustmentPage
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Title = resource.GetString("Adjustments_GammaTransfer");

            string offset = resource.GetString("Adjustments_GammaTransfer_Offset");
            string exponent = resource.GetString("Adjustments_GammaTransfer_Exponent");
            string amplitude = resource.GetString("Adjustments_GammaTransfer_Amplitude");

            string alpha = resource.GetString("Adjustments_GammaTransfer_Alpha");
            this.ConstructStringsAlpha(alpha, offset, exponent, amplitude);
            string red = resource.GetString("Adjustments_GammaTransfer_Red");
            this.ConstructStringsRed(red, offset, exponent, amplitude);
            string green = resource.GetString("Adjustments_GammaTransfer_Green");
            this.ConstructStringsGreen(green, offset, exponent, amplitude);
            string blue = resource.GetString("Adjustments_GammaTransfer_Blue");
            this.ConstructStringsBlue(blue, offset, exponent, amplitude);
        }

        /// <summary>
        /// Reset the <see cref="IAdjustmentPage"/>'s data.
        /// </summary>
        public void Reset()
        {
            this.ResetAlpha();
            this.ResetRed();
            this.ResetGreen();
            this.ResetBlue();

            if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                if (layer.Filter.Adjustments[this.Index] is GammaTransferAdjustment adjustment)
                {
                    //History
                    LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_ResetAdjustment_GammaTransfer);

                    var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                    var previousAlpha1 = adjustment.AlphaDisable;
                    var previousAlpha2 = adjustment.AlphaOffset;
                    var previousAlpha3 = adjustment.AlphaExponent;
                    var previousAlpha4 = adjustment.AlphaAmplitude;

                    var previousRed1 = adjustment.RedDisable;
                    var previousRed2 = adjustment.RedOffset;
                    var previousRed3 = adjustment.RedExponent;
                    var previousRed4 = adjustment.RedAmplitude;

                    var previousGreen1 = adjustment.GreenDisable;
                    var previousGreen2 = adjustment.GreenOffset;
                    var previousGreen3 = adjustment.GreenExponent;
                    var previousGreen4 = adjustment.GreenAmplitude;

                    var previousBlue1 = adjustment.BlueDisable;
                    var previousBlue2 = adjustment.BlueOffset;
                    var previousBlue3 = adjustment.BlueExponent;
                    var previousBlue4 = adjustment.BlueAmplitude;
                    history.UndoAction += () =>
                    {
                        if (previous < 0) return;
                        if (previous > layer.Filter.Adjustments.Count - 1) return;
                        if (layer.Filter.Adjustments[previous] is GammaTransferAdjustment adjustment2)
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;

                            adjustment2.AlphaDisable = previousAlpha1;
                            adjustment2.AlphaOffset = previousAlpha2;
                            adjustment2.AlphaExponent = previousAlpha3;
                            adjustment2.AlphaAmplitude = previousAlpha4;

                            adjustment2.RedDisable = previousRed1;
                            adjustment2.RedOffset = previousRed2;
                            adjustment2.RedExponent = previousRed3;
                            adjustment2.RedAmplitude = previousRed4;

                            adjustment2.GreenDisable = previousGreen1;
                            adjustment2.GreenOffset = previousGreen2;
                            adjustment2.GreenExponent = previousGreen3;
                            adjustment2.GreenAmplitude = previousGreen4;

                            adjustment2.AlphaDisable = previousAlpha1;
                            adjustment2.AlphaOffset = previousAlpha2;
                            adjustment2.AlphaExponent = previousAlpha3;
                            adjustment2.AlphaAmplitude = previousAlpha4;
                        }
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();

                    adjustment.AlphaDisable = true;
                    adjustment.AlphaOffset = 0.0f;
                    adjustment.AlphaExponent = 1.0f;
                    adjustment.AlphaAmplitude = 1.0f;

                    adjustment.RedDisable = true;
                    adjustment.RedOffset = 0.0f;
                    adjustment.RedExponent = 1.0f;
                    adjustment.RedAmplitude = 1.0f;

                    adjustment.GreenDisable = true;
                    adjustment.GreenOffset = 0.0f;
                    adjustment.GreenExponent = 1.0f;
                    adjustment.GreenAmplitude = 1.0f;

                    adjustment.BlueDisable = true;
                    adjustment.BlueOffset = 0.0f;
                    adjustment.BlueExponent = 1.0f;
                    adjustment.BlueAmplitude = 1.0f;

                    //History
                    this.ViewModel.HistoryPush(history);

                    this.ViewModel.Invalidate();//Invalidate
                }
            }
        }
        /// <summary>
        /// <see cref="IAdjustmentPage"/>'s value follows the <see cref="IAdjustment"/>.
        /// </summary>
        public void Follow()
        {
            if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                if (layer.Filter.Adjustments[this.Index] is GammaTransferAdjustment adjustment)
                {
                    this.FollowAlpha(adjustment);
                    this.FollowRed(adjustment);
                    this.FollowGreen(adjustment);
                    this.FollowBlue(adjustment);
                }
            }
        }

    }
}