using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="LayerBase"/>'s GroupLayer .
    /// </summary>
    public class GroupLayer : LayerBase, ILayer
    {
        //@Static     
        public const string ID = "GroupLayer";

        //@Construct
        /// <summary>
        /// Construct a group-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public GroupLayer(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a group-layer.
        /// </summary>
        public GroupLayer()
        {
            base.Type = GroupLayer.ID;
            base.Control = new LayerControl(this)
            {
                Icon = new GroupIcon(),
                Text = "Group",
            };
        }
        

        public override Transformer GetActualDestinationWithRefactoringTransformer
        {
            get
            {
                if (this.IsRefactoringTransformer)
                {
                    Transformer transformer = LayerCollection.RefactoringTransformer(this.Children);
                    this.TransformManager.Source = transformer;
                    this.TransformManager.Destination = transformer;

                    this.IsRefactoringTransformer = false;
                }

                return base.GetActualDestinationWithRefactoringTransformer;
            }
        }
        
        public ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix)
        { 
            CanvasCommandList command = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession drawingSession = command.CreateDrawingSession())
            {
                foreach (ILayer child in this.Children)
                {
                    if (child.Visibility == Visibility.Collapsed) continue;
                    if (child.Opacity == 0) continue;

                    //GetRender
                    ICanvasImage currentImage = child.GetRender(resourceCreator, previousImage, canvasToVirtualMatrix);
                    drawingSession.DrawImage(currentImage);
                }
            }
            return command;
        }
        

        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GroupLayer groupLayer = new GroupLayer();

            LayerBase.CopyWith(resourceCreator, groupLayer, this);
            return groupLayer;
        }


        public XElement Save()
        {
            XElement element = new XElement("GroupLayer");

            LayerBase.SaveWidth(element, this);
            return element;
        }
        public void Load(XElement element)
        {
            LayerBase.LoadWith(element, this);
        }

    }
}