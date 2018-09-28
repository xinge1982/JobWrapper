using NDesk.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace JobWrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                InteractiveRun(args);
            }
            catch (Exception e)
            {
                ConsoleWriteLineWithColor(ConsoleColor.Red, e.Message);
                Environment.Exit(-1);
            }
        }

        private static void InteractiveRun(string[] args)
        {
            String dllPath = "";
            String absoluteDllPath = "";
            String className = "";
            String methodName = "";
            String paramJson = "";
            Action actionToTake = null;

            var optionSet = new OptionSet();
            optionSet.Add("dll=", OptionCategory.General, "DLL file path", s => dllPath = s);
            optionSet.Add("class=", OptionCategory.General, "DLL class name", s => className = s);
            optionSet.Add("method=", OptionCategory.General, "DLL method name", s => methodName = s);
            optionSet.Add("json=", OptionCategory.General, "DLL parameter json", s => paramJson = s);
            optionSet.Add("print", OptionCategory.General, "Show DLL information", key =>
            {
                actionToTake = () => ShowDynamic(absoluteDllPath);
            });
            optionSet.Add("help", OptionCategory.Help, "Help about the command line interface", key =>
            {
                actionToTake = () => PrintUsage(optionSet);
            });
            try
            {
                if (args.Length == 0) // we default to executing in debug mode 
                    args = new[] { "--debug" };

                optionSet.Parse(args);
            }
            catch (Exception e)
            {
                ConsoleWriteLineWithColor(ConsoleColor.Red, e.Message);
                PrintUsage(optionSet);
                ConsoleWriteLineWithColor(ConsoleColor.Red, e.Message);
                Environment.Exit(-1);
                return;
            }

            if (String.IsNullOrEmpty(dllPath))
            {
                ConsoleWrite("Please set --dll.");
                PrintUsage(optionSet);
                Environment.Exit(-1);
                return;
            }
            else
            {
                absoluteDllPath = Path.GetFullPath(dllPath);
                if (!File.Exists(absoluteDllPath))
                {
                    ConsoleWrite("File does not exists: " + absoluteDllPath);
                    Environment.Exit(-1);
                    return;
                }
            }

            if (actionToTake == null)
                actionToTake = () => PrintUsage(optionSet);

            if (!String.IsNullOrEmpty(absoluteDllPath) && 
                !String.IsNullOrEmpty(className))
            {
                actionToTake = () => LoadDynamic(absoluteDllPath, className, methodName, paramJson);
            }

            actionToTake();
        }

        private static void ShowDynamic(String path)
        {
            if (String.IsNullOrEmpty(path))
            {
                ConsoleWrite("File path is empty.");
                Environment.Exit(-1);
            }

            String className = null;
            var DLL = Assembly.LoadFile(path);
            foreach (Type type in DLL.GetExportedTypes())
            {
                if (className == null)
                    className = type.ToString();
                ConsoleWrite("" + type.ToString());
                MethodInfo[] methords = type.GetMethods();
                for (int i = 0; i < methords.Length; i++)
                {
                    MethodInfo m = methords[i];
                    ConsoleWrite("\tMethod:" + m.ToString());
                    ParameterInfo[] pis = m.GetParameters();
                    for (int j = 0; j < pis.Length; j++)
                    {
                        ConsoleWrite("\t\tParameter:" + pis[j].ToString());
                    }
                }
            }
        }

        private static void LoadDynamic(String path, String className, String methodName, String parameterJson)
        {
            var DLL = Assembly.LoadFile(path);
            ConsoleWrite("Loaded File: " + path);

            if (String.IsNullOrEmpty(className))
            {
                ConsoleWrite("Class name is empty.");
                Environment.Exit(-1);
            }

            if (String.IsNullOrEmpty(methodName))
            {
                ConsoleWrite("Method name is empty.");
                Environment.Exit(-1);
            }

            if (String.IsNullOrEmpty(parameterJson))
            {
                parameterJson = "[]";
            }

            var theType = DLL.GetType(className);
            if (theType == null)
            {
                ConsoleWrite("Unknown Class: " + className);
                Environment.Exit(-1);
            }
            var c = Activator.CreateInstance(theType);
            ConsoleWrite("Loaded Class: " + className);

            var method = theType.GetMethod(methodName);
            if (method == null)
            {
                ConsoleWrite("Unknown Method: " + methodName);
                Environment.Exit(-1);
            }
            ConsoleWrite("Loaded Method: " + methodName);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Object[] parameters = serializer.Deserialize<Object[]>(parameterJson);
            var result = method.Invoke(c, parameters);
            ConsoleWrite("Done Method: " + methodName);
        }

        private static void PrintUsage(OptionSet optionSet)
        {
            ConsoleWriteLineWithColor(ConsoleColor.DarkMagenta,
                @"
JobWrapper for the .NET Platform
----------------------------------------------
Command line options:",
                SystemTime.UtcNow.Year);

            optionSet.WriteOptionDescriptions(Console.Out);

            Console.WriteLine(@"
Enjoy...
");
        }

        private static void ConsoleWriteLineWithColor(ConsoleColor color, string message, params object[] args)
        {
            ConsoleWriteWithColor(new ConsoleText
            {
                ForegroundColor = color,
                BackgroundColor = Console.BackgroundColor,
                IsNewLinePostPended = true,
                Message = message,
                Args = args
            });
        }

        private static void EmitWarningInRed()
        {
            var old = Console.ForegroundColor;
            if (Type.GetType("Mono.Runtime") == null)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("A critical error occurred while starting the server. Please see the exception details bellow for more details:");
            if (Type.GetType("Mono.Runtime") == null)
                Console.ForegroundColor = old;
        }


        private static void ConsoleWriteWithColor(params ConsoleText[] consoleTexts)
        {
            if (consoleTexts == null)
            {
                throw new ArgumentNullException("consoleTexts");
            }

            var previousForegroundColor = Console.ForegroundColor;
            var previousBackgroundColor = Console.BackgroundColor;

            foreach (var consoleText in consoleTexts)
            {
                if (Type.GetType("Mono.Runtime") == null)
                    Console.ForegroundColor = consoleText.ForegroundColor;
                if (Type.GetType("Mono.Runtime") == null)
                    Console.BackgroundColor = consoleText.BackgroundColor;

                Console.Write(consoleText.Message, consoleText.Args);

                if (consoleText.IsNewLinePostPended)
                {
                    Console.WriteLine();
                }
            }

            if (Type.GetType("Mono.Runtime") == null)
                Console.ForegroundColor = previousForegroundColor;
            if (Type.GetType("Mono.Runtime") == null)
                Console.BackgroundColor = previousBackgroundColor;
        }

        private static void ConsoleWriteWithColor(ConsoleColor color, string message, params object[] args)
        {
            ConsoleWriteWithColor(new ConsoleText
            {
                ForegroundColor = color,
                BackgroundColor = Console.BackgroundColor,
                IsNewLinePostPended = false,
                Message = message,
                Args = args
            });
        }

        private static void ConsoleWrite(string message, params object[] args)
        {
            Console.WriteLine("*** " + message);
        }
    }
}
