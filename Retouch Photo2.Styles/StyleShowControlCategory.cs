// Core:              ★★★★
// Referenced:   ★★★★
// Difficult:         
// Only:              ★★★
// Complete:      ★
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Text;

namespace Retouch_Photo2.Styles
{
    /// <summary>
    /// Controls of <see cref="StyleCategory"/>
    /// </summary>
    public class StyleShowControlCategory : ObservableCollection<StyleShowControl>
    {

        /// <summary> Gets the title. </summary>
        public string Title { get; private set; } = string.Empty;
        public FontWeight Weight { get; private set; } = FontWeights.Normal;

        /// <summary> <see cref="StyleCategory.Name"/> </summary>
        public string Name { get; set; }

        /// <summary> <see cref="StyleCategory.Strings"/> </summary>
        public IDictionary<string, string> Strings { get; private set; }

        //@Construct
        /// <summary>
        /// Initializes a StyleShowControlCategory. 
        /// </summary>
        public StyleShowControlCategory() : base() { }
        /// <summary>
        /// Initializes a StyleShowControlCategory. 
        /// </summary>
        /// <param name="StyleCategory"> The source <see cref="StyleCategory"/>. </param>
        public StyleShowControlCategory(StyleCategory StyleCategory) : base
        (
            from style
            in StyleCategory.Styles
            select new StyleShowControl
            {
                Style2 = style
            }
        )
        {
            this.Name = StyleCategory.Name;
            this.Strings = StyleCategory.Strings;
        }

        public void Rename(string language)
        {
            this.Title = Retouch_Photo2.Elements.XML.CreateString(this.Name, this.Strings, language);
            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(Title))); // Notify
            this.Weight = this.Strings == null ? FontWeights.Normal : FontWeights.Bold;
            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(Weight))); // Notify
        }

        public StyleCategory ToStyleCategory() => new StyleCategory
        {
            Name = this.Name,
            Strings = this.Strings,
            Styles =
            (
                from control
                in this
                select control.Style2
            )
        };

    }
}