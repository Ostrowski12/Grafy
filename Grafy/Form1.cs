using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Grafy
{
    public partial class Form1 : Form
    {
        private const int r = 10;
        private Graphics g;
        private Pen pWierzcholek;
        private Pen pWierzcholekAktywny;
        private Pen pKrawedz;
        private Wierzcholek MouseDownWierzcholek;

        private List<Wierzcholek> wierzcholki = new List<Wierzcholek>();
        private BFS test = new BFS();
        private DFS test2 = new DFS();
        private A test3 = new A();

        List<Wierzcholek> NowaLista = new List<Wierzcholek>();
        public Form1()
        {
            InitializeComponent();

            pictureBox1.Image = new Bitmap(500, 500);

            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            pWierzcholek = new Pen(Color.Orange);
            pWierzcholek.Width = 3;
            pWierzcholekAktywny = new Pen(Color.Red);
            pWierzcholekAktywny.Width = 3;

            pKrawedz = new Pen(Color.Blue);
            pKrawedz.Width = 5;
            pKrawedz.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
        }


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseDownWierzcholek = null;
                foreach (Wierzcholek w in wierzcholki)
                {
                    if (w.Odleglosc(e.Location) < r)
                    {
                        MouseDownWierzcholek = w;
                    }
                }
                odrysujGraf();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && MouseDownWierzcholek != null)
            {
                foreach (Wierzcholek w in wierzcholki)
                {
                    if (w.Odleglosc(e.Location) < r)
                    {
                        MouseDownWierzcholek.Nastpniki.Add(w);
                        w.Nastpniki.Add(MouseDownWierzcholek);
                    }
                }
                MouseDownWierzcholek = null;
                odrysujGraf();
            }
            else if (e.Button == MouseButtons.Middle)
            {
                wierzcholki.Add(new Wierzcholek(e.Location));
                odrysujGraf();
            }
        }

        private void odrysujGraf()
        {
            g.Clear(Color.White);
            foreach (Wierzcholek w in wierzcholki)
            {
                g.DrawEllipse(new Pen(w.Kolor), w.Polozenie.X - r, w.Polozenie.Y - r, 2 * r, 2 * r);
                g.DrawString(w.Id.ToString(),
                             new System.Drawing.Font("Microsoft Sans Serif", r),
                             new SolidBrush(Color.Red),
                             w.Polozenie.X + r,
                             w.Polozenie.Y + r);

                if (w == MouseDownWierzcholek)
                {
                    g.DrawEllipse(pWierzcholekAktywny, w.Polozenie.X - r, w.Polozenie.Y - r, 2 * r, 2 * r);
                }

                foreach (Wierzcholek wn in w.Nastpniki)
                {
                    g.DrawLine(pKrawedz, w.Polozenie, wn.Polozenie);
                }
            }
            pictureBox1.Refresh();
        }



        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && MouseDownWierzcholek != null)
            {
                odrysujGraf();
                g.DrawLine(pKrawedz, MouseDownWierzcholek.Polozenie, e.Location);
                pictureBox1.Refresh();
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    
        private async void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {

                if (wierzcholki.Count > 0)
                {
                    Wierzcholek startowy = wierzcholki[0];

                    NowaLista = test.Przeszukaj(startowy);

                    foreach (Wierzcholek w in NowaLista)
                    {
                        w.Kolor = Color.Green;

                        await Task.Delay(2000);

                        odrysujGraf();
                    }

                    odrysujGraf();
                }
            }
            else if (radioButton2.Checked)
            {
                if (wierzcholki.Count > 0)
                {
                    Wierzcholek startowy = wierzcholki[0];

                    NowaLista = test2.Przeszukaj(startowy);

                    foreach (Wierzcholek w in NowaLista)
                    {
                        w.Kolor = Color.Green;

                        await Task.Delay(2000);

                        odrysujGraf();
                    }

                    odrysujGraf();
                }
            }
            else if (radioButton3.Checked)
            {
                int x,y;
                x = Convert.ToInt32(numericUpDown1.Value-1);
                y = Convert.ToInt32(numericUpDown2.Value-1);
                if (wierzcholki.Count > 0)
                {
                    Wierzcholek startowy = wierzcholki[x];
                    Wierzcholek koncowy = wierzcholki[y];

                    NowaLista = test3.A_(startowy, koncowy);

                    foreach (Wierzcholek w in NowaLista)
                    {
                        w.Kolor = Color.Green;
                        Invoke((MethodInvoker)delegate { odrysujGraf(); }); // Aktualizacja grafiki na głównym wątku

                        await Task.Delay(2000);
                    }

                    odrysujGraf();
                }

            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void button3_Click(object sender, EventArgs e)
        {
            foreach (Wierzcholek w in wierzcholki)
            {
                w.Kolor = Color.Orange;
                odrysujGraf();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            wierzcholki.Clear();
            Wierzcholek.ResetujLicznikId();
            pictureBox1.Refresh();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
