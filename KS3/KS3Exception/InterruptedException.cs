using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.KS3Exception
{
    public class InterruptedException : Exception
    {
        public InterruptedException(String message) :
            base(message) { }
    }
}
