// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Blends
{
    /// <summary>
    /// Segmented of <see cref="TagType"/>
    /// </summary>
    public sealed partial class TagTypeSegmented : UserControl
    {

        //@Delegate
        /// <summary> Occurs when tag-type change. </summary>
        public EventHandler<TagType> TypeChanged;

        //@VisualState
        TagType _vsTagType;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsTagType)
                {
                    case TagType.None: return this.None;
                    case TagType.Red: return this.Red;
                    case TagType.Orange: return this.Orange;
                    case TagType.Yellow: return this.Yellow;
                    case TagType.Green: return this.Green;
                    case TagType.Blue: return this.Blue;
                    case TagType.Purple: return this.Purple;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }
        
        #region DependencyProperty


        /// <summary> Gets or sets the tag type. </summary>
        public TagType Type
        {
            get  => (TagType)base.GetValue(TypeProperty);
            set => base.SetValue(TypeProperty, value);
        }
        /// <summary> Identifies the <see cref = "TagTypeSegmented.Type" /> dependency property. </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(TagType), typeof(TagTypeSegmented), new PropertyMetadata(TagType.None, (sender, e) =>
        {
            TagTypeSegmented control = (TagTypeSegmented)sender;

            if (e.NewValue is TagType value)
            {
                control._vsTagType = value;
                control.VisualState = control.VisualState;//State
            }
        }));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a TagTypeControl. 
        /// </summary>
        public TagTypeSegmented()
        {
            this.InitializeComponent();

            this.ConstructRadioButton(this.NoneButton, TagType.None);
            this.ConstructRadioButton(this.RedButton, TagType.Red);
            this.ConstructRadioButton(this.OrangeButton, TagType.Orange);
            this.ConstructRadioButton(this.YellowButton, TagType.Yellow);
            this.ConstructRadioButton(this.GreenButton, TagType.Green);
            this.ConstructRadioButton(this.BlueButton, TagType.Blue);
            this.ConstructRadioButton(this.PurpleButton, TagType.Purple);

            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }


        private void ConstructRadioButton(RadioButton radioButton, TagType tagType)
        {
            Color color = tagType.ToColor();

            radioButton.Background = new SolidColorBrush(color);
            radioButton.Click += (s, e) =>
            {
                this.TypeChanged?.Invoke(this, tagType); //Delegate
            };
        }

    }
}
