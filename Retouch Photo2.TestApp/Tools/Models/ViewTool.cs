﻿using Microsoft.Graphics.Canvas;
using Retouch_Photo2.TestApp.Tools.Controls;
using Retouch_Photo2.TestApp.ViewModels;
using System.Numerics;

namespace Retouch_Photo2.TestApp.Tools.Models
{
    /// <summary>
    /// <see cref="Tool"/>'s ViewTool .
    /// </summary>
    public class ViewTool : Tool
    {
        //ViewModel
        ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;

        Vector2 StartPosition;

        public ViewTool()
        {
            base.Type = ToolType.View;
            base.Icon = new ViewControl();
            base.ShowIcon = new ViewControl();
            base.Page = null;
        }

        //@Override
        public override void Starting(Vector2 point) { }
        public override void Started(Vector2 startingPoint, Vector2 point)
        {
            this.StartPosition = this.ViewModel.CanvasTransformer.Position;
            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public override void Delta(Vector2 startingPoint, Vector2 point)
        {
            this.ViewModel.CanvasTransformer.Position = this.StartPosition - startingPoint + point;
            this.ViewModel.Invalidate();//Invalidate
        }
        public override void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            if (isSingleStarted)
            {
                this.ViewModel.CanvasTransformer.Position = this.StartPosition - startingPoint + point;
            }
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        } 

        public override void Draw(CanvasDrawingSession ds) { }
    }
}