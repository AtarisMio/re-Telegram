using System;
using System.Windows.Media;

namespace FirstFloor.ModernUI.Presentation
{
    /// <summary>
    /// Represents a displayable link.
    /// </summary>
    public class User_Link
        : Displayable
    {
        private Uri source;
        private ImageSource img;
        /// <summary>
        /// Gets or sets the source uri.
        /// </summary>
        /// <value>The source.</value>
        public Uri Source
        {
            get { return this.source; }
            set
            {
                if (this.source != value) {
                    this.source = value;
                    OnPropertyChanged("Source");
                }
            }
        }
        /// <summary>
        /// Gets or sets the img uri.
        /// </summary>
        /// <value>The source.</value>
        public ImageSource ImgSrc
        {
            get
            {
                return img;
            }

            set
            {
                img = value;
            }
        }
    }
}
