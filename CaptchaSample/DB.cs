using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptchaSample
{
    internal class DB
    {
        static Mactak1Context instance;
        public static Mactak1Context Instance
        {
            get {
                if (instance == null)
                    instance = new Mactak1Context();
                return instance; 
            } }
    }
}
