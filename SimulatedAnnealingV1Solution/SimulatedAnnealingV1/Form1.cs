using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading; // sleep kullanmak için bu kütüphane gerekli !!!
using System.Collections; // ArrayList kullanmak için bu kütüphane gerekli !!!

namespace SimulatedAnnealingV1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

/////////////////////   Global Değişkenler   //////////////////////////


        //  int komsu = 10;

        //  int index = 0;
        //  int deger = 0;

        // int baslangicIndex = 0;
        // int baslangicDeger = 0;

        int[] dizi = new int[99];  // main dizi

       // ArrayList listeMax = new ArrayList(); //  maximumların tutulduğu dizi                 //
        ArrayList listeLocalMax = new ArrayList(); // Local maximumların belirlendiği dizi      //  ---------------
                                                                                                //  BUNLARI AYARLA !!!!!!
      //  ArrayList listeMin = new ArrayList(); //  minimumların tutulduğu dizi                 //  ---------------
        ArrayList listeLocalMin = new ArrayList(); // Local  minimumların belirlendiği dizi     //


        int baslangicSicakligi = 100;
        

        int pointer = 0;

        int eniyiIndex;
        int eniyiDeger;

        int enIYI = 0;

       
        Random r = new Random();

        int[] global = new int[100];

        ///////////////////////////////////////////////////////////////////////


        // Metodlar
        public void diziolustur(int sayi)
        {
            Random rastgele = new Random();
            for (int i = 0; i < dizi.Length; i++)
            {
                dizi[i] = rastgele.Next(sayi);
                listBox1.Items.Add(Convert.ToString(dizi[i]));
            }       
        }

        public int komsuhesapla()
        {
            int min = 9;
            int max = dizi.Length - 10;
            Random rnd = new Random();
            int komsuBaslangic = rnd.Next(min, max);
            return komsuBaslangic;
        }

        public void verileriCiz()
        {
            int sayac = 0;
            chart1.Series["Pointer"].Points.AddXY(1, dizi[1]);

            foreach (var eleman in dizi)
            {
                chart1.Series["Veri"].Points.AddXY(sayac, eleman);
                chart2.Series["Veri"].Points.AddXY(sayac, eleman);
                sayac++;
            }
        }

        public void pointerGoster(int konum, int deger)
        {
            chart1.Series["Pointer"].Points.Clear();
            chart1.Series["Pointer"].Points.AddXY(konum, deger);
            for (int i = 0; i <20000; i++)
            {
                //
            }           
        }



///////////////////////////////////////////////////////////////////////


                /////
        private async void button1_Click(object sender, EventArgs e)  //     !!! ASYNC kullandım !!! (thread sıkıntı yaptı)
        {       /////

            int derece = baslangicSicakligi;
            int sayac = 0;
            int iterasyon = 0;
            while (baslangicSicakligi > 0)
            {
                iterasyon++;
                listBox2.Items.Add(Convert.ToString(iterasyon + ". iterasyon"));
                label1.Text = "SICAKLIK = " + Convert.ToString(derece) + "°";

                if (iterasyon == 1)
                {
                    pointer = 0;
                }
                else
                {
                    pointer = komsuhesapla();
                }

                int localMax = 0;
                for (int i = pointer; i < (pointer+10); i++)
                {

                    int delta = dizi[i+1] - dizi[i];

                    

                    if (delta > 0) //|| (rastt.Next() < ((delta/baslangicSicakligi))))
                    {
                        

                        eniyiIndex = i + 1;
                        eniyiDeger = dizi[eniyiIndex];
                        listBox2.Items.Add("  a) " + Convert.ToString(dizi[eniyiIndex]));

                        global[eniyiIndex] = dizi[eniyiIndex];


                        if (dizi[eniyiIndex] > dizi[eniyiIndex - 1])
                        {

                            localMax = eniyiIndex;
                            listeLocalMax.Add(eniyiIndex);
                            sayac++;
                           // chart2.Series["LocalMax"].Points.AddXY(localMax, dizi[localMax]);
                        }

                        /* if (eniyiDeger> enIYI)
                        {
                            enIYI = eniyiDeger;
                        }*/

                        ////// pointerGoster(eniyiIndex, eniyiDeger);   // bu yemedi :)

                        chart1.Series["Pointer"].Points.Clear();                             // LokalMax ları bakılan komşulardan bul 
                        chart1.Series["Pointer"].Points.AddXY(eniyiIndex, dizi[eniyiIndex]); //
                        await Task.Delay(25);
                        //////// await i görünce işlemi duraklatır (asenkron) ///////

                        if ((r.Next() > (Math.Exp(((delta) / baslangicSicakligi)))))
                        {
                            eniyiIndex = i + 1;
                            eniyiDeger = dizi[eniyiIndex];
                           
                            if (eniyiDeger > enIYI)
                            {
                                enIYI = eniyiDeger;
                                listBox2.Items.Add("     b " + Convert.ToString(dizi[eniyiIndex]));
                               
                            }
                        }
                    }
                   /* else
                    {
                       
                    }*/
                } 
                 
                listBox2.Items.Add("    global max " + Convert.ToString(enIYI));      
                listBox2.Items.Add("    global max index " + Convert.ToString(Array.IndexOf(dizi,enIYI)));
                listBox2.Items.Add(" ");
                int x = Array.IndexOf(dizi, enIYI);
                
                baslangicSicakligi--;
                derece--;
            }
            chart1.Series["Pointer"].Points.Clear();                                  // GlobalMax ı pointer olarak göster
            chart1.Series["Pointer"].Points.AddXY(Array.IndexOf(dizi,enIYI), enIYI);  //
            
            label1.Text = "SICAKLIK = " + Convert.ToString(derece) + "°";


            for (int a = 0; a < global.Length-1; a++)                                  //
            {                                                                          //                                                                                      //
                if ((global[a+1]) > global[a])                                         //
                {                                                                      //
                    global[a] = 0;                                                     //
                }                                                                      // 
            }                                                                          // LocalMax lar bulundu
                                                                                       //
            global[Array.IndexOf(dizi, enIYI)] = 0;                                    //
                                                                                       //
            for (int a = 0; a < global.Length; a++)                                    //
            {                                                                          //
                chart2.Series["LocalMax"].Points.AddXY(a, global[a]);                  //
            }                                                                          //

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            baslangicSicakligi = 100;
            enIYI = 0;

            diziolustur(200); // 0-200 arası rastgele dizi[99] (100 elemanlı)

            chart1.Series["Veri"].Points.Clear();
            chart1.Series["Pointer"].Points.Clear();

            chart2.Series["Veri"].Points.Clear();
            chart2.Series["LocalMax"].Points.Clear();

            verileriCiz();  // dizi deki verileri 'spline' olarak çiz
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
