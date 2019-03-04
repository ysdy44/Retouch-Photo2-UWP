using System;
using System.Linq;
using System.Numerics;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;

namespace Retouch_Photo.Library
{
    public static class Adjustmentaaaaaaaaaaa
    {

        //羽化方法
        public static ICanvasImage GetFeather(ICanvasImage image, float Feather)
        {
            if (Feather < 0) Feather = 0;

            return new GaussianBlurEffect//高斯模糊
            {
                Source = image,
                BlurAmount = Feather,
            };
        }

        //边界算法
        public static ICanvasImage GetBorder(ICanvasImage image, int Border)
        {
            if (Border == 0) return image;
            else if (Border >= 90) Border = 90;
            else if (Border <= -90) Border = -90;

            return new MorphologyEffect//形态学
            {
                Source = new BorderEffect { Source = image },//边界效应
                Mode = (Border > 0) ? MorphologyEffectMode.Dilate : MorphologyEffectMode.Erode,//扩张&侵蚀
                Height = Math.Abs(Border),
                Width = Math.Abs(Border)//地区扩张
            };
        }





        //由魔棒得到区域
        public static ICanvasImage GetMagicWandr(CanvasRenderTarget SourceBitmap, int x, int y, float dragDistance)
        {
            Color clickColor = SourceBitmap.GetPixelColors(x, y, 1, 1).Single();

            return new ColorMatrixEffect
            {
                //ChromaKeyEffect：浓度帧作用
                //（替换指定的颜色和透明度）
                Source = new ChromaKeyEffect
                {
                    Source = SourceBitmap,
                    Color = clickColor,//指定的颜色
                    Tolerance = dragDistance,//容差
                    InvertAlpha = true//反转α
                },

                ColorMatrix = new Matrix5x4
                {
                    // 保持alpha
                    M44 = 1,

                    // 设置RGB =白色。
                    M51 = 1,
                    M52 = 1,
                    M53 = 1,
                }
            };
        }




        //黑白方法 
        public static ICanvasImage GetGrayscale(ICanvasImage image)
        {
            return new GrayscaleEffect { Source = image };
        }


        //反色方法 
        public static ICanvasImage GetInvert(ICanvasImage image)
        {
            return new InvertEffect { Source = image };
        }


        //曝光方法
        public static ICanvasImage GetExposure(ICanvasImage image, float exposure)
        {
            if (exposure < -2) exposure = -2;
            else if (exposure > 2) exposure = 2;

            return new ExposureEffect
            {
                Source = image,
                Exposure = exposure,
            };
        }


        //亮度方法 
        public static ICanvasImage GetBrightness(ICanvasImage image, float Brightness)
        {
            float WhiteX = Math.Min(2 - Brightness, 1);
            float WhiteY = 1f;
            float BlackX = Math.Max(1 - Brightness, 0);
            float BlackY = 0f;

            return GetBrightness(image, WhiteX, WhiteY, BlackX, BlackY);
        }
        public static ICanvasImage GetBrightness(ICanvasImage image, float WhiteX, float WhiteY, float BlackX, float BlackY)
        {
            if (WhiteX > 1) WhiteX = 1;
            else if (WhiteX < 0) WhiteX = 0;
            if (WhiteY > 1) WhiteY = 1;
            else if (WhiteY < 0) WhiteY = 0;

            if (BlackX > 1) BlackX = 1;
            else if (BlackX < 0) BlackX = 0;
            if (BlackY > 1) BlackY = 1;
            else if (BlackY < 0) BlackY = 0;

            return new BrightnessEffect
            {
                Source = image,
                WhitePoint = new Vector2(WhiteX, WhiteY),
                //亮度转移曲线的下半部分，黑点会调整图像中较暗部分的外观，介于0~1
                BlackPoint = new Vector2(BlackX, BlackY),
            };
        }


        //饱和度方法
        public static ICanvasImage GetSaturation(ICanvasImage image, float saturation)
        {
            if (saturation < 0) saturation = 0;
            else if (saturation > 2) saturation = 2;

            return new SaturationEffect
            {
                Source = image,
                Saturation = saturation,
            };
        }


        //色相方法
        public static ICanvasImage GetHueRotation(ICanvasImage image, float Angle)
        {
            if (Angle < 0f) Angle = 0f;
            else if (Angle > 6f) Angle = 6f;

            return new HueRotationEffect
            {
                Source = image,
                Angle = Angle,
            };
        }


        //对比度方法
        public static ICanvasImage GetContrast(ICanvasImage image, float Contrast)
        {
            if (Contrast < -1) Contrast = -1;
            else if (Contrast > 1) Contrast = 1;

            return new ContrastEffect
            {
                Source = image,
                Contrast = Contrast,
            };
        }


        //冷暖方法
        public static ICanvasImage GetTemperature(ICanvasImage image, float Temperature, float Tint = 0f)
        {
            if (Temperature < -1) Temperature = -1;
            else if (Temperature > 1) Temperature = 1;
            if (Tint < -1) Tint = -1;
            else if (Tint > 1) Tint = 1;

            return new TemperatureAndTintEffect
            {
                Source = image,
                Temperature = Temperature,
                Tint = Tint,
            };
        }


        //高光阴影方法
        public static ICanvasImage GetHighlightsAndShadows(ICanvasImage image, float Shadows, float Highlights, float Clarity = 0f, float MaskBlurAmount = 1.25f, bool SourceIsLinearGamma = false)
        {
            if (Shadows < -1) Shadows = -1;
            else if (Shadows > 1) Shadows = 1;
            if (Highlights < -1) Highlights = -1;
            else if (Highlights > 1) Highlights = 1;

            if (Clarity < -1) Clarity = -1;
            else if (Clarity > 1) Clarity = 1;
            if (MaskBlurAmount < 0) MaskBlurAmount = 0;
            else if (MaskBlurAmount > 10) MaskBlurAmount = 10;

            return new HighlightsAndShadowsEffect
            {
                Source = image,

                Shadows = Shadows,
                Highlights = Highlights,
                Clarity = Clarity,
                MaskBlurAmount = MaskBlurAmount,
                SourceIsLinearGamma = SourceIsLinearGamma,
            };
        }




        //高斯模糊方法
        public static ICanvasImage GetGaussianBlur(ICanvasImage image, float BlurAmount, EffectOptimization Optimization = EffectOptimization.Speed, EffectBorderMode BorderMode = EffectBorderMode.Soft)
        {
            if (BlurAmount < 0) BlurAmount = 0;
            else if (BlurAmount > 100) BlurAmount = 100;

            return new GaussianBlurEffect
            {
                Source = image,
                BlurAmount = BlurAmount,

                Optimization = Optimization,
                BorderMode = BorderMode,
            };
        }


        //定向模糊方法
        public static ICanvasImage GetDirectionalBlur(ICanvasImage image, float BlurAmount, float Angle, EffectOptimization Optimization, EffectBorderMode BorderMode)
        {
            if (BlurAmount < 0) BlurAmount = 0;
            else if (BlurAmount > 100) BlurAmount = 100;

            return new DirectionalBlurEffect
            {
                Source = image,
                BlurAmount = BlurAmount,
                Angle = Angle,
                Optimization = Optimization,
                BorderMode = BorderMode,
            };
        }


        //锐化方法
        public static ICanvasImage GetSharpen(ICanvasImage image, float Amount)
        {
            if (Amount < 0) Amount = 0;
            else if (Amount > 10) Amount = 10;

            return new SharpenEffect
            {
                Source = image,
                Amount = Amount,
            };
        }


        //阴影方法   
        public static ICanvasImage GetShadow(ICanvasImage image, Matrix3x2 m,Color color)// Virtual & Animated：虚拟 & 动画
        {
            return GetShadow(new Transform2DEffect
            {
                Source = image,
                TransformMatrix = m,
                InterpolationMode = CanvasImageInterpolation.NearestNeighbor,
            }, 7, color, 5, 5);
        }
        public static ICanvasImage GetShadow(ICanvasImage image, float BlurAmount, Color ShadowColor)
        {
            if (BlurAmount < 0) BlurAmount = 0;
            else if (BlurAmount > 100) BlurAmount = 100;

            return new CompositeEffect
            {
                Sources =  {new ShadowEffect
                {
                    Source = image,
                    BlurAmount = BlurAmount,
                    ShadowColor = ShadowColor,
                },
                    image
                }
            };
        }
        public static ICanvasImage GetShadow(ICanvasImage image, float BlurAmount, Color ShadowColor, float X, float Y)
        {
            if (BlurAmount < 0) BlurAmount = 0;
            else if (BlurAmount > 100) BlurAmount = 100;

            return new CompositeEffect
            {
                Sources =
                {
                          new Transform2DEffect
                        {
                            Source =   new ShadowEffect
                            {
                                Source = image,
                                BlurAmount = BlurAmount,
                                ShadowColor = ShadowColor,
                            },
                            TransformMatrix = Matrix3x2.CreateTranslation(X, Y),
                     },
                    image
                }
            };
        }
        public static ICanvasImage GetShadow(ICanvasImage image, float BlurAmount, Color ShadowColor, float X, float Y, float Opacity)
        {
            if (BlurAmount < 0) BlurAmount = 0;
            else if (BlurAmount > 100) BlurAmount = 100;
            if (Opacity < 0) Opacity = 0;
            else if (Opacity > 1) Opacity = 1;

            return new CompositeEffect
            {
                Sources =
                {
                    new OpacityEffect
                    {
                        Source =  new Transform2DEffect
                        {
                            Source =   new ShadowEffect
                            {
                                Source = image,
                                BlurAmount = BlurAmount,
                                ShadowColor = ShadowColor,
                            },
                            TransformMatrix = Matrix3x2.CreateTranslation(X, Y),
                        },
                        Opacity =Opacity
                    },
                    image
                }
            };
        }


        //消除颜色方法
        public static ICanvasImage GetChromaKey(ICanvasImage image, float Tolerance, Color Color, bool Feather, bool InvertAlpha)
        {
            if (Tolerance < 0) Tolerance = 0;
            else if (Tolerance > 1) Tolerance = 1;

            return new ChromaKeyEffect
            {
                Source = image,
                Tolerance = Tolerance,
                Color = Color,
                Feather = Feather,
                InvertAlpha = InvertAlpha,
            };
        }


        //边缘检测方法
        public static ICanvasImage GetEdgeDetection(ICanvasImage image, float Amount, float BlurAmount = 0, EdgeDetectionEffectMode Mode = EdgeDetectionEffectMode.Sobel, CanvasAlphaMode AlphaMode = CanvasAlphaMode.Premultiplied, bool OverlayEdges = false)
        {
            if (Amount < 0) Amount = 0;
            else if (Amount > 1) Amount = 1;
            if (BlurAmount < 0) BlurAmount = 0;
            else if (BlurAmount > 10) BlurAmount = 10;

            return new EdgeDetectionEffect
            {
                Source = image,

                Amount = Amount,
                BlurAmount = BlurAmount,

                Mode = Mode,
                AlphaMode = AlphaMode,
                OverlayEdges = OverlayEdges,
            };
        }


        //边界方法
        public static ICanvasImage GetChromaKey(ICanvasImage image, CanvasEdgeBehavior ExtendX, CanvasEdgeBehavior ExtendY)
        {
            return new BorderEffect
            {
                Source = image,
                ExtendX = ExtendX,
                ExtendY = ExtendY,
            };
        }


        //浮雕方法
        public static ICanvasImage GetEmboss(ICanvasImage image, float Amount, float Angle)
        {
            if (Amount < 0) Amount = 0;
            else if (Amount > 10) Amount = 10;

            if (Angle < 0) Angle = 0;
            else if (Angle > 2 * (float)(Math.PI)) Angle = (float)(2 * Math.PI);

            return new EmbossEffect
            {
                Source = image,
                Amount = Amount,
                Angle = Angle,
            };
        }



        //亮度转透明度
        public static ICanvasImage GetLuminanceToAlpha(ICanvasImage image)
        {
            return new LuminanceToAlphaEffect { Source = image };
        }

        //深褐色
        public static ICanvasImage GetSepia(ICanvasImage image)
        {
            return new SepiaEffect { Source = image };
        }

        //多色调分印
        public static ICanvasImage GetPosterize(ICanvasImage image)
        {
            return new PosterizeEffect { Source = image };
        }


        //色彩方法
        public static ICanvasImage GetTint(ICanvasImage image, Color Color)
        {
            return new TintEffect
            {
                Source = image,

                Color = Color
            };
        }
        public static ICanvasImage GetTint(ICanvasImage image, float Opacity, Color Color)
        {
            if (Opacity < 0) Opacity = 0;
            else if (Opacity > 1) Opacity = 1;

            return new CompositeEffect
            {
                Sources =
                {
                    image,
                    new OpacityEffect
                    {
                        Opacity = Opacity,
                        Source = new TintEffect
                        {
                            Source = image,
                            Color = Color
                        }
                    }
                }
            };
        }


        //装饰图案方法
        public static ICanvasImage GetVignette(ICanvasImage image, float Amount, Color Color)
        {
            if (Amount < 0) Amount = 0;
            else if (Amount > 1) Amount = 1;

            return new VignetteEffect
            {
                Source = image,
                Amount = Amount,
                Color = Color
            };
        }


        //伽马转换方法
        public static ICanvasImage GetGammaTransfer(ICanvasImage image, float AO, float RO, float GO, float BO)
        {
            return new GammaTransferEffect
            {
                Source = image,
                AlphaOffset = AO,
                RedOffset = RO,
                GreenOffset = GO,
                BlueOffset = BO,
            };
        }
        public static ICanvasImage GetGammaTransfer(ICanvasImage image, float AA, float AE, float AO, float RA, float RE, float RO, float GA, float GE, float GO, float BA, float BE, float BO)
        {
            return new GammaTransferEffect
            {
                Source = image,

                AlphaAmplitude = AA,//振幅
                AlphaExponent = AE,//指数
                AlphaOffset = AO,//偏移

                RedAmplitude = RA,
                RedExponent = RE,
                RedOffset = RO,

                GreenAmplitude = GA,
                GreenExponent = GE,
                GreenOffset = GO,

                BlueAmplitude = BA,
                BlueExponent = BE,
                BlueOffset = BO,
            };
        }





        //柏林噪声
        public static ICanvasImage GetTurbulence(float width, float height)
        {
            return new TurbulenceEffect//柏林噪波
            {
                Octaves = 8,
                Size = new Vector2(width, height),
            };
        }


        //置换贴图
        public static ICanvasImage GetDisplacementMap(ICanvasImage image, ICanvasImage source, float Amount,
            EffectChannelSelect XChannelSelect = EffectChannelSelect.Red,
            EffectChannelSelect YChannelSelect = EffectChannelSelect.Green)
        {
            return new DisplacementMapEffect//取代一个输入图像的像素强度值的第二个图片。
            {
                Source = image,
                Displacement = source,

                XChannelSelect = XChannelSelect,
                YChannelSelect = YChannelSelect,

                Amount = Amount
            };
        }



        //形态学方法
        public static ICanvasImage GetMorphology(ICanvasImage image, float size)
        {
            int s = (int)Math.Abs(size) / 2 + 1;
            if (s > 90) s = 90;
            return new MorphologyEffect
            {
                Mode = (size < 0) ? MorphologyEffectMode.Erode : MorphologyEffectMode.Dilate,
                Width = s,
                Height = s,
                Source = image,
            };
        }




    }
}
