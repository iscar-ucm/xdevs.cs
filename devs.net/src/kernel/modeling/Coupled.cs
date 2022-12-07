using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using devs.net.kernel.modeling;

namespace devs.net.kernel.modeling
{
    public class Coupled : Atomic
    {
        protected List<Devs> components;
        public List<Devs> getComponents() { return components; }
        public void addComponent(Devs component)
        {
            components.Add(component);
        }



        /** Constructor. */
        public Coupled(String name)
            : base(name)
        {
            components = new List<Devs>();
        }
        // ---------------------------------------------------------------
        // DEVS PROTOCOL
        // ---------------------------------------------------------------
        override public double ta()
        {
            sigma = INFINITY;
            foreach (Devs component in components)
            {
                if (component.ta() < sigma)
                {
                    sigma = component.getSigma();
                }
            }
            return sigma;
        }

        public override void deltint()
        {
            foreach (Devs component in components)
            {
                if (sigma == component.getSigma())
                {
                    component.deltint();
                }
            }

        }

        public override void deltext(Double e)
        {
            // Propagate the input
            List<String> inputPorts = base.getInputPorts();
            foreach (String inputPort in inputPorts)
            {
                List<Object> values = base.getValuesOnInputPort(inputPort);
                if (values.Count > 0)
                    foreach (Connection connection in connections)
                        if (connection.getPortFrom().Equals(inputPort))
                            foreach (Object value in values)
                            {
                                connection.getComponentTo().receive(connection.getPortTo(), value);
                                connection.getComponentTo().deltext(e);
                                connection.getComponentTo().clearInputPort(connection.getPortTo());
                            }
            }
            base.resume(e);
        }

        /// <summary>
        /// DEVS lambda function.
        /// </summary>
        public override void lambda()
        {
            foreach (Devs component in components)
            {
                if (sigma == component.getSigma())
                {
                    component.lambda();
                    component.propagateOutput();
                    component.clearOutput();
                }
            }
        }

        public void deltfcn()
        {
            foreach (Devs component in components)
            {
                Boolean isInputEmpty = component.isInputEmpty();

                if (sigma == component.getSigma())
                {
                    if (isInputEmpty)
                        component.deltint();
                    else
                    {
                        component.deltcon(sigma);
                        component.clearInput();
                    }
                }
                else if (sigma < component.getSigma())
                {
                    if (!isInputEmpty)
                    {
                        component.deltext(sigma);
                        component.clearInput();
                    }
                    else
                        component.resume(sigma);
                }
                else
                    throw new Exception("deltfcn -> " + base.getName() + "::sigma cannot be greater than " + component.getName() + "::sigma");
            }
        }

    }
}

