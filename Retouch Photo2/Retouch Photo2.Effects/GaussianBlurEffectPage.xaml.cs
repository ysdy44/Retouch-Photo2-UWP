using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Retouch_Photo2.Historys;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Layers;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "Effect.GaussianBlur_IsOn"/>.
    /// </summary>
    public sealed partial class GaussianBlurEffectPage : Page, IEffectPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        /// <summary>
        /// Initializes a GaussianBlurEffectPage. 
        /// </summary>
        public GaussianBlurEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();
            this.ConstructButton();
            this.ConstructSharpen_Amount();
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.GaussianBlur_IsOn"/>.
    /// </summary>
    public sealed partial class GaussianBlurEffectPage : Page, IEffectPage
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Text = resource.GetString("/Effects/GaussianBlur");

            this.RadiusTextBlock.Text = resource.GetString("/Effects/GaussianBlur_Radius");
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public EffectType Type => EffectType.GaussianBlur;
        /// <summary> Gets the page. </summary>
        public FrameworkElement Page => this;
        /// <summary> Gets the button. </summary>
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new GaussianBlurIcon()
        };
        
        public void Reset()
        {
            this.RadiusSlider.Value = 0;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set effect gaussian blur");

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                var previous = layer.Effect.GaussianBlur_Radius;
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Effect.GaussianBlur_Radius = previous;
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.Effect.GaussianBlur_Radius = 0;
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }
        public void FollowButton(Effect effect)
        {
            this.Button.IsOn = effect.GaussianBlur_IsOn;
        }
        public void FollowPage(Effect effect)
        {
            this.RadiusSlider.Value = effect.GaussianBlur_Radius;
        }
    }
    
    /// <summary>
    /// Page of <see cref = "Effect.GaussianBlur_IsOn"/>.
    /// </summary>
    public sealed partial class GaussianBlurEffectPage : Page, IEffectPage
    {
        private void ConstructButton()
        {
            this.Button.Toggled += (isOn) =>
            {               
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set effect gaussian blur");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.GaussianBlur_IsOn;
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layer.Effect.GaussianBlur_IsOn = previous;
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.Effect.GaussianBlur_IsOn = isOn;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        private void ConstructSharpen_Amount()
        {
            //Radius
            this.RadiusSlider.ValueChangeStarted += (s, value) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.Effect.CacheGaussianBlur();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.RadiusSlider.ValueChangeDelta += (s, value) =>
            {
                float radius = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layerage.RefactoringParentsRender();
                    layer.Effect.GaussianBlur_Radius = radius;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.RadiusSlider.ValueChangeCompleted += (s, value) =>
            {
                float radius = (float)value;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set effect gaussian blur");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Effect.StartingGaussianBlur_Radius;
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layer.Effect.GaussianBlur_Radius = previous;
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.Effect.GaussianBlur_Radius = radius;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };
        }

    }
}