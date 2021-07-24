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

            
                if ((args.Length != 0) && (args.Length == 3))
                {
                    List<string> ListProperty = new List<string>();
                    
                    // Получаем имя процесса
                    ListProperty.Add(RemoveExe(args[0]));
                    // Получаем количество минут жизни процесса
                    ListProperty.Add(args[1]);
                    // Интервал проверки 
                    int CheckTime = int.Parse(args[2]);
                    TimerCallback timerCallback = new TimerCallback(CheckProcess);
                    Timer timer = new Timer(timerCallback, ListProperty, 0, CheckTime * 60000);

                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Error count arguments");
                }
           
        }

        // Получаем время старта процесса
        static DateTime GetProcessLifeTime(string ProcessName)
        {

            Process[] ProcessRuning = Process.GetProcessesByName(ProcessName);
            
            return ProcessRuning[0].StartTime;
        }

        // Удаляем из строки .exe
        static string RemoveExe(string str)
        {
            return str.Remove(str.IndexOf('.'), 4);
        }

        // Мониторим время жизни процесса и если оно равно или больше заданного то уничтожаем процесс
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
                    Process.GetCurrentProcess().Kill();
                }
                
            } else
            {
                Console.WriteLine("Wait...");
            }
        }


    }
}
