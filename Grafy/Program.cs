using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grafy
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    public class Wierzcholek
    {
        public Point Polozenie { get; }
        public Int32 Id { get; }
        public Color Kolor { get; set; }

        private List<Wierzcholek> nastpniki = new List<Wierzcholek>();
        public List<Wierzcholek> Nastpniki
        {
            get
            {
                return nastpniki;
            }
        }

        private static int newId = 0;
        private static int NewId
        {
            get { return ++newId; }
        }

        public static void ResetujLicznikId()
        {
            newId = 0;
        }

        public Wierzcholek(Point Polozenie)
        {
            this.Polozenie = Polozenie;
            this.Id = NewId;
            Kolor = Color.Orange;

        }

        internal int Odleglosc(Point p)
        {
            return (int)Math.Sqrt(Math.Pow(Polozenie.X - p.X, 2) + Math.Pow(Polozenie.Y - p.Y, 2));
        }
    }


    public abstract class PrzeszukiwanieGrafu
    {
        protected List<Wierzcholek> odwiedzone = new List<Wierzcholek>();
        public abstract List<Wierzcholek> Przeszukaj(Wierzcholek start);
    }
    public class BFS: PrzeszukiwanieGrafu
    {

        public override List<Wierzcholek> Przeszukaj(Wierzcholek start)
        {
            odwiedzone.Clear();
            Queue<Wierzcholek> kolejka = new Queue<Wierzcholek>();

            kolejka.Enqueue(start);
            odwiedzone.Add(start);

            while (kolejka.Count > 0)
            {
                Wierzcholek aktualny = kolejka.Dequeue();

                foreach (Wierzcholek sasiad in aktualny.Nastpniki)
                {
                    if (!odwiedzone.Contains(sasiad))
                    {
                        kolejka.Enqueue(sasiad);
                        odwiedzone.Add(sasiad);
                    }
                }
            }

            return odwiedzone;
        }
    }

    public class DFS : PrzeszukiwanieGrafu
    {
        public override List<Wierzcholek> Przeszukaj(Wierzcholek start)
        {
            odwiedzone.Clear();
            PrzeszukajRekurencyjnie(start);
            return odwiedzone;
        }

        private void PrzeszukajRekurencyjnie(Wierzcholek wierzcholek)
        {
            if (!odwiedzone.Contains(wierzcholek))
            {
                odwiedzone.Add(wierzcholek);

                foreach (Wierzcholek sasiad in wierzcholek.Nastpniki)
                {
                    PrzeszukajRekurencyjnie(sasiad);
                }
            }
        }
    }

    public class A : PrzeszukiwanieGrafu
    {
        public override List<Wierzcholek> Przeszukaj(Wierzcholek start)
        {
            odwiedzone.Clear();

            return odwiedzone;
        }

        public List<Wierzcholek> A_(Wierzcholek start, Wierzcholek meta)
        {
            
            List<Wierzcholek> lista = new List<Wierzcholek> { start };
            Dictionary<Wierzcholek, double> Odleglosc = new Dictionary<Wierzcholek, double> { { start, 0 } };
            Dictionary<Wierzcholek, Wierzcholek> poprzednik = new Dictionary<Wierzcholek, Wierzcholek>();

            while (lista.Any())
            {
                Wierzcholek aktualny = WybierzNajlepszy(lista, Odleglosc, meta);

                lista.Remove(aktualny);

                if (aktualny.Equals(meta))
                {
                    List<Wierzcholek> sciezka = KonstruujSciezke(start, meta, poprzednik, Odleglosc);
                    sciezka.Reverse();
                    return sciezka;
                }

                foreach (Wierzcholek sasiad in aktualny.Nastpniki)
                {
                    double nowyKoszt = Odleglosc[aktualny] + OdlegloscMiedzyWierzcholkami(aktualny, sasiad); 

                    if (!Odleglosc.ContainsKey(sasiad) || nowyKoszt < Odleglosc[sasiad])
                    {
                        Odleglosc[sasiad] = nowyKoszt;
                        poprzednik[sasiad] = aktualny;

                        if (!lista.Contains(sasiad))
                            lista.Add(sasiad);
                    }
                }
            }

            return null;


        }

        private static Wierzcholek WybierzNajlepszy(List<Wierzcholek> lista, Dictionary<Wierzcholek, double> Odleglosc, Wierzcholek cel)
        {
            Wierzcholek najlepszy = null;
            double najlepszyKoszt = double.MaxValue;

            foreach (Wierzcholek w in lista)
            {
                double koszt = Odleglosc[w];

                if (koszt < najlepszyKoszt)
                {
                    najlepszyKoszt = koszt;
                    najlepszy = w;
                }
            }

            return najlepszy;
        }

        private static List<Wierzcholek> KonstruujSciezke(Wierzcholek start, Wierzcholek cel, Dictionary<Wierzcholek, Wierzcholek> poprzednik, Dictionary<Wierzcholek, double> Odleglosc)
        {
            List<Wierzcholek> punkty = new List<Wierzcholek>();
            Wierzcholek aktualny = cel;

            while (aktualny != null)
            {
                punkty.Add(aktualny);
                aktualny = poprzednik.ContainsKey(aktualny) ? poprzednik[aktualny] : null;
            }

            return punkty;
        }


        private double OdlegloscMiedzyWierzcholkami(Wierzcholek a, Wierzcholek b)
        {
            return Math.Sqrt(Math.Pow(a.Polozenie.X - b.Polozenie.X, 2) + Math.Pow(a.Polozenie.Y - b.Polozenie.Y, 2));
        }
    }

}


