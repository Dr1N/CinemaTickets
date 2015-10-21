using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cinema
{
    public partial class frmCinema : DialogWindow
    {
        private string tableName = "Cinema";
        private string imageName = null;
        private int minRow = 5;
        private int maxRow = 20;
        private int minPlace = 5;
        private int maxPlace = 20;
        
        public frmCinema(DbHelper db) : base(db)
        {
            InitializeComponent();

            this.Load += frmCinema_Load;
            this.FormClosing += frmCinema_FormClosing;
            this.Shown += frmCinema_Shown;
        }

        public frmCinema(DbHelper db, DataRow dr, FormMode mode)
            : base(db, dr, mode)
        {
            InitializeComponent();

            this.Load += frmCinema_Load;
            this.FormClosing += frmCinema_FormClosing;
            this.Shown += frmCinema_Shown;
        }

        private void frmCinema_Load(object sender, EventArgs e)
        {
            //Количество рядов

            for (int i = this.minRow; i < this.maxRow; i++)
            {
                this.cbCinemaRows.Items.Add(i);
            }

            //Количество мест в ряде

            for (int i = this.minPlace; i < this.maxPlace; i++)
            {
                this.cbCinemaPlaces.Items.Add(i);
            }

            //Заполняем поля

            switch (this.Mode)
            {
                case FormMode.NEW:
                    this.Text = "Добавление кинотеатра";
                    this.pbCinemaPoster.Image = this.GetImage(null);
                    this.cbCinemaRows.SelectedIndex = 0;
                    this.cbCinemaPlaces.SelectedIndex = 0;
                    break;
                case FormMode.EDIT:
                    this.Text = "Редактирование кинотеатра";
                    this.FillControls();
                    break;
                case FormMode.VIEW:
                    this.Text = "Просмотр кинотеатра";
                    this.FillControls();

                    this.tbCinemaName.ReadOnly = true;
                    this.tbCinemaAddress.ReadOnly = true;
                    this.cbCinemaRows.Enabled = false;
                    this.cbCinemaPlaces.Enabled = false;
                    this.btnLoadImage.Visible = false;
                    this.btnDeleteImage.Visible = false;
                    break;
                default:
                    throw new Exception("Неизвестный режим отображения формы");
            }
        }
       
        private void frmCinema_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Mode == FormMode.VIEW) { return; }

            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (this.IsValidData() == true)
                {
                    DataRow dataRow = (this.Mode == FormMode.NEW ? this.dataBase.Tables[this.tableName].NewRow() : this.currentDataRow);
                    dataRow["name"] = this.tbCinemaName.Text.Trim();
                    dataRow["address"] = this.tbCinemaAddress.Text.Trim();
                    dataRow["rows"] = (int)this.cbCinemaRows.SelectedItem;
                    dataRow["places"] = (int)this.cbCinemaPlaces.SelectedItem;
                    dataRow["image"] = this.imageName;

                    if (this.Mode == FormMode.NEW)
                    {
                        this.dataBase.Tables[this.tableName].Rows.Add(dataRow);
                    }
                    return;
                }
                e.Cancel = true;
            }
        }

        private void frmCinema_Shown(object sender, EventArgs e)
        {
            this.tbCinemaName.Focus();
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Изображения(.jpg)|*jpg|Изображения(.bmp)|*bmp|Изображения(.png)|*png|Изображения(.gif)|*gif";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    //новое имя файла (временная метка)

                    string path = ConfigurationManager.AppSettings["image_path"];
                    string extention = ofd.SafeFileName.Substring(ofd.SafeFileName.LastIndexOf("."));
                    string myFileName = DateTime.Now.Ticks.ToString() + extention;
                    
                    //сохраняем файл в каталог
                    
                    File.Copy(ofd.FileName, Path.Combine(path, myFileName));
                    this.imageName = myFileName;
                    this.pbCinemaPoster.Image = Image.FromFile(Path.Combine(path, myFileName));
                }
                catch (Exception exc)
                {
                    MessageBox.Show("В процессе копирования файла произошли ошибки\n" + exc.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.pbCinemaPoster.Image = null;
                }
            }
        }

        private void btnDeleteImage_Click(object sender, EventArgs e)
        {
            this.imageName = null;
            this.pbCinemaPoster.Image = this.GetImage(null);
        }

        protected override bool IsValidData()
        {
            //Имя

            if (this.tbCinemaName.Text.Trim().Length < 1 || 64 < this.tbCinemaName.Text.Trim().Length)
            {
                this.errorProvider.SetError(this.tbCinemaName, "Некорректное имя кинотеатра");
                return false;
            }
            else
            {
                this.errorProvider.SetError(this.tbCinemaName, "");
            }
            
            //Адрес

            if (this.tbCinemaAddress.Text.Trim().Length < 3 || 64 < this.tbCinemaAddress.Text.Trim().Length)
            {
                this.errorProvider.SetError(this.tbCinemaAddress, "Некорректный адресс кинотеатра");
                return false;
            }
            else
            {
                this.errorProvider.SetError(this.tbCinemaAddress, "");
            }

            //Рядов to-do (проверить билеты) 

            if (this.cbCinemaRows.SelectedIndex == -1)
            {
                this.errorProvider.SetError(this.cbCinemaRows, "Выберете количество рядов в кинотеатре");
                return false;
            }
            else
            {
                this.errorProvider.SetError(this.cbCinemaRows, "");
            }

            //Мест to-do (проверить билеты)

            if (this.cbCinemaPlaces.SelectedIndex == -1)
            {
                this.errorProvider.SetError(this.cbCinemaPlaces, "Выберете количество мест в кинотеатре");
                return false;
            }
            else
            {
                this.errorProvider.SetError(this.cbCinemaPlaces, "");
            }

            return true;
        }

        protected override void FillControls()
        {
            this.tbCinemaName.Text = this.currentDataRow["name"].ToString();
            this.tbCinemaAddress.Text = this.currentDataRow["address"].ToString();
            this.cbCinemaRows.SelectedItem = (int)this.currentDataRow["rows"];
            this.cbCinemaPlaces.SelectedItem = (int)this.currentDataRow["places"];
            this.pbCinemaPoster.Image = this.GetImage(this.currentDataRow["image"].ToString());
            this.imageName = this.currentDataRow["image"].ToString();
        }
    }
}