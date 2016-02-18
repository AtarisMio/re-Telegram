using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Telegram.Pages.Controls
{
    public class PopoChat : Control, INotifyPropertyChanged
    {
        /// <summary>
        /// Identifies the PopoWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty PopoWidthProperty = DependencyProperty.Register("PopoWidth", typeof(GridLength), typeof(PopoChat), new PropertyMetadata(new GridLength(320)));
        /// <summary>
        /// Identifies the PopoHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty PopoHeightProperty = DependencyProperty.Register("PopoHeight", typeof(GridLength), typeof(PopoChat), new PropertyMetadata(new GridLength(110)));
        /// <summary>
        /// Identifies the isLeft dependency property.
        /// </summary>
        public static readonly DependencyProperty isLeftProperty = DependencyProperty.Register("isLeft", typeof(ChatLayout), typeof(PopoChat), new PropertyMetadata(ChatLayout.Others));
        /// <summary>
        /// Identifies the PopoFigure dependency property.
        /// </summary>
        public static readonly DependencyProperty PopoFigureProperty = DependencyProperty.Register("PopoFigure", typeof(Geometry), typeof(PopoChat), new PropertyMetadata(Geometry.Parse("M10,10 c 0,-5.523 4.477,-10 10,-10 h 230 c 5.523,0 10,4.477 10,10 v 70 c 0,5.523 -4.477,10 -10,10 h -230 c -5.523,0 -10,-4.477 -10,-10 v -60 l -10,-10 z")));

        //private Geometry PopoFigureProperty;
        private ImageSource IconProperty;
        private string PopoTextProperty;

        public event PropertyChangedEventHandler PropertyChanged;

        public PopoChat()
        {
            this.DefaultStyleKey = typeof(PopoChat);
            Icon = new BitmapImage(new Uri(@"C:\Users\mio51\Source\Repos\re-Telegram\Telegram\Asset\icon.jpg"));
            this.isLeft = ChatLayout.Me;
        }

        public PopoChat(string text)
        {
            Icon = new BitmapImage(new Uri(@"C:\Users\mio51\Source\Repos\re-Telegram\Telegram\Asset\icon.jpg"));
            this.isLeft = ChatLayout.Me;
            this.PopoTextProperty = text;
        }

        public GridLength PopoWidth
        {
            get
            {
                return (GridLength)GetValue(PopoWidthProperty);
            }

            set
            {
                SetValue(PopoWidthProperty, value);
            }
        }
        public GridLength PopoHeight
        {
            get
            {
                return (GridLength)GetValue(PopoHeightProperty);
            }

            set
            {
                SetValue(PopoHeightProperty, value);
            }
        }
        public ImageSource Icon
        {
            get
            {
                return IconProperty;
                //return (ImageSource)GetValue(IconProperty);
            }

            set
            {
                if (this.IconProperty != value)
                {
                    this.IconProperty = value;
                    OnPropertyChanged("Icon");
                }
                //SetValue(IconProperty, value);
            }
        }
        public ChatLayout isLeft
        {
            get
            {
                return (ChatLayout)GetValue(isLeftProperty);
            }

            set
            {
                SetValue(isLeftProperty, value);
            }
        }

        //public string PopoText
        //{
        //    get
        //    {
        //        return (string)GetValue(PopoTextProperty);
        //    }

        //    set
        //    {
        //        SetValue(PopoTextProperty, value);
        //    }
        //}

        public string PopoText
        {
            get { return this.PopoTextProperty; }
            set
            {
                if (this.PopoTextProperty != value)
                {
                    this.PopoTextProperty = value;
                    //this.CreateGraphics()
                    //PopoFigure
                    //PopoFigure = (new PathGeometry() { Figures = (new PathFigureCollection())"M 260, 90 c 0, 5.523 -4.477, 10 -10, 10 h -230 c -5.523, 0 -10, -4.477 -10, -10 v -60 l -10, -10 l 10, 0 v -5 c 0, -5.523 4.477, -10 10, -10 h 230 c 5.523, 0 10, 4.477 10, 10 v 70 z" };
                    OnPropertyChanged("PopoText");
                }
            }
        }
        

        public Geometry PopoFigure
        {
            get
            {
                return (Geometry)GetValue(PopoFigureProperty);
            }

            set
            {
                SetValue(PopoFigureProperty, value);
                //if (this.PopoFigureProperty != value)
                //{
                //    this.PopoFigureProperty = value;
                //    OnPropertyChanged("PopoFigure");
                //}
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    public enum ChatLayout
    {
        Others, Me
    }
}
