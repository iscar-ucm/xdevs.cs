using System;
using System.Collections.Generic;
using System.Text;
using devs.net.kernel.modeling;
using devs.net.kernel.simulation;

namespace devs.net.lib.examples.efp
{
    public class Efp : Coupled
    {
        public Efp()
            : base("Efp")
        {
            Atomic proc = new Processor("Processor", 2.5);
            Coupled ef = new Ef("ExperimentalFrame", 1.0, 100.0);
            addComponent(ef);
            addComponent(proc);

            ef.addConnection(Ef.portOut, proc, Processor.portIn);
            proc.addConnection(Processor.portOut, ef, Ef.portIn);
        }
    }
}
