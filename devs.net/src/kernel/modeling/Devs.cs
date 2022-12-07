using System;
using System.Collections.Generic;
using System.Text;

/** TODO:
 * - Los modelos se almacenan en el coordinador por "nombre". Hay que implementar un método para chequear que 
 *   no hay modelos "repetidos". Tiene que ser por nombre por detalles del .NET remoting.
 * 
 */

namespace devs.net.kernel.modeling
{
    /// <summary>
    /// The basic DEVS component, parent of both atomic an coupled models. It can be shared using .NET Remoting.
    /// </summary>
    public abstract class Devs : MarshalByRefObject 
    {
        static public Double INFINITY = Double.PositiveInfinity;
        /// <summary>
        /// Component name. It must be unique in the whole DEVS model.
        /// </summary>
        protected String name;
        /// <summary>
        /// Getter.
        /// </summary>
        /// <returns>Returns the name of the DEVS component</returns>
        public String getName() { return name; }
        /// <summary>
        /// Setter.
        /// </summary>
        /// <param name="name">Sets the name of this component.</param>
        public void setName(String name) { this.name = name; }

        private Dictionary<String, List<Object>> inPorts;
        public List<Object> getValuesOnInputPort(String portName)
        {
            return inPorts[portName];
        }
        public List<String> getInputPorts()
        {
            List<String> inputPorts = new List<String>();
            Dictionary<String, List<Object>>.KeyCollection keys = inPorts.Keys;
            foreach (String key in keys)
            {
                inputPorts.Add(key);
            }
            return inputPorts;
        }
        public Boolean isInputEmpty()
        {
            foreach (List<Object> values in inPorts.Values)
                if (values.Count > 0)
                    return false;
            return true;
        }
        public void addInport(String portName)
        {
            inPorts.Add(portName, new List<Object>());
        }
        public void clearInputPort(String portName)
        {
            inPorts[portName].Clear();
        }
        public void clearInput()
        {
            foreach (String key in inPorts.Keys)
            {
                inPorts[key].Clear();
            }
        }

        private Dictionary<String, List<Object>> outPorts;
        public void addOutport(String portName)
        {
            outPorts.Add(portName, new List<Object>());
        }
        public void addValueOnOutputPort(String portName, Object value)
        {
            outPorts[portName].Add(value);
        }
        public void clearOutput()
        {
            foreach (String key in outPorts.Keys)
            {
                outPorts[key].Clear();
            }
        }

        /// <summary>
        /// Out connections from this component to others.
        /// </summary>
        protected List<Connection> connections;
        /// <summary>
        /// Adds a connection.
        /// </summary>
        /// <param name="portFrom">The origin port.</param>
        /// <param name="componentTo">Destination (component).</param>
        /// <param name="portTo">Destination (port name).</param>
        public void addConnection(String portFrom, Devs componentTo, String portTo)
        {
            Connection connection = new Connection(portFrom, componentTo, portTo);
            connections.Add(connection);
        }

        /// <summary>
        /// DEVS sigma.
        /// </summary>
        protected Double sigma;
        /// <summary>
        /// Getter.
        /// </summary>
        /// <returns>Returns sigma.</returns>
        public Double getSigma() { return sigma; }
        /// <summary>
        /// Setter.
        /// </summary>
        /// <param name="sigma">Sets sigma.</param>
        public void setSigma(Double sigma) { this.sigma = sigma; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Component name. It must be unique for the whole DEVS model</param>
        public Devs(String name)
        {
            this.name = name;
            inPorts = new Dictionary<String, List<Object>>();
            outPorts = new Dictionary<String, List<Object>>();
            connections = new List<Connection>();
        }

        /// <summary>
        /// DEVS time advance function.
        /// </summary>
        /// <returns>The time in which the next event happens.</returns>
        abstract public Double ta();

        /// <summary>
        /// DEVS internal transition function.
        /// </summary>
        abstract public void deltint();

        /// <summary>
        /// DEVS external transition function.
        /// </summary>
        /// <param name="e">Elapsed time.</param>
        abstract public void deltext(Double e);

        /// <summary>
        /// DEVS confluent function.
        /// </summary>
        /// <param name="e">Elapsed time.</param>
        abstract public void deltcon(Double e);

        /// <summary>
        /// DEVS output function.
        /// </summary>
        abstract public void lambda();

        // =================================================================================
        public void propagateOutput()
        {
            // First, we propagate
            foreach (Connection connection in connections)
            {
                String portFrom = connection.getPortFrom();
                Devs componentTo = connection.getComponentTo();
                String portTo = connection.getPortTo();

                // Get the output at portFrom
                List<Object> values = outPorts[portFrom];
                foreach (Object value in values)
                {
                    componentTo.receive(portTo, value);
                }
            }
        }

        public void receive(String portName, Object value)
        {
            List<Object> values = inPorts[portName];
            values.Add(value);
        }

        /// <summary>
        /// Continues the execiton doing sigma = sigma - e.
        /// </summary>
        /// <param name="e">The elapsed time.</param>
        public void resume(Double e)
        {
            sigma -= e;
        }


    }
}
