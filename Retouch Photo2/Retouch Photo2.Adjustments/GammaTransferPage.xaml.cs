using Retouch_Photo2.Adjustments.Icons;
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
    public sealed partial class GammaTransferPage : IAdjustmentGenericPage<GammaTransferAdjustment>
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        //@Generic
        public GammaTransferAdjustment Adjustment { get; set; }

        //@Converter
        private string VisibilityToGlyphConverter(Visibility visibility) => visibility == Visibility.Visible ? "\xE011" : "\xE017";


        //@Construct
        public GammaTransferPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();


            this.ConstructAlphaDisable();
            this.ConstructAlphaOffset();
            this.ConstructAlphaExponent();
            this.ConstructAlphaAmplitude();

            this.ConstructRedDisable();
            this.ConstructRedOffset();
            this.ConstructRedExponent();
            this.ConstructRedAmplitude();

            this.ConstructGreenDisable();
            this.ConstructGreenOffset();
            this.ConstructGreenExponent();
            this.ConstructGreenAmplitude();

            this.ConstructBlueDisable();
            this.ConstructBlueOffset();
            this.ConstructBlueExponent();
            this.ConstructBlueAmplitude();
        }
    }

    /// <summary>
    /// Page of <see cref = "GammaTransferAdjustment"/>.
    /// </summary>
    public sealed partial class GammaTransferPage : IAdjustmentGenericPage<GammaTransferAdjustment>
    {

        //Strings
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/GammaTransfer");

            string offset = resource.GetString("/Adjustments/GammaTransfer_Offset");
            string exponent = resource.GetString("/Adjustments/GammaTransfer_Exponent");
            string amplitude = resource.GetString("/Adjustments/GammaTransfer_Amplitude");

            string alpha = resource.GetString("/Adjustments/GammaTransfer_Alpha");
            this.ConstructStringsAlpha(alpha, offset, exponent, amplitude);
            string red = resource.GetString("/Adjustments/GammaTransfer_Red");
            this.ConstructStringsRed(red, offset, exponent, amplitude);
            string green = resource.GetString("/Adjustments/GammaTransfer_Green");
            this.ConstructStringsGreen(green, offset, exponent, amplitude);
            string blue = resource.GetString("/Adjustments/GammaTransfer_Blue");
            this.ConstructStringsBlue(blue, offset, exponent, amplitude);
        }

        //@Content
        public AdjustmentType Type => AdjustmentType.GammaTransfer;
        public FrameworkElement Icon { get; } = new GammaTransferIcon();
        public FrameworkElement Self => this;
        public string Text { get; private set; }


        public IAdjustment GetNewAdjustment() => new GammaTransferAdjustment();


        public void Reset()
        {
            this.ResetAlpha();
            this.ResetRed();
            this.ResetGreen();
            this.ResetBlue();

            if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    //History
                    LayersPropertyHistory history = new LayersPropertyHistory("Set gamma transfer adjustment");

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
                    history.UndoActions.Push(() =>
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
                    });

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
        public void Follow(GammaTransferAdjustment adjustment)
        {
            this.FollowAlpha(adjustment);
            this.FollowRed(adjustment);
            this.FollowGreen(adjustment);
            this.FollowBlue(adjustment);
        }

    }
}