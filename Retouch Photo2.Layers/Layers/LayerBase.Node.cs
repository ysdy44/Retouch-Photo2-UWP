using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Effects;
using Retouch_Photo2.Filters;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer that can have render properties. Provides a rendering method.
    /// </summary>
    public abstract partial class LayerBase
    {

        public abstract IEnumerable<IEnumerable<Node>> ConvertToCurves();


        public virtual void DrawNode(CanvasDrawingSession drawingSession, Matrix3x2 matrix, Windows.UI.Color accentColor) { }


        public virtual NodeCollectionMode ContainsNodeCollectionMode(Vector2 point, Matrix3x2 matrix) => NodeCollectionMode.None;


        public virtual void NodeCacheTransform() { }
        public virtual void NodeTransformMultiplies(Matrix3x2 matrix) { }
        public virtual void NodeTransformAdd(Vector2 vector) { }


        public virtual bool NodeSelectionOnlyOne(Vector2 point, Matrix3x2 matrix) => false;


        public virtual void NodeBoxChoose(TransformerRect boxRect) { }


        public virtual void NodeMovePoint(Vector2 point) { }
        public virtual void NodeControllerControlPoint(SelfControlPointMode mode, EachControlPointLengthMode lengthMode, EachControlPointAngleMode angleMode, Vector2 point, bool isLeftControlPoint) { }


        public virtual NodeRemoveMode NodeRemoveCheckedNodes() => NodeRemoveMode.None;
        public virtual void NodeInterpolationCheckedNodes() { }
        public virtual void NodeSharpCheckedNodes() { }
        public virtual void NodeSmoothCheckedNodes() { }

    }
}