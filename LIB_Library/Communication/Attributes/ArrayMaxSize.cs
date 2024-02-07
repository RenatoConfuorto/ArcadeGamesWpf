using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Communication.Attributes
{
    public class ArrayMaxSize : Attribute
    {
        public int ArraySize { get; set; }

        public ArrayMaxSize() { }
        public ArrayMaxSize(int arraySize) 
            :this()
        {
            this.ArraySize = arraySize;
        }
    }
}
