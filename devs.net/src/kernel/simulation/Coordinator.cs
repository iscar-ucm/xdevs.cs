using System;
using System.Collections.Generic;
using System.Text;
using devs.net.kernel.modeling;

namespace devs.net.kernel.simulation
{
    public class Coordinator
    {
        /// <summary>
        /// The coupled model to simulate.
        /// </summary>
        protected Coupled model;
        protected Double t0;
        protected Double tL;
        protected Double tN;
        protected ulong numIterations;

        public Coordinator(Coupled model, ulong numIterations, Double t0)
        {
            this.model = model;
            this.numIterations = numIterations;
            this.t0 = t0;
        }

        public Coordinator(Coupled model, ulong numIterations)
        {
            this.model = model;
            this.numIterations = numIterations;
            this.t0 = 0.0;
        }

        public Coordinator(Coupled model)
        {
            this.model = model;
            this.numIterations = ulong.MaxValue;
            this.t0 = 0.0;
        }

        public void simulate()
        {
            double t = t0;
            ulong counter;
            for (counter = 0; counter < numIterations ; ++counter)
            {
                tL = t;
                tN = t + model.ta();
                if (tN >= Devs.INFINITY)
                    break;
                model.lambda();
                model.deltfcn();
                t = tN;
            }
            Console.WriteLine(counter + " iterations. Press ENTER to exit.");
            Console.ReadLine();
        }

    }
}
