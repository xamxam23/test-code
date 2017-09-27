using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_MultiChoice.Model
{
    public class AnimalCount
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public class AnimalCountPerGroup
    {
        public int Group_ID { get; set; }
        public int Count { get; set; }
    }
}
