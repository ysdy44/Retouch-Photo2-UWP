using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Layers;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Historys.Models
{

    public class OpacityHistory : IHistoryBase, IHistory
    {
        public HistoryType Type => HistoryType.Opacity;
        public void Add(ILayer layer, float previous, float subsequent)
        {
            this.Undos.Push(() => layer.Opacity = previous);
            //this.Redos.Push(() => layer.Opacity = subsequent);
        }
    }

    public class BlendModeHistory : IHistoryBase, IHistory
    {
        public HistoryType Type => HistoryType.BlendMode;
        public void Add(ILayer layer, BlendEffectMode? previous, BlendEffectMode? subsequent)
        {
            this.Undos.Push(() => layer.BlendMode = previous);
            //this.Redos.Push(() => layer.BlendMode = subsequent);
        }
    }

    public class VisibilityHistory : IHistoryBase, IHistory
    {
        public HistoryType Type => HistoryType.Visibility;
        public void Add(ILayer layer, Visibility previous, Visibility subsequent)
        {
            this.Undos.Push(() => layer.Visibility = previous);
            //this.Redos.Push(() => layer.Visibility = subsequent);
        }
    }

    public class TagTypeHistory : IHistoryBase, IHistory
    {
        public HistoryType Type => HistoryType.TagType;
        public void Add(ILayer layer, TagType previous, TagType subsequent)
        {
            this.Undos.Push(() => layer.TagType = previous);
            //this.Redos.Push(() => layer.TagType = subsequent);
        }
    }






    public class SelectModeHistory : IHistoryBase, IHistory
    {
        public HistoryType Type => HistoryType.SelectMode;
        public void Add(ILayer layer, SelectMode previous, SelectMode subsequent)
        {
            this.Undos.Push(() => layer.SelectMode = previous);
            //this.Redos.Push(() => layer.SelectMode = subsequent);
        }
    }


}