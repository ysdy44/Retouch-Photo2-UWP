using System;
using System.Numerics;
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.Tools.ITool;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Keyboards;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="Tool"/>'s ImageTool.
    /// </summary>
    public class ImageTool : Tool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        MezzanineViewModel MezzanineViewModel => App.MezzanineViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        float SizeWidth;
        float SizeHeight;

        //@Construct
        public ImageTool()
        {
            base.Type = ToolType.Image;
            base.Icon = new ImageControl();
            base.ShowIcon = new ImageControl();
            base.Page = new ImagePage();
        }


        public override void Starting(Vector2 point)
        {

        }
        public override void Started(Vector2 startingPoint, Vector2 point)
        {
            ImageRe imageRe = this.SelectionViewModel.ImageRe;

            //ImageRe
            if (imageRe == null)
            {
                this.SelectionViewModel.ImageRe = new ImageRe { IsStoryboardNotify = true };
                return;
            }
            if (imageRe.IsStoryboardNotify == true)
            {
                this.SelectionViewModel.ImageRe = new ImageRe { IsStoryboardNotify = true };
                return;
            }

            //Transformer
            this.SizeWidth = imageRe.Width;
            this.SizeHeight = imageRe.Height;
            Transformer transformerSource = new Transformer(imageRe.Width, imageRe.Height, Vector2.Zero);
            Transformer transformerDestination = this.CreateTransformer(startingPoint, point, imageRe.Width, imageRe.Height);

            //Mezzanine
            Layer createLayer = new ImageLayer()
            {
                ImageRe = imageRe,
                Source = transformerSource,
                Destination = transformerDestination,
                IsChecked = true,
            };
            this.MezzanineViewModel.SetLayer(createLayer, this.ViewModel.Layers);

            this.SelectionViewModel.Transformer = transformerDestination;//Selection

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public override void Delta(Vector2 startingPoint, Vector2 point)
        {
            if (this.MezzanineViewModel.Layer == null) return;

            //Transformer
            Transformer transformerDestination = this.CreateTransformer(startingPoint, point, this.SizeWidth, this.SizeHeight);

            this.MezzanineViewModel.Layer.Destination = transformerDestination;//Mezzanine

            this.SelectionViewModel.Transformer = transformerDestination;//Selection

            this.ViewModel.Invalidate();//Invalidate
        }
        public override void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            if (this.MezzanineViewModel.Layer == null) return;

            if (isSingleStarted)
            {
                ImageRe imageRe = this.SelectionViewModel.ImageRe;

                //ImageRe
                if (imageRe == null) return;
                if (imageRe.IsStoryboardNotify == true) return;
                
                //Transformer
                float sizeWidth = imageRe.Width;
                float sizeHeight = imageRe.Height;
                Transformer transformer = this.CreateTransformer(startingPoint, point, sizeWidth, sizeHeight);

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.IsChecked = false;
                });

                //Transformer
                this.SizeWidth = imageRe.Width;
                this.SizeHeight = imageRe.Height;
                Transformer transformerDestination = this.CreateTransformer(startingPoint, point, imageRe.Width, imageRe.Height);
                Transformer transformerSource = new Transformer(imageRe.Width, imageRe.Height, Vector2.Zero);

                //Mezzanine
                Layer createLayer = new ImageLayer()
                {
                    ImageRe = imageRe,
                    Source = transformerSource,
                    Destination = transformerDestination,
                    IsChecked = true,
                };
                this.MezzanineViewModel.Insert(createLayer, this.ViewModel.Layers);
            }
            else this.MezzanineViewModel.None();//Mezzanine

            this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }


        public override void Draw(CanvasDrawingSession ds)
        {
        }


        public Transformer CreateTransformer(Vector2 startingPoint, Vector2 point, float sizeWidth, float sizeHeight)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Transformer canvasTransformer = this.GetAspectRatioRectangle(startingPoint, point, sizeWidth, sizeHeight);
            return canvasTransformer * inverseMatrix;
        }

        /// <summary>
        /// Get a rectangle with the same size scale.
        /// </summary>
        /// <param name="startingPoint"> starting-point </param>
        /// <param name="point"> point </param>
        /// <param name="sizeWidth"> The source size width. </param>
        /// <param name="sizeHeight"> The source size height. </param>
        /// <returns> Transformer </returns>
        private Transformer GetAspectRatioRectangle(Vector2 startingPoint, Vector2 point, float sizeWidth, float sizeHeight)
        {
            float lengthSquared = Vector2.DistanceSquared(startingPoint, point);

            //Height not less than 10
            if (sizeWidth > sizeHeight)
            {
                float heightSquared = lengthSquared / (1 + (sizeWidth * sizeWidth) / (sizeHeight * sizeHeight));
                float height = (float)Math.Sqrt(heightSquared) / 1.4142135623730950488016887242097f;

                if (height < 10) height = 10;
                float width = height * sizeWidth / sizeHeight;

                return this.GetRectangleInQuadrant(startingPoint, point, width, height);
            }
            //Width not less than 10
            else if (sizeWidth < sizeHeight)
            {
                float widthSquared = lengthSquared / (1 + (sizeHeight * sizeHeight) / (sizeWidth * sizeWidth));
                float width = (float)Math.Sqrt(widthSquared) / 1.4142135623730950488016887242097f; ;

                if (width < 10) width = 10;
                float height = width * sizeHeight / sizeWidth;

                return this.GetRectangleInQuadrant(startingPoint, point, width, height);
            }
            //Width equals height
            else
            {
                float spare = (float)Math.Sqrt(lengthSquared) / 1.4142135623730950488016887242097f; ;

                return this.GetRectangleInQuadrant(startingPoint, point, spare, spare);
            }
        }

        /// <summary>
        /// Get a rectangle corresponding to the 1, 2, 3 or 4 quadrant.
        /// </summary>
        /// <param name="startingPoint"> starting-point </param>
        /// <param name="point"> point </param>
        /// <param name="sizeWidth"> The source size width. </param>
        /// <param name="sizeHeight"> The source size height. </param>
        /// <returns> Transformer </returns>
        private Transformer GetRectangleInQuadrant(Vector2 startingPoint, Vector2 point, float width, float height)
        {
            bool xAxis = (point.X >= startingPoint.X);
            bool yAxis = (point.Y >= startingPoint.Y);

            //Fourth Quadrant  
            if (xAxis && yAxis)
            {
                return new Transformer
                {
                    LeftTop = new Vector2(startingPoint.X, startingPoint.Y),
                    RightTop = new Vector2(startingPoint.X + width, startingPoint.Y),
                    RightBottom = new Vector2(startingPoint.X + width, startingPoint.Y + height),
                    LeftBottom = new Vector2(startingPoint.X, startingPoint.Y + height),
                };
            }
            //Third Quadrant  
            else if (xAxis == false && yAxis)
            {
                return new Transformer
                {
                    LeftTop = new Vector2(startingPoint.X, startingPoint.Y),
                    RightTop = new Vector2(startingPoint.X - width, startingPoint.Y),
                    RightBottom = new Vector2(startingPoint.X - width, startingPoint.Y + height),
                    LeftBottom = new Vector2(startingPoint.X, startingPoint.Y + height),
                };
            }
            //First Quantity
            else if (xAxis && yAxis == false)
            {
                return new Transformer
                {
                    LeftTop = new Vector2(startingPoint.X, startingPoint.Y),
                    RightTop = new Vector2(startingPoint.X + width, startingPoint.Y),
                    RightBottom = new Vector2(startingPoint.X + width, startingPoint.Y - height),
                    LeftBottom = new Vector2(startingPoint.X, startingPoint.Y - height),
                };
            }
            //Second Quadrant
            else
            {
                return new Transformer
                {
                    LeftTop = new Vector2(startingPoint.X, startingPoint.Y),
                    RightTop = new Vector2(startingPoint.X - width, startingPoint.Y),
                    RightBottom = new Vector2(startingPoint.X - width, startingPoint.Y - height),
                    LeftBottom = new Vector2(startingPoint.X, startingPoint.Y - height),
                };
            }
        }

    }
}