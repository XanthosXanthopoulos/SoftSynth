using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synth
{
    public class FMPanelProfile
    {
        #region Public Properties

        public double[,] ModulationIndex { get; set; }

        #endregion

        #region Constructors

        public FMPanelProfile()
        {
            ModulationIndex = new double[3, 5];

            ModulationIndex[0, 4] = 1;
            ModulationIndex[0, 3] = ModulationIndex[1, 3] = ModulationIndex[2, 3] = 0.5;
        }

        #endregion
    }
}
