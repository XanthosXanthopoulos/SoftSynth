using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synth
{
    public interface IModule
    {
        public abstract void Press(int key);

        public abstract void Press();

        public abstract void Release();
    }
}
