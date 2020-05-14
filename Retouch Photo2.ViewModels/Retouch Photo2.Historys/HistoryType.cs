using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Retouch_Photo2.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Historys
{
    /// <summary> 
    /// Type of History.
    /// </summary>
    public enum HistoryType
    {


        /// <summary> <see cref="ILayer.Opacity"/>. </summary>
        Opacity,
        /// <summary> <see cref="ILayer.BlendMode"/>. </summary>
        BlendMode,

        /// <summary> <see cref="ILayer.Visibility"/>. </summary>
        Visibility,
        /// <summary> <see cref="ILayer.TagType"/>. </summary>
        TagType,
        

        Source,
        Destination,
        ICrop,


        SelectMode,



    }
}