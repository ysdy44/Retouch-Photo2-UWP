using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo.Pickers
{
    /// <summary>
    /// Palette Base
    /// </summary>
    public abstract class PaletteBase
    {
        public string Name;
        public string Unit;
        public double Minimum;
        public double Maximum;

        public abstract HSL GetHSL(HSL HSL, double value);
        public abstract double GetValue(HSL HSL);

        public abstract GradientStopCollection GetSliderBrush(HSL HSL);

        public abstract void Draw(CanvasControl CanvasControl, CanvasDrawingSession ds, HSL HSL, Vector2 Center, float SquareHalfWidth, float SquareHalfHeight);
        public abstract HSL Delta(HSL HSL, Vector2 v, float SquareHalfWidth, float SquareHalfHeight);
    }
}
