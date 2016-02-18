using FirstFloor.ModernUI.Presentation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FirstFloor.ModernUI.Windows.Controls
{
    /// <summary>
    /// Represents a control that contains multiple pages that share the same space on screen.
    /// </summary>
    public class UserList
        : Control
    {
        /// <summary>
        /// Identifies the ContentLoader dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentLoaderProperty = DependencyProperty.Register("ContentLoader", typeof(IContentLoader), typeof(UserList), new PropertyMetadata(new DefaultContentLoader()));
        /// <summary>
        /// Identifies the Layout dependency property.
        /// </summary>
        public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register("Layout", typeof(TabLayout), typeof(UserList), new PropertyMetadata(TabLayout.Tab));
        /// <summary>
        /// Identifies the ListWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty ListWidthProperty = DependencyProperty.Register("ListWidth", typeof(GridLength), typeof(UserList), new PropertyMetadata(new GridLength(170)));
        /// <summary>
        /// Identifies the MyLinks dependency property.
        /// </summary>
        public static readonly DependencyProperty MyLinksProperty = DependencyProperty.Register("MyLinks", typeof(MyMyLinkCollection), typeof(UserList), new PropertyMetadata(OnMyLinksChanged));
        /// <summary>
        /// Identifies the SelectedSource dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedSourceProperty = DependencyProperty.Register("SelectedSource", typeof(Uri), typeof(UserList), new PropertyMetadata(OnSelectedSourceChanged));

        /// <summary>
        /// Occurs when the selected source has changed.
        /// </summary>
        public event EventHandler<SourceEventArgs> SelectedSourceChanged;

        private ListBox MyLinkList;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModernTab"/> control.
        /// </summary>
        public UserList()
        {
            this.DefaultStyleKey = typeof(UserList);

            // create a default MyLinks collection
            SetCurrentValue(MyLinksProperty, new MyMyLinkCollection());
        }

        private static void OnMyLinksChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((UserList)o).UpdateSelection();
        }

        private static void OnSelectedSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((UserList)o).OnSelectedSourceChanged((Uri)e.OldValue, (Uri)e.NewValue);
        }

        private void OnSelectedSourceChanged(Uri oldValue, Uri newValue)
        {
            UpdateSelection();

            // raise SelectedSourceChanged event
            var handler = this.SelectedSourceChanged;
            if (handler != null)
            {
                handler(this, new SourceEventArgs(newValue));
            }
        }

        private void UpdateSelection()
        {
            if (this.MyLinkList == null || this.MyLinks == null)
            {
                return;
            }

            // sync list selection with current source
            this.MyLinkList.SelectedItem = this.MyLinks.FirstOrDefault(l => l.Source == this.SelectedSource);
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call System.Windows.FrameworkElement.ApplyTemplate().
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.MyLinkList != null)
            {
                this.MyLinkList.SelectionChanged -= OnMyLinkListSelectionChanged;
            }

            this.MyLinkList = GetTemplateChild("MyLinkList") as ListBox;
            if (this.MyLinkList != null)
            {
                this.MyLinkList.SelectionChanged += OnMyLinkListSelectionChanged;
            }

            UpdateSelection();
        }

        private void OnMyLinkListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var MyLink = this.MyLinkList.SelectedItem as MyLink;
            if (MyLink != null && MyLink.Source != this.SelectedSource)
            {
                SetCurrentValue(SelectedSourceProperty, MyLink.Source);
            }
        }

        /// <summary>
        /// Gets or sets the content loader.
        /// </summary>
        public IContentLoader ContentLoader
        {
            get { return (IContentLoader)GetValue(ContentLoaderProperty); }
            set { SetValue(ContentLoaderProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating how the tab should be rendered.
        /// </summary>
        public TabLayout Layout
        {
            get { return (TabLayout)GetValue(LayoutProperty); }
            set { SetValue(LayoutProperty, value); }
        }

        /// <summary>
        /// Gets or sets the collection of MyLinks that define the available content in this tab.
        /// </summary>
        public MyMyLinkCollection MyLinks
        {
            get { return (MyMyLinkCollection)GetValue(MyLinksProperty); }
            set { SetValue(MyLinksProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the list when Layout is set to List.
        /// </summary>
        /// <value>
        /// The width of the list.
        /// </value>
        public GridLength ListWidth
        {
            get { return (GridLength)GetValue(ListWidthProperty); }
            set { SetValue(ListWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the source URI of the selected MyLink.
        /// </summary>
        /// <value>The source URI of the selected MyLink.</value>
        public Uri SelectedSource
        {
            get { return (Uri)GetValue(SelectedSourceProperty); }
            set { SetValue(SelectedSourceProperty, value); }
        }
    }
}
namespace FirstFloor.ModernUI.Presentation
{ 
    public class MyMyLinkCollection
    : ObservableCollection<MyLink>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyMyLinkCollection"/> class.
        /// </summary>
        public MyMyLinkCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MyMyLinkCollection"/> class that contains specified MyLinks.
        /// </summary>
        /// <param name="MyLinks">The MyLinks that are copied to this collection.</param>
        public MyMyLinkCollection(IEnumerable<MyLink> MyLinks)
        {
            if (MyLinks == null)
            {
                throw new ArgumentNullException("MyLinks");
            }
            foreach (var MyLink in MyLinks)
            {
                Add(MyLink);
            }
        }
    }
    public class MyLink
        : Displayable
    {
        private Uri source;

        /// <summary>
        /// Gets or sets the source uri.
        /// </summary>
        /// <value>The source.</value>
        public Uri Source
        {
            get { return this.source; }
            set
            {
                if (this.source != value)
                {
                    this.source = value;
                    OnPropertyChanged("Source");
                }
            }
        }
    }
}