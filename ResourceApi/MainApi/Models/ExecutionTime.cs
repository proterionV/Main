using MainApi.Models.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainApi.Models
{
    public class ExecutionTime
    {
        public ExecutionTime()
        {

        }

        public ExecutionTime(Operations operation, double elapsed)
        {
            Operation = operation.ToString();
            Elapsed = elapsed;
        }

        [Key]
        public int Id { get; set; }
        public string Operation { get; set; }
        public double Elapsed { get; set; }
    }
}
