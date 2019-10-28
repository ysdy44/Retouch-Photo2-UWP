using Retouch_Photo2.Elements;
using Retouch_Photo2.Menus;
using Retouch_Photo2.Tools;
using Retouch_Photo2.Tools.Elements;
using Retouch_Photo2.ViewModels;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;
using System.Numerics;
using System.Xml.Linq;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "DrawPage" />. 
    /// </summary>
    public sealed partial class DrawPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        static bool _isLoaded;
        
        #region DependencyProperty


        /// <summary> Gets or sets the canvas transition. </summary>
        public float Transition
        {
            get { return (float)GetValue(TransitionProperty); }
            set { SetValue(TransitionProperty, value); }
        }
        /// <summary> Identifies the <see cref = "DrawPage.Transition" /> dependency property. </summary>
        public static readonly DependencyProperty TransitionProperty = DependencyProperty.Register(nameof(Transition), typeof(float), typeof(DrawPage), new PropertyMetadata(0.0f, (sender, e) =>
        {
            DrawPage con = (DrawPage)sender;

            if (e.NewValue is float value)
            {
                con.ViewModel.CanvasTransformer.Transition(value);
                con.ViewModel.Invalidate();//Invalidate
            }
        }));      
        
        
        /// <summary> Sets the page layout is full-screen. </summary>
        public bool IsFullScreen
        {
            set
            {
                this.UnFullScreenButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                this.DrawLayout.IsFullScreen = value;

                Vector2 offset = this.DrawLayout.FullScreenOffset;
                if (value)
                    this.ViewModel.CanvasTransformer.Position += offset;
                else
                    this.ViewModel.CanvasTransformer.Position -= offset;

                this.ViewModel.CanvasTransformer.ReloadMatrix();
            }
        }


        #endregion

        //@Construct
        public DrawPage()
        {
            this.InitializeComponent();

            //ViewModel
            this.ConstructViewModel();
            this.ConstructKeyboardViewModel();

            //MoreButton
            MoreTransformButton.Flyout = this.MoreTransformFlyout;
            MoreCreateButton.Flyout = this.MoreCreateFlyout;

            this.Loaded += (s, e) =>
            {
                if (DrawPage._isLoaded == false)
                {
                    DrawPage._isLoaded = true;
                    this.NavigatedTo();
                }
            };


            // Binding own DependencyProperty to the Storyboard
            Storyboard.SetTarget(this.TransitionKeyFrames, this);
            this.TransitionKeyFrames.Completed += (s, e) => this.NavigatedToComplete();
            this.DrawLayout.BackButton.Tapped += (s, e) => this.NavigatedFrom();


            //Button
            this.UnFullScreenButton.Tapped += (s, e) => this.KeyboardViewModel.IsFullScreen = false;
            this.FullScreenButton.Tapped += (s, e) => this.KeyboardViewModel.IsFullScreen = true;
            this.SaveButton.Tapped += (s, e) =>
            {
                Project project = new Project
                {
                    Width = this.ViewModel.CanvasTransformer.Width,
                    Height = this.ViewModel.CanvasTransformer.Height,
                    Layers = this.ViewModel.Layers.RootLayers
                };
                XDocument document = Retouch_Photo2.ViewModels.XML.SaveProject(project);

                string path = ApplicationData.Current.LocalFolder.Path + "/" + "Unsdasd" + ".photo2";
                document.Save(path);                
            };
            this.ThemeButton.Tapped += (s, e) =>
            {
                //Trigger switching theme.
                ElementTheme theme = this.ThemeControl.Theme;
                theme = (theme == ElementTheme.Dark) ? ElementTheme.Light : ElementTheme.Dark;

                this.ThemeControl.Theme = theme;
                ApplicationViewTitleBarBackgroundExtension.SetTheme(theme);

                this.SettingViewModel.ElementTheme = theme;
                this.SettingViewModel.WriteToLocalFolder();//Write
            };


            //Layers
            this.LayersControl.WidthButton.Tapped += (s, e) => this.DrawLayout.PadChangeLayersWidth();


            //Tool
            foreach (ITool tool in this.TipViewModel.Tools)
            {
                this.ConstructTool(tool);
            }
            this.TooLeft.Add(this.MoreToolButton);
            this.ToolFirst();

            //Menu
            foreach (IMenu menu in this.TipViewModel.Menus)
            {
                this.ConstructMenu(menu);
            }
            this.OverlayCanvas.Tapped += (s, e) => this.MenusHideAndCrop(isCrop: false);
            this.OverlayCanvas.SizeChanged += (s, e) => this.MenusHideAndCrop(isCrop: true);
        }

        //The current page becomes the active page
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Theme
            ElementTheme theme = this.SettingViewModel.ElementTheme;
            this.ThemeControl.Theme = theme;

            //Layout
            this.DrawLayout.VisualStateDeviceType = this.SettingViewModel.LayoutDeviceType;
            this.DrawLayout.VisualStatePhoneMaxWidth = this.SettingViewModel.LayoutPhoneMaxWidth;
            this.DrawLayout.VisualStatePadMaxWidth = this.SettingViewModel.LayoutPadMaxWidth;
            
            if (DrawPage._isLoaded)
            {
                this.NavigatedTo();
            }
        }
        //The current page no longer becomes an active page
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

    }
}