using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace testing
{
    public class StatusChecker
    {
        public static void CheckPfStatus()
        {
            var isFinished = false;
            while (!isFinished)
            {
                Thread.Sleep(10000);
                isFinished = true;
            }

            return;
        }
}
}