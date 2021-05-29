// Core:              ★★★★
// Referenced:   ★★★★★
// Difficult:         ★★★★★
// Only:              ★★★★
// Complete:      ★★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Effects;
using Retouch_Photo2.Filters;
using Retouch_Photo2.Styles;
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

        /// <summary> Gets or sets the control. </summary>
        LayerControl Control { get; }


        #region Instance


        /// <summary> Gets or sets <see cref="ILayer"/>'s id. </summary>
        string Id { get; set; }

        /// <summary>
        ///Returns a boolean indicating whether the given <see cref="Layerage"/> is equal to this <see cref="ILayer"/> instance.
        /// </summary>
        /// <param name="other"> The <see cref="Layerage"/> to compare this instance to. </param>
        /// <returns> True if the other <see cref="Layerage"/> is equal to this instance; False otherwise. </returns>
        bool Equals(Layerage other);
        /// <summary>
        ///Returns a boolean indicating whether the given <see cref="LayerBase"/> is equal to this <see cref="ILayer"/> instance.
        /// </summary>
        /// <param name="other"> The <see cref="LayerBase"/> to compare this instance to. </param>
        /// <returns> True if the other <see cref="LayerBase"/> is equal to this instance; False otherwise. </returns>
        bool Equals(LayerBase other);


        #endregion


        #region Property


        /// <summary> Gets the type. </summary>
        LayerType Type { get; }
        /// <summary> Gets or sets the name. </summary>
        string Name { get; set; } 
        /// <summary> Gets or sets the blend mode. </summary>
        BlendEffectMode? BlendMode { get; set; }
        
        /// <summary> Gets or sets the opacity. </summary>
        float Opacity { get; set; }
        /// <summary> The cache of <see cref="ILayer.Opacity"/>. </summary>
        float StartingOpacity { get; }
        /// <summary> Cache the <see cref="ILayer.Opacity"/>. </summary>
        void CacheOpacity();

        /// <summary> Gets or sets the visibility. </summary>
        Visibility Visibility { get; set; }
        /// <summary> Gets or sets the tag type. </summary>
        TagType TagType { get; set; }

        /// <summary> Gets or sets the expand. </summary>
        bool IsExpand { get; set; }
        /// <summary> Gets or sets the selected. </summary>
        bool IsSelected { get; set; }


        /// <summary> Gets or sets ILayer is need to refactoring transformer. </summary>
        bool IsRefactoringTransformer { get; set; }
        /// <summary>
        /// Gets the actually transformer.
        /// </summary>
        /// <param name="layerage"> The layerage. </param>
        Transformer GetActualTransformer(Layerage layerage);


        /// <summary> Gets or sets the style. </summary>
        IStyle Style { get; set; }
        /// <summary> Gets or sets the transform. </summary>
        Transform Transform { get; set; }
        /// <summary> Gets or sets the effect. </summary>
        Effect Effect { get; set; }
        /// <summary> Gets or sets the filter. </summary>
        Filter Filter { get; set; }


        /// <summary>
        /// Get own copy.
        /// </summary>
        /// <returns> The cloned <see cref="ILayer"/>. </returns>
        ILayer Clone();

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


        #region Render


        /// <summary> Gets or sets ILayer is need to refactoring render. </summary>
        bool IsRefactoringRender { get; set; }

        /// <summary> Gets or sets ILayer is need to refactoring icon render. </summary>
        bool IsRefactoringIconRender { get; set; }

        /// <summary>
        /// Gets a specific actual rended-layer (with icon render).
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="layerage"> The layerage. </param>
        /// <returns> The rendered layer. </returns>
        ICanvasImage GetActualRender(ICanvasResourceCreator resourceCreator, Layerage layerage);

        /// <summary>
        /// Gets a specific rended-layer.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="layerage"> The layerage. </param>
        /// <returns> The rendered layer. </returns>
        ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, Layerage layerage);


        /// <summary>
        /// Draw wireframe.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="accentColor"> The accent color. </param>
        void DrawWireframe(CanvasDrawingSession drawingSession, Matrix3x2 matrix, Windows.UI.Color accentColor);


        /// <summary>
        /// Create a specific geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns> The product geometry. </returns>   
        CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator);
        /// <summary>
        /// Create a specific geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product geometry. </returns>   
        CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix);


        /// <summary>
        ///Returns whether the area filled by the layer contains the specified point.
        /// </summary>
        /// <param name="layerage"> The layerage. </param>
        /// <param name="point"> The point. </param>
        /// <returns> If the fill contains points, return **True**. </returns>
        bool FillContainsPoint(Layerage layerage, Vector2 point);


        #endregion


        #region Node


        /// <summary> Gets or sets the nodes. </summary>
        NodeCollection Nodes { get; }

        /// <summary>
        ///  Convert to curves.
        /// </summary>
        /// <returns> The product curves. </returns>
        NodeCollection ConvertToCurves(ICanvasResourceCreator resourceCreator);


        #endregion

    }
}