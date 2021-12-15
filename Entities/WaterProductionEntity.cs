using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterProductionVisualization.Entities
{
    public class WaterProductionEntity
    {
        /// <summary>
        /// Production Source
        /// </summary>
        public string UretimKaynagi { get; set; }
        /// <summary>
        /// Production Amount
        /// </summary>
        public long UretimMiktari { get; set; }
        /// <summary>
        /// Year
        /// </summary>
        public int Yil { get; set; }
        /// <summary>
        /// Month
        /// </summary>
        public int Ay { get; set; }
    }
}
