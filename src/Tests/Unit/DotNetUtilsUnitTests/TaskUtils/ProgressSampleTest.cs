using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DotNetUtils.TaskUtils;
using NUnit.Framework;

namespace DotNetUtilsUnitTests.TaskUtils
{
    [TestFixture]
    public class ProgressSampleTest
    {
        [Test]
        public void TestContinuousLinear()
        {
            var sample = new ProgressSample();

            for (var i = 0; i <= 10; i++)
            {
                var percentComplete = 10.0 * i;
                sample.Add(percentComplete);
                Console.WriteLine("{0}%: {1} remaining", percentComplete, sample.EstimatedTimeRemaining);
                Thread.Sleep(1000);
            }
        }

        [Test]
        public void TestStalled()
        {
            var sample = new ProgressSample();

            var percentComplete = 0.0;

            for (var i = 0; i <= 5; i++)
            {
                percentComplete = 10.0 * i;
                sample.Add(percentComplete);
                Console.WriteLine("{0}%: {1} remaining", percentComplete, sample.EstimatedTimeRemaining);
                Thread.Sleep(1000);
            }

            for (var i = 6; i <= 20; i++)
            {
                Console.WriteLine("{0}%: {1} remaining", percentComplete, sample.EstimatedTimeRemaining);
                Thread.Sleep(1000);
            }
        }

        [Test]
        public void TestStallFollowedByResume()
        {
            var sample = new ProgressSample();

            var percentComplete = 0.0;

            for (var i = 0; i <= 5; i++)
            {
                percentComplete = 10.0 * i;
                sample.Add(percentComplete);
                Console.WriteLine("{0}%: {1} remaining", percentComplete, sample.EstimatedTimeRemaining);
                Thread.Sleep(1000);
            }

            for (var i = 0; i <= 5; i++)
            {
                Console.WriteLine("{0}%: {1} remaining", percentComplete, sample.EstimatedTimeRemaining);
                Thread.Sleep(1000);
            }

            for (var i = 6; i <= 10; i++)
            {
                percentComplete = 10.0 * i;
                sample.Add(percentComplete);
                Console.WriteLine("{0}%: {1} remaining", percentComplete, sample.EstimatedTimeRemaining);
                Thread.Sleep(1000);
            }
        }

        [Test]
        public void TestStallFollowedByResumeMicro1()
        {
            var sample = new ProgressSample();

            var percentComplete = 0.0;

            var lastTick = DateTime.MinValue;

            var tick = new Action(delegate
                                  {
                                      if (DateTime.Now - lastTick < TimeSpan.FromSeconds(1)) { return; }
                                      Console.WriteLine("{0}%: {1} remaining", percentComplete,
                                                        sample.EstimatedTimeRemaining);
                                      lastTick = DateTime.Now;
                                  });

            for (var i = 0; i <= 25; i++)
            {
                percentComplete = i;
                sample.Add(percentComplete);
                tick();
                Thread.Sleep(250);
            }

            for (var i = 0; i <= 15; i++)
            {
                tick();
                Thread.Sleep(250);
            }

            for (var i = 26; i <= 67; i++)
            {
                percentComplete = i;
                sample.Add(percentComplete);
                tick();
                Thread.Sleep(250);
            }

            for (var i = 0; i <= 10; i++)
            {
                tick();
                Thread.Sleep(250);
            }

            for (var i = 68; i <= 97; i++)
            {
                percentComplete = i;
                sample.Add(percentComplete);
                tick();
                Thread.Sleep(250);
            }

            for (var i = 0; i <= 20; i++)
            {
                tick();
                Thread.Sleep(250);
            }

            for (var i = 98; i <= 100; i++)
            {
                percentComplete = i;
                sample.Add(percentComplete);
                tick();
                Thread.Sleep(250);
            }

            lastTick = DateTime.MinValue;
            tick();
        }

        [Test]
        public void TestStallFollowedByResumeMicro2()
        {
            var sample = new ProgressSample();

            var percentComplete = 0.0;

            var lastTick = DateTime.MinValue;

            var tick = new Action(delegate
                                  {
                                      if (DateTime.Now - lastTick < TimeSpan.FromSeconds(1)) { return; }
                                      Console.WriteLine("{0}%: {1} remaining", percentComplete,
                                                        sample.EstimatedTimeRemaining);
                                      lastTick = DateTime.Now;
                                  });

            for (var i = 0; i <= 25; i++)
            {
                percentComplete = i;
                sample.Add(percentComplete);
                tick();
                Thread.Sleep(250);
            }

            for (var i = 0; i <= 15; i++)
            {
                tick();
                Thread.Sleep(250);
            }

            for (var i = 26; i <= 67; i++)
            {
                percentComplete = i;
                sample.Add(percentComplete);
                tick();
                Thread.Sleep(250);
            }

            for (var i = 0; i <= 10; i++)
            {
                tick();
                Thread.Sleep(250);
            }

            for (var i = 68; i <= 97; i++)
            {
                percentComplete = i;
                sample.Add(percentComplete);
                tick();
                Thread.Sleep(250);
            }

//            for (var i = 0; i <= 20; i++)
//            {
//                tick();
//                Thread.Sleep(250);
//            }

            for (var i = 98; i <= 100; i++)
            {
                percentComplete = i;
                sample.Add(percentComplete);
                tick();
                Thread.Sleep(250);
            }

            lastTick = DateTime.MinValue;
            tick();
        }

        [Test]
        public void TestStallFollowedByResumeMicro3()
        {
            var sample = new ProgressSample();

            var percentComplete = 0.0;

            var lastTick = DateTime.MinValue;

            var tick = new Action(delegate
                                  {
                                      if (DateTime.Now - lastTick < TimeSpan.FromSeconds(1)) { return; }
                                      Console.WriteLine("{0}%: {1} remaining", percentComplete,
                                                        sample.EstimatedTimeRemaining);
                                      lastTick = DateTime.Now;
                                  });

            for (var i = 0; i <= 25; i++)
            {
                percentComplete = i;
                sample.Add(percentComplete);
                tick();
                Thread.Sleep(250);
            }

            for (var i = 0; i <= 15; i++)
            {
                tick();
                Thread.Sleep(250);
            }

            for (var i = 26; i <= 67; i++)
            {
                percentComplete = i;
                sample.Add(percentComplete);
                tick();
                Thread.Sleep(250);
            }

            for (var i = 0; i <= 30; i++)
            {
                tick();
                Thread.Sleep(250);
            }

            for (var i = 68; i <= 97; i++)
            {
                percentComplete = i;
                sample.Add(percentComplete);
                tick();
                Thread.Sleep(250);
            }

            for (var i = 0; i <= 40; i++)
            {
                tick();
                Thread.Sleep(250);
            }

            for (var i = 98; i <= 100; i++)
            {
                percentComplete = i;
                sample.Add(percentComplete);
                tick();
                Thread.Sleep(250);
            }

            lastTick = DateTime.MinValue;
            tick();
        }
    }
}
