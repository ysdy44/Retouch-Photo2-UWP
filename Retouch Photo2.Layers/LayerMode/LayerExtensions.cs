namespace Retouch_Photo2.Layers
{
    public static class LayerExtensions
    {
        public static bool ToBool(this SelectMode selectMode)=> (selectMode == SelectMode.Selected || selectMode == SelectMode.ParentsSelected);
    }
}