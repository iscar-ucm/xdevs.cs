using System;

namespace devs.net.kernel.modeling
{
    /// <summary>
    /// The DEVS atomic model.
    /// </summary>
    public abstract class Atomic : Devs
    {
        /// <summary>
        /// Minimum state information.
        /// </summary>
        protected String phase;
        /// <summary>
        /// Getter.
        /// </summary>
        /// <returns>Returns the phase.</returns>
        public String getPhase() { return phase; }
        /// <summary>
        /// Setter.
        /// </summary>
        /// <param name="phase">Sets the phase.</param>
        public void setPhase(String phase) { this.phase = phase; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of this atomic model</param>
        public Atomic(String name) : base(name)
        {
            passivate();
        }

        // ---------------------------------------------------------------
        // DEVS PROTOCOL
        // ---------------------------------------------------------------
        /// <summary>
        /// Time advance function.
        /// </summary>
        /// <returns>Returns sigma.</returns>
        override public Double ta() {
            return sigma; 
        }

        /// <summary>
        /// Confluent transition function.
        /// </summary>
        /// <param name="e">Elapsed time.</param>
        /// <param name="x">Input.</param>
        override public void deltcon(Double e)
        {
            deltint();
            deltext(0);
        }
        // ---------------------------------------------------------------
        

        /// <summary>
        /// Passivates the current model (phase=passive, sigma=INFINITY)
        /// </summary>
        public void passivate()
        {
            phase = "passive";
            sigma = Devs.INFINITY;
        }

        public void holdIn(String phase, Double sigma)
        {
            this.phase = phase;
            this.sigma = sigma;
        }

    }
}
