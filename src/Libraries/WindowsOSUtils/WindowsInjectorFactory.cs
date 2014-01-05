using System.Collections.Generic;
using WindowsOSUtils.DriveDetector;
using WindowsOSUtils.JobObjects;
using WindowsOSUtils.TaskbarUtils;
using Ninject.Modules;
using OSUtils.DriveDetector;
using OSUtils.JobObjects;
using OSUtils.TaskbarUtils;

namespace WindowsOSUtils
{
    public static class WindowsInjectorFactory
    {
        public static IEnumerable<INinjectModule> CreateMainModules()
        {
            return new INinjectModule[]
                   {
                       new BDHeroModule(),
                       new DriveDetectorModule(),
                       new TaskbarModule()
                   };
        }
    }

    internal class BDHeroModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IJobObject>().To<JobObject>();
            Bind<IJobObjectManager>().To<JobObjectManager>();
        }
    }

    internal class DriveDetectorModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDriveDetector>().To<WindowsDriveDetector>();
        }
    }

    internal class TaskbarModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ITaskbarItemFactory>().To<WindowsTaskbarItemFactory>();
        }
    }
}
