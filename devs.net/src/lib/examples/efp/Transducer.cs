using System;
using System.Text;
using devs.net.kernel.modeling;
using System.Collections;
using System.Collections.Generic;

namespace devs.net.lib.examples.efp
{
    public class Transducer : Atomic
    {
        // Ports
        public static String portArrived = "arrived";
        public static String portSolved = "solved";
        public static String portOut = "out";

        // State
        protected System.Collections.Generic.List<Job> jobsArrived;
        protected System.Collections.Generic.List<Job> jobsSolved;
        protected double observationTime;
        protected double total_ta;
        protected double clock;

        public Transducer(String name, double observationTime)
            : base(name)
        {
            base.addInport(portArrived);
            base.addInport(portSolved);
            base.addOutport(portOut);

            jobsArrived = new System.Collections.Generic.List<Job>();
            jobsSolved = new System.Collections.Generic.List<Job>();
            this.observationTime = observationTime;
            sigma = observationTime;
            total_ta = 0;
            clock = 0;
        }

        public override void deltint()
        {
            clock = clock + getSigma();
            double throughput;
            double avg_ta_time;
            if (jobsSolved.Count > 0)
            {
                avg_ta_time = total_ta / jobsSolved.Count;
                if (clock > 0.0) throughput = jobsSolved.Count / clock;
                else throughput = 0.0;
            }
            else
            {
                avg_ta_time = 0.0;
                throughput = 0.0;
            }
            System.Console.WriteLine("End time: " + clock);
            System.Console.WriteLine("jobs arrived : " + jobsArrived.Count);
            System.Console.WriteLine("jobs solved : " + jobsSolved.Count);
            System.Console.WriteLine("AVERAGE TA = " + avg_ta_time);
            System.Console.WriteLine("THROUGHPUT = " + throughput);
            setSigma(INFINITY);
        }

        public override void deltext(Double e)
        {
            resume(e);
            clock = clock + e;

            if (base.getValuesOnInputPort(Transducer.portArrived).Count > 0)
            {
                Job job = (Job)base.getValuesOnInputPort(Transducer.portArrived)[0];
                System.Console.WriteLine("Start job " + job.name + " @ t = " + clock);
                job.time = clock;
                jobsArrived.Add(job);
            }

            if (base.getValuesOnInputPort(Transducer.portSolved).Count > 0)
            {
                Job job = (Job)base.getValuesOnInputPort(Transducer.portSolved)[0];
                total_ta += (clock - job.time);
                System.Console.WriteLine("Finish job " + job.name + " @ t = " + clock);
                job.time = clock;
                jobsSolved.Add(job);
            }
        }


        override public void lambda()
        {
            Job job = new Job("null");
            base.addValueOnOutputPort(Transducer.portOut, job);
        }
    }
}
