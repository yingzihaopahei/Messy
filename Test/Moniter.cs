using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;

namespace Test
{
    public class Moniter
    {
        public void TEST()
        {
            int i = 0;
            while (true)
            {
                i++;
                System.IO.File.AppendAllText(@"C:\Users\Fan\Desktop\test.txt",i.ToString());
                Thread.Sleep(1 * 1000);
            }
        
        }
    }
}