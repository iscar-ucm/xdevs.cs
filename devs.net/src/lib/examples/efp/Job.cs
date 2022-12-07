using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using devs.net.kernel.modeling;

namespace devs.net.lib.examples.efp
{
    [Serializable]
    public class Job
    {
        public String name;
        public Double time;

        public Job()
        {
            name = "aJob";
            time = Devs.INFINITY;
        }

        public Job(String aname)
        {
            name = aname;
            time = 0.0;
        }

    }
}
