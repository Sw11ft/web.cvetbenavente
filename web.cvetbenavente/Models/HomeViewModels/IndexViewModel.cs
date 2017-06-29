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

        public class MainGraphData
        {
            public class Mes
            {
                public int NovosAnimais { get; set; } = 0;
                public int NovosClientes { get; set; } = 0;
                public string Nome { get; set; }
            }

            public List<Mes> Meses { get; set; } = new List<Mes>();
        }

        public PieData TopEspecies { get; set; } = new PieData();

        public MainGraphData MainGraph { get; set; } = new MainGraphData();

        public int NrAnimais { get; set; } = 0;

        public int NrClientes { get; set; } = 0;

        public double AnimaisPorCliente { get; set; } = 0;
    }
}
