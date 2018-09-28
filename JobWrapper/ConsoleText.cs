using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobWrapper
{
    internal class ConsoleText
    {
        public ConsoleText()
        {
            ForegroundColor = Console.ForegroundColor;
            BackgroundColor = Console.BackgroundColor;
        }

        public string Message { get; set; }
        public object[] Args { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }
        public bool IsNewLinePostPended { get; set; }
    }
}
