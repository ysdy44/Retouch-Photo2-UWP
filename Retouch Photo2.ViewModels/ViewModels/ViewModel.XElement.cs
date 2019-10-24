using Retouch_Photo2.Layers;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ViewModel" />.
    /// </summary>
    public partial class ViewModel
    {
        
        
        public XDocument XElementSave()
        {
            //Create an XDocument object.
            XDocument xDocument = new XDocument
            {
                //Set the document definition for xml.
                Declaration = new XDeclaration("1.0", "utf-8", "no"),
            };
            
            //Create a root
            XElement root = new XElement("Root");
            xDocument.Add(root);

            //3.WIdth and height.
            root.Add(new XElement("Width", this.CanvasTransformer.Width));
            root.Add(new XElement("Height", this.CanvasTransformer.Height));

            //Layers
            XElement layers = new XElement("Layers");
            root.Add(layers);
            {
                foreach (ILayer layer in this.Layers.RootLayers)
                {
                    XElement element = layer.Save();
                    layers.Add(element);
                }
            }
            
            return xDocument;
        }
        

        public void XElementLoad(XDocument xDocument)
        {
            //Create a root
            XElement root = xDocument.Elements("Root").Single();

            //WIdth and height.
            int width = (int)root.Elements("Width").Single();
            int height = (int)root.Elements("Height").Single();
            this.CanvasTransformer.Width = width;
            this.CanvasTransformer.Height = height;

            //Layers
            XElement rootLayer = root.Elements("Layers").Single();
            IEnumerable<XElement> layers = rootLayer.Elements();

            this.Layers.RootLayers.Clear();
            foreach (XElement element in layers)
            {
                ILayer layer = ILayerFactory.CreateILayer(element);

                if (layer!=null)
                {
                    this.Layers.RootLayers.Add(layer);
                }
            }

            this.Layers.ArrangeLayersControlsWithClearAndAdd();
            this.Layers.ArrangeLayersParents();            
        }

    }
}