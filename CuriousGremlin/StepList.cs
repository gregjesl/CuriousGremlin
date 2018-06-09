using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin
{
    public class StepList : List<string>
    {
        public override string ToString()
        {
            return string.Join(".", this);
        }
    }
}
