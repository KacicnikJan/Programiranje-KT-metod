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
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Controls.DataVisualization.Charting;



namespace SO_2_Kacicnik
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<List<string>> parametri;
        public List<List<string>> alternative;
        List<string> vrednost_P;
        List<string> vrednost_A;
        public List<double> alternativa_KT;
        TextBlock block;
        int st_parametrov;
        int dodani_param;
        int margin_left = 240;

        public MainWindow()
        {
            InitializeComponent();
            parametri = new List<List<string>>();
            alternative = new List<List<string>>();
            vrednost_P = new List<string>();
            vrednost_A = new List<string>();
            st_parametrov = 0;
            dodani_param = 0;
            block = new TextBlock();
            alternativa_KT = new List<double>();
        }

        private void btnDodajParameter_Click(object sender, RoutedEventArgs e)
        {
            vrednost_P.Add(txtParameter.Text);
            vrednost_P.Add(SliderUtez.Value.ToString());

            parametri.Add(vrednost_P);

            vrednost_P = new List<string>();
        }

        private void btnKoncajVnos_Click(object sender, RoutedEventArgs e)
        {
            txtParameter.IsEnabled = false;
            btnDodajParameter.IsEnabled = false;
            btnDodajAlternativo.IsEnabled = true;
            txtAlternativa.IsEnabled = true;
            st_parametrov = parametri.Count;
            txtBlockParam.Text = "Parametri\n\n";
            txtBlockUtezi.Text = "Utezi\n\n";
            for (int i = 0; i < parametri.Count; i++)
            {
                txtBlockParam.Text += parametri.ElementAt(i).ElementAt(0) + "\n";
                txtBlockUtezi.Text += parametri.ElementAt(i).ElementAt(1) + "\n"; ;
            }
            txtBlockParam.Text += "\nSkupaj:";
        }

        private void btnDodajAlternativo_Click(object sender, RoutedEventArgs e)
        {
            btnTockeAlt.IsEnabled = true;
            btnDodajAlternativo.IsEnabled = false;
            vrednost_A.Add(txtAlternativa.Text);
            alternative.Add(vrednost_A);
            vrednost_A = new List<string>();
            labelKateri.Content = "Vnesite tocke za " + parametri.ElementAt(dodani_param).ElementAt(0) + " alternative:";
        }

       private void podatki_alternative() {
            alternative.Last().Add(sliderVrednostAtr.Value.ToString());
            dodani_param++;
            if (dodani_param < st_parametrov)
            labelKateri.Content = "Vnesite tocke za " + parametri.ElementAt(dodani_param).ElementAt(0) + " alternative:";
        }

        private void btnTockeAlt_Click(object sender, RoutedEventArgs e)
        {
            double izracun_KT = 0;
                podatki_alternative();
            
            if(dodani_param == st_parametrov) { 

                block.Width = 100;
                block.Height = 350;
                block.VerticalAlignment = VerticalAlignment.Top;
                block.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                block.Margin = new Thickness(margin_left, 310, 0, 0);
                block.Text = "\n";

                for (int j = 0; j < alternative.Last().Count; j++)
                {
                    string a = alternative.Last().ElementAt(j).ToString() + "\n";
                    if(j != 0)
                    izracun_KT += double.Parse(parametri.ElementAt(j-1).ElementAt(1).ToString()) * (double.Parse(alternative.Last().ElementAt(j).ToString()));
                    
                    block.Text += alternative.Last().ElementAt(j).ToString() + "\n";
                }
                alternativa_KT.Add(izracun_KT);
                block.Text += "\n" + izracun_KT.ToString();
                okno.Children.Add(block);
                block = new TextBlock();
                dodani_param = 0;
                btnDodajAlternativo.IsEnabled = true;
                btnTockeAlt.IsEnabled = false;
                margin_left += 110;
            }
        }

        private void btnKoncajVnosAlt_Click(object sender, RoutedEventArgs e)
        {
            double najvisja_skupna  = 0;
            if (alternativa_KT.Count != 0)
            {
                najvisja_skupna = alternativa_KT.Max();
                tbNajvisja.Content = "Alternativa z najvišjo vrednostjo: " + alternative.ElementAt(alternativa_KT.IndexOf(najvisja_skupna)).ElementAt(0) + " (" + najvisja_skupna.ToString() + ")";

                KeyValuePair<string, double>[] podatki = new KeyValuePair<string, double>[alternativa_KT.Count];
                for (int i = 0; i < alternativa_KT.Count; i++)
                {
                    podatki[i] = new KeyValuePair<string, double>(alternative.ElementAt(i).ElementAt(0), alternativa_KT.ElementAt(i));
                }
                ((BarSeries)mcChart.Series[0]).ItemsSource = podatki;
            }

            if (parametri.Count != 0)
            {
                KeyValuePair<string, double>[] pie_podatki = new KeyValuePair<string, double>[parametri.Count];
                for (int i = 0; i < parametri.Count; i++)
                {
                    pie_podatki[i] = new KeyValuePair<string, double>(parametri.ElementAt(i).ElementAt(0), double.Parse(parametri.ElementAt(i).ElementAt(1)));
                }
           ((PieSeries)mcPie.Series[0]).ItemsSource = pie_podatki;
            }
        }
    }
}