using System.Drawing;

namespace OSUtils.TaskbarUtils
{
    public class MockTaskbarItem : ITaskbarItem
    {
        public double Progress { get; set; }

        public ITaskbarItem SetOverlayIcon(Icon icon, string accessibilityText)
        {
            return this;
        }

        public ITaskbarItem NoProgress()
        {
            return this;
        }

        public ITaskbarItem Indeterminate()
        {
            return this;
        }

        public ITaskbarItem Normal()
        {
            return this;
        }

        public ITaskbarItem Error()
        {
            return this;
        }

        public ITaskbarItem Pause()
        {
            return this;
        }

        public ITaskbarItem SetProgress(double percent)
        {
            Progress = percent;
            return this;
        }
    }
}
