// Core:              ★★★★
// Referenced:   ★★
// Difficult:         ★★
// Only:              ★★★★
// Complete:      ★★
using Microsoft.Graphics.Canvas;
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
        /// <param name="customDevice"> The custom-device. </param>
        public TextFrameLayer(CanvasDevice customDevice)
        {
            base.Control = new LayerControl(customDevice, this)
            {
                Type = this.ConstructStrings(),
            };
        }


        public override ILayer Clone(CanvasDevice customDevice)
        {   
            TextFrameLayer textFrameLayer = new TextFrameLayer(customDevice);

            TextLayer.FontCopyWith(textFrameLayer, this);
            LayerBase.CopyWith(customDevice, textFrameLayer, this);
            return textFrameLayer;
        }


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("Layers_TextFrame");
        }

    }
}