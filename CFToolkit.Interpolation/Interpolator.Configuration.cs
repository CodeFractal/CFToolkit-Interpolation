using System;
using System.Collections.Generic;
using System.Text;

namespace CFToolkit.Interpolation
{
    public partial class Interpolator
    {
        public class Configuration
        {
            private Configuration() { }

            internal static Configuration Default {
                get {
                    var result = new Configuration();
                    return result;
                }
            }
        }
    }
}
