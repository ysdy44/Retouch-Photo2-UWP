using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using System.ComponentModel;

namespace Retouch_Photo2.ViewModels
{
    public partial class ViewModel : INotifyPropertyChanged
    {


        /// <summary> Sets the PatternLayer. </summary>     
        private void SetPatternLayer(ILayer layer)
        {
            if (layer is null) return;

            switch (layer.Type)
            {
                case LayerType.PatternGrid:
                    PatternGridLayer gridLayer = (PatternGridLayer)layer;
                    this.PatternGrid_Type = gridLayer.GridType;
                    this.PatternGrid_HorizontalStep = gridLayer.HorizontalStep;
                    this.PatternGrid_VerticalStep = gridLayer.VerticalStep;
                    break;

                case LayerType.PatternDiagonal:
                    PatternDiagonalLayer diagonalLayer = (PatternDiagonalLayer)layer;
                    this.PatternDiagonal_Offset = diagonalLayer.Offset;
                    this.PatternDiagonal_HorizontalStep = diagonalLayer.HorizontalStep;
                    break;

                case LayerType.PatternSpotted:
                    PatternSpottedLayer spottedLayer = (PatternSpottedLayer)layer;
                    this.PatternSpotted_Radius = spottedLayer.Radius;
                    this.PatternSpotted_Step = spottedLayer.Step;
                    break;
            }
        }


        #region Pattern


        /// <summary> <see cref="PatternGridLayer"/>'s Type. </summary>     
        public PatternGridType PatternGrid_Type
        {
            get => this.patternGrid_Type;
            set
            {
                if (this.patternGrid_Type == value) return;
                this.patternGrid_Type = value;
                this.OnPropertyChanged(nameof(PatternGrid_Type)); // Notify 
            }
        }
        private PatternGridType patternGrid_Type = PatternGridType.Grid;

        /// <summary> <see cref="PatternGridLayer"/>'s HorizontalStep. </summary>     
        public float PatternGrid_HorizontalStep
        {
            get => this.patternGrid_HorizontalStep;
            set
            {
                if (this.patternGrid_HorizontalStep == value) return;
                this.patternGrid_HorizontalStep = value;
                this.OnPropertyChanged(nameof(PatternGrid_HorizontalStep)); // Notify 
            }
        }
        private float patternGrid_HorizontalStep = 30.0f;

        /// <summary> <see cref="PatternGridLayer"/>'s VerticalStep. </summary>     
        public float PatternGrid_VerticalStep
        {
            get => this.patternGrid_VerticalStep;
            set
            {
                if (this.patternGrid_VerticalStep == value) return;
                this.patternGrid_VerticalStep = value;
                this.OnPropertyChanged(nameof(PatternGrid_VerticalStep)); // Notify 
            }
        }
        private float patternGrid_VerticalStep = 30.0f;



        /// <summary> <see cref="PatternSpottedLayer"/>'s offset. </summary>     
        public float PatternDiagonal_Offset
        {
            get => this.patternDiagonal_Offset;
            set
            {
                if (this.patternDiagonal_Offset == value) return;
                this.patternDiagonal_Offset = value;
                this.OnPropertyChanged(nameof(PatternDiagonal_Offset)); // Notify 
            }
        }
        private float patternDiagonal_Offset = 30.0f;

        /// <summary> <see cref="PatternDiagonalLayer"/>'s HorizontalStep. </summary>     
        public float PatternDiagonal_HorizontalStep
        {
            get => this.patternDiagonal_HorizontalStep;
            set
            {
                if (this.patternDiagonal_HorizontalStep == value) return;
                this.patternDiagonal_HorizontalStep = value;
                this.OnPropertyChanged(nameof(PatternDiagonal_HorizontalStep)); // Notify 
            }
        }
        private float patternDiagonal_HorizontalStep = 30.0f;



        /// <summary> <see cref="PatternSpottedLayer"/>'s radius. </summary>     
        public float PatternSpotted_Radius
        {
            get => this.patternSpotted_Radius;
            set
            {
                if (this.patternSpotted_Radius == value) return;
                this.patternSpotted_Radius = value;
                this.OnPropertyChanged(nameof(PatternSpotted_Radius)); // Notify 
            }
        }
        private float patternSpotted_Radius = 8.0f;

        /// <summary> <see cref="PatternSpottedLayer"/>'s step. </summary>     
        public float PatternSpotted_Step
        {
            get => this.patternSpotted_Step;
            set
            {
                if (this.patternSpotted_Step == value) return;
                this.patternSpotted_Step = value;
                this.OnPropertyChanged(nameof(PatternSpotted_Step)); // Notify 
            }
        }
        private float patternSpotted_Step = 30.0f;


        #endregion

    }
}