using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace BlackJack3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static int OhanyadikLap = 0, JhanyadikLap = 0;
        static string[] lapok = new string[52];
        static int[] pontok = new int[52];
        static int sumOpont = 0, sumJpont = 0;
        static int tet = 100, osszeg = 10000;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnHit_Click(object sender, RoutedEventArgs e)
        {
            if (JhanyadikLap < 2)
            {
                UjJatek();
            }
            else
            {
                Random rnd = new Random();
                int vel, lappont;
                Uri resourceuri;
                string lapnev, imgnev;

                JhanyadikLap++;
                vel = rnd.Next(0, 51);
                lapnev = lapok[vel];
                lappont = pontok[vel];
                sumJpont += lappont;
                Jpont.Content = sumJpont;
                imgnev = "JImg" + JhanyadikLap.ToString();
                resourceuri = new Uri("cards/" + lapnev, UriKind.Relative);
                (this.FindName(imgnev) as Image).Source = new BitmapImage(resourceuri);
                if (sumJpont > 21)
                {

                    Uzenet("Ez túl sok! Vesztettél!\nÚj játék, változatlan téttel?", "Sok!");

                }
            }


        }



        private void Nullazo()
        {
            JhanyadikLap = 0;
            JImg1.Source = null;
            JImg2.Source = null;
            JImg3.Source = null;
            JImg4.Source = null;
            JImg5.Source = null;
            JImg6.Source = null;
            JImg7.Source = null;
            JImg8.Source = null;
            JImg9.Source = null;
            OImg1.Source = null;
            OImg2.Source = null;
            OImg3.Source = null;
            OImg4.Source = null;
            OImg5.Source = null;
            OImg6.Source = null;
            OImg7.Source = null;
            OImg8.Source = null;
            OImg9.Source = null;
            sumJpont = 0;
            sumOpont = 0;
        }

        private void btnStand_Click(object sender, RoutedEventArgs e)
        {
            while (sumOpont < 17)
            {
                Random rnd = new Random();
                int vel, lappont;
                Uri resourceuri;
                string lapnev, imgnev;

                OhanyadikLap++;
                vel = rnd.Next(0, 51);
                lapnev = lapok[vel];
                lappont = pontok[vel];
                sumOpont += lappont;
                Opont.Content = sumOpont;
                imgnev = "OImg" + OhanyadikLap.ToString();
                resourceuri = new Uri("cards/" + lapnev, UriKind.Relative);
                (this.FindName(imgnev) as Image).Source = new BitmapImage(resourceuri);
            }
            if (sumJpont > sumOpont || sumOpont > 21)
            {
                osszeg += 2 * tet;
                txtOsszeg.Text = osszeg.ToString();
                Uzenet("Nyertél " + (2 * tet) + " forintot!\n Új játék ugyanazzal a téttel?", "Nyertél!");
            }
            if (sumJpont == sumOpont)
            {
                osszeg += tet;
                txtOsszeg.Text = osszeg.ToString();
                Uzenet("Döntetlen! \n Új játék ugyanazzal a téttel?", "Döntgetlen!");
            }
            if (sumOpont > sumJpont && sumOpont <= 21)
            {
                Uzenet("Az osztó nyert! \n Új játék ugyanazzal a téttel?", "Vesztettél!");
            }


        }

        private void UjJatek()
        {
            Nullazo();

            if (Convert.ToInt32(TxtTet.Text) > Convert.ToInt32(txtOsszeg.Text) || Convert.ToInt32(TxtTet.Text) <= 0)
            {
                MessageBox.Show("A tét nem lehet nagyobb, mint a rendelkezésedre álló összeg, vagy nem lehet negatív vagy nulla!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                StreamReader sr = new StreamReader("card.txt");
                string sor;
                string[] szetszed;
                Random rnd = new Random();
                int vel = rnd.Next(0, 51);
                for (int i = 0; i < 52; i++)
                {
                    sor = sr.ReadLine();
                    szetszed = sor.Split('\t');
                    lapok[i] = szetszed[0];
                    pontok[i] = Convert.ToInt32(szetszed[1]);
                }
                sr.Close();
                LapElehelyezes(lapok[vel], pontok[vel], "OImg1");
                OhanyadikLap++;
                vel = rnd.Next(0, 51);
                LapElehelyezes(lapok[vel], pontok[vel], "JImg1");
                JhanyadikLap++;
                vel = rnd.Next(0, 51);
                LapElehelyezes(lapok[vel], pontok[vel], "OImg2");
                OhanyadikLap++;
                vel = rnd.Next(0, 51);
                LapElehelyezes(lapok[vel], pontok[vel], "JImg2");
                JhanyadikLap++;
                tet = Convert.ToInt32(TxtTet.Text);
                osszeg = Convert.ToInt32(txtOsszeg.Text) - tet;
                txtOsszeg.Text = osszeg.ToString();
            }



        }
        private void LapElehelyezes(string lapnev, int lapont, string imgnev)
        {
            Uri resourceuri = new Uri("cards/" + lapnev, UriKind.Relative);
            (this.FindName(imgnev) as Image).Source = new BitmapImage(resourceuri);
            if (imgnev[0] == 'J')
            {
                sumJpont += lapont;
                Jpont.Content = sumJpont;
            }
            else
            {
                sumOpont += lapont;
                Opont.Content = sumOpont;
            }
        }

        private void Uzenet(string uzi, string cim)
        {
            MessageBoxResult result = MessageBox.Show(uzi, cim, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {

                UjJatek();
            }
            else
            {
                JhanyadikLap = 0;
            }
        }
    }
}
