using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Historys.Models;
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
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        Transformer Transformer { get => this.SelectionViewModel.Transformer; set => this.SelectionViewModel.Transformer = value; }
        ListViewSelectionMode Mode => this.SelectionViewModel.SelectionMode;
        MarqueeCompositeMode MarqueeCompositeMode => this.SettingViewModel.CompositeMode;
        bool IsRatio => this.SettingViewModel.IsRatio;
        bool IsCenter => this.SettingViewModel.IsCenter;
        bool IsStepFrequency => this.SettingViewModel.IsStepFrequency;
        bool DisabledRadian => this.SelectionViewModel.DisabledRadian;


        Transformer _startingTransformer;
        TransformerMode _transformerMode = TransformerMode.None;


        public bool Started(Vector2 startingPoint, Vector2 point)
        {
            if (this.Mode == ListViewSelectionMode.None) return false;
            this._transformerMode = this._getTransformerMode(startingPoint);
            if (this._transformerMode == TransformerMode.None) return false;

            //Selection
            this._startingTransformer = this.Transformer;
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
            if (this._transformerMode == TransformerMode.None) return false;
            
            //Transformer
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Transformer transformer = Transformer.Controller(this._transformerMode, startingPoint, point, this._startingTransformer, inverseMatrix, this.IsRatio, this.IsCenter, this.IsStepFrequency);
            this.Transformer = transformer;

            //Selection
            Matrix3x2 matrix = Transformer.FindHomography(this._startingTransformer, transformer);
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
            if (this._transformerMode == TransformerMode.None) return false;

            //History
            DestinationHistory history = new DestinationHistory();
            this.ViewModel.Push(history);

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                //History
                Transform m = layer.Transform;
                history.Add(m, m.StartingDestination, m.Destination);
            });

            this._transformerMode = TransformerMode.None;//TransformerMode
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate

            return true;
        }


        public void Draw(CanvasDrawingSession drawingSession)
        {
            if (this.Mode == ListViewSelectionMode.None) return;

            //Transformer
            drawingSession.DrawBoundNodes
            (
                transformer: this.Transformer,
                matrix: this.ViewModel.CanvasTransformer.GetMatrix(),
                accentColor: this.ViewModel.AccentColor,
                disabledRadian: this.SelectionViewModel.DisabledRadian
             );
        }


        public TransformerMode _getTransformerMode(Vector2 startingPoint)
        {
            switch (this.Mode)
            {
                case ListViewSelectionMode.None: break;
                case ListViewSelectionMode.Single:
                case ListViewSelectionMode.Multiple:
                    {
                        //Transformer
                        Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                        TransformerMode transformerMode = Transformer.ContainsNodeMode(startingPoint, this.Transformer, matrix, this.DisabledRadian);

                        if (transformerMode != TransformerMode.None) return transformerMode;
                    }
                    break;
            }


            //SelectedLayer
            ILayer selectedLayer = this._getSelectedLayer(startingPoint);

            if (selectedLayer == null)
            {
                this.ClickeNone();
                return TransformerMode.None;
            }
            else
            {
                this.ClickeNew(selectedLayer);
                return TransformerMode.Translation;
            }
        }

    }
}