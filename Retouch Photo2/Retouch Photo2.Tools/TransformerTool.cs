using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI;
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
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        
        Transformer Transformer { get => this.SelectionViewModel.Transformer; set => this.SelectionViewModel.Transformer = value; }
        ListViewSelectionMode Mode => this.SelectionViewModel.SelectionMode;

        VectorBorderSnap Snap => this.ViewModel.VectorBorderSnap;
        bool IsSnap => this.SettingViewModel.IsSnap;
        bool IsRatio => this.SettingViewModel.IsRatio;
        bool IsCenter => this.SettingViewModel.IsCenter;
        bool IsStepFrequency => this.SettingViewModel.IsStepFrequency;
        bool DisabledRadian => this.SelectionViewModel.DisabledRadian;


        Transformer StartingTransformer;
        TransformerMode TransformerMode = TransformerMode.None;
        
        public bool Started(Vector2 startingPoint, Vector2 point)
        {
            if (this.Mode == ListViewSelectionMode.None) return false;

            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            this.TransformerMode = Transformer.ContainsNodeMode(startingPoint, this.Transformer, matrix, this.DisabledRadian);
            if (this.TransformerMode == TransformerMode.None) return false;

            //Snap
            if (this.IsSnap) this.ViewModel.VectorBorderSnapStarted(this.SelectionViewModel.GetFirstLayer());

            //Selection
            this.StartingTransformer = this.Transformer;
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.CacheTransform();
            });
            
            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
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
            Transformer transformer = Transformer.Controller(this.TransformerMode, canvasStartingPoint, canvasPoint, this.StartingTransformer, this.IsRatio, this.IsCenter, this.IsStepFrequency);
            this.Transformer = transformer;
            Matrix3x2 matrix = Transformer.FindHomography(this.StartingTransformer, transformer);
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.TransformMultiplies(matrix);
            });
                    
            this.ViewModel.Invalidate();//Invalidate
            return true;
        }
        public bool Complete(Vector2 startingPoint, Vector2 point)
        {
            if (this.Mode == ListViewSelectionMode.None) return false;
            if (this.TransformerMode == TransformerMode.None) return false;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            //Snap
            if (this.IsSnap)
            {
                if (this.TransformerMode.IsScale()) canvasPoint = this.Snap.Snap(canvasPoint);
                this.Snap.Default();
            }

            //History
            IHistoryBase history = new IHistoryBase("Transform");

            //Selection
            Transformer transformer = Transformer.Controller(this.TransformerMode, canvasStartingPoint, canvasPoint, this.StartingTransformer, this.IsRatio, this.IsCenter, this.IsStepFrequency);
            this.Transformer = transformer;
            Matrix3x2 matrix = Transformer.FindHomography(this.StartingTransformer, transformer);
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.TransformMultiplies(matrix);

                //History
                var previous = layer.Transform.StartingDestination;
                history.Undos.Push(() => layer.Transform.Destination = previous);
            });

            //History
            this.ViewModel.Push(history);

            this.TransformerMode = TransformerMode.None;//TransformerMode
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            return true;
        }


        public void Draw(CanvasDrawingSession drawingSession)
        {
            if (this.Mode == ListViewSelectionMode.None) return;

            //Transformer
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            drawingSession.DrawBoundNodes(this.Transformer, matrix, this.ViewModel.AccentColor, this.SelectionViewModel.DisabledRadian);

            //Snapping
            if (this.IsSnap)
            {
                this.Snap.Draw(drawingSession, matrix);
                this.Snap.DrawNode2(drawingSession, matrix);
            }
        }

    }
}