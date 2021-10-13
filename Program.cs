using System;
using System.Collections.Generic;

namespace ConsoleHermit
{
    class Program
    {
        static void Main(string[] args)
        {
            HermitFileHandler hfh = new HermitFileHandler();
            HermitHttpHandler hhh = new HermitHttpHandler();
            HermitBackend hbe = new HermitBackend(hfh, hhh);
            HermitUI hui = new HermitUI(hbe);
            HermitController HC = new HermitController(hbe, hui);

            HC.ParseTerminalArgs(args);
            
            

        }
    }
}
