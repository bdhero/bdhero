using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Versioner
{
    partial class Usage
    {
        private readonly string _exeName;

        public Usage(string exeName)
        {
            _exeName = exeName;
        }
    }
}
