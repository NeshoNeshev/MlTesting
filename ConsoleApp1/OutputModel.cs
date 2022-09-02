using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class OutputModel
    {
        [ColumnName("PredictedLabel")]
        public string Answear { get; set; }

        public float[] Score { get; set; }
    }
}
