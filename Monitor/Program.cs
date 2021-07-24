using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Monitor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine( args.Length);
            if((args.Length != 0 ) && (args.Length == 3))
            {
                List<string> ListProperty = new List<string>();
                ListProperty.Add(args[0]);
                ListProperty.Add(args[1]);
                int CheckTime = int.Parse(args[2]);

                TimerCallback timerCallback = new TimerCallback(CheckProcess);
                Timer timer = new Timer(timerCallback, ListProperty, 0, CheckTime * 60000);

                Console.WriteLine(DateTime.Now);

                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Error count arguments");
            }
          
            
        }

        static DateTime GetProcessLifeTime(string ProcessName)
        {

            Process[] ProcessRuning = Process.GetProcessesByName(ProcessName);
            
            return ProcessRuning[0].StartTime;
        }

        static void CheckProcess(object obj)
        {
            List<string> listProp = (List<string>)obj;

            DateTime TimeProcess = GetProcessLifeTime(listProp[0]);

            TimeSpan timeSpan = TimeProcess.Subtract(DateTime.Now);

            double minutes = timeSpan.TotalMinutes * -1;
            if (minutes  >=  int.Parse(listProp[1]))
            {
                foreach(Process process in Process.GetProcessesByName(listProp[0]))
                {
                    process.Kill();
                    Console.WriteLine($"Process {listProp[0]} killed");
                }
                
            } else
            {
                Console.WriteLine("Wait...");
            }
        }


    }
}
