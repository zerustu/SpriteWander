using System;
using System.Collections.Generic;
using System.Text;

namespace Walking_pokemon
{
    class Options
    {
        public int TickFrequency;

        public Options(int tickFrequency = 60)
        {
            TickFrequency = tickFrequency;
        }
    }
}
