using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using devs.net.lib.examples.efp;
using devs.net.kernel.simulation;
using devs.net.kernel.modeling;

namespace devs.examples
{
    class LocalGpt : Coupled {
        public LocalGpt() : base("LocalGpt") {
   		    Atomic genr = new Generator("Generator", 1.0);
            Atomic proc = new Processor("Processor", 2.5);
		    Atomic tran = new Transducer("Transducer", 100.0);
        
            addComponent(genr);
		    addComponent(proc);
            addComponent(tran);

            genr.addConnection(Generator.portOut, proc, Processor.portIn);
            genr.addConnection(Generator.portOut, tran, Transducer.portArrived);
            proc.addConnection(Processor.portOut, tran, Transducer.portSolved);
            tran.addConnection(Transducer.portOut, genr, Generator.portStop);	
        }

        static void Main(string[] args)
        {
            try
            {
                LocalGpt model = new LocalGpt();
                RTCentralCoordinator coordinator = new RTCentralCoordinator(model);
                coordinator.simulate();
                
            }
            catch (Exception ee)
            {
                Console.Error.WriteLine(ee.ToString());
                System.Console.Read();
            }
        }
    }
}
