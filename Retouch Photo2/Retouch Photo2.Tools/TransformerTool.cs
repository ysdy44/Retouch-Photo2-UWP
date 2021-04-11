// Core:              ★★★★★
// Referenced:   ★★★★
// Difficult:         ★★★★
// Only:              ★★★
// Complete:      ★★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// <see cref="ITransformerTool"/>'s TransformerTool.
    /// </summary>
    public partial class TransformerTool : ITransformerTool
    {        

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;        

        Transformer Transformer { get => this.SelectionViewModel.Transformer; set => this.SelectionViewModel.Transformer = value; }
        ListViewSelectionMode Mode => this.SelectionViewModel.SelectionMode;

        VectorBorderSnap Snap => this.ViewModel.VectorBorderSnap;
        bool IsSnap => this.SettingViewModel.IsSnap;
        bool IsRatio => this.SettingViewModel.IsRatio;
        bool IsCenter => this.SettingViewModel.IsCenter;
        bool IsStepFrequency => this.SettingViewModel.IsStepFrequency;


        TransformerMode TransformerMode = TransformerMode.None;
        

        public bool Started(Vector2 startingPoint, Vector2 point)
        {
            if (this.Mode == ListViewSelectionMode.None) return false;

            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            this.TransformerMode = Transformer.ContainsNodeMode(startingPoint, this.Transformer, matrix);
            //Cursor
            CoreCursorExtension.RotateSkewScale_ManipulationStarted(this.TransformerMode);
            if (this.TransformerMode == TransformerMode.None) return false;

            //Snap
            if (this.IsSnap) this.ViewModel.VectorBorderSnapInitiate(this.SelectionViewModel.GetFirstSelectedLayerage());

            //Method
            this.MethodViewModel.MethodTransformMultipliesStarted();
            return true;
        }
        public bool Delta(Vector2 startingPoint, Vector2 point)
        {
            if (this.Mode == ListViewSelectionMode.None) return false;
            if (this.TransformerMode == TransformerMode.None) return false;
            
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            //Snap
            if (this.IsSnap && this.TransformerMode.IsScale()) canvasPoint = this.Snap.Snap(canvasPoint);
            
            //Selection
            Transformer transformer = Transformer.Controller(this.TransformerMode, canvasStartingPoint, canvasPoint, this.SelectionViewModel.StartingTransformer, this.IsRatio, this.IsCenter, this.IsStepFrequency);
      
            //Method
            this.MethodViewModel.MethodTransformMultipliesDelta(transformer);
            return true;
        }
        public bool Complete(Vector2 startingPoint, Vector2 point)
        {
            if (this.Mode == ListViewSelectionMode.None) return false;
            if (this.TransformerMode == TransformerMode.None) return false;
            //Cursor
            CoreCursorExtension.None_ManipulationStarted();

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            //Snap
            if (this.IsSnap)
            {
                if (this.TransformerMode.IsScale()) canvasPoint = this.Snap.Snap(canvasPoint);
                this.Snap.Default();
            }

            //Selection
            Transformer transformer = Transformer.Controller(this.TransformerMode, canvasStartingPoint, canvasPoint, this.SelectionViewModel.StartingTransformer, this.IsRatio, this.IsCenter, this.IsStepFrequency);

            //Method
            this.MethodViewModel.MethodTransformMultipliesComplete(transformer);
            this.TransformerMode = TransformerMode.None;//TransformerMode
            return true;
        }


        public void Draw(CanvasDrawingSession drawingSession)
        {
            if (this.Mode == ListViewSelectionMode.None) return;

            //Transformer
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            drawingSession.DrawBoundNodes(this.Transformer, matrix, this.ViewModel.AccentColor);

            //Snapping
            if (this.IsSnap)
            {
                this.Snap.Draw(drawingSession, matrix);
                this.Snap.DrawNode2(drawingSession, matrix);
            }
        }

    }
}