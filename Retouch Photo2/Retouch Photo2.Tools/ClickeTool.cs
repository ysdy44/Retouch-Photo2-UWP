// Core:              ★★★★★
// Referenced:   ★★★★
// Difficult:         ★★★★
// Only:              ★★★
// Complete:      ★★★★★
using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// <see cref="IClickeTool"/>'s ClickeTool.
    /// </summary>
    public partial class ClickeTool : IClickeTool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        Transformer Transformer { get => this.SelectionViewModel.Transformer; set => this.SelectionViewModel.Transformer = value; }
        ListViewSelectionMode Mode => this.SelectionViewModel.SelectionMode;
        MarqueeCompositeMode CompositeMode => this.SettingViewModel.CompositeMode;


        public bool Clicke(Vector2 point)
        {
            //SelectedLayer
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);
            Layerage selectedLayerage = this.SelectionViewModel.GetClickSelectedLayerage(canvasPoint);

            if (selectedLayerage == null)
            {
                this.MethodViewModel.MethodSelectedNone();//Method
                return false;
            }


            switch (this.CompositeMode)
            {
                //Method
                case MarqueeCompositeMode.New: this.MethodViewModel.MethodSelectedNew(selectedLayerage); return true;
                case MarqueeCompositeMode.Add: this.MethodViewModel.MethodSelectedAdd(selectedLayerage); return true;
                case MarqueeCompositeMode.Subtract: this.MethodViewModel.MethodSelectedSubtract(selectedLayerage); return true;
                //case MarqueeCompositeMode.Intersect: this.MethodViewModel.MethodSelectedIntersect(selectedLayerage); return true;
                default: return false;
            }
        }


        public bool Cursor(Vector2 point)
        {
            if (this.Mode == ListViewSelectionMode.None) return this.TipViewModel.Cursor_PointerEntered_None();
            if (this.TipViewModel.tool_IsManipulationStarted) return true;
            if (this.TipViewModel.move_IsManipulationStarted) return true;
            if (this.TipViewModel.skew_IsManipulationStarted) return true;
            if (this.TipViewModel.scale_IsManipulationStarted) return true;


            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            TransformerMode mode = Transformer.ContainsNodeMode(point, this.Transformer, matrix);


            float radians = 90 + Transformer.GetRadians(point - Vector2.Transform(this.Transformer.Center, matrix));
            this.TipViewModel.pointer_Angle = radians;

            return this.TipViewModel.Cursor_PointerEntered_RotateSkewScale(mode);
        }

    }
}
