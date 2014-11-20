using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace SwapStff.Entity
{
    public class BaseEntity<T>
    {
        public T ID { get; set; }
    }
}
