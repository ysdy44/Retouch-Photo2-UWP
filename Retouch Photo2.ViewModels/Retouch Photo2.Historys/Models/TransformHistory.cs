using Retouch_Photo2.Layers;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Blends;
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.UI;
using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Historys.Models;

namespace Retouch_Photo2.Historys.Models
{

    public class SourceHistory : IHistoryBase, IHistory
    {
        public HistoryType Type => HistoryType.Source;
        public void Add(Transform transform, Transformer previous, Transformer subsequent)
        {
            this.Undos.Push(() => transform.Source = previous);
            //this.Redos.Push(() => transform.Source = subsequent);
        }
    }

    public class DestinationHistory : IHistoryBase, IHistory
    {
        public HistoryType Type => HistoryType.Destination;
        public void Add(Transform transform, Transformer previous, Transformer subsequent)
        {
            this.Undos.Push(() => transform.Destination = previous);
            //this.Redos.Push(() => transform.Destination = subsequent);
        }
    }

    public class ICropHistory : IHistoryBase, IHistory
    {
        public HistoryType Type => HistoryType.Destination;
        public void Add(Transform transform, bool previous, bool subsequent)
        {
            this.Undos.Push(() => transform.IsCrop = previous);
            //this.Redos.Push(() => transform.IsCrop = subsequent);
        }
    }

}