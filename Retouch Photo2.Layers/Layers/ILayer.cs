using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Effects;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer that can have render properties.
    /// </summary>
    public interface ILayer : ICacheTransform
    {

        #region Instance


        /// <summary> Gets or sets <see cref="ILayer"/>'s id. </summary>
        string Id { get; set; }

        /// <summary>
        /// To <see cref="Layerage"/>.
        /// </summary>
        /// <returns> The producted layerage. </returns>
        Layerage ToLayerage();

        /// <summary>
        /// Returns a boolean indicating whether the given <see cref="Layerage"/> is equal to this <see cref="ILayer"/> instance.
        /// </summary>
        /// <param name="other"> The <see cref="Layerage"/> to compare this instance to. </param>
        /// <returns> True if the other <see cref="Layerage"/> is equal to this instance; False otherwise. </returns>
        bool Equals(Layerage other);
        /// <summary>
        /// Returns a boolean indicating whether the given <see cref="Layer"/> is equal to this <see cref="ILayer"/> instance.
        /// </summary>
        /// <param name="other"> The <see cref="Layer"/> to compare this instance to. </param>
        /// <returns> True if the other <see cref="Layer"/> is equal to this instance; False otherwise. </returns>
        bool Equals(Layer other);


        #endregion


        #region Property


        /// <summary> Gets ILayer's type. </summary>
        LayerType Type { get; }
        /// <summary> Gets or sets ILayer's name. </summary>
        string Name { get; set; } 
        /// <summary> Gets or sets ILayer's blend mode. </summary>
        BlendEffectMode? BlendMode { get; set; }
        
        /// <summary> Gets or sets ILayer's opacity. </summary>
        float Opacity { get; set; }
        /// <summary> The cache of <see cref="ILayer.Opacity"/>. </summary>
        float StartingOpacity { get; }
        /// <summary> Cache the <see cref="ILayer.Opacity"/>. </summary>
        void CacheOpacity();

        /// <summary> Gets or sets ILayer's visibility. </summary>
        Visibility Visibility { get; set; }
        /// <summary> Gets or sets ILayer's tag-type. </summary>
        TagType TagType { get; set; }

        /// <summary> Gets or sets ILayer is need to refactoring transformer. </summary>
        bool IsRefactoringTransformer { get; set; }
        /// <summary> Gets ILayer's actually transformer. </summary>
        Transformer GetActualDestinationWithRefactoringTransformer { get; }

        /// <summary> Gets or sets ILayer's style. </summary>
        Retouch_Photo2.Brushs.Style Style { get; set; }
        /// <summary> Gets or sets ILayer's transform. </summary>
        Transform Transform { get; set; }
        /// <summary> Gets or sets ILayer's effect. </summary>
        Effect Effect { get; set; }
        /// <summary> Gets or sets ILayer's filter. </summary>
        Filter Filter { get; set; }


        /// <summary>
        /// Get own copy.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns> The cloned ILayer. </returns>
        ILayer Clone(ICanvasResourceCreator resourceCreator);

        /// <summary>
        /// Saves the entire <see cref="ILayer"/> to a XElement.
        /// </summary>
        /// <param name="element"> The destination XElement. </param>
        void SaveWith(XElement element);
        /// <summary>
        /// Load the entire <see cref="ILayer"/> form a XElement.
        /// </summary>
        /// <param name="element"> The destination XElement. </param>
        void Load(XElement element);


        #endregion


        #region Control


        /// <summary> Gets or sets ILayer's control. </summary>
        LayerControl Control { get; }


        /// <summary> Gets or sets ILayer's expand. </summary>
        bool IsExpand { get; set; }

        /// <summary> Gets or sets ILayer's selected. </summary>
        bool IsSelected { get; set; }


        #endregion


        #region Render


        /// <summary>
        /// Gets a specific rended-layer.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="previousImage"> Previous rendered images. </param>
        /// <param name="canvasToVirtualMatrix"> The canvas-to-virtual matrix. </param>
        /// <param name="children"> The children layerage. </param>
        /// <returns> The rendered layer. </returns>
        ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix, IList<Layerage> children);

        /// <summary>
        /// Draw lines on bound.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="accentColor"> The accent color. </param>
        /// <param name="children"> The children layerage. </param>
        void DrawBound(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, Matrix3x2 matrix, IList<Layerage> children, Windows.UI.Color accentColor);


        /// <summary>
        /// Create a specific geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="canvasToVirtualMatrix"> The canvas-to-virtual matrix. </param>
        /// <returns> The product geometry. </returns>   
        CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix);
     
        /// <summary>
        ///  Convert to curves.
        /// </summary>
        /// <returns> The product curves. </returns>
        IEnumerable<IEnumerable<Node>> ConvertToCurves();


        /// <summary>
        /// Returns whether the area filled by the layer contains the specified point.
        /// </summary>
        bool FillContainsPoint(Vector2 point);


        #endregion

    }
}