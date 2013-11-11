using System.Collections.Generic;
using WindowsOSUtils.DriveDetector;
using WindowsOSUtils.JobObjects;
using Ninject.Modules;
using OSUtils.DriveDetector;
using OSUtils.JobObjects;

namespace WindowsOSUtils
{
    public static class WindowsInjectorFactory
    {
        public static IEnumerable<INinjectModule> CreateMainModules()
        {
            return new INinjectModule[] { new BDHeroModule(), new DriveDetectorModule() };
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
}
