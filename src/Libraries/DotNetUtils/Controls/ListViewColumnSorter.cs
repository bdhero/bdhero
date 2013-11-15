using System.Collections;
using System.Windows.Forms;

namespace DotNetUtils.Controls
{
    /// <summary>
    ///     This class is an implementation of the 'IComparer' interface.
    /// </summary>
    /// <seealso cref="http://support.microsoft.com/kb/319401" />
    public class ListViewColumnSorter : IComparer
    {
        /// <summary>
        ///     Case insensitive comparer object
        /// </summary>
        private readonly CaseInsensitiveComparer _objectCompare;

        /// <summary>
        ///     Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        private SortOrder _orderOfSort;

        /// <summary>
        ///     Specifies the column to be sorted
        /// </summary>
        private int _columnToSort;

        /// <summary>
        ///     Class constructor.  Initializes various elements
        /// </summary>
        public ListViewColumnSorter()
        {
            // Initialize the column to '0'
            _columnToSort = 0;

            // Initialize the sort order to 'none'
            _orderOfSort = SortOrder.None;

            // Initialize the CaseInsensitiveComparer object
            _objectCompare = new CaseInsensitiveComparer();
        }

        /// <summary>
        ///     Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn
        {
            set { _columnToSort = value; }
            get { return _columnToSort; }
        }

        /// <summary>
        ///     Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order
        {
            set { _orderOfSort = value; }
            get { return _orderOfSort; }
        }

        /// <summary>
        ///     This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive
        ///     comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>
        ///     The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater
        ///     than 'y'
        /// </returns>
        public int Compare(object x, object y)
        {
            // Cast the objects to be compared to ListViewItem objects
            var listviewX = (ListViewItem) x;
            var listviewY = (ListViewItem) y;

            // Compare the two items
            var itemX = listviewX.SubItems[_columnToSort];
            var itemY = listviewY.SubItems[_columnToSort];

            var objectX = itemX.Tag ?? itemX.Text;
            var objectY = itemY.Tag ?? itemY.Text;

            int compareResult = _objectCompare.Compare(objectX, objectY);

            // Calculate correct return value based on object comparison
            if (_orderOfSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return +compareResult;
            }

            if (_orderOfSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return -compareResult;
            }

            // Return '0' to indicate they are equal
            return 0;
        }
    }
}
