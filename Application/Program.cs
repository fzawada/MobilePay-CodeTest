using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUsers
{
    using System.Threading;

    using LogTest;

    class Program
    {
        static void Main(string[] args)
        {
            ILog  logger = new AsyncLog();

            for (int i = 0; i < 15; i++)
            {
                logger.Write("Number with Flush: " + i);
            }

            logger.StopWithFlush();

            ILog logger2 = new AsyncLog();

            for (int i = 50; i > 0; i--)
            {
                logger2.Write("Number with No flush: " + i);
            }

            logger2.StopWithoutFlush();

            Console.WriteLine("Done. Press enter");
            Console.ReadLine();
        }
    }
}
