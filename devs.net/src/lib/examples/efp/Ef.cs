using System;
using System.Collections.Generic;
using System.Text;
using devs.net.kernel.modeling;

namespace devs.net.lib.examples.efp
{
    public class Ef : Coupled
    {
        // Ports
        public static String portIn = "in";
        public static String portOut = "out";

        public Ef(String name, Double period, Double observationTime)
            : base(name)
        {

            Atomic genr = new Generator("Generator", period);
            Atomic tran = new Transducer("Transducer", observationTime);
            addComponent(genr);
            addComponent(tran);

            this.addConnection(Ef.portIn, tran, Transducer.portSolved);
            genr.addConnection(Generator.portOut, this, Ef.portOut);
            genr.addConnection(Generator.portOut, tran, Transducer.portArrived);
            tran.addConnection(Transducer.portOut, genr, Generator.portStop);
        }
    }
}
