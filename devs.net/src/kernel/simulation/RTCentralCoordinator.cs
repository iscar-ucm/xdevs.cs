using System;
using System.Collections.Generic;
using System.Text;
using devs.net.kernel.modeling;
using System.Threading;

namespace devs.net.kernel.simulation
{
    public class RTCentralCoordinator : Coordinator
    {
        protected Thread myThread;

        public RTCentralCoordinator(Coupled model, ulong numIterations)
            : base(model, numIterations)
        {
            myThread = new Thread(new ThreadStart(this.run));
        }

        public RTCentralCoordinator(Coupled model)
            : base(model)
        {
            myThread = new Thread(new ThreadStart(this.run));
        }

        public void simulate()
        {
            myThread.Start();
        }

        public void run()
        {
            DateTime dateTime = DateTime.UtcNow;
            double t = (dateTime - dateTime).TotalMilliseconds / 1000.0;

            ulong counter;
            for (counter = 0; counter < numIterations; ++counter)
            {

                tL = t;
                tN = t + model.ta();
                if (tN >= Devs.INFINITY)
                    break;

                while ((DateTime.UtcNow - dateTime).TotalMilliseconds < 1000 * t - 10)
                {
                    double timeSleeping = (1000 * t - (DateTime.UtcNow - dateTime).TotalMilliseconds);
                    if (timeSleeping >= 0)
                        Thread.Sleep((int)timeSleeping);
                }

                model.lambda();
                model.deltfcn();
                t = tN;

            }
            Console.WriteLine(counter + " iterations. Press ENTER to exit.");
            Console.ReadLine();
        }

    }
}
