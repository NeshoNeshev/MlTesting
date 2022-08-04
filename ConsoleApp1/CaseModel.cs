using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class CaseModel
    {
        [LoadColumn(0)]
        public string Category { get; set; }

        [LoadColumn(1)]
        public string Content { get; set; }
    }
}
