namespace OSUtils.Window
{
    public interface IWindowMenu
    {
        void AppendMenu(IWindowMenuItem menuItem);

        void AppendSeparator();

        void InsertMenu(uint position, IWindowMenuItem menuItem);

        void InsertSeparator(uint position);

        void UpdateMenu(IWindowMenuItem menuItem);
    }
}
