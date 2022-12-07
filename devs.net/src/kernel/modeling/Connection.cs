using System;
using System.Collections.Generic;
using System.Text;

namespace devs.net.kernel.modeling 
{
    public class Connection
    {	
        protected String portFrom;
        public String getPortFrom() { return portFrom; }
        public void setPortFrom(String portFrom) { this.portFrom = portFrom; }

        protected Devs componentTo;
        public Devs getComponentTo() { return componentTo; }
        public void setComponentTo(Devs component) { this.componentTo = component; }

        protected String portTo;
        public String getPortTo() { return portTo; }
        public void setPortTo(String portTo) { this.portTo = portTo; }

        public Connection(String portFrom, Devs componentTo, String portTo)
        {
            this.portFrom = portFrom;
            this.componentTo = componentTo;
            this.portTo = portTo;
        }
    }
}
