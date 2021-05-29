// Core:              ★★★★★
// Referenced:   ★★★★
// Difficult:         ★★★★
// Only:              ★★★
// Complete:      ★★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
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
        bool IsSnapToTick => this.SettingViewModel.IsSnapToTick;


        TransformerMode TransformerMode = TransformerMode.None;


        public bool Started(Vector2 startingPoint, Vector2 point)
        {
            if (this.Mode == ListViewSelectionMode.None)
            {
                // Cursor
                CoreCursorExtension.IsManipulationStarted = false;
                CoreCursorExtension.SizeTranfrom(TransformerMode.None, 0);
                return false;
            }

            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            this.TransformerMode = Transformer.ContainsNodeMode(startingPoint, this.Transformer, matrix);

            if (this.TransformerMode == TransformerMode.None)
            {
                // Cursor
                CoreCursorExtension.IsManipulationStarted = false;
                CoreCursorExtension.SizeTranfrom(TransformerMode.None, 0);
                return false;
            }

            // Cursor
            Vector2 horizontal = this.Transformer.Horizontal;
            float angle = Transformer.GetRadians(horizontal);
            CoreCursorExtension.IsManipulationStarted = true;
            CoreCursorExtension.SizeTranfrom(this.TransformerMode, angle);

            // Snap
            if (this.IsSnap) this.ViewModel.VectorBorderSnapInitiate(this.SelectionViewModel.GetFirstSelectedLayerage());

            // Method
            this.MethodViewModel.MethodTransformMultipliesStarted();
            return true;
        }
        public bool Delta(Vector2 startingPoint, Vector2 point)
        {
            if (this.Mode == ListViewSelectionMode.None) return false;
            if (this.TransformerMode == TransformerMode.None) return false;

            if (this.TransformerMode == TransformerMode.Rotation)
            {
                // Cursor
                Vector2 horizontal = this.Transformer.Horizontal;
                float angle = Transformer.GetRadians(horizontal);
                CoreCursorExtension.IsManipulationStarted = true;
                CoreCursorExtension.SizeTranfrom(this.TransformerMode, angle);
            }

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            // Snap
            if (this.IsSnap && this.TransformerMode.IsScale()) canvasPoint = this.Snap.Snap(canvasPoint);

            // Selection
            /// Scaling <see cref="TextArtisticLayer"/> equally.
            bool isRatio = this.IsRatio || this.SelectionViewModel.LayerType == LayerType.TextArtistic;
            Transformer transformer = Transformer.Controller(this.TransformerMode, canvasStartingPoint, canvasPoint, this.SelectionViewModel.StartingTransformer, isRatio, this.IsCenter, this.IsSnapToTick);

            // Method
            this.MethodViewModel.MethodTransformMultipliesDelta(transformer);
            return true;
        }
        public bool Complete(Vector2 startingPoint, Vector2 point)
        {
            // Cursor
            Vector2 horizontal = this.Transformer.Horizontal;
            float angle = Transformer.GetRadians(horizontal);
            CoreCursorExtension.IsManipulationStarted = false;
            CoreCursorExtension.SizeTranfrom(this.TransformerMode, angle);

            if (this.Mode == ListViewSelectionMode.None) return false;
            if (this.TransformerMode == TransformerMode.None) return false;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            // Snap
            if (this.IsSnap)
            {
                if (this.TransformerMode.IsScale()) canvasPoint = this.Snap.Snap(canvasPoint);
                this.Snap.Default();
            }

            // Selection
            /// Scaling <see cref="TextArtisticLayer"/> equally.
            bool isRatio = this.IsRatio || this.SelectionViewModel.LayerType == LayerType.TextArtistic;
            Transformer transformer = Transformer.Controller(this.TransformerMode, canvasStartingPoint, canvasPoint, this.SelectionViewModel.StartingTransformer, isRatio, this.IsCenter, this.IsSnapToTick);

            // Method
            this.MethodViewModel.MethodTransformMultipliesComplete(transformer);
            this.TransformerMode = TransformerMode.None;// TransformerMode


            /// Gets font size for <see cref="TextArtisticLayer"/>.
            if (this.SelectionViewModel.LayerType == LayerType.TextArtistic)
            {
                // Selection
                this.SelectionViewModel.SetValueWithChildren((layerage) =>
                {
                    ILayer layer = layerage.Self;
                    if (layer.Type.IsText())
                    {
                        if (layer is TextArtisticLayer textArtisticLayer)
                        {
                            this.SelectionViewModel.FontSize = textArtisticLayer.FontSize;
                        }
                    }
                });
            }

            return true;
        }


        public void Draw(CanvasDrawingSession drawingSession)
        {
            if (this.Mode == ListViewSelectionMode.None) return;

            // Transformer
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            drawingSession.DrawBoundNodes(this.Transformer, matrix, this.ViewModel.AccentColor);

            // Snapping
            if (this.IsSnap)
            {
                this.Snap.Draw(drawingSession, matrix);
                this.Snap.DrawNode2(drawingSession, matrix);
            }
        }

    }
}