using DockingLibrary.Commands;
namespace DockingLibrary
{
    internal static class DockingCommands
    {
        public static DockingCloseCommand Close = new DockingCloseCommand();
        public static DockingAutoHideCommand AutoHide = new DockingAutoHideCommand();
        public static ShowOpenDocumentViewListCommand ShowOpenDocuments = new ShowOpenDocumentViewListCommand();
        public static ShowHiddenDocumentViewCommand ShowHiddenDocument = new ShowHiddenDocumentViewCommand();
    }
}
