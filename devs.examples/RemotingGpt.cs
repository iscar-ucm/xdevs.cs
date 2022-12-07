using System;

using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

using devs.net.kernel.modeling;
using devs.net.kernel.simulation;
using devs.net.lib.examples.efp;

namespace devs.examples
{
    public class RemotingGpt : Coupled {
        public RemotingGpt() : base("GptRemoting") {
   		    Atomic genr = new Generator("Generator", 1.0);

            TcpChannel clientChannel = new TcpChannel();
            ChannelServices.RegisterChannel(clientChannel, false);
            // Create an instance of the remote object
            //Atomic proc = (Processor)Activator.GetObject(typeof(devs.net.lib.examples.efp.Processor), "tcp://127.0.0.1:9090/Processor");

            // TODO: Utilizando CAO es algo como:
            RemotingConfiguration.RegisterActivatedClientType(typeof(Processor), "tcp://127.0.0.1:9090/DEVS.NET");
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
                RemotingGpt model = new RemotingGpt();
                Coordinator coordinator = new Coordinator(model);
                coordinator.simulate();
                Console.Read();
            }
            catch (Exception ee)
            {
                Console.Error.WriteLine(ee.ToString());
                Console.Read();
            }
            /*// Create an instance of a channel
            TcpChannel clientChannel = new TcpChannel();
            ChannelServices.RegisterChannel(clientChannel, false);

            // Create an instance of the remote object
            SampleObject obj = (SampleObject)Activator.GetObject(typeof(devs.net.src.lib.examples.efp.SampleObject), "tcp://127.0.0.1:9090/HelloWorld");

            // Use the object
            Console.WriteLine(obj.HelloWorld());*/

        }


    }
}
