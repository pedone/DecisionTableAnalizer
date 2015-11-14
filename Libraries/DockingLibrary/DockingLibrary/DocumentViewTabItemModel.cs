using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DockingLibrary
{
    internal class DocumentViewTabItemModel
    {

        #region Variables

        public View View { get; private set; }
        public DocumentGroup DocumentGroup { get; private set; }

        #endregion

        public DocumentViewTabItemModel(View view, DocumentGroup parent)
        {
            if (view == null)
                throw new ArgumentNullException("view", "view is null.");
            if (parent == null)
                throw new ArgumentNullException("parent", "parent is null.");
            if (view.ParentContent != parent)
                throw new ArgumentNullException("parent", "view in not child of parent.");

            View = view;
            DocumentGroup = parent;
        }

    }
}
