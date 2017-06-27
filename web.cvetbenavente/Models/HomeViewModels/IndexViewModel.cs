using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.cvetbenavente.Models.HomeViewModels
{
    public class IndexViewModel
    {
        public class PieData
        {
            public List<float> Valores { get; set; } 
            public List<string> Labels { get; set; }
            public List<Tuple<int, int, int>> RGB { get; set; }
        }

        public PieData TopEspecies { get; set; }
    }
}
