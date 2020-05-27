using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers.Icons;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="TextLayer"/>'s TextFrameLayer .
    /// </summary>
    public class TextFrameLayer : TextLayer, ITextLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.TextFrame;

        public override float FontSize { get; set; } = 22.0f;

        //@Construct
        /// <summary>
        /// Initializes a TextFrame-layer.
        /// </summary>
        public TextFrameLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new TextFrameIcon(),
                Type = this.ConstructStrings(),
            };
        }


        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {   
            TextFrameLayer textFrameLayer = new TextFrameLayer();

            TextLayer.FontCopyWith(textFrameLayer, this);
            LayerBase.CopyWith(resourceCreator, textFrameLayer, this);
            return textFrameLayer;
        }


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/TextFrame");
        }

    }
}