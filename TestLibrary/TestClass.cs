using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestLibrary
{
    public class TestClass
    {
        public void Hello(String name)
        {
            Console.WriteLine("Hello " + name);
        }

        public void MultiHello(String name, int age, String location)
        {
            Console.WriteLine("Hello " + name + ", " + age + " years old from " + location + ".");
            Thread.Sleep(3000);
            Console.WriteLine("done.");
        }
    }
}
