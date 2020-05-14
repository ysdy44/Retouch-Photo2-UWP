using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers.Icons;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="TextLayerBase"/>'s TextFrameLayer .
    /// </summary>
    public class TextFrameLayer : TextLayerBase, ILayer, ITextLayer
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
                Text = this.ConstructStrings(),
            };
        }


        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {   
            TextFrameLayer textFrameLayer = new TextFrameLayer();

            TextLayerBase.FontCopyWith(textFrameLayer, this);
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