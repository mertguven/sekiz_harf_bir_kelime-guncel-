using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace sekiz_harf_bir_kelime
{
    public partial class Form1 : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );
        public Form1()
        {
            InitializeComponent();
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 15, 15));
        }
        List<string> sekizli = new List<string>();
        List<string> yedili = new List<string>();
        List<string> altili = new List<string>();
        List<string> besli = new List<string>();
        List<string> dortlu = new List<string>();
        List<string> uclu = new List<string>();

        tdk_kelimeEntities ent = new tdk_kelimeEntities();//entity framework nesnesi oluşturuluyor
        Random rastgele = new Random();
        ArrayList eleman = new ArrayList();

        char[] dizi = { 'a', 'b', 'c', 'ç', 'd', 'e', 'f', 'g', 'ğ', 'h', 'ı', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'ö', 'p', 'r', 's', 'ş', 't', 'u', 'ü', 'v', 'y', 'z' };
        string metin = "";
        List<string> harflerq = new List<string>();
        char[] karakterler = new char[8];
        int harfyeri;

        private void rastgeleBtn_Click(object sender, EventArgs e)//Rastgele oluşturulan değerlerin kombinasyonunu alır ve veritabanı ile eşleşenleri listbox3'e atar
        {
            MessageBox.Show("Bu işlem biraz uzun sürebilir!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            harflerTxt.Text = "";
            for (int i = 0; i < 8; i++)
            {
                harfyeri = rastgele.Next(0, dizi.Length);
                karakterler[i] = dizi[harfyeri];
                harflerTxt.Text += karakterler[i].ToString();
                harflerq.Add(karakterler[i].ToString());
            }
            OrtakIslem();
            eslestirBtn.Enabled = false;
            rastgeleBtn.Enabled = false;
            veriCekBtn.Enabled = false;
        }

        private void eslestirBtn_Click(object sender, EventArgs e)//Elle girilen değerlerin kombinasyonunu alır ve veritabanı ile eşleşenleri listbox3'e atar
        {
            
            if (harflerTxt.Text == "")
            {
                MessageBox.Show("Harfler kısmını doldurun!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (harflerTxt.TextLength < 9)
            {
                MessageBox.Show("Minimum 9 harf girmeniz gerekiyor!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Bu işlem biraz uzun sürebilir!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                metin = harflerTxt.Text;
                karakterler = metin.ToCharArray();
                for (int i = 0; i < 8; i++)
                {
                    harflerq.Add(karakterler[i].ToString());
                }
                OrtakIslem();
                rastgeleBtn.Enabled = false;
                eslestirBtn.Enabled = false;
                veriCekBtn.Enabled = false;
            }

        }

        private void veriCekBtn_Click(object sender, EventArgs e)//Entity framework kullanarak veritabanından kelimeleri çeker
        {
            veritabani veriler = new veritabani();
            dataGridView1.DataSource = veriler.vericek();
            eslestirBtn.Enabled = true;
            rastgeleBtn.Enabled = true;
            yeniOyunBtn.Enabled = true;
        }

        private void yeniOyunBtn_Click(object sender, EventArgs e)//Her şeyi sıfırlar
        {
            veriCekBtn.Enabled = true;
            eslestirBtn.Enabled = false;
            rastgeleBtn.Enabled = false;
            yeniOyunBtn.Enabled = false;
            listBox3.Items.Clear();
            sekizli.Clear();
            yedili.Clear();
            altili.Clear();
            besli.Clear();
            dortlu.Clear();
            uclu.Clear();
            dataGridView1.DataSource = null;
            harflerTxt.Text = "";
            harflerq.Clear();
        }

        public void OrtakIslem()//Rastgele ve elle eklenen işlemlerde ortak olanları bir yerde topladım
        {
            List<string> possibleCombination = GetCombination(harflerq, new List<string>(), "");
            foreach (string item in possibleCombination)
            {
                if (item.Count() == 8)
                {
                    sekizli.Add(item);
                }
                if (item.Count() == 7)
                {
                    yedili.Add(item);
                }
                if (item.Count() == 6)
                {
                    altili.Add(item);
                }
                if (item.Count() == 5)
                {
                    besli.Add(item);
                }
                if (item.Count() == 4)
                {
                    dortlu.Add(item);
                }
                if (item.Count() == 3)
                {
                    uclu.Add(item);
                }
            }
            for (int j = 0; j < dataGridView1.RowCount; j++)//Eşleşenlerin listbox3'e yazıldığı yer
            {
                if (sekizli.Contains(dataGridView1.Rows[j].Cells["kelime"].Value.ToString()))
                {
                    listBox3.Items.Add(dataGridView1.Rows[j].Cells["kelime"].Value.ToString() + ": 11 Puan");
                }
                if (yedili.Contains(dataGridView1.Rows[j].Cells["kelime"].Value.ToString()))
                {
                    listBox3.Items.Add(dataGridView1.Rows[j].Cells["kelime"].Value.ToString() + ": 9 Puan");
                }
                if (altili.Contains(dataGridView1.Rows[j].Cells["kelime"].Value.ToString()))
                {
                    listBox3.Items.Add(dataGridView1.Rows[j].Cells["kelime"].Value.ToString() + ": 7 Puan");
                }
                if (besli.Contains(dataGridView1.Rows[j].Cells["kelime"].Value.ToString()))
                {
                    listBox3.Items.Add(dataGridView1.Rows[j].Cells["kelime"].Value.ToString() + ": 5 Puan");
                }
                if (dortlu.Contains(dataGridView1.Rows[j].Cells["kelime"].Value.ToString()))
                {
                    listBox3.Items.Add(dataGridView1.Rows[j].Cells["kelime"].Value.ToString() + ": 4 Puan");
                }
                if (uclu.Contains(dataGridView1.Rows[j].Cells["kelime"].Value.ToString()))
                {
                    listBox3.Items.Add(dataGridView1.Rows[j].Cells["kelime"].Value.ToString() + ": 3 Puan");
                }
            }
            if (listBox3.Items.Count == 0)
            {
                MessageBox.Show("Veritabanıyla eşleşen kelime bulunamadı!", "BULUNAMADI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Kelimeler bulundu!", "BULUNDU", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        static List<string> GetCombination(List<string> list, List<string> combinations, string sumNum, bool addNumberToResult = false)//Kombinasyon işleminin yapıldığı yer
        {
            if (list.Count == 0)
            {
                return combinations;
            }
            string tmp;
            for (int i = 0; i <= list.Count - 1; i++)
            {
                tmp = string.Concat(sumNum, list[i]);
                if (addNumberToResult)
                {
                    combinations.Add(tmp);
                }
                List<string> tmp_list = new List<string>(list);
                tmp_list.RemoveAt(i);
                GetCombination(tmp_list, combinations, tmp, true);
            }
            return combinations;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            eslestirBtn.Enabled = false;
            rastgeleBtn.Enabled = false;
            yeniOyunBtn.Enabled = false;
        }

        private void nasilBtn_Click(object sender, EventArgs e)
        {
            kelimeOyunuPnl.Visible = false;
            nasilCalisirPnl.Visible = true;
            nasilBtn.Normalcolor = Color.FromArgb(0, 196, 204);
            oynaBtn.Normalcolor = Color.Transparent;
        }

        private void oynaBtn_Click(object sender, EventArgs e)
        {
            kelimeOyunuPnl.Visible = true;
            nasilCalisirPnl.Visible = false;
            oynaBtn.Normalcolor = Color.FromArgb(0, 196, 204);
            nasilBtn.Normalcolor = Color.Transparent;
        }
    }
}
