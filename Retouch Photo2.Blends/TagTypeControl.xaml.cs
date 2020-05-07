using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Blends
{
    /// <summary>
    /// Retouch_Photo2 Tag 's Control.
    /// </summary>
    public sealed partial class TagTypeControl : UserControl
    {
        //@Delegate
        public EventHandler<TagType> TypeChanged;

        //@VisualState
        TagType _vsTagType;
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
            get { return (TagType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "TagTypeControl.Type" /> dependency property. </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(TagType), typeof(TagTypeControl), new PropertyMetadata(TagType.None, (sender, e) =>
        {
            TagTypeControl con = (TagTypeControl)sender;

            if (e.NewValue is TagType value)
            {
                con._vsTagType = value;
                con.VisualState = con.VisualState;//State
            }
        }));


        #endregion


        //@Construct
        public TagTypeControl()
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
            Color color = TagTypeHelper.TagConverter(tagType);

            radioButton.Background = new SolidColorBrush(color);
            radioButton.Tapped += (s, e) =>
            {
                this.TypeChanged?.Invoke(this, tagType); //Delegate
            };
        }

    }
}
