using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using Microsoft.VisualBasic;
using System.Drawing.Drawing2D;

namespace WindowsFormsApp1._1
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public object PictureEdit1 { get; private set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void kaydet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {         
            SaveFileDialog kaydet = new SaveFileDialog();
            kaydet.Filter = "Resim Dosyası | *.jpg;*.png;*.bmp;*.gif | Tüm Dosyalar|*.*";
            kaydet.Title = "Resmi Kaydet";
            kaydet.ShowDialog();
            int k = 1;
            try
            {
                
                if (pictureEdit2.Image != null)
                {
                    Image img = pictureEdit2.Image;
                    Bitmap bmp = new Bitmap(img.Width, img.Height);
                    Graphics gra = Graphics.FromImage(bmp);
                    gra.DrawImageUnscaled(img, new Point(0, 0));
                    gra.Dispose();
                    
                    string belgelerim = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                    bmp.Save(belgelerim + "\\Resim" + k + ".jpg", ImageFormat.Jpeg);
                    k++;
                    bmp.Dispose();
                    //pictureEdit2.Image.Save(belgelerim,ImageFormat.Jpeg);
                }
            }
            catch { }
        }

        private void resimYukle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                pictureEdit1.Visible = true; pictureEdit2.Visible = true;
                XtraOpenFileDialog dosya = new XtraOpenFileDialog();
                dosya.Filter = "Resim Dosyası | *.jpg;*.png;*.bmp;*.gif | Tüm Dosyalar|*.*";
                dosya.Title = "Resim Kaydet";
                dosya.ShowDialog();
                string dosyaYolu = dosya.FileName;
                pictureEdit1.Image = Image.FromFile(dosyaYolu);
            }
            catch (Exception)
            {
                // hata vermemesi için
                pictureEdit1.Image = Image.FromFile(@"C:\Users\Dogan\source\repos\WindowsFormsApp1.1\WindowsFormsApp1.1\images\corona.jpg");
            }
        }

        private void btnGriTon_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                Color OkunanRenk, DonusenRenk;

                Bitmap GirisResmi = new Bitmap(pictureEdit1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                Bitmap CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi); //Cikis resmini oluşturuyor.Boyutları giriş resmi ile aynı olur.

                int GriDeger = 0;
                for (int x = 0; x < ResimGenisligi; x++)
                {
                    for (int y = 0; y < ResimYuksekligi; y++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x, y);
                        double R = OkunanRenk.R;
                        double G = OkunanRenk.G;
                        double B = OkunanRenk.B;

                        //GriDeger = Convert.ToInt16((R + G + B) / 3);
                        GriDeger = Convert.ToInt16(R * 0.3 + G * 0.6 + B * 0.1);
                        DonusenRenk = Color.FromArgb(GriDeger, GriDeger, GriDeger);
                        CikisResmi.SetPixel(x, y, DonusenRenk);
                    }
                }
                pictureEdit2.Image = CikisResmi;
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void negatifCevir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                Color OkunanRenk, DonusenRenk;
                int R = 0, G = 0, B = 0;

                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureEdit1.Image);

                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;

                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);

                for (int x = 0; x < ResimGenisligi; x++)
                {
                    for (int y = 0; y < ResimYuksekligi; y++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x, y);

                        R = 255 - OkunanRenk.R;
                        G = 255 - OkunanRenk.G;
                        B = 255 - OkunanRenk.B;

                        DonusenRenk = Color.FromArgb(R, G, B);
                        CikisResmi.SetPixel(x, y, DonusenRenk);
                    }
                }
                pictureEdit2.Image = CikisResmi;
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void histogram_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //using System.Collections; //Arraylist in kütüphanesi. Sayfada olması gerekir.
            if (pictureEdit1.Image != null)
            {
                ArrayList DiziPiksel = new ArrayList();
                int OrtalamaRenk = 0;
                Color OkunanRenk;
                int R = 0, G = 0, B = 0;
                Bitmap GirisResmi; //Histogram için giriş resmi gri-ton olmalıdır.
                GirisResmi = new Bitmap(pictureEdit1.Image);
                int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı.
                int ResimYuksekligi = GirisResmi.Height;

                for (int x = 0; x < GirisResmi.Width; x++)
                {
                    for (int y = 0; y < GirisResmi.Height; y++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x, y);
                        OrtalamaRenk = (int)(OkunanRenk.R + OkunanRenk.G + OkunanRenk.B) / 3; //Griton resimde üç kanal rengi aynı değere sahiptir.
                        DiziPiksel.Add(OrtalamaRenk); //Resimdeki tüm noktaları diziye atıyor.
                    }
                }

                int[] DiziPikselSayilari = new int[256];
                for (int r = 0; r <= 255; r++) //256 tane renk tonu için dönecek.
                {
                    int PikselSayisi = 0;
                    for (int s = 0; s < DiziPiksel.Count; s++) //resimdeki piksel sayısınca dönecek.
                    {
                        if (r == Convert.ToInt16(DiziPiksel[s]))
                            PikselSayisi++;
                    }
                    DiziPikselSayilari[r] = PikselSayisi;
                }
                //Değerleri listbox'a ekliyor diğer formda bu kod.
                int RenkMaksPikselSayisi = 0; //Grafikte y eksenini ölçeklerken kullanılacak.
                for (int k = 0; k <= 255; k++)
                {
                    //Maksimum piksel sayısını bulmaya çalışıyor.
                    if (DiziPikselSayilari[k] > RenkMaksPikselSayisi)
                    {
                        RenkMaksPikselSayisi = DiziPikselSayilari[k];
                    }
                }

                //Grafiği çiziyor.
                Graphics CizimAlani;
                Pen Kalem1 = new Pen(System.Drawing.Color.Yellow, 1);
                Pen Kalem2 = new Pen(System.Drawing.Color.Red, 1);
                CizimAlani = pictureEdit2.CreateGraphics();
                pictureEdit2.Refresh();
                int GrafikYuksekligi = 300;
                double OlcekY = RenkMaksPikselSayisi / GrafikYuksekligi;
                double OlcekX = 1.5;
                int X_kaydirma = 10;
                for (int x = 0; x <= 255; x++)
                {
                    if (x % 50 == 0)
                        CizimAlani.DrawLine(Kalem2, (int)(X_kaydirma + x * OlcekX),
                       GrafikYuksekligi, (int)(X_kaydirma + x * OlcekX), 0);
                    CizimAlani.DrawLine(Kalem1, (int)(X_kaydirma + x * OlcekX), GrafikYuksekligi,
                   (int)(X_kaydirma + x * OlcekX), (GrafikYuksekligi - (int)(DiziPikselSayilari[x] / OlcekY)));
                    //Dikey kırmızı çizgiler.

                }

                histogramGrafik frm = new histogramGrafik();
                frm.gRESMİ = GirisResmi;
                frm.Histogram();
                frm.ShowDialog();
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnParlaklık_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                String parlaklikDeger = Interaction.InputBox("Parlaklık kaç artsın?");

                int R = 0, G = 0, B = 0;
                Color okunanRenk, DonusenRenk;
                Bitmap girisResmi, cikisResmi;

                girisResmi = new Bitmap(pictureEdit1.Image);
                int resimGenisligi = girisResmi.Width;
                int resimYuksekligi = girisResmi.Height;
                cikisResmi = new Bitmap(resimGenisligi, resimYuksekligi);

                int i = 0, j = 0; // çıkış resminin x ve y si olacak
                for (int x = 0; x < resimGenisligi; x++)
                {
                    j = 0;
                    for (int y = 0; y < resimYuksekligi; y++)
                    {
                        okunanRenk = girisResmi.GetPixel(x, y);
                        R = okunanRenk.R + Convert.ToInt16(parlaklikDeger);
                        G = okunanRenk.G + Convert.ToInt16(parlaklikDeger);
                        B = okunanRenk.B + Convert.ToInt16(parlaklikDeger);

                        if (R < 0) R = 0;
                        if (G< 0) G= 0;
                        if (B< 0) B= 0;

                        if (R > 255) R = 255;
                        if (G > 255) G = 255;
                        if (B > 255) B = 255;

                        DonusenRenk = Color.FromArgb(R, G, B);
                        cikisResmi.SetPixel(i, j, DonusenRenk);
                        j++;
                    }
                    i++;
                }
                pictureEdit2.Image = cikisResmi;
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEsikleme_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                String esiklemeDegeri = Interaction.InputBox("Eşikleme değerini girin!");

                int R = 0, G = 0, B = 0;
                Color okunanRenk, donusenRenk;
                Bitmap girisResmi, cikisResmi;
                girisResmi = new Bitmap(pictureEdit1.Image);
                int resimYuksekligi = girisResmi.Height;
                int resimGenisligi = girisResmi.Width;
                cikisResmi = new Bitmap(resimGenisligi, resimYuksekligi);

                for (int x = 0; x < resimGenisligi; x++)
                {
                    for (int y = 0; y < resimYuksekligi; y++)
                    {
                        okunanRenk = girisResmi.GetPixel(x, y);
                        if (okunanRenk.R >= Convert.ToInt16(esiklemeDegeri))
                            R = 255;
                        else
                            R = 0;
                        if (okunanRenk.G >= Convert.ToInt16(esiklemeDegeri))
                            G = 255;
                        else
                            G = 0;
                        if (okunanRenk.B >= Convert.ToInt16(esiklemeDegeri))
                            B = 255;
                        else
                            B = 0;
                        donusenRenk = Color.FromArgb(R, G, B);
                        cikisResmi.SetPixel(x, y, donusenRenk);
                    }
                }
                pictureEdit2.Image = cikisResmi;
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnKontrast_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                String C_KontrastSeviyesi = Interaction.InputBox("Kontrast Seviyesi");

                int R = 0, G = 0, B = 0;
                Color OkunanRenk, DonusenRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureEdit1.Image);
                int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı.
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi); //Cikis resmini oluşturuyor. Boyutları giriş resmi ile aynı olur.

                double F_KontrastFaktoru = (259 * (Convert.ToInt16(C_KontrastSeviyesi) + 255)) / (255 * (259 - Convert.ToInt16(C_KontrastSeviyesi)));
                for (int x = 0; x < ResimGenisligi; x++)
                {
                    for (int y = 0; y < ResimYuksekligi; y++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x, y);
                        R = OkunanRenk.R;
                        G = OkunanRenk.G;
                        B = OkunanRenk.B;
                        R = (int)((F_KontrastFaktoru * (R - 128)) + 128);
                        G = (int)((F_KontrastFaktoru * (G - 128)) + 128);
                        B = (int)((F_KontrastFaktoru * (B - 128)) + 128);
                        //Renkler sınırların dışına çıktıysa, sınır değer alınacak.
                        if (R > 255) R = 255;
                        if (G > 255) G = 255;
                        if (B > 255) B = 255;
                        if (R < 0) R = 0;
                        if (G < 0) G = 0;
                        if (B < 0) B = 0;
                        DonusenRenk = Color.FromArgb(R, G, B);
                        CikisResmi.SetPixel(x, y, DonusenRenk);
                    }
                }
                pictureEdit2.Image = CikisResmi;
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnPrlKarsıtlık_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                String C_KontrastSeviyesi = Interaction.InputBox("Kontrast Seviyesi");
                String parlaklikDegeri = Interaction.InputBox("Parlaklık Değerini Girin");

                int R = 0, G = 0, B = 0;
                Color OkunanRenk, DonusenRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureEdit1.Image);
                int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı.
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi); //Cikis resmini oluşturuyor. Boyutları giriş resmi ile aynı olur.

                double F_KontrastFaktoru = (259 * (Convert.ToInt16(C_KontrastSeviyesi) + 255)) / (255 * (259 - Convert.ToInt16(C_KontrastSeviyesi)));
                for (int x = 0; x < ResimGenisligi; x++)
                {
                    for (int y = 0; y < ResimYuksekligi; y++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x, y);
                        R = OkunanRenk.R;
                        G = OkunanRenk.G;
                        B = OkunanRenk.B;        // a*T[f(x,y)] + b   b den çöceki kısım kontrast +b sabiti eklenince resmin parlaklığı ayarlanmış olur
                        R = (int)((F_KontrastFaktoru * (R - 128)) + 128) + Convert.ToInt16(parlaklikDegeri);
                        G = (int)((F_KontrastFaktoru * (G - 128)) + 128) + Convert.ToInt16(parlaklikDegeri);
                        B = (int)((F_KontrastFaktoru * (B - 128)) + 128) + Convert.ToInt16(parlaklikDegeri);
                        //Renkler sınırların dışına çıktıysa, sınır değer alınacak.
                        if (R > 255) R = 255;
                        if (G > 255) G = 255;
                        if (B > 255) B = 255;
                        if (R < 0) R = 0;
                        if (G < 0) G = 0;
                        if (B < 0) B = 0;
                        DonusenRenk = Color.FromArgb(R, G, B);
                        CikisResmi.SetPixel(x, y, DonusenRenk);
                    }
                }
                pictureEdit2.Image = CikisResmi;
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barButtonItem19_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnKontrastGerme_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                String C_KontrastSeviyesi = Interaction.InputBox("Kontrast Seviyesi");

                int R = 0, G = 0, B = 0;
                Color OkunanRenk, DonusenRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureEdit1.Image);
                int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı.
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi); //Cikis resmini oluşturuyor. Boyutları giriş resmi ile aynı olur.

                double F_KontrastFaktoru = (259 * (Convert.ToInt16(C_KontrastSeviyesi) + 255)) / (255 * (259 - Convert.ToInt16(C_KontrastSeviyesi)));
                for (int x = 0; x < ResimGenisligi; x++)
                {
                    for (int y = 0; y < ResimYuksekligi; y++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x, y);
                        R = OkunanRenk.R;
                        G = OkunanRenk.G;
                        B = OkunanRenk.B;
                        R = (int)((F_KontrastFaktoru * (R - Convert.ToInt16(C_KontrastSeviyesi))) + Convert.ToInt16(C_KontrastSeviyesi));
                        G = (int)((F_KontrastFaktoru * (G - Convert.ToInt16(C_KontrastSeviyesi))) + Convert.ToInt16(C_KontrastSeviyesi));
                        B = (int)((F_KontrastFaktoru * (B - Convert.ToInt16(C_KontrastSeviyesi))) + Convert.ToInt16(C_KontrastSeviyesi));
                        //Renkler sınırların dışına çıktıysa, sınır değer alınacak.
                        if (R > 255) R = 255;
                        if (G > 255) G = 255;
                        if (B > 255) B = 255;
                        if (R < 0) R = 0;
                        if (G < 0) G = 0;
                        if (B < 0) B = 0;
                        DonusenRenk = Color.FromArgb(R, G, B);
                        CikisResmi.SetPixel(x, y, DonusenRenk);
                    }
                }
                pictureEdit2.Image = CikisResmi;
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnHistogramEsitle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                ArrayList DiziPiksel = new ArrayList();
                int OrtalamaRenk = 0;
                Color OkunanRenk;
                int Rs = 0, Gs = 0, Bs = 0;
                Bitmap GirisResmi; //Histogram için giriş resmi gri-ton olmalıdır.
                GirisResmi = new Bitmap(pictureEdit1.Image);
                int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı.
                int ResimYuksekligi = GirisResmi.Height;

                int i = 0; //piksel sayısı tutulacak.
                for (int xs = 0; xs < GirisResmi.Width; xs++)
                {
                    for (int ys = 0; ys < GirisResmi.Height; ys++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(xs, ys);
                        OrtalamaRenk = (int)(OkunanRenk.R + OkunanRenk.G + OkunanRenk.B) / 3; //Griton resimde üç kanal rengi aynı değere sahiptir.
                        DiziPiksel.Add(OrtalamaRenk); //Resimdeki tüm noktaları diziye atıyor.
                    }
                }

                int[] DiziPikselSayilari = new int[256];
                for (int r = 0; r <= 255; r++) //256 tane renk tonu için dönecek.
                {
                    int PikselSayisi = 0;
                    for (int s = 0; s < DiziPiksel.Count; s++) //resimdeki piksel sayısınca dönecek.
                    {
                        if (r == Convert.ToInt16(DiziPiksel[s]))
                            PikselSayisi++;
                    }
                    DiziPikselSayilari[r] = PikselSayisi;
                }

                //Değerleri listbox'a ekliyor.
                int RenkMaksPikselSayisi = 0; //Grafikte y eksenini ölçeklerken kullanılacak.
                for (int k = 0; k <= 255; k++)
                {

                    //Maksimum piksel sayısını bulmaya çalışıyor.
                    if (DiziPikselSayilari[k] > RenkMaksPikselSayisi)
                    {
                        RenkMaksPikselSayisi = DiziPikselSayilari[k];
                    }
                }

                Graphics CizimAlani;
                Pen Kalem1 = new Pen(System.Drawing.Color.Yellow, 1);
                Pen Kalem2 = new Pen(System.Drawing.Color.Red, 1);
                CizimAlani = pictureEdit2.CreateGraphics();
                pictureEdit2.Refresh();
                int GrafikYuksekligi = 300;
                double OlcekY = RenkMaksPikselSayisi / GrafikYuksekligi;
                double OlcekX = 1.5;
                int X_kaydirma = 10;
                for (int x = 0; x <= 255; x++)
                {
                    if (x % 50 == 0)
                        CizimAlani.DrawLine(Kalem2, (int)(X_kaydirma + x * OlcekX),
                       GrafikYuksekligi, (int)(X_kaydirma + x * OlcekX), 0);
                    CizimAlani.DrawLine(Kalem1, (int)(X_kaydirma + x * OlcekX), GrafikYuksekligi,
                   (int)(X_kaydirma + x * OlcekX), (GrafikYuksekligi - (int)(DiziPikselSayilari[x] / OlcekY)));
                    //Dikey kırmızı çizgiler.

                }

                histEsitlemeGrafik frm = new histEsitlemeGrafik();
                frm.gResimi = new Bitmap(pictureEdit1.Image);
                frm.GrafikÇiz();
                frm.ShowDialog();

                //pictureEdit2.Image = renderedImage;
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barButtonItem22_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                String filtreBoyut = Interaction.InputBox("Filtre Boyutu Kaç olsun?");

                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureEdit1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);

                int SablonBoyutu = Convert.ToInt32(filtreBoyut); 

                int x, y, i, j, toplamR, toplamG, toplamB, ortalamaR, ortalamaG, ortalamaB;
                for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++)
                {
                    for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        toplamR = 0;
                        toplamG = 0;
                        toplamB = 0;
                        for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                        {
                            for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                            {
                                OkunanRenk = GirisResmi.GetPixel(x + i, y + j);
                                toplamR = toplamR + OkunanRenk.R;
                                toplamG = toplamG + OkunanRenk.G;
                                toplamB = toplamB + OkunanRenk.B;
                            }
                        }
                        ortalamaR = toplamR / (SablonBoyutu * SablonBoyutu);
                        ortalamaG = toplamG / (SablonBoyutu * SablonBoyutu);
                        ortalamaB = toplamB / (SablonBoyutu * SablonBoyutu);
                        CikisResmi.SetPixel(x, y, Color.FromArgb(ortalamaR, ortalamaG, ortalamaB));
                    }
                }
                pictureEdit2.Image = CikisResmi;

            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void barButtonItem24_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                String filtreBoyut = Interaction.InputBox("Filtre Boyutu Kaç olsun? (3 den büyük tek rakam olmalıdır 3,5,7 gibi)");

                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureEdit1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);

                int SablonBoyutu = Convert.ToInt32(filtreBoyut); //şablon boyutu 3 den büyük tek rakam olmalıdır(3, 5, 7 gibi).

                int ElemanSayisi = SablonBoyutu * SablonBoyutu;
                int[] R = new int[ElemanSayisi];
                int[] G = new int[ElemanSayisi];
                int[] B = new int[ElemanSayisi];
                int[] Gri = new int[ElemanSayisi];
                int x, y, i, j;
                for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++)
                {
                    for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        //Şablon bölgesi (çekirdek matris) içindeki pikselleri tarıyor.
                        int k = 0;
                        for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                        {
                            for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                            {
                                OkunanRenk = GirisResmi.GetPixel(x + i, y + j);
                                R[k] = OkunanRenk.R;
                                G[k] = OkunanRenk.G;
                                B[k] = OkunanRenk.B;
                                Gri[k] = Convert.ToInt16(R[k] * 0.299 + G[k] * 0.587 + B[k] * 0.114); //Gri ton formülü
                                k++;
                            }
                        }
                        //Gri tona göre sıralama yapıyor. Aynı anda üç rengide değiştiriyor.
                        int GeciciSayi = 0;
                        for (i = 0; i < ElemanSayisi; i++)
                        {
                            for (j = i + 1; j < ElemanSayisi; j++)
                            {
                                if (Gri[j] < Gri[i])
                                {
                                    GeciciSayi = Gri[i];
                                    Gri[i] = Gri[j];
                                    Gri[j] = GeciciSayi;
                                    GeciciSayi = R[i];
                                    R[i] = R[j];
                                    R[j] = GeciciSayi;
                                    GeciciSayi = G[i];
                                    G[i] = G[j];
                                    G[j] = GeciciSayi;
                                    GeciciSayi = B[i];
                                    B[i] = B[j];
                                    B[j] = GeciciSayi;
                                }
                            }
                        }
                        //Sıralama sonrası ortadaki değeri çıkış resminin piksel değeri olarak atıyor.
                        CikisResmi.SetPixel(x, y, Color.FromArgb(R[(ElemanSayisi - 1) / 2], G[(ElemanSayisi - 1) /
                       2], B[(ElemanSayisi - 1) / 2]));
                    }
                }
                pictureEdit2.Image = CikisResmi;

            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barButtonItem26_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureEdit1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int SablonBoyutu = 5; //Çekirdek matrisin boyutu
                int ElemanSayisi = SablonBoyutu * SablonBoyutu;
                int x, y, i, j, toplamR, toplamG, toplamB, ortalamaR, ortalamaG, ortalamaB;
                int[] Matris = { 1, 4, 7, 4, 1, 4, 20, 33, 20, 4, 7, 33, 55, 33, 7, 4, 20, 33, 20, 4, 1, 4, 7, 4, 1 };
                int MatrisToplami = 1 + 4 + 7 + 4 + 1 + 4 + 20 + 33 + 20 + 4 + 7 + 33 + 55 + 33 + 7 + 4 + 20 + 33 + 20 + 4 + 1 + 4 + 7 + 4 + 1;

                //int[] Matris = { 1, 2, 4, 2, 1, 2, 6, 9, 6, 2, 4, 9, 16, 9, 4, 2, 6, 9, 6, 2, 1,2, 4, 2, 1 };
                //int MatrisToplami = 1 + 2 + 4 + 2 + 1 + 2 + 6 + 9 + 6 + 2 + 4 + 9 + 16 + 9 + 4 + 2 +6 + 9 + 6 + 2 + 1 + 2 + 4 + 2 + 1;
                for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++) //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek.
                {
                    for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        toplamR = 0;
                        toplamG = 0;
                        toplamB = 0;

                        //Şablon bölgesi (çekirdek matris) içindeki pikselleri tarıyor.
                        int k = 0; //matris içindeki elemanları sırayla okurken kullanılacak.
                        for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                        {
                            for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                            {
                                OkunanRenk = GirisResmi.GetPixel(x + i, y + j);
                                toplamR = toplamR + OkunanRenk.R * Matris[k];
                                toplamG = toplamG + OkunanRenk.G * Matris[k];
                                toplamB = toplamB + OkunanRenk.B * Matris[k];
                                k++;
                            }
                            ortalamaR = toplamR / MatrisToplami;
                            ortalamaG = toplamG / MatrisToplami;
                            ortalamaB = toplamB / MatrisToplami;
                            CikisResmi.SetPixel(x, y, Color.FromArgb(ortalamaR, ortalamaG, ortalamaB));
                        }
                    }
                }
                pictureEdit2.Image = CikisResmi;
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barButtonItem28_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                String aci = Interaction.InputBox("Kaç Derece Dönsün");

                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureEdit1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);

                int Aci = Convert.ToInt16(aci);

                double RadyanAci = Aci * 2 * Math.PI / 360;
                double x2 = 0, y2 = 0;
                //Resim merkezini buluyor. Resim merkezi etrafında döndürecek.
                int x0 = ResimGenisligi / 2;
                int y0 = ResimYuksekligi / 2;
                for (int x1 = 0; x1 < (ResimGenisligi); x1++)
                {
                    for (int y1 = 0; y1 < (ResimYuksekligi); y1++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x1, y1);

                        // Merkeze Göre döndürür bu kodlar
                        //x2 = Math.Cos(RadyanAci) * (x1 - x0) - Math.Sin(RadyanAci) * (y1 - y0) + x0;
                        //y2 = Math.Sin(RadyanAci) * (x1 - x0) + Math.Cos(RadyanAci) * (y1 - y0) + y0;


                        //Aliaslı Döndürme -Sağa Kaydırma
                        x2 = (x1 - x0) - Math.Tan(RadyanAci / 2) * (y1 - y0) + x0;
                        y2 = (y1 - y0) + y0;
                        x2 = Convert.ToInt64(x2);
                        y2 = Convert.ToInt32(y2);
                        //Aliaslı Döndürme -Aşağı kaydırma
                        x2 = (x2 - x0) + x0;
                        y2 = Math.Sin(RadyanAci) * (x2 - x0) + (y2 - y0) + y0;
                        x2 = Convert.ToInt64(x2);
                        y2 = Convert.ToInt64(y2);
                        //Aliaslı Döndürme -Sağa Kaydırma
                        x2 = (x2 - x0) - Math.Tan(RadyanAci / 2) * (y2 - y0) + x0;
                        y2 = (y2 - y0) + y0;
                        x2 = Convert.ToInt64(x2);
                        y2 = Convert.ToInt64(y2);
                        if (x2 > 0 && x2 < ResimGenisligi && y2 > 0 && y2 < ResimYuksekligi)
                            CikisResmi.SetPixel((int)x2, (int)y2, OkunanRenk);
                    }
                }
                pictureEdit2.Image = CikisResmi;
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barButtonItem31_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                Bitmap GirisResmi, CikisResmiXY, CikisResmiX, CikisResmiY;
                GirisResmi = new Bitmap(pictureEdit1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmiX = new Bitmap(ResimGenisligi, ResimYuksekligi);
                CikisResmiY = new Bitmap(ResimGenisligi, ResimYuksekligi);
                CikisResmiXY = new Bitmap(ResimGenisligi, ResimYuksekligi);

                int SablonBoyutu = 3;
                int ElemanSayisi = SablonBoyutu * SablonBoyutu;
                int x, y;
                Color Renk;
                int P1, P2, P3, P4, P5, P6, P7, P8, P9;
                for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++) //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek.
                {
                    for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        Renk = GirisResmi.GetPixel(x - 1, y - 1);
                        P1 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x, y - 1);
                        P2 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x + 1, y - 1);
                        P3 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x - 1, y);
                        P4 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x, y);
                        P5 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x + 1, y);
                        P6 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x - 1, y + 1);
                        P7 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x, y + 1);
                        P8 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x + 1, y + 1);
                        P9 = (Renk.R + Renk.G + Renk.B) / 3;

                        //Hesaplamayı yapan Sobel Temsili matrisi ve formülü.
                        int Gx = Math.Abs(-P1 + P3 - 2 * P4 + 2 * P6 - P7 + P9); //Dikey çizgiler
                        int Gy = Math.Abs(P1 + 2 * P2 + P3 - P7 - 2 * P8 - P9); //Yatay Çizgiler

                        int Gxy = Gx + Gy;
                        //Renkler sınırların dışına çıktıysa, sınır değer alınacak. Negatif olamaz, formüllerde mutlak değer vardır.
                        if (Gx > 255) Gx = 255;
                        if (Gy > 255) Gy = 255;
                        if (Gxy > 255) Gxy = 255;

                        CikisResmiX.SetPixel(x, y, Color.FromArgb(Gx, Gx, Gx));
                        CikisResmiY.SetPixel(x, y, Color.FromArgb(Gy, Gy, Gy));
                        CikisResmiXY.SetPixel(x, y, Color.FromArgb(Gxy, Gxy, Gxy));
                    }
                }
                pictureEdit2.Image = CikisResmiXY; //X VE Y NİN BİRLEŞMİŞ HALİ
                // pictureEdit2.Image = CikisResmiX;
                // pictureEdit2.Image = CikisResmiY;
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barButtonItem30_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                Bitmap img = new Bitmap(pictureEdit1.Image);
                Bitmap image = new Bitmap(img.Width, img.Height);

                int width = img.Width - 1, height = img.Height - 1;
                for (int x = 1; x < width - 1; x++)
                {
                    for (int y = 1; y < height - 1; y++)
                    {
                        Color color2, color4, color5, color6, color8;
                        color2 = img.GetPixel(x, y - 1);
                        color4 = img.GetPixel(x - 1, y);
                        color5 = img.GetPixel(x, y);
                        color6 = img.GetPixel(x + 1, y);
                        color8 = img.GetPixel(x, y + 1);
                        int r = (color2.R + color4.R + color5.R * (-4)) + color6.R + color8.R;
                        int g = (color2.G + color4.G + color5.G * (-4)) + color6.G + color8.G;
                        int b = (color2.B + color4.B + color5.B * (-4)) + color6.B + color8.B;

                        int avg = ((r + g + b) / 3);
                        if (avg > 255) avg = 255;
                        if (avg < 0) avg = 0;
                        image.SetPixel(x, y, Color.FromArgb(avg, avg, avg));
                    }
                }
                pictureEdit2.Image = image;
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barButtonItem34_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureEdit1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);

                int SablonBoyutu = 3;
                int ElemanSayisi = SablonBoyutu * SablonBoyutu;
                int x, y;
                Color Renk;
                int P1, P2, P3, P4, P5, P6, P7, P8, P9;
                for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++) //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek.
                {
                    for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        Renk = GirisResmi.GetPixel(x - 1, y - 1);
                        P1 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x, y - 1);
                        P2 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x + 1, y - 1);
                        P3 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x - 1, y);
                        P4 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x, y);
                        P5 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x + 1, y);
                        P6 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x - 1, y + 1);
                        P7 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x, y + 1);
                        P8 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x + 1, y + 1);
                        P9 = (Renk.R + Renk.G + Renk.B) / 3;

                        int Gx = Math.Abs(-P1 + P3 - P4 + P6 - P7 + P9); //Dikey çizgileri Bulur
                        int Gy = Math.Abs(P1 + P2 + P3 - P7 - P8 - P9); //Yatay Çizgileri Bulur.
                        int PrewittDegeri = 0;
                        PrewittDegeri = Gx;
                        PrewittDegeri = Gy;
                        PrewittDegeri = Gx + Gy; //1. Formül
                                                 //PrewittDegeri = Convert.ToInt16(Math.Sqrt(Gx * Gx + Gy * Gy)); //2.Formül
                                                 //Renkler sınırların dışına çıktıysa, sınır değer alınacak.
                        if (PrewittDegeri > 255) PrewittDegeri = 255;
                        //Eşikleme: Örnek olarak 100 değeri kullanıldı.
                        //if (PrewittDegeri > 100)
                        //PrewittDegeri = 255;
                        //else
                        //PrewittDegeri = 0;
                        CikisResmi.SetPixel(x, y, Color.FromArgb(PrewittDegeri, PrewittDegeri, PrewittDegeri));
                    }
                }
                pictureEdit2.Image = CikisResmi;
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barButtonItem36_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureEdit1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);

                int Aci = Convert.ToInt16(180);

                double RadyanAci = Aci * 2 * Math.PI / 360;
                double x2 = 0, y2 = 0;
                //Resim merkezini buluyor. Resim merkezi etrafında döndürecek.
                int x0 = ResimGenisligi / 2;
                int y0 = ResimYuksekligi / 2;
                for (int x1 = 0; x1 < (ResimGenisligi); x1++)
                {
                    for (int y1 = 0; y1 < (ResimYuksekligi); y1++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x1, y1);

                        // Merkeze Göre döndürür bu kodlar
                        x2 = Math.Cos(RadyanAci) * (x1 - x0) - Math.Sin(RadyanAci) * (y1 - y0) + x0;
                        y2 = Math.Sin(RadyanAci) * (x1 - x0) + Math.Cos(RadyanAci) * (y1 - y0) + y0;



                        if (x2 > 0 && x2 < ResimGenisligi && y2 > 0 && y2 < ResimYuksekligi)
                            CikisResmi.SetPixel((int)x2, (int)y2, OkunanRenk);
                    }
                }
                pictureEdit2.Image = CikisResmi;
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barButtonItem40_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureEdit1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                double x2 = 0, y2 = 0;
                //Taşıma mesafelerini atıyor.
                int Tx = 100;
                int Ty = 50;
                for (int x1 = 0; x1 < (ResimGenisligi); x1++)
                {
                    for (int y1 = 0; y1 < (ResimYuksekligi); y1++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x1, y1);
                        x2 = x1 + Tx;
                        y2 = y1 + Ty;
                        if (x2 > 0 && x2 < ResimGenisligi && y2 > 0 && y2 < ResimYuksekligi)
                            CikisResmi.SetPixel((int)x2, (int)y2, OkunanRenk);
                    }
                }
                pictureEdit2.Image = CikisResmi;
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barButtonItem42_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                Color OkunanRenk, DonusenRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureEdit1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int x2 = 0, y2 = 0; //Çıkış resminin x ve y si olacak.

                int KucultmeKatsayisi = 2;
                for (int x1 = 0; x1 < ResimGenisligi; x1 = x1 + KucultmeKatsayisi)
                {
                    y2 = 0;
                    for (int y1 = 0; y1 < ResimYuksekligi; y1 = y1 + KucultmeKatsayisi)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x1, y1);
                        DonusenRenk = OkunanRenk;
                        CikisResmi.SetPixel(x2, y2, DonusenRenk);
                        y2++;
                    }
                    x2++;
                }
                pictureEdit2.Image = CikisResmi;
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void barButtonItem44_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                int R = 0, G = 0, B = 0;
                Bitmap input_img = new Bitmap(pictureEdit1.Image);
                int width = input_img.Width;
                int height = input_img.Height;
                Bitmap output_img = new Bitmap(width * 2, height * 2);

                int i = 0, j = 0;
                int a1, b1, c1, d1, a2, b2, c2, d2, a3, b3, c3, d3;

                for (int x = 0; x < width * 2; x++)
                {
                    for (int y = 0; y < height * 2; y++)
                    {
                        try
                        {
                            if ((x + 2) <= width && (y + 2) <= height)
                            {
                                a1 = input_img.GetPixel(x, y).R / 4;
                                b1 = input_img.GetPixel(x, y + 1).R / 4;
                                c1 = input_img.GetPixel(x + 1, y).R / 4;
                                d1 = input_img.GetPixel(x + 1, y + 1).R / 4;

                                R = Convert.ToInt32(a1 + b1 + c1 + d1);

                                a2 = input_img.GetPixel(x, y).G / 4;
                                b2 = input_img.GetPixel(x, y + 1).G / 4;
                                c2 = input_img.GetPixel(x + 1, y).G / 4;
                                d2 = input_img.GetPixel(x + 1, y + 1).G / 4;

                                G = Convert.ToInt32(a2 + b2 + c2 + d2);

                                a3 = input_img.GetPixel(x, y).B / 4;
                                b3 = input_img.GetPixel(x, y + 1).B / 4;
                                c3 = input_img.GetPixel(x + 1, y).B / 4;
                                d3 = input_img.GetPixel(x + 1, y + 1).B / 4;

                                B = Convert.ToInt32(a3 + b3 + c3 + d3);

                                output_img.SetPixel(i, j, Color.FromArgb(R, G, B));
                                output_img.SetPixel(i, j + 1, Color.FromArgb(R, G, B));
                                output_img.SetPixel(i + 1, j, Color.FromArgb(R, G, B));
                                output_img.SetPixel(i + 1, j + 1, Color.FromArgb(R, G, B));
                            }
                        }
                        catch { }
                        j += 2;
                    }
                    j = 0;
                    i += 2;
                }

                pictureEdit2.Image = output_img;
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        
        private void barButtonItem45_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureEdit1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int SablonBoyutu = 3;
                int ElemanSayisi = SablonBoyutu * SablonBoyutu;
                int x, y, i, j, toplamR, toplamG, toplamB;
                int R, G, B;
                int[] Matris = { 0, -2, 0, -2, 11, -2, 0, -2, 0 };
                int MatrisToplami = 0 + -2 + 0 + -2 + 11 + -2 + 0 + -2 + 0;

                //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek
                for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++)
                {
                    for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        toplamR = 0;
                        toplamG = 0;
                        toplamB = 0;
                        //Şablon bölgesi (çekirdek matris) içindeki pikselleri tarıyor.
                        int k = 0;//matris içindeki elemanları sırayla okurken kullanılacak. ,
                        for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                        {
                            for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                            {
                                OkunanRenk = GirisResmi.GetPixel(x + i, y + j);
                                toplamR = toplamR + OkunanRenk.R * Matris[k];
                                toplamG = toplamG + OkunanRenk.G * Matris[k];
                                toplamB = toplamB + OkunanRenk.B * Matris[k];
                                k++;
                            }
                        }
                        R = toplamR / MatrisToplami;
                        G = toplamG / MatrisToplami;
                        B = toplamB / MatrisToplami;
                        //=========================================
                        //Renkler sınırların dışına çıktıysa, sınır değer alınacak.
                        if (R > 255) R = 255;
                        if (G > 255) G = 255;
                        if (B > 255) B = 255;
                        if (R < 0) R = 0;
                        if (G < 0) G = 0;
                        if (B < 0) B = 0;
                        //=========================================
                        CikisResmi.SetPixel(x, y, Color.FromArgb(R, G, B));
                    }
                }

                pictureEdit2.Image = CikisResmi;
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barButtonItem47_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                pictureEdit2.Visible = false;
                panelControl1.Visible = true;
                //double x1 = Convert.ToDouble(148);
                //double y1 = Convert.ToDouble(156);
                //double x2 = Convert.ToDouble(224);
                //double y2 = Convert.ToDouble(155);
                //double x3 = Convert.ToDouble(151);
                //double y3 = Convert.ToDouble(207);
                //double x4 = Convert.ToDouble(223);
                //double y4 = Convert.ToDouble(206);
                //double X1 = Convert.ToDouble(0);
                //double Y1 = Convert.ToDouble(0);
                //double X2 = Convert.ToDouble(379);
                //double Y2 = Convert.ToDouble(0);
                //double X3 = Convert.ToDouble(0);
                //double Y3 = Convert.ToDouble(380);
                //double X4 = Convert.ToDouble(379);
                //double Y4 = Convert.ToDouble(380);
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                double x1 = Convert.ToDouble(textBox1.Text);
                double y1 = Convert.ToDouble(textBox2.Text);
                double x2 = Convert.ToDouble(textBox3.Text);
                double y2 = Convert.ToDouble(textBox4.Text);
                double x3 = Convert.ToDouble(textBox5.Text);
                double y3 = Convert.ToDouble(textBox6.Text);
                double x4 = Convert.ToDouble(textBox7.Text);
                double y4 = Convert.ToDouble(textBox8.Text);
                double X1 = Convert.ToDouble(txtX1.Text);
                double Y1 = Convert.ToDouble(txtY1.Text);
                double X2 = Convert.ToDouble(txtX2.Text);
                double Y2 = Convert.ToDouble(txtY2.Text);
                double X3 = Convert.ToDouble(txtX3.Text);
                double Y3 = Convert.ToDouble(txtY3.Text);
                double X4 = Convert.ToDouble(txtX4.Text);
                double Y4 = Convert.ToDouble(txtY4.Text);


                double[,] GirisMatrisi = new double[8, 8];
                // { x1, y1, 1, 0, 0, 0, -x1 * X1, -y1 * X1 }
                GirisMatrisi[0, 0] = x1;
                GirisMatrisi[0, 1] = y1;
                GirisMatrisi[0, 2] = 1;
                GirisMatrisi[0, 3] = 0;
                GirisMatrisi[0, 4] = 0;
                GirisMatrisi[0, 5] = 0;
                GirisMatrisi[0, 6] = -x1 * X1;
                GirisMatrisi[0, 7] = -y1 * X1;
                //{ 0, 0, 0, x1, y1, 1, -x1 * Y1, -y1 * Y1 }
                GirisMatrisi[1, 0] = 0;
                GirisMatrisi[1, 1] = 0;
                GirisMatrisi[1, 2] = 0;
                GirisMatrisi[1, 3] = x1;
                GirisMatrisi[1, 4] = y1;
                GirisMatrisi[1, 5] = 1;
                GirisMatrisi[1, 6] = -x1 * Y1;
                GirisMatrisi[1, 7] = -y1 * Y1;
                //{ x2, y2, 1, 0, 0, 0, -x2 * X2, -y2 * X2 }
                GirisMatrisi[2, 0] = x2;
                GirisMatrisi[2, 1] = y2;
                GirisMatrisi[2, 2] = 1;
                GirisMatrisi[2, 3] = 0;
                GirisMatrisi[2, 4] = 0;
                GirisMatrisi[2, 5] = 0;
                GirisMatrisi[2, 6] = -x2 * X2;
                GirisMatrisi[2, 7] = -y2 * X2;
                //{ 0, 0, 0, x2, y2, 1, -x2 * Y2, -y2 * Y2 }
                GirisMatrisi[3, 0] = 0;
                GirisMatrisi[3, 1] = 0;
                GirisMatrisi[3, 2] = 0;
                GirisMatrisi[3, 3] = x2;
                GirisMatrisi[3, 4] = y2;
                GirisMatrisi[3, 5] = 1;
                GirisMatrisi[3, 6] = -x2 * Y2;
                GirisMatrisi[3, 7] = -y2 * Y2;
                //{ x3, y3, 1, 0, 0, 0, -x3 * X3, -y3 * X3 }
                GirisMatrisi[4, 0] = x3;
                GirisMatrisi[4, 1] = y3;
                GirisMatrisi[4, 2] = 1;
                GirisMatrisi[4, 3] = 0;
                GirisMatrisi[4, 4] = 0;
                GirisMatrisi[4, 5] = 0;
                GirisMatrisi[4, 6] = -x3 * X3;
                GirisMatrisi[4, 7] = -y3 * X3;
                //{ 0, 0, 0, x3, y3, 1, -x3 * Y3, -y3 * Y3 }
                GirisMatrisi[5, 0] = 0;
                GirisMatrisi[5, 1] = 0;
                GirisMatrisi[5, 2] = 0;

                GirisMatrisi[5, 3] = x3;
                GirisMatrisi[5, 4] = y3;
                GirisMatrisi[5, 5] = 1;
                GirisMatrisi[5, 6] = -x3 * Y3;
                GirisMatrisi[5, 7] = -y3 * Y3;
                //{ x4, y4, 1, 0, 0, 0, -x4 * X4, -y4 * X4 }
                GirisMatrisi[6, 0] = x4;
                GirisMatrisi[6, 1] = y4;
                GirisMatrisi[6, 2] = 1;
                GirisMatrisi[6, 3] = 0;
                GirisMatrisi[6, 4] = 0;
                GirisMatrisi[6, 5] = 0;
                GirisMatrisi[6, 6] = -x4 * X4;
                GirisMatrisi[6, 7] = -y4 * X4;
                //{ 0, 0, 0, x4, y4, 1, -x4 * Y4, -y4 * Y4 }
                GirisMatrisi[7, 0] = 0;
                GirisMatrisi[7, 1] = 0;
                GirisMatrisi[7, 2] = 0;
                GirisMatrisi[7, 3] = x4;
                GirisMatrisi[7, 4] = y4;
                GirisMatrisi[7, 5] = 1;
                GirisMatrisi[7, 6] = -x4 * Y4;
                GirisMatrisi[7, 7] = -y4 * Y4;
                //---------------------------------------------------------------------------
                double[,] matrisBTersi = MatrisTersiniAl(GirisMatrisi);
                //----------------------------------- A Dönüşüm Matrisi (3x3) -----------------

                double a00 = 0, a01 = 0, a02 = 0, a10 = 0, a11 = 0, a12 = 0, a20 = 0, a21 = 0, a22 = 0;

                a00 = matrisBTersi[0, 0] * X1 + matrisBTersi[0, 1] * Y1 + matrisBTersi[0, 2] *
               X2 + matrisBTersi[0, 3] * Y2 + matrisBTersi[0, 4] * X3 + matrisBTersi[0, 5] * Y3 +
               matrisBTersi[0, 6] * X4 + matrisBTersi[0, 7] * Y4;
                a01 = matrisBTersi[1, 0] * X1 + matrisBTersi[1, 1] * Y1 + matrisBTersi[1, 2] *
               X2 + matrisBTersi[1, 3] * Y2 + matrisBTersi[1, 4] * X3 + matrisBTersi[1, 5] * Y3 +
               matrisBTersi[1, 6] * X4 + matrisBTersi[1, 7] * Y4;
                a02 = matrisBTersi[2, 0] * X1 + matrisBTersi[2, 1] * Y1 + matrisBTersi[2, 2] *
               X2 + matrisBTersi[2, 3] * Y2 + matrisBTersi[2, 4] * X3 + matrisBTersi[2, 5] * Y3 +
               matrisBTersi[2, 6] * X4 + matrisBTersi[2, 7] * Y4;
                a10 = matrisBTersi[3, 0] * X1 + matrisBTersi[3, 1] * Y1 + matrisBTersi[3, 2] *
               X2 + matrisBTersi[3, 3] * Y2 + matrisBTersi[3, 4] * X3 + matrisBTersi[3, 5] * Y3 +
               matrisBTersi[3, 6] * X4 + matrisBTersi[3, 7] * Y4;
                a11 = matrisBTersi[4, 0] * X1 + matrisBTersi[4, 1] * Y1 + matrisBTersi[4, 2] *
               X2 + matrisBTersi[4, 3] * Y2 + matrisBTersi[4, 4] * X3 + matrisBTersi[4, 5] * Y3 +
               matrisBTersi[4, 6] * X4 + matrisBTersi[4, 7] * Y4;
                a12 = matrisBTersi[5, 0] * X1 + matrisBTersi[5, 1] * Y1 + matrisBTersi[5, 2] *
               X2 + matrisBTersi[5, 3] * Y2 + matrisBTersi[5, 4] * X3 + matrisBTersi[5, 5] * Y3 +
               matrisBTersi[5, 6] * X4 + matrisBTersi[5, 7] * Y4;
                a20 = matrisBTersi[6, 0] * X1 + matrisBTersi[6, 1] * Y1 + matrisBTersi[6, 2] *
               X2 + matrisBTersi[6, 3] * Y2 + matrisBTersi[6, 4] * X3 + matrisBTersi[6, 5] * Y3 +
               matrisBTersi[6, 6] * X4 + matrisBTersi[6, 7] * Y4;

                a21 = matrisBTersi[7, 0] * X1 + matrisBTersi[7, 1] * Y1 + matrisBTersi[7, 2] * X2 + matrisBTersi[7, 3] * Y2 + matrisBTersi[7, 4] * X3 + matrisBTersi[7, 5] * Y3 +
                 matrisBTersi[7, 6] * X4 + matrisBTersi[7, 7] * Y4;
                a22 = 1;
                //------------------------- Perspektif düzeltme işlemi ------------------------

                PerspektifDuzelt(a00, a01, a02, a10, a11, a12, a20, a21, a22);

            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        public void PerspektifDuzelt(double a00, double a01, double a02, double a10, double a11, double a12, double a20, double a21, double a22)
        {
            Bitmap GirisResmi, CikisResmi;
            Color OkunanRenk;
            GirisResmi = new Bitmap(pictureEdit1.Image);
            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;

            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            double X, Y, z;
            for (int x = 0; x < (ResimGenisligi); x++)
            {
                for (int y = 0; y < (ResimYuksekligi); y++)
                {
                    OkunanRenk = GirisResmi.GetPixel(x, y);
                    z = a20 * x + a21 * y + 1;
                    X = (a00 * x + a01 * y + a02) / z;
                    Y = (a10 * x + a11 * y + a12) / z;
                    if (X > 0 && X < ResimGenisligi && Y > 0 && Y < ResimYuksekligi)
                        //Picturebox ın dışına çıkan kısımlar oluşturulmayacak.
                        CikisResmi.SetPixel((int)X, (int)Y, OkunanRenk);
                }
            }
            panelControl1.Visible = false;
            pictureEdit2.Visible = true;
            pictureEdit2.Image = CikisResmi;
        }


        public double[,] MatrisTersiniAl(double[,] GirisMatrisi)
        {
            int MatrisBoyutu = Convert.ToInt16(Math.Sqrt(GirisMatrisi.Length));
            //matris boyutu içindeki eleman sayısı olduğu için kare matrisde
            //karekökü matris boyutu olur.
            double[,] CikisMatrisi = new double[MatrisBoyutu, MatrisBoyutu]; //A nın
                                                                             //tersi alındığında bu matris içinde tutulacak
                                                                             //--I Birim matrisin içeriğini dolduruyor
            for (int i = 0; i < MatrisBoyutu; i++)
            {
                for (int j = 0; j < MatrisBoyutu; j++)
                {
                    if (i == j)
                        CikisMatrisi[i, j] = 1;
                    else
                        CikisMatrisi[i, j] = 0;
                }
            }
            //--Matris Tersini alma işlemi---------
            double d, k;
            for (int i = 0; i < MatrisBoyutu; i++)
            {
                d = GirisMatrisi[i, i];
                for (int j = 0; j < MatrisBoyutu; j++)
                {
                    if (d == 0)
                    {
                        d = 0.0001; //0 bölme hata veriyordu.
                    }
                    GirisMatrisi[i, j] = GirisMatrisi[i, j] / d;
                    CikisMatrisi[i, j] = CikisMatrisi[i, j] / d;
                }
                for (int x = 0; x < MatrisBoyutu; x++)
                {
                    if (x != i)
                    {
                        k = GirisMatrisi[x, i];
                        for (int j = 0; j < MatrisBoyutu; j++)
                        {
                            GirisMatrisi[x, j] = GirisMatrisi[x, j] - GirisMatrisi[i, j] * k;
                            CikisMatrisi[x, j] = CikisMatrisi[x, j] - CikisMatrisi[i, j] * k;
                        }
                    }
                }
            }
            return CikisMatrisi;
        }

        private void pictureEdit1_MouseUp(object sender, MouseEventArgs e)
        {
            label1.Text = "X: " + e.X.ToString() + "    Y: " + e.Y.ToString();
        }

        private void barButtonItem49_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null) Dilation(new Bitmap(pictureEdit1.Image));
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Dilation(Bitmap bmp)
        {
            int[,] bmpMatris = new int[bmp.Height, bmp.Width];
            int b1, b2, b3, b4;
            Color c;

            for (int y = 1; y < bmp.Height - 1; y++)
            {
                for (int x = 1; x < bmp.Width - 1; x++)
                {
                    c = bmp.GetPixel(x, y);
                    c = bmp.GetPixel(x - 1, y);
                    b1 = c.R;
                    c = bmp.GetPixel(x + 1, y);
                    b2 = c.R;
                    c = bmp.GetPixel(x, y - 1);
                    b3 = c.R;
                    c = bmp.GetPixel(x, y + 1);
                    b4 = c.R;

                    if (b1 == 255 || b2 == 255 || b3 == 255 || b4 == 255)
                    {
                        bmpMatris[y, x] = 255;
                    }

                    else
                    {
                        bmpMatris[y, x] = 0;
                    }
                }
            }

            for (int y = 1; y < bmp.Height - 1; y++)
            {
                for (int x = 1; x < bmp.Width - 1; x++)
                {
                    if (bmpMatris[y, x] == 255)
                    {
                        bmp.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    }
                    else bmp.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                }
            }
            pictureEdit2.Image = bmp;
        }

        private void barButtonItem50_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null) Erosion(new Bitmap(pictureEdit1.Image));
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Erosion(Bitmap bmp)
        {
            int[,] bmpMatris = new int[bmp.Height, bmp.Width];
            int b1, b2, b3, b4;
            Color c;

            for (int y = 1; y < bmp.Height - 1; y++)
            {
                for (int x = 1; x < bmp.Width - 1; x++)
                {
                    c = bmp.GetPixel(x, y);
                    c = bmp.GetPixel(x - 1, y);
                    b1 = c.R;
                    c = bmp.GetPixel(x + 1, y);
                    b2 = c.R;
                    c = bmp.GetPixel(x, y - 1);
                    b3 = c.R;
                    c = bmp.GetPixel(x, y + 1);
                    b4 = c.R;

                    if (b1 == 255 && b2 == 255 && b3 == 255 && b4 == 255)
                    {
                        bmpMatris[y, x] = 255;
                    }

                    else
                    {
                        bmpMatris[y, x] = 0;
                    }
                }
            }

            for (int y = 1; y < bmp.Height - 1; y++)
            {
                for (int x = 1; x < bmp.Width - 1; x++)
                {
                    if (bmpMatris[y, x] == 255)
                    {
                        bmp.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    }
                    else bmp.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                }
            }
            pictureEdit2.Image = bmp;
        }

        private void barButtonItem57_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                Bitmap bmp = new Bitmap(pictureEdit1.Image);
                int gradient;
                int[,] bmpMatris = new int[bmp.Height, bmp.Width];
                int[,] gradientMatris = { { 0, 1 }, { 1, 0 } };

                for (int y = 1; y < bmp.Height - 2; y++)
                {
                    for (int x = 1; x < bmp.Width - 2; x++)
                    {

                        gradient = gradientMatris[0, 0] * bmp.GetPixel(x, y).R +
                                   gradientMatris[0, 1] * bmp.GetPixel(x + 1, y).R +
                                   gradientMatris[1, 0] * bmp.GetPixel(x, y + 1).R +
                                   gradientMatris[1, 1] * bmp.GetPixel(x + 1, y + 1).R;

                        if (gradient > 255) bmpMatris[y, x] = 255;
                        else if (gradient < 0) bmpMatris[y, x] = 0;
                        else bmpMatris[y, x] = gradient;
                    }
                }

                for (int x = 1; x < (bmp.Width - 2); x++)
                {
                    for (int y = 1; y < (bmp.Height - 2); y++)

                        if (bmpMatris[y, x] == 0)
                        {
                            bmp.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        }
                        else bmp.SetPixel(x, y, Color.FromArgb(bmpMatris[y, x], bmpMatris[y, x], bmpMatris[y, x]));
                }
                pictureEdit2.Image = bmp;
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        public void yontem2(System.Drawing.Image img, int istenenEn, int istenenBoy)
        {
            Size istenenimg = new Size(istenenBoy, istenenEn);
            int genislik = img.Width;
            int yukseklik = img.Height;

            float Oran = 0;
            float genislikOranı = 0;
            float yukseklikOranı = 0;

            genislikOranı = ((float)istenenimg.Width / (float)genislik);
            yukseklikOranı = ((float)istenenimg.Height / (float)yukseklik);

            if (yukseklikOranı < genislikOranı)
                Oran = yukseklikOranı;
            else
                Oran = genislikOranı;

            int yenigenislik = (int)(genislik * Oran);
            int yeniyukseklik = (int)(yukseklik * Oran);

            Bitmap sonimg = new Bitmap(yenigenislik, yeniyukseklik);
            Graphics g = Graphics.FromImage((System.Drawing.Image)sonimg);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, 0, 0, yenigenislik, yeniyukseklik);

            pictureEdit2.Image = sonimg;
        }

        private void barButtonItem53_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                Erosion(new Bitmap(pictureEdit1.Image));
                Dilation(new Bitmap(pictureEdit2.Image));
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barButtonItem54_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                Dilation(new Bitmap(pictureEdit1.Image));
                Erosion(new Bitmap(pictureEdit2.Image));
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barButtonItem61_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            BenzerGörüntü(new Bitmap(pictureEdit1.Image),new Bitmap(pictureEdit2.Image));
        }

        public double BenzerGörüntü(Bitmap bmpImage1, Bitmap bmpImage2)
        {
            int correct = 0;
            for (int i = 0; i < bmpImage1.Width; i++)
            {
                for (int j = 0; j < bmpImage1.Height; j++)
                {
                    Color c1 = bmpImage1.GetPixel(i, j);
                    Color c2 = bmpImage2.GetPixel(i, j);
                    if (c1.ToArgb() == c2.ToArgb())
                        correct++;
                }
            }
            int maxPixels = bmpImage1.Width * bmpImage1.Height;
            double SimilarityPercent = (100.0 * correct) / maxPixels;
            return SimilarityPercent;
        }

        private void barButtonItem62_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
               if (pictureEdit1.Image != null)
                {
                    RenkBul frm = new RenkBul();
                    frm.resimRenkBul = new Bitmap(pictureEdit1.Image);
                    frm.ShowDialog();
                }
                else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAynalama_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pictureEdit1.Image != null)
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureEdit1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);

                double Aci = Convert.ToDouble(90);

                double RadyanAci = Aci * 2 * Math.PI / 360;
                double x2 = 0, y2 = 0;
                //Resim merkezini buluyor. Resim merkezi etrafında döndürecek.
                int x0 = ResimGenisligi / 2;
                int y0 = ResimYuksekligi / 2;
                for (int x1 = 0; x1 < (ResimGenisligi); x1++)
                {
                    for (int y1 = 0; y1 < (ResimYuksekligi); y1++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x1, y1);
                        //----A-Orta dikey eksen etrafında aynalama ----------------
                        //x2 = Convert.ToInt16(-x1 + 2 * x0);
                        //y2 = Convert.ToInt16(y1);
                        //----B-Orta yatay eksen etrafında aynalama ----------------
                        //x2 = Convert.ToInt16(x1);
                        //y2 = Convert.ToInt16(-y1 + 2 *y0);

                        //----C-Ortadan geçen 45 açılı çizgi etrafında aynalama----------
                        double Delta = (x1 - x0) * Math.Sin(RadyanAci) - (y1 - y0) * Math.Cos(RadyanAci);
                        x2 = Convert.ToInt16(x1 + 2 * Delta * (-Math.Sin(RadyanAci)));
                        y2 = Convert.ToInt16(y1 + 2 * Delta * (Math.Cos(RadyanAci)));
                        if (x2 > 0 && x2 < ResimGenisligi && y2 > 0 && y2 < ResimYuksekligi)
                            CikisResmi.SetPixel((int)x2, (int)y2, OkunanRenk);
                    }
                }
                pictureEdit2.Image = CikisResmi;   
            }
            else MessageBox.Show("Resim yükleyin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            panelControl1.Visible = false;
            pictureEdit2.Visible = true;
        }
    }
}


