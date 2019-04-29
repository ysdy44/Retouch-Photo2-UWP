using System;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace Retouch_Photo2.Controls.TransformerControls
{

    public enum IndicatorMode
    {
        LeftTop,
        RightTop,
        RightBottom,
        LeftBottom,

        Left,
        Top,
        Right,
        Bottom,

        Center,
    }

    public sealed partial class IndicatorControl : UserControl
    {
        //Delegate
        public delegate void IndicatorModeHandler(IndicatorMode mode);
        public event IndicatorModeHandler ModeChanged;

        #region DependencyProperty

        public IndicatorMode Mode
        {
            get { return (IndicatorMode)GetValue(LayerProperty); }
            set { SetValue(LayerProperty, value); }
        }
        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register(nameof(Mode), typeof(IndicatorMode), typeof(IndicatorControl), new PropertyMetadata(IndicatorMode.LeftTop, (sender, e) =>
        {
            IndicatorControl con = (IndicatorControl)sender;

            if (e.OldValue is IndicatorMode oldValue)
            {
                switch (oldValue)
                {
                    case IndicatorMode.LeftTop: con.Fade(con.LeftTopRectangle); break;
                    case IndicatorMode.RightTop: con.Fade(con.RightTopRectangle); break;
                    case IndicatorMode.RightBottom: con.Fade(con.RightBottomRectangle); break;
                    case IndicatorMode.LeftBottom: con.Fade(con.LeftBottomRectangle); break;

                    case IndicatorMode.Left: con.Fade(con.LeftRectangle); break;
                    case IndicatorMode.Top: con.Fade(con.TopRectangle); break;
                    case IndicatorMode.Right: con.Fade(con.RightRectangle); break;
                    case IndicatorMode.Bottom: con.Fade(con.BottomRectangle); break;

                    case IndicatorMode.Center: con.Fade(con.CenterRectangle); break;

                    default: break;
                }
            }

            if (e.NewValue is IndicatorMode value)
            {
                switch (value)
                {
                    case IndicatorMode.LeftTop: con.Show(con.LeftTopRectangle); break;
                    case IndicatorMode.RightTop: con.Show(con.RightTopRectangle); break;
                    case IndicatorMode.RightBottom: con.Show(con.RightBottomRectangle); break;
                    case IndicatorMode.LeftBottom: con.Show(con.LeftBottomRectangle); break;

                    case IndicatorMode.Left: con.Show(con.LeftRectangle); break;
                    case IndicatorMode.Top: con.Show(con.TopRectangle); break;
                    case IndicatorMode.Right: con.Show(con.RightRectangle); break;
                    case IndicatorMode.Bottom: con.Show(con.BottomRectangle); break;

                    case IndicatorMode.Center: con.Show(con.CenterRectangle); break;

                    default: break;
                }
                con.ModeChanged?.Invoke(value);//Delegate
            }
        }));

        #endregion

        public double Radians { get => this.RotateTransform.Angle; set => this.RotateTransform.Angle = value; }


        public IndicatorControl()
        {
            this.InitializeComponent();


            //Button
            this.LeftTopButton.Tapped += (s, e) => this.Mode = IndicatorMode.LeftTop;
            this.RightTopButton.Tapped += (s, e) => this.Mode = IndicatorMode.RightTop;
            this.RightBottomButton.Tapped += (s, e) => this.Mode = IndicatorMode.RightBottom;
            this.LeftBottomButton.Tapped += (s, e) => this.Mode = IndicatorMode.LeftBottom;

            this.LeftButton.Tapped += (s, e) => this.Mode = IndicatorMode.Left;
            this.TopButton.Tapped += (s, e) => this.Mode = IndicatorMode.Top;
            this.RightButton.Tapped += (s, e) => this.Mode = IndicatorMode.Right;
            this.BottomButton.Tapped += (s, e) => this.Mode = IndicatorMode.Bottom;

            this.CenterButton.Tapped += (s, e) => this.Mode = IndicatorMode.Center;


            this.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;

                //Size
                float size = (float)Math.Min(e.NewSize.Width, e.NewSize.Height);
                float square = size / 3 / 1.4142135623730950488016887242097f;//Root Number 2
                float squareHalf = square / 2;


                //Control
                this.RootGrid.Width = this.RootGrid.Height = size;


                //Rectangle
                this.LeftTopButton.Width = this.LeftTopButton.Height =
                this.RightTopButton.Width = this.RightTopButton.Height =
                this.RightBottomButton.Width = this.RightBottomButton.Height =
                this.LeftBottomButton.Width = this.LeftBottomButton.Height =

                this.LeftButton.Width = this.LeftButton.Height =
                this.TopButton.Width = this.TopButton.Height =
                this.RightButton.Width = this.RightButton.Height =
                this.BottomButton.Width = this.BottomButton.Height =

                this.CenterButton.Width = this.CenterButton.Height = square;


                //Vector
                Vector2 center = new Vector2(size / 2);

                Vector2 leftTop = new Vector2(-square, -square) + center;
                Vector2 rightTop = new Vector2(square, -square) + center;
                Vector2 rightBottom = new Vector2(square, square) + center;
                Vector2 leftBottom = new Vector2(-square, square) + center;

                Vector2 left = new Vector2(-square, 0) + center;
                Vector2 top = new Vector2(0, -square) + center;
                Vector2 right = new Vector2(square, 0) + center;
                Vector2 bottom = new Vector2(0, square) + center;


                //Rectangle
                Canvas.SetLeft(this.CenterButton, center.X - squareHalf);
                Canvas.SetTop(this.CenterButton, center.Y - squareHalf);

                Canvas.SetLeft(this.LeftTopButton, leftTop.X - squareHalf);
                Canvas.SetTop(this.LeftTopButton, leftTop.Y - squareHalf);
                Canvas.SetLeft(this.RightTopButton, rightTop.X - squareHalf);
                Canvas.SetTop(this.RightTopButton, rightTop.Y - squareHalf);
                Canvas.SetLeft(this.RightBottomButton, rightBottom.X - squareHalf);
                Canvas.SetTop(this.RightBottomButton, rightBottom.Y - squareHalf);
                Canvas.SetLeft(this.LeftBottomButton, leftBottom.X - squareHalf);
                Canvas.SetTop(this.LeftBottomButton, leftBottom.Y - squareHalf);

                Canvas.SetLeft(this.LeftButton, left.X - squareHalf);
                Canvas.SetTop(this.LeftButton, left.Y - squareHalf);
                Canvas.SetLeft(this.TopButton, top.X - squareHalf);
                Canvas.SetTop(this.TopButton, top.Y - squareHalf);
                Canvas.SetLeft(this.RightButton, right.X - squareHalf);
                Canvas.SetTop(this.RightButton, right.Y - squareHalf);
                Canvas.SetLeft(this.BottomButton, bottom.X - squareHalf);
                Canvas.SetTop(this.BottomButton, bottom.Y - squareHalf);


                //Line
                this.ForeTopLine.X1 = this.ForeLeftLine.X2 = this.BackTopLine.X1 = this.BackLeftLine.X2 = leftTop.X;
                this.ForeTopLine.Y1 = this.ForeLeftLine.Y2 = this.BackTopLine.Y1 = this.BackLeftLine.Y2 = leftTop.Y;

                this.ForeRightLine.X1 = this.ForeTopLine.X2 = this.BackRightLine.X1 = this.BackTopLine.X2 = rightTop.X;
                this.ForeRightLine.Y1 = this.ForeTopLine.Y2 = this.BackRightLine.Y1 = this.BackTopLine.Y2 = rightTop.Y;

                this.ForeBottomLine.X1 = this.ForeRightLine.X2 = this.BackBottomLine.X1 = this.BackRightLine.X2 = rightBottom.X;
                this.ForeBottomLine.Y1 = this.ForeRightLine.Y2 = this.BackBottomLine.Y1 = this.BackRightLine.Y2 = rightBottom.Y;

                this.ForeLeftLine.X1 = this.ForeBottomLine.X2 = this.BackLeftLine.X1 = this.BackBottomLine.X2 = leftBottom.X;
                this.ForeLeftLine.Y1 = this.ForeBottomLine.Y2 = this.BackLeftLine.Y1 = this.BackBottomLine.Y2 = leftBottom.Y;
            };
        }


        private void Show(Rectangle rectangle)
        {
            this.ShowStoryboard.Stop();

            Storyboard.SetTarget(this.ShowDoubleAnimationX, rectangle);
            Storyboard.SetTarget(this.ShowDoubleAnimationY, rectangle);

            this.ShowStoryboard.Begin();
        }
        private void Fade(Rectangle rectangle)
        {
            this.FadeStoryboard.Stop();

            Storyboard.SetTarget(this.FadeDoubleAnimationX, rectangle);
            Storyboard.SetTarget(this.FadeDoubleAnimationY, rectangle);

            this.FadeStoryboard.Begin();
        }
    }
}