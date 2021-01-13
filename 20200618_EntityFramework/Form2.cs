using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _20200618_EntityFramework
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        NorthwindDataContext ctx = new NorthwindDataContext();
        private void Form2_Load(object sender, EventArgs e)
        {
            var sonuc = from urun in ctx.Urunlers
                        join sd in ctx.SatisDetays
                        on urun.UrunID equals sd.UrunID
                        join satis in ctx.Satislars
                        on sd.SatisID equals satis.SatisID
                        join musteri in ctx.Musterilers
                        on satis.MusteriID equals musteri.MusteriID
                        orderby urun.Fiyat descending //Sıralama
                        select new
                        {
                            urun.UrunAdi,
                            sd.Adet,
                            ÜrünFiyat = urun.Fiyat,
                            SatışFiyat = sd.Fiyat,
                            satis.SatisTarihi,
                            musteri.SirketAdi
                        };
            dataGridView1.DataSource = sonuc;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var sonuc = from urun in ctx.Urunlers
                        join sd in ctx.SatisDetays
                        on urun.UrunID equals sd.UrunID
                        join satis in ctx.Satislars
                        on sd.SatisID equals satis.SatisID
                        join musteri in ctx.Musterilers
                        on satis.MusteriID equals musteri.MusteriID
                        join kategori in ctx.Kategorilers
                        on urun.KategoriID equals kategori.KategoriID
                        group new { sd,kategori} by new { urun.UrunAdi,kategori.KategoriAdi } into grup
                            let ÜrünAdı = grup.Key.UrunAdi
                            let KategoriAdı = grup.Key.KategoriAdi
                            let SatışAdet = grup.Count()
                            let ToplamSatış = grup.Sum(total => total.sd.Adet * total.sd.Fiyat)
                            orderby ToplamSatış
                        select new
                        {
                           ÜrünAdı,SatışAdet,ToplamSatış,KategoriAdı
                        };
                      
            dataGridView1.DataSource = sonuc;
        }
    }
}
