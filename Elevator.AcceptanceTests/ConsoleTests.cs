using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Elevator.AcceptanceTests
{
    class ConsoleTests
    {
        [TestFixture]
        public class When_running
        {
            [Test]
            public void bla()
            {
                var process = new Process();
                process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                process.StartInfo.FileName = "Elevator.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;

                process.Start();
                
                using (var sr = process.StandardOutput)
                {
                    Console.WriteLine(sr.ReadToEnd());
                }
            }
        }

    }
}
