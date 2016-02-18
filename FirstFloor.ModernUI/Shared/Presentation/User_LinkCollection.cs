using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FirstFloor.ModernUI.Presentation
{
    /// <summary>
    /// Represents an observable collection of links.
    /// </summary>
    public class User_LinkCollection
        : ObservableCollection<User_Link>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User_LinkCollection"/> class.
        /// </summary>
        public User_LinkCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User_LinkCollection"/> class that contains specified links.
        /// </summary>
        /// <param name="links">The links that are copied to this collection.</param>
        public User_LinkCollection(IEnumerable<User_Link> links)
        {
            if (links == null) {
                throw new ArgumentNullException("User_Link");
            }
            foreach (var link in links) {
                Add(link);
            }
        }
    }
}
