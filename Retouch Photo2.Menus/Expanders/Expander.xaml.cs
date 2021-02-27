using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Menus
{
    /// <summary> 
    /// Represents the control that a drawer can be folded.
    /// </summary>
    public abstract partial class Expander : UserControl
    {

        //@Static
        /// <summary>
        /// A canvas, covered by a lot of <see cref="Expander"/>.
        /// </summary>
        public MenusExpanderCanvas OverlayCanvas { private get; set; }
               

        //@VisualState
        bool _vsIsSecondPage = false;
        ExpanderState _vsState = ExpanderState.Hide;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsState)
                {
                    case ExpanderState.Hide: return this.Hide;
                    case ExpanderState.FlyoutShow: return this._vsIsSecondPage ? this.FlyoutShowSecondPage : this.FlyoutShow;
                    case ExpanderState.OverlayNotExpanded: return this.OverlayNotExpanded;
                    case ExpanderState.Overlay: return this._vsIsSecondPage ? this.OverlaySecondPage : this.Overlay;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

               
        //@Construct     
        /// <summary>
        /// Initializes a Expander. 
        /// </summary>
        public Expander()
        {
            this.InitializeComponent();

            this.ConstructWidthStoryboard();
            this.ConstructHeightStoryboard();

            this.Tapped += (s, e) => e.Handled = true;
            this.Page = this.MainPage;
            this.VisualState = this.VisualState;//State 

            /////////////////////////////////
            
            //Button
            this.Button.Self.Tapped += (s, e) =>
            {
                if (this.State == ExpanderState.Hide) this.CalculatePostion(this.Button.Self, this.PlacementMode);

                this.State = this.GetButtonState(this.State);
            };

            this.CloseButton.Click += (s, e) => this.State = ExpanderState.Hide;
            this.StateButton.Click += (s, e) => this.State = this.GetState(this.State);

            this.BackButton.Click += (s, e) => this.Back();
            this.ResetButton.Click += (s, e) => this.Reset();

            /////////////////////////////////
            
            //TitleGrid
            this.TitleGrid.DoubleTapped += (s, e) => this.State = this.GetState(this.State);

            this.TitleGrid.ManipulationMode = ManipulationModes.All;
            this.TitleGrid.ManipulationStarted += (s, e) =>
            {
                if (this.State == ExpanderState.FlyoutShow) return;

                this._postionX = this.PostionX;
                this._postionY = this.PostionY;

                this.Move(); //Delegate
            };
            this.TitleGrid.ManipulationDelta += (s, e) =>
            {
                if (this.State == ExpanderState.FlyoutShow) return;

                this._postionX += e.Delta.Translation.X;
                this._postionY += e.Delta.Translation.Y;

                this.PostionX = this.GetBoundPostionX(this._postionX);
                this.PostionY = this.GetBoundPostionY(this._postionY);
            };
            this.TitleGrid.ManipulationCompleted += (s, e) =>
            {
                this._postionX = this.PostionX;
                this._postionY = this.PostionY;
            };
        }
        

        private ExpanderState GetState(ExpanderState state)
        {
            switch (state)
            {
                case ExpanderState.Overlay: return ExpanderState.OverlayNotExpanded;
                case ExpanderState.OverlayNotExpanded: return ExpanderState.Overlay;
            }
            return ExpanderState.Overlay;
        }
        private ExpanderState GetButtonState(ExpanderState state)
        {
            switch (state)
            {
                case ExpanderState.Hide: return ExpanderState.FlyoutShow;
                case ExpanderState.FlyoutShow: return ExpanderState.Hide;

                case ExpanderState.Overlay: return ExpanderState.OverlayNotExpanded;
                case ExpanderState.OverlayNotExpanded: return ExpanderState.Overlay;
            }
            return ExpanderState.FlyoutShow;
        }


    }
}