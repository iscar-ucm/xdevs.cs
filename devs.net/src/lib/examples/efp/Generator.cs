using System;
using System.Collections.Generic;
using System.Text;
using devs.net.kernel.modeling;
using System.Collections;

namespace devs.net.lib.examples.efp
{
    public class Generator : Atomic
    {// Ports
        public static String portStop = "stop";
        public static String portOut = "out";

        // State
        protected double period;
        protected int count;

        public Generator(String name, double period)
            : base(name)
        {
            base.addInport(Generator.portStop);
            base.addOutport(Generator.portOut);

            this.period = period;
            this.setSigma(period);
            count = 0;
        }

        public override void deltint()
        {
            count++;
            this.setSigma(period);
        }

        public override void deltext(Double e)
        {
            resume(e);
            List<Object> values = getValuesOnInputPort(Generator.portStop);
            if (values.Count > 0) this.setSigma(INFINITY);
        }

        override public void lambda()
        {
            Job job = new Job("" + count + "");
            base.addValueOnOutputPort(Generator.portOut, job);
        }

    }
}
