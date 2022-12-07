using System;
using System.Collections.Generic;
using System.Text;
using devs.net.kernel.modeling;
using System.Collections;

namespace devs.net.lib.examples.efp
{
    public class Processor : Atomic
    {
        // Ports
        public static String portIn = "in";
        public static String portOut = "out";

        // State	
        protected Job currentJob;
        protected double processingTime;

        public Processor(String name, double processingTime)
            : base(name)
        {
            base.addInport(Processor.portIn);
            base.addOutport(Processor.portOut);

            currentJob = null;
            this.processingTime = processingTime;
        }

        public override void deltint()
        {
            sigma = INFINITY;
            currentJob = null;
        }

        public override void deltext(Double e)
        {
            resume(e);
            if (base.sigma == INFINITY)
            {
                if (base.getValuesOnInputPort(Processor.portIn).Count == 1) 
                {
                    currentJob = (Job)base.getValuesOnInputPort(Processor.portIn)[0];
                    sigma = processingTime;
                }
                else
                    throw new Exception("The input port " + Processor.portIn + " can just contain 1 pending job.");
            }
        }

        public override void lambda()
        {
            if (currentJob != null)
                base.addValueOnOutputPort(Processor.portOut, currentJob);
        }
    }
}
