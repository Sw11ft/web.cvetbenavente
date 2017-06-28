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
            public List<float> Valores { get; set; } = new List<float>();
            public List<string> Labels { get; set; } = new List<string>();
            public List<Tuple<int, int, int>> RGB { get; set; } = new List<Tuple<int, int, int>>();
        }

        public PieData TopEspecies { get; set; } = new PieData();

        public int NrAnimais { get; set; } = 0;
    }
}
