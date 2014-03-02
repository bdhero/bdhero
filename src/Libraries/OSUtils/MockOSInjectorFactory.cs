// Copyright 2012-2014 Andrew C. Dvorak
//
// This file is part of BDHero.
//
// BDHero is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BDHero is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with BDHero.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using DotNetUtils.Annotations;
using Ninject.Modules;
using OSUtils.DriveDetector;
using OSUtils.JobObjects;
using OSUtils.TaskbarUtils;
using OSUtils.Window;

namespace OSUtils
{
    public static class MockOSInjectorFactory
    {
        public static IEnumerable<INinjectModule> CreateMainModules()
        {
            return new INinjectModule[]
                   {
                       new MockJobObjectModule(),
                       new MockDriveDetectorModule(),
                       new MockTaskbarModule(),
                       new MockWindowMenuModule()
                   };
        }
    }

    internal class MockJobObjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IJobObject>().To<MockJobObject>();
            Bind<IJobObjectManager>().To<MockJobObjectManager>();
        }

        #region Mock interface implementations

        [UsedImplicitly]
        private class MockJobObject : IJobObject
        {
            public void Dispose()
            {
            }

            public void Assign(Process process)
            {
            }

            public void KillOnClose()
            {
            }
        }

        [UsedImplicitly]
        private class MockJobObjectManager : IJobObjectManager
        {
            public IJobObject CreateJobObject()
            {
                return new MockJobObject();
            }

            public bool IsAssignedToJob(Process process)
            {
                return false;
            }

            public bool TryBypassPCA(string[] args)
            {
                return false;
            }
        }

        #endregion
    }

    internal class MockDriveDetectorModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDriveDetector>().To<MockDriveDetector>();
        }

        #region Mock interface implementations

        [UsedImplicitly]
        private class MockDriveDetector : IDriveDetector
        {
            public event DriveDetectorEventHandler DeviceArrived;
            public event DriveDetectorEventHandler DeviceRemoved;

            public void WndProc(ref Message m)
            {
            }
        }

        #endregion
    }

    internal class MockTaskbarModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ITaskbarItemFactory>().To<MockTaskbarItemFactory>();
        }

        #region Mock interface implementations

        [UsedImplicitly]
        private class MockTaskbarItemFactory : ITaskbarItemFactory
        {
            private readonly ConcurrentDictionary<IntPtr, ITaskbarItem> _taskbarItems =
                new ConcurrentDictionary<IntPtr, ITaskbarItem>();

            public ITaskbarItem GetInstance(IntPtr windowHandle)
            {
                return _taskbarItems.GetOrAdd(windowHandle, ValueFactory);
            }

            private static ITaskbarItem ValueFactory(IntPtr windowHandle)
            {
                return new MockTaskbarItem();
            }
        }

        #endregion
    }

    internal class MockWindowMenuModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IWindowMenuFactory>().To<MockWindowMenuFactory>();
        }

        #region Mock interface implementations

        [UsedImplicitly]
        private class MockWindowMenuFactory : IWindowMenuFactory
        {
            private uint _menuItemIdCounter = 0x1;

            public IWindowMenu CreateMenu(Form form)
            {
                return new MockWindowMenu();
            }

            public IWindowMenuItem CreateMenuItem(string text = null, EventHandler clickHandler = null)
            {
                return new MockWindowMenuItem(_menuItemIdCounter++) { Text = text };
            }
        }

        [UsedImplicitly]
        private class MockWindowMenu : IWindowMenu
        {
            public void AppendMenu(IWindowMenuItem menuItem)
            {
            }

            public void AppendSeparator()
            {
            }

            public void InsertMenu(uint position, IWindowMenuItem menuItem)
            {
            }

            public void InsertSeparator(uint position)
            {
            }

            public void UpdateMenu(IWindowMenuItem menuItem)
            {
            }
        }

        [UsedImplicitly]
        private class MockWindowMenuItem : IWindowMenuItem
        {
            public uint Id { get; private set; }
            
            public string Text { get; set; }
            
            public bool Enabled { get; set; }
            
            public bool Checked { get; set; }
            
            public event EventHandler Clicked;

            public MockWindowMenuItem(uint id)
            {
                Id = id;
                Enabled = true;
                Checked = false;
            }

            public void Click(EventArgs eventArgs)
            {
            }
        }

        #endregion
    }
}
