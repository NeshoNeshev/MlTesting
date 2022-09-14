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
        public string Id { get; set; }

        [LoadColumn(1)]
        public string TypeOfCase { get; set; }

        [LoadColumn(4)]
        public string Content { get; set; }

        [LoadColumn(5)]
        public string Answer { get; set; }

        [LoadColumn(6)]
        public string Decision { get; set; }
    }
}
