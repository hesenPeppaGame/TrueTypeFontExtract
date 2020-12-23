using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media;

namespace FontCreater
{
    class Program
    {
        static void Main(string[] args)
        {
            args = new string[] {
                @"C:/Patch1.4.1/Tools/",
                @"C:/Patch1.4.1/Assets/Resources/Fonts/HYZhongSong.TTF",
                "我",
                @"C:/Patch1.4.1/Assets/Resources/Fonts/HYZhongSong1.TTF",
            };

            RunCMDCommand(out var str, new string[] { $"cd {args[0]}", $"java -jar sfnttool.jar  -s {args[2]} {args[1]} {args[3]}" });

            Console.WriteLine($"输出完成  {str}");
        }

        public static void RunCMDCommand(out string outPut, params string[] command)
        {
            using (Process pc = new Process())
            {
                pc.StartInfo.FileName = "cmd.exe";
                pc.StartInfo.CreateNoWindow = true;//隐藏窗口运行
                pc.StartInfo.RedirectStandardError = true;//重定向错误流
                pc.StartInfo.RedirectStandardInput = true;//重定向输入流
                pc.StartInfo.RedirectStandardOutput = true;//重定向输出流
                pc.StartInfo.UseShellExecute = false;
                pc.Start();
                int lenght = command.Length;
                foreach (string com in command)
                {
                    pc.StandardInput.WriteLine(com);//输入CMD命令
                }
                pc.StandardInput.WriteLine("exit");//结束执行，很重要的
                pc.StandardInput.AutoFlush = true;

                outPut = pc.StandardOutput.ReadToEnd();//读取结果        

                pc.WaitForExit();
                pc.Close();
            }
        }

    }
}
