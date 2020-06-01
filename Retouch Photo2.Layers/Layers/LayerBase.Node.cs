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

        public NodeCollection Nodes { get; protected set; } = null;


        public abstract NodeCollection ConvertToCurves(ICanvasResourceCreator resourceCreator);

    }
}