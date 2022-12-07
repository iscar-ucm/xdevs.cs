using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace devs.net.kernel.modeling
{
    [Serializable]
    public class Message
    {
        private Dictionary<String, List<Object>> messages;
	
	    public Message() {
            messages = new Dictionary<String, List<Object>>();
	    }
	
	    public void add(String portName, Object value) {
            if (value == null) return;
            if (messages.ContainsKey(portName))
            {
                List<Object> values = messages[portName];
                values.Add(value);
            }
            else
            {
                List<Object> values = new List<Object>();
                values.Add(value);
                messages[portName] = values;
            }
        }

        public List<Object> getValuesOnPort(String portName)
        {
            if (!messages.ContainsKey(portName)) return new List<Object>();
            List<Object> values = messages[portName];
		    return values;
	    }
	
	    public Boolean isEmpty() {
            IEnumerator<List<Object>> itr = messages.Values.GetEnumerator();
            while (itr.MoveNext())
            {
                List<Object> values = itr.Current;
                if (values.Count > 0) return false;
            }
		    return true;
	    }
	
	    public void receive(String portFrom, Message newMessage, String portTo) {
            List<Object> valuesFrom = newMessage.getValuesOnPort(portFrom);
		    messages[portTo] = valuesFrom;
	    }
    }
}
