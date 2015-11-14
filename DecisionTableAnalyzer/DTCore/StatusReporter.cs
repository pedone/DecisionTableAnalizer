using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCore
{

    public delegate void StatusMessageEventHandler(string message);

    public class StatusReporter
    {

        public event StatusMessageEventHandler StatusMessage;
        public static StatusReporter Instance { get; private set; }

        static StatusReporter()
        {
            Instance = new StatusReporter();
        }

        private StatusReporter()
        { }

        public void Add(string message)
        {
            if (StatusMessage != null)
                StatusMessage(message);
        }

    }
}
