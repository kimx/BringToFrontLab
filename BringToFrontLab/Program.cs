using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BringToFrontLab
{
    //1.http://stackoverflow.com/questions/697058/single-instance-windows-forms-application-and-how-to-get-reference-on-it
    //2.http://stackoverflow.com/questions/2636721/bring-another-processes-window-to-foreground-when-it-has-showintaskbar-false
    /// <summary>
    /// Find By AnotherProcess
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (Mutex mutex = new Mutex(false, "MyAppForm"))
            {
                if (!mutex.WaitOne(0, true))
                {
                    bringToFront();
                }
                else
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());
                }
            }

        }


        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("USER32.DLL")]
        public static extern bool ShowWindow(IntPtr hWnd, int i);

        public static void bringToFront()
        {

            Process otherProcess = FindOtherProcess();
            IntPtr handle = otherProcess.MainWindowHandle;
            //放大
            ShowWindow(handle, 1);
            //設為焦點
            SetForegroundWindow(handle);

        }

        public static Process FindOtherProcess()
        {
            Process thisProcess = Process.GetCurrentProcess();
            Process[] allProcesses = Process.GetProcessesByName(thisProcess.ProcessName);
            foreach (Process p in allProcesses)
            {
                if ((p.Id != thisProcess.Id) && (p.MainModule.FileName == thisProcess.MainModule.FileName))
                    return p;
            }
            return null;
        }
    }

    ///// <summary>
    ///// Find By FormName
    ///// </summary>
    //public static class Program
    //{
    //    /// <summary>
    //    /// 應用程式的主要進入點。
    //    /// </summary>
    //    [STAThread]
    //   public static void Main()
    //    {
    //        using (Mutex mutex = new Mutex(false, "Form1"))
    //        {
    //            if (!mutex.WaitOne(0, true))
    //            {
    //                bringToFront("Form1");
    //            }
    //            else
    //            {
    //                Application.EnableVisualStyles();
    //                Application.SetCompatibleTextRenderingDefault(false);
    //                Application.Run(new Form1());
    //            }
    //        }
      
    //    }

    //    [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
    //    public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);

    //    [DllImport("USER32.DLL")]
    //    public static extern bool SetForegroundWindow(IntPtr hWnd);

    //     [DllImport("USER32.DLL")]
    //    public static extern bool ShowWindow(IntPtr hWnd,int i);

    //    public static void bringToFront(string title)
    //    {
    //        // Get a handle to the Calculator application.
    //        IntPtr handle = FindWindow(null, title);

    //        // Verify that Calculator is a running process.
    //        if (handle == IntPtr.Zero)
    //        {
    //            return;
    //        }
    //        ShowWindow(handle, 1);
    //        // Make Calculator the foreground application
    //        SetForegroundWindow(handle);
    //    }

    //    public static Process FindOtherProcess() {

    //        Process thisProcess = Process.GetCurrentProcess();
    //        Process[] allProcesses = Process.GetProcessesByName(thisProcess.ProcessName);
    //        foreach (Process p in allProcesses)
    //        {
    //            if ((p.Id != thisProcess.Id) && (p.MainModule.FileName == thisProcess.MainModule.FileName))
    //                return p;
    //        }
    //        return null;
    //    }
    //}


}
