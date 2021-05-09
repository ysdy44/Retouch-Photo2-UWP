// Core:              ★★
// Referenced:   ★★
// Difficult:         ★★★★
// Only:              ★★★
// Complete:      ★★★★
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Filters
{
    /// <summary>
    /// A control used to show a filter.
    /// </summary>
    public sealed partial class FilterShowControl : UserControl
    {

        //@Static
        private static readonly CanvasDevice CanvasDevice = new CanvasDevice();
        private static CanvasBitmap Bitmap = null;

        private CanvasBitmap BitmapCore = null;


        //@Task
        private static readonly Task<CanvasBitmap> TaskBitmap = Task.Run<CanvasBitmap>(async () => await CanvasBitmap.LoadAsync(CanvasDevice, @"Icons\Lenna.jpg"));
        private static async Task CreateResourceAsync()
        {
            FilterShowControl.TaskBitmap.Wait();
            FilterShowControl.Bitmap = await FilterShowControl.TaskBitmap;
        }



        #region DependencyProperty

        /// <summary> Gets or sets <see cref = "FilterShowControl" />'s Filter. </summary>
        public Filter Filter
        {
            get => (Filter)base.GetValue(FilterProperty);
            set => base.SetValue(FilterProperty, value);
        }
        /// <summary> Identifies the <see cref = "FilterShowControl.Filter" /> dependency property. </summary>
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof(Filter), typeof(Filter), typeof(FilterShowControl), new PropertyMetadata(null, (sender, e) =>
        {
            FilterShowControl control = (FilterShowControl)sender;

            if (e.NewValue is Filter value)
            {
                control.CanvasControl.Invalidate();//Invalidate
            }
        }));

        #endregion


        //@Construct
        /// <summary>
        /// Initializes a FilterShowControl. 
        /// </summary>
        public FilterShowControl()
        {
            this.InitializeComponent();

            this.CanvasControl.UseSharedDevice = true;
            this.CanvasControl.CustomDevice = FilterShowControl.CanvasDevice;
            this.CanvasControl.CreateResources += (sender, arges) =>
            {
                if (FilterShowControl.Bitmap == null)
                {
                    Task task = FilterShowControl.CreateResourceAsync();
                    IAsyncAction action = task.AsAsyncAction();
                    arges.TrackAsyncAction(action);
                }

                if (FilterShowControl.Bitmap != null)
                {
                    CanvasRenderTarget renderTarget = new CanvasRenderTarget(sender, (float)this.Width, (float)this.Height);
                    using (CanvasDrawingSession ds = renderTarget.CreateDrawingSession())
                    {
                        ds.DrawImage(Filter.Render(this.Filter, new ScaleEffect
                        {
                            Scale = new Vector2
                            (
                                (float)(this.Width / FilterShowControl.Bitmap.Size.Width),
                                (float)(this.Height / FilterShowControl.Bitmap.Size.Height)
                            ),
                            Source = FilterShowControl.Bitmap
                        }));
                    }
                    this.BitmapCore = renderTarget;
                }
            };
            this.CanvasControl.Draw += (sender, args) =>
            {
                if (this.BitmapCore != null)
                {
                    args.DrawingSession.DrawImage(this.BitmapCore);
                }
            };
        }
    }
}