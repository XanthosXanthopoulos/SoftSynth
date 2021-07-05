using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synth
{
	public class OscillatorUpdatedEventArgs : EventArgs
	{
		public OscillatorUpdatedComponent Component { get; set; }

		public OscillatorUpdatedEventArgs(OscillatorUpdatedComponent component)
		{
			Component = component;
		}
	}

	public enum OscillatorUpdatedComponent
	{
		Frequency,
		Balance,
		Signal,
		Mix
	}
}
