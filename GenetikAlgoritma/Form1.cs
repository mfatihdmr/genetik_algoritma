using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Schema;

namespace GenetikAlgoritma
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool isRunning = false;
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        
        
        // Algoritma çalışıyor ise durdurur, duruyorsa çalıştırır.
        private bool ToggleKontrol()
        {
            if (isRunning)
            {
                isRunning = false;
                button1.Text = "HESAPLA";
            }
            else
            {
                button1.Text = "Durdur";
                isRunning = true;
            }

            return isRunning;
        }
        private Series GenSeries()
        {
            // Önce flowLayoutPanel1 içindeki kontrolleri temizle
            flowLayoutPanel1.Controls.Clear();
            // Label11'in metnini "Toplam:0" olarak ayarla
            label11.Text = "Toplam:0";
            // chart1'in mevcut serilerini temizle
            this.chart1.Series.Clear();
            // Yeni bir seri oluştur ve chart1'e ekle
            Series series = this.chart1.Series.Add("Sonuclar");
            // Seri tipini "Alan" olarak ayarla
            series.ChartType = SeriesChartType.Area;
            // Seri kenar genişliğini ayarla
            series.BorderWidth = 3;
            // Seri rengini ayarla
            series.Color = Color.IndianRed;
            // Oluşturulan seriyi döndür
            return series;
        }



        private async void button1_Click(object sender, EventArgs e)
        {
            // Eğer algoritma zaten çalışıyorsa, ToggleKontrol() metodu çağrılarak çalışmayı durdur.
            if (!ToggleKontrol()) return;

            // GenSeries metodu çağrılarak yeni bir seri oluşturulur ve series değişkenine atılır.
            Series series = GenSeries();

            // Kullanıcı arayüzündeki parametre kontrollerinden gerekli değerler alınır.
            int popSayi = (int)numericUpDown1.Value;
            int elitPop = (int)numericUpDown5.Value;
            int iterasyon = (int)numericUpDown4.Value;
            double carazlamaOran = (double)numericUpDown2.Value / 100;
            double mutasyonOran = (double)numericUpDown3.Value / 100;

            // GenetikDriver sınıfından bir nesne oluşturulur ve popülasyon sayısı ile başlatılır.
            GenetikDriver GenDrv = new GenetikDriver(popSayi);
            // Elit popülasyon sayısını belirler.
            GenDrv.elitPop = elitPop;

            // Görüntü işlemleri için bir Image nesnesi oluşturulur. (Örneğin: matyas resmi gibi)
            Image img = Properties.Resources.matyas;

            // Belirlenen iterasyon sayısı kadar döngü başlatılır.
            for (int j = 0; j < iterasyon; j++)
            {
                // Elitizm adımı gerçekleştirilir.
                GenDrv.Elitizm();
                // Turnuva çifti oluşturma adımı gerçekleştirilir.
                GenDrv.TurnuvaCiftiOlustur();
                // Çaprazlama adımı gerçekleştirilir.
                GenDrv.Caprazla(carazlamaOran);
                // Mutasyon adımı gerçekleştirilir.
                GenDrv.Mutasyon(mutasyonOran);

                // En iyi canlının elitizm bileşenine eklenmesi sağlanır.
                ElitizmFlowLayoutEkle(GenDrv.BestCanli());

                // Sonuçların chart üzerinde gösterilmesi için en iyi skor eklenir.
                var eniyiSkor = GenDrv.BestCanli().Gen.MatyasFormulSkor * 10000;
                series.Points.AddXY(j, eniyiSkor);

                // En iyi canlının x1 ve x2 değerleri ekrana yazdırılır.
                label8.Text = GenDrv.BestCanli().Gen.x1.ToString();
                label9.Text = GenDrv.BestCanli().Gen.x2.ToString();

                // Eğer algoritma çalışmıyorsa döngüden çıkılır.
                if (!isRunning) break;
                // Eğer son iterasyona gelinmişse algoritma durdurulur.
                if (j == iterasyon - 1) ToggleKontrol();
            }
        }


        public bool ElitizmFlowLayoutEkle(Canli c)
        {
            // flowLayoutPanel1 içerisindeki ElitizmComponent bileşenlerini tek tek kontrol et
            foreach (var elitizm in flowLayoutPanel1.Controls.OfType<ElitizmComponent>())
            {
                // Eğer eklenmek istenen Canli'nın skoru zaten bir ElitizmComponent içinde varsa, eklenmez.
                if (c.Gen.MatyasFormulSkor == elitizm.Canli.Gen.MatyasFormulSkor)
                    return false;
            }

            // Elitizm bileşeni sayısını label11 üzerine yazdır.
            label11.Text = "Toplam:" + (flowLayoutPanel1.Controls.Count + 1);

            // Yeni bir ElitizmComponent nesnesi oluştur ve flowLayoutPanel1'e ekle.
            var comp = new ElitizmComponent(c, flowLayoutPanel1.Controls.Count + 1);

            // Yeni oluşturulan ElitizmComponent bileşeninin silme butonuna tıklanıldığında
            // ilgili Canli nesnesini bir listeye ekler.
            comp.pictureBox2.Click += (s, arg) =>
            {
                var canli = ((s as Control).Parent.Parent.Parent as ElitizmComponent).Canli;
                var list = new List<Canli>();
                list.Add(canli);
            };

            // Yeni ElitizmComponent bileşenini flowLayoutPanel1'e ekle.
            flowLayoutPanel1.Controls.Add(comp);

            // Ekleme işlemi başarılı olduğu için true değeri döndürülür.
            return true;
        }



        private void buttonClr1_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonClr2_Click(object sender, EventArgs e)
        {
            

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    

}
