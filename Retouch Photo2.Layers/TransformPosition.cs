using FanKit.Transformers;
using System.Numerics;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Temp:
    /// Transform position of the <see cref="ILayer.Style"/> and <see cref="ILayer.Transform"/>.
    /// </summary>
    public struct TransformPosition
    {

        Vector2 Style_Fill_Center;
        Vector2 Style_Fill_XPoint;
        Vector2 Style_Fill_YPoint;


        Vector2 Style_Stroke_Center;
        Vector2 Style_Stroke_XPoint;
        Vector2 Style_Stroke_YPoint;


        Transformer Transform_Transformer;
        Transformer Transform_CropTransformer;


        public static TransformPosition GetStartingLayer(ILayer layer)
        {
            return new TransformPosition
            {
                Style_Fill_Center = layer.Style.Fill.StartingCenter,
                Style_Fill_XPoint = layer.Style.Fill.StartingXPoint,
                Style_Fill_YPoint = layer.Style.Fill.StartingYPoint,

                Style_Stroke_Center = layer.Style.Stroke.StartingCenter,
                Style_Stroke_XPoint = layer.Style.Stroke.StartingXPoint,
                Style_Stroke_YPoint = layer.Style.Stroke.StartingYPoint,

                Transform_Transformer = layer.Transform.StartingTransformer,
                Transform_CropTransformer = layer.Transform.StartingCropTransformer,
            };
        }
        public static TransformPosition GetLayer(ILayer layer)
        {
            return new TransformPosition
            {
                Style_Fill_Center = layer.Style.Fill.Center,
                Style_Fill_XPoint = layer.Style.Fill.XPoint,
                Style_Fill_YPoint = layer.Style.Fill.YPoint,

                Style_Stroke_Center = layer.Style.Stroke.Center,
                Style_Stroke_XPoint = layer.Style.Stroke.XPoint,
                Style_Stroke_YPoint = layer.Style.Stroke.YPoint,

                Transform_Transformer = layer.Transform.Transformer,
                Transform_CropTransformer = layer.Transform.CropTransformer,
            };
        }
        public static void SetLayer(ILayer layer, TransformPosition position)
        {
            layer.Style.Fill.Center = position.Style_Fill_Center;
            layer.Style.Fill.XPoint = position.Style_Fill_XPoint;
            layer.Style.Fill.YPoint = position.Style_Fill_YPoint;

            layer.Style.Stroke.Center = position.Style_Stroke_Center;
            layer.Style.Stroke.XPoint = position.Style_Stroke_XPoint;
            layer.Style.Stroke.YPoint = position.Style_Stroke_YPoint;

            layer.Transform.Transformer = position.Transform_Transformer;
            layer.Transform.CropTransformer = position.Transform_CropTransformer;
        }
        
    }
}
