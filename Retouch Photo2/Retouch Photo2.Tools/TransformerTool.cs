using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
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
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;

        Transformer Transformer { get => this.SelectionViewModel.Transformer; set => this.SelectionViewModel.Transformer = value; }
        ListViewSelectionMode Mode => this.SelectionViewModel.Mode;
        CompositeMode CompositeMode => this.KeyboardViewModel.CompositeMode;
        bool IsRatio => this.KeyboardViewModel.IsRatio;
        bool IsCenter => this.KeyboardViewModel.IsCenter;
        bool IsStepFrequency => this.KeyboardViewModel.IsStepFrequency;
         

        Transformer _oldTransformer;
        TransformerMode _transformerMode;
        

        public bool Starting(Vector2 point)
        {
            switch (this.Mode)
            {
                case ListViewSelectionMode.None:
                    {
                        this._transformerMode = TransformerMode.None;//TransformerMode
                        bool isSelect = this.SelectSingleLayer(point);
                        return isSelect;
                    }
                case ListViewSelectionMode.Single:
                case ListViewSelectionMode.Multiple:
                    {
                        //Transformer
                        Transformer transformer = this.Transformer;
                        Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                        bool dsabledRadian = this.SelectionViewModel.DsabledRadian;
                        TransformerMode transformerMode = Transformer.ContainsNodeMode
                        (
                             point,
                             transformer,
                             matrix,
                             dsabledRadian
                        );
                        this._transformerMode = transformerMode;

                        return this.StartingFromTransformerMode(point,transformerMode);
                    }
            }

            return true;
        }
        public bool Started(Vector2 startingPoint, Vector2 point, bool isSetTransformerMode = true)
        {
            if (this.Mode == ListViewSelectionMode.None) return false;

            this._oldTransformer = this.Transformer;

            if (isSetTransformerMode)
            {
                //Transformer
                Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                bool dsabledRadian = this.SelectionViewModel.DsabledRadian;
                TransformerMode transformerMode = Transformer.ContainsNodeMode
                (
                    startingPoint,
                    this._oldTransformer,
                    matrix,
                    dsabledRadian
                );
                this._transformerMode = transformerMode;

                if (transformerMode == TransformerMode.None) return false;
            }

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.CacheTransform();
            }, true);

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            return true;
        }
        public bool Delta(Vector2 startingPoint, Vector2 point)
        {
            if (this.Mode == ListViewSelectionMode.None) return false;
            if (this._transformerMode == TransformerMode.None) return false;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();

            //Transformer
            Transformer transformer = Transformer.Controller
            (
                this._transformerMode, 
                startingPoint, 
                point, 
                this._oldTransformer,
                inverseMatrix,
                this.IsRatio, 
                this.IsCenter, 
                this.IsStepFrequency
            );
            this.Transformer = transformer;

            Matrix3x2 matrix = Transformer.FindHomography(this._oldTransformer, transformer);

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.TransformMultiplies(matrix);
            }, true);

            this.ViewModel.Invalidate();//Invalidate
            return true;
        }
        public bool Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            switch (this.Mode)
            {
                case ListViewSelectionMode.None: return false;
                case ListViewSelectionMode.Single:
                case ListViewSelectionMode.Multiple:
                    {
                        switch (this.CompositeMode)
                        {
                            case CompositeMode.New:
                                {
                                    if (this._transformerMode == TransformerMode.None)
                                    {
                                        //Selection
                                        this.SelectionViewModel.SetValue((layer) =>
                                        {
                                            layer.IsChecked = false;
                                        });
                                        this.SelectionViewModel.SetModeNone();//Selection

                                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                                        return false;
                                    }
                                }
                                break;
                            case CompositeMode.Add: break;
                            case CompositeMode.Subtract: break;
                            case CompositeMode.Intersect: break;
                        }
                    }
                    break;
            }

            this._transformerMode = TransformerMode.None;//TransformerMode
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate

            return true;
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            //Selection
            switch (this.Mode)
            {
                case ListViewSelectionMode.None: break;
                case ListViewSelectionMode.Single:
                case ListViewSelectionMode.Multiple:
                    {
                        //Transformer
                        Transformer transformer = this.Transformer;
                        Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                        Color accentColor = this.ViewModel.AccentColor;
                        bool dsabledRadian = this.SelectionViewModel.DsabledRadian;
                        drawingSession.DrawBoundNodes
                        (
                            transformer, 
                            matrix, 
                            accentColor, 
                            dsabledRadian
                         );
                    }
                    break;
            }
        }
    }
}