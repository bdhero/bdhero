using System;
using System.Collections.Generic;
using System.Net;
using BDHero.ErrorReporting;
using BDHero.Plugin;
using BDHero.Startup;
using NUnit.Framework;

namespace DotNetTests
{
    [TestFixture]
    public class ErrorReporterTest
    {
        [Test]
        public void TestThrowsWin32Exception()
        {
            try
            {
                throw new WebException("Blah blah blah");
            }
            catch (Exception e)
            {
                ErrorReporter.Report(new ErrorReport(e, new MockPluginRepository(), new MockDirectoryLocator()));
            }
        }
    }

    class MockPluginRepository : IPluginRepository
    {
        public MockPluginRepository()
        {
            Count = 0;
            PluginsByType = new IPlugin[0];
            DiscReaderPlugins = new IDiscReaderPlugin[0];
            MetadataProviderPlugins = new IMetadataProviderPlugin[0];
            AutoDetectorPlugins = new IAutoDetectorPlugin[0];
            NameProviderPlugins = new INameProviderPlugin[0];
            MuxerPlugins = new IMuxerPlugin[0];
            PostProcessorPlugins = new IPostProcessorPlugin[0];
        }

        public void ReportProgress(IPlugin plugin, double percentComplete, string status)
        {
        }

        public ProgressProvider GetProgressProvider(IPlugin plugin)
        {
            return null;
        }

        public int Count { get; private set; }

        public IList<IPlugin> PluginsByType { get; private set; }
        public IList<IDiscReaderPlugin> DiscReaderPlugins { get; private set; }
        public IList<IMetadataProviderPlugin> MetadataProviderPlugins { get; private set; }
        public IList<IAutoDetectorPlugin> AutoDetectorPlugins { get; private set; }
        public IList<INameProviderPlugin> NameProviderPlugins { get; private set; }
        public IList<IMuxerPlugin> MuxerPlugins { get; private set; }
        public IList<IPostProcessorPlugin> PostProcessorPlugins { get; private set; }

        public void Clear()
        {
        }

        public void Add(IPlugin plugin)
        {
        }
    }

    class MockDirectoryLocator : IDirectoryLocator
    {
        public bool IsPortable { get; private set; }
        public string InstallDir { get; private set; }
        public string AppConfigDir { get; private set; }
        public string PluginConfigDir { get; private set; }
        public string RequiredPluginDir { get; private set; }
        public string CustomPluginDir { get; private set; }
        public string LogDir { get; private set; }
    }
}
