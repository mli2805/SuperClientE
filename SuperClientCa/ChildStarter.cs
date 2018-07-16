using System;
using System.Diagnostics;

namespace SuperClientCa
{
    public class ChildStarter
    {
        const string ClientFilename = @"c:\VsGitProjects\SuperClientE\LittleClient\bin\Debug\LittleClient";
        //        const string ClientFilename = @"c:\VsGitProjects\Fibertest\Client\WpfClient\bin\Debug\Iit.Fibertest.Client.exe";

      


        public Process StartChild(int number, string serverIp, string serverPort)
        {
            var process = new Process
            {
                StartInfo = {
                    FileName = ClientFilename,
                    Arguments = $"{number} superclient superclient {serverIp} {serverPort}"
                }
            };
            process.Start();
            //            var pause = number == 3 ? 45 : 10;
            var pause = 2;
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(pause));
            return process;
       //     return process.MainWindowHandle;
        }
    }
}
