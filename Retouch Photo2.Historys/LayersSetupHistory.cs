using FanKit.Transformers;
using System;

namespace Retouch_Photo2.Historys
{
    /// <summary>
    /// Represents a history used to change document serup.
    /// </summary>
    public class LayersSetupHistory : LayersTransformHistory
    {

        int Width = 1024;
        int Height = 1024;

        readonly Action SizeAction;


        //@Construct
        /// <summary>
        /// Initializes a LayersSetupHistory.
        /// </summary>
        /// <param name="title"> The title. </param>  
        /// <param name="canvasTransformer"> The canvas-transformer. </param>  
        public LayersSetupHistory(string title, CanvasTransformer canvasTransformer) : base(title)
        {
            this.Width = canvasTransformer.Width;
            this.Height = canvasTransformer.Height;

            this.SizeAction = () =>
            {
                canvasTransformer.Width = this.Width;
                canvasTransformer.Height = this.Height;
                canvasTransformer.ReloadMatrix();
            };
        }

        public override void Undo()
        {
            base.Undo();

            this.SizeAction?.Invoke();
        }

    }
}