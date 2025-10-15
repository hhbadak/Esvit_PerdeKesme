using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerdeKesme
{
    public partial class Home : Form
    {
        public static Employee LoginUser;
        DataModel dm = new DataModel();
        public Home()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            LoginPage frm = new LoginPage();

            if (frm.ShowDialog() == DialogResult.OK)
            {
                // Assume Helpers.isLogin is a static property holding the logged-in user's info
                Employee model = Helpers.isLogin;
                Home.LoginUser = model; // LoginUser nesnesini doğrudan ata
            }
            else
            {
                // Eğer giriş yapılmadıysa formu kapat
                this.Close();
                return;
            }

            // ComboBox'a hata listesini yüklüyoruz
            var resultList = dm.getResultList();
            if (resultList != null)
            {
                cb_result.DataSource = resultList;
                cb_result.ValueMember = "ID";
                cb_result.DisplayMember = "Name";

                // ID=1 olan öğeyi otomatik seç (genellikle "Hatasız")
                cb_result.SelectedValue = 1;
            }
            else
            {
                MessageBox.Show("Hata listesi yüklenemedi.");
            }

            loadGrid();
        }

        private void loadGrid()
        {
            var result = dm.logEntryListStoning(new DataAccessLayer.Kalite_PerdeKesme
            {
                Barcode = tb_barcode.Text,
            });

            if (result != null)
            {
                var rt = result.OrderByDescending(r => r.ID).ToList();
                DataTable dt = new DataTable();

                dt.Columns.Add("ID");
                dt.Columns.Add("Barkod No");
                dt.Columns.Add("Kalite");
                dt.Columns.Add("Yapılan İşlem");
                dt.Columns.Add("Kontrol Tarihi");
                dt.Columns.Add("Perde Kesme Personeli");

                foreach (var item in rt)
                {
                    DataRow r = dt.NewRow();
                    r["ID"] = item.ID;
                    r["Barkod No"] = item.Barcode;
                    r["Kalite"] = item.Quality;
                    r["Yapılan İşlem"] = item.Result;
                    r["Kontrol Tarihi"] = item.Datetime.ToShortDateString();
                    r["Perde Kesme Personeli"] = item.QualityPersonal;
                    dt.Rows.Add(r);
                }

                dgv_Stoning.DataSource = dt;
                // Yalnızca veri içeren satırları say
                int nonEmptyRowCount = dgv_Stoning.Rows.Cast<DataGridViewRow>()
                    .Count(row => !row.IsNewRow && row.Cells.Cast<DataGridViewCell>().Any(cell => cell.Value != null && cell.Value.ToString() != ""));

                lbl_number.Text = "Bakılan Ürün sayısı: " + nonEmptyRowCount;

            }
            else
            {
                MessageBox.Show("Veri yüklenirken bir hata oluştu.");
            }
            tb_barcode.Select();
        }


        private void tb_barcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Önce yapılan işlem seçilmiş mi kontrol et
                if (cb_result.SelectedValue == null)
                {
                    MessageBox.Show("Lütfen yapılan işlemi seçiniz.");
                    cb_result.Focus();
                    return;
                }

                // Barkod uzunluğunu kontrol et
                if (tb_barcode.Text.Length != 10)
                {
                    MessageBox.Show("Barkod numarası 10 haneli olmalıdır.");
                    tb_barcode.SelectAll();
                    return;
                }

                // Barkod daha önce girilmiş mi kontrol et (Kalite_PerdeKesme tablosunda)
                if (dm.isBarcodeExists(tb_barcode.Text))
                {
                    MessageBox.Show("Bu barkod numarası daha önce sisteme girilmiş. Lütfen farklı bir barkod okutunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tb_barcode.SelectAll();
                    return;
                }

                // Barkodun Products tablosunda var olup olmadığını kontrol et
                if (!dm.isProductExists(tb_barcode.Text))
                {
                    MessageBox.Show("Kalite girişi yapılmamıştır.\n\nBu barkod Products tablosunda bulunmuyor.\nLütfen önce ürünü sisteme ekleyiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tb_barcode.SelectAll();
                    return;
                }

                DataAccessLayer.Kalite_PerdeKesme taharet = new DataAccessLayer.Kalite_PerdeKesme();

                taharet.Barcode = tb_barcode.Text;

                // ResultID'yi ComboBox'tan al
                int selectedResultID = Convert.ToInt32(cb_result.SelectedValue);
                taharet.ResultID = selectedResultID;

                // QualityID'yi ResultID'ye göre otomatik belirle
                if (cb_fire.Checked)
                {
                    // ISKARTA işaretliyse
                    taharet.QualityID = 5; // ISKARTA
                }
                else
                {
                    // ResultID'ye göre QualityID belirle
                    if (selectedResultID == 1)
                    {
                        // Hatasız durumu
                        taharet.QualityID = 1; // Geçti/OK
                    }
                    else
                    {
                        // Herhangi bir hata varsa
                        taharet.QualityID = 2; // Hatalı/Reddedildi
                    }
                }

                // Eğer ISKARTA veya Hatalı ise, ürün kalitesini güncelle
                if (taharet.QualityID == 5 || taharet.QualityID == 2)
                {
                    if (!dm.updateProductQuality(taharet))
                    {
                        MessageBox.Show("Ürün kalitesi güncellenemedi.\n\nOlası nedenler:\n• Bu barkod Products tablosunda bulunmuyor\n• Veritabanı bağlantı sorunu", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                taharet.Datetime = DateTime.Now;
                
                // Kullanıcı kontrolü
                if (Home.LoginUser == null || Home.LoginUser.ID == 0)
                {
                    MessageBox.Show("Kullanıcı bilgisi bulunamadı. Lütfen tekrar giriş yapın.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                taharet.QualityPersonalID = Home.LoginUser.ID;

                if (dm.createTaharetBoruMontaj(taharet))
                {
                    MessageBox.Show("Kayıt başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tb_barcode.Text = "";
                    cb_fire.Checked = false;
                    loadGrid(); // Grid'i yenile
                }
                else
                {
                    MessageBox.Show("Kayıt eklenirken bir hata oluştu.\n\nOlası nedenler:\n• Bu barkod Products tablosunda bulunmuyor\n• Kullanıcı bilgisi geçersiz\n• Veritabanı bağlantı sorunu", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
