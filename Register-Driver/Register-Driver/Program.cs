using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace Register_Driver
{
    internal class Program
    {
        public static string RuntimePath = "";
        public static string K2EXPath = "";
        static void Main(string[] args)
        {
            Console.Title = "K2EX Driver Registration";
            Console.WriteLine("K2EX Driver Registration by AuroraNemoia");
            Console.WriteLine("Source is available at https://github.com/KinectToVR/K2EX-Driver-Registration");
            Console.WriteLine("");
            // Find SteamVR path from running process
            try
            {
                var process = Process.GetProcessesByName("vrserver").First();
                string VRServerPath = process.MainModule.FileName;
                RuntimePath = System.IO.Path.GetDirectoryName(VRServerPath);
                Console.WriteLine($"Found SteamVR at {RuntimePath}");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Could not find SteamVR runtime path!\n\nRelaunch this while SteamVR and KinectToVR are both running.\n\n{e.Message}\n{e.StackTrace}", "K2EX Driver Registration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Find K2EX path
            try
            {
                var process = Process.GetProcessesByName("kinectv1process").First();
                if (process == null)
                {
                    process = Process.GetProcessesByName("kinectv2process").First();
                    if (process == null)
                    {
                        process = Process.GetProcessesByName("psmsprocess").First();
                    }
                }
                string K2EXProcessPath = process.MainModule.FileName;
                K2EXPath = System.IO.Path.GetDirectoryName(K2EXProcessPath);
                Console.WriteLine($"Found K2EX at {K2EXPath}");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Could not find K2EX install path!\n\nRelaunch this while SteamVR and KinectToVR are both running.\n\n{e.Message}\n{e.StackTrace}", "K2EX Driver Registration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var SteamVR = Process.GetProcessesByName("vrmonitor").First();
            SteamVR.Kill();
            Process VRPathReg = new Process();
            VRPathReg.StartInfo.FileName = "vrpathreg.exe";
            VRPathReg.StartInfo.WorkingDirectory = RuntimePath;
            VRPathReg.StartInfo.Arguments = $"adddriver {K2EXPath}\\KinectToVR";
            VRPathReg.StartInfo.CreateNoWindow = true;
            VRPathReg.Start();
            MessageBox.Show("Driver registration complete! Please relaunch SteamVR.", "K2EX Driver Registration", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
