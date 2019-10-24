using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.UI;

namespace Retouch_Photo2.Layers
{
    /// <summary> 
    /// <see cref = "Transformer" />'s manager. 
    /// </summary>
    public partial class TransformManager : ICacheTransform
    {
        
        //@Static
        /// <summary>
        /// Saves the entire TransformManager to a XElement.
        /// </summary>
        /// <returns> The saved XElement. </returns>
        public static XElement Save(TransformManager transformManager)
        {
            XElement element = new XElement("TransformManager");
            {
                //Source
                XElement source = new XElement("Source");
                TransformManager._save(source, transformManager.Source);
                element.Add(source);

                //Destination
                XElement destination = new XElement("Destination");
                TransformManager._save(destination, transformManager.Destination);
                element.Add(destination);

                //DisabledRadian
                element.Add(new XElement("DisabledRadian", transformManager.DisabledRadian));

                //IsCrop
                element.Add(new XElement("IsCrop", transformManager.IsCrop));

                //CropDestination
                XElement cropDestination = new XElement("CropDestination");
                TransformManager._save(cropDestination, transformManager.CropDestination);
                element.Add(cropDestination);
            }
            return element;
        }
        private static void _save(XElement element, Transformer transformer)
        {
            //LeftTop
            XElement leftTop = new XElement("LeftTop");
            leftTop.Add(new XElement("X", transformer.LeftTop.X));
            leftTop.Add(new XElement("Y", transformer.LeftTop.Y));
            element.Add(leftTop);

            //RightTop
            XElement rightTop = new XElement("RightTop");
            rightTop.Add(new XElement("X", transformer.RightTop.X));
            rightTop.Add(new XElement("Y", transformer.RightTop.Y));
            element.Add(rightTop);

            //RightBottom
            XElement rightBottom = new XElement("RightBottom");
            rightBottom.Add(new XElement("X", transformer.RightBottom.X));
            rightBottom.Add(new XElement("Y", transformer.RightBottom.Y));
            element.Add(rightBottom);

            //LeftBottom
            XElement leftBottom = new XElement("LeftBottom");
            leftBottom.Add(new XElement("X", transformer.LeftBottom.X));
            leftBottom.Add(new XElement("Y", transformer.LeftBottom.Y));
            element.Add(leftBottom);
        }


        /// <summary>
        ///  Loads a TransformManager from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded TransformManager. </returns>
        public static TransformManager Load(XElement element)
        {
            return new TransformManager
            {
                Source = TransformManager._load(element.Element("Source")),
                Destination = TransformManager._load(element.Element("Destination")),
                DisabledRadian = (bool)element.Element("DisabledRadian"),

                IsCrop = (bool)element.Element("IsCrop"),
                CropDestination = TransformManager._load(element.Element("CropDestination")),
            };
        }
        private static Transformer _load(XElement element)
        {
            XElement leftTop = element.Element("LeftTop");
            XElement rightTop = element.Element("RightTop");
            XElement rightBottom = element.Element("RightBottom");
            XElement leftBottom = element.Element("LeftBottom");

            return new Transformer
            {
                LeftTop = new Vector2
                {
                    X = (float)leftTop.Element("X"),
                    Y = (float)leftTop.Element("Y"),
                },
                RightTop = new Vector2
                {
                    X = (float)rightTop.Element("X"),
                    Y = (float)rightTop.Element("Y"),
                },
                RightBottom = new Vector2
                {
                    X = (float)rightBottom.Element("X"),
                    Y = (float)rightBottom.Element("Y"),
                },
                LeftBottom = new Vector2
                {
                    X = (float)leftBottom.Element("X"),
                    Y = (float)leftBottom.Element("Y"),
                },
            };
        }
                
    }
}