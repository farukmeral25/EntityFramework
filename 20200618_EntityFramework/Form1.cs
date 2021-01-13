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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        NorthwindDataContext ctx = new NorthwindDataContext();
        private void Form1_Load(object sender, EventArgs e)
        {
            UrunleriListele();
        }

        private void UrunleriListele()
        {
            //Custom Listeleme
            //var sonuc = from urun in ctx.Urunlers
            //            select new
            //            {
            //                urun.UrunID,
            //                urun.UrunAdi,
            //                urun.Fiyat,
            //                urun.Stok
            //            };


            var sonuc = from urun in ctx.Urunlers
                        join kategori in ctx.Kategorilers
                        on urun.KategoriID equals kategori.KategoriID
                        join tedarikci in ctx.Tedarikcilers
                        on urun.TedarikciID equals tedarikci.TedarikciID
                        select new
                        {
                            urun.UrunID,
                            urun.UrunAdi,
                            urun.Fiyat,
                            urun.Stok,
                            kategori.KategoriAdi,
                            tedarikci.SirketAdi
                        };


            dataGridView1.DataSource = sonuc;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            Urunler urun = new Urunler();
            urun.UrunAdi = txtUrunAdi.Text;
            urun.Fiyat = nudFiyat.Value;
            urun.Stok = Convert.ToInt16(nudStok.Value);
            ctx.Urunlers.InsertOnSubmit(urun);
            ctx.SubmitChanges();
            UrunleriListele();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            //seçili satırın id'sini alıyoruz.
            int id = (int)dataGridView1.CurrentRow.Cells["UrunID"].Value;
            //Single olsaydı eğer sonuç bulamazsa hata verirdi, oluşabilecek hataları önlemek için singleorDefafult kullanılır.Eğer sonuç bulamazsa geriye null değer dönderir.
            Urunler urun = ctx.Urunlers.SingleOrDefault(urunId => urunId.UrunID ==  id); 
            ctx.Urunlers.DeleteOnSubmit(urun);
            ctx.SubmitChanges();
            UrunleriListele();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView1.CurrentRow;
            txtUrunAdi.Text = row.Cells["UrunAdi"].Value.ToString();
            nudFiyat.Value = Convert.ToDecimal(row.Cells["Fiyat"].Value);
            nudStok.Value = Convert.ToDecimal(row.Cells["Stok"].Value);
            txtUrunAdi.Tag = row.Cells["UrunID"].Value;
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            int id = (int)txtUrunAdi.Tag;
            Urunler urun =  ctx.Urunlers.SingleOrDefault(p => p.UrunID == id);
            urun.UrunAdi = txtUrunAdi.Text;
            urun.Fiyat = nudFiyat.Value;
            urun.Stok = Convert.ToInt16(nudStok.Value);
            ctx.SubmitChanges();
            UrunleriListele();
        }
    }
}
