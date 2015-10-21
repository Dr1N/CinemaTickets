using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;

namespace Cinema
{
    public partial class frmMovies : DialogWindow
    {
        private string tableName = "Movies";
        private string imageName = null;
        private int minYear = 1980;
        private int maxDuration = 600;

        public frmMovies(DbHelper db)
            : base(db)
        {
            InitializeComponent();

            this.Load += frmMovies_Load;
            this.FormClosing += frmMovies_FormClosing;
            this.Shown += frmMovies_Shown;
        }

        public frmMovies(DbHelper db, DataRow dr, FormMode mode)
            : base(db, dr, mode)
        {
            InitializeComponent();

            this.Load += frmMovies_Load;
            this.FormClosing += frmMovies_FormClosing;
            this.Shown += frmMovies_Shown;
        }

        private void frmMovies_Load(object sender, EventArgs e)
        {
            //Подготавливаем форму
            //Список жанров

            for (int i = 0; i < this.dataBase.Tables["Genres"].Rows.Count; i++)
            {
                this.cbMovieGenre.Items.Add(this.dataBase.Tables["Genres"].Rows[i]["name"].ToString());
            }

            //Список годов выпуска

            for (int i = this.minYear; i <= DateTime.Now.Year; i++)
            {
                this.cbMovieYear.Items.Add(i);
            }

            //Заполняем элементы управления

            switch (this.Mode)
            {
                case FormMode.NEW:
                    this.Text = "Добавление фильма";
                    this.pbMoviePoster.Image = this.GetImage(null);
                    this.cbMovieGenre.SelectedIndex = 0;
                    this.cbMovieYear.SelectedIndex = 0;
                    break;
                case FormMode.EDIT:
                    this.Text = "Редактирование фильма";
                    this.FillControls();
                    break;
                case FormMode.VIEW:
                    this.Text = "Просмотр фильма";
                    this.FillControls();

                    this.cbMovieGenre.Enabled = false;
                    this.tbMovieName.ReadOnly = true;
                    this.tbMovieDuration.ReadOnly = true;
                    this.cbMovieYear.Enabled = false;
                    this.btnLoadImage.Visible = false;
                    this.btnDeleteImage.Visible = false;
                    break;
                default:
                    throw new Exception("Неизвестный режим отображения формы");
            }
        }
       
        private void frmMovies_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Mode == FormMode.VIEW) { return; }

            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (this.IsValidData() == true)
                {
                    DataRow dataRow = (this.Mode == FormMode.NEW ? this.dataBase.Tables[this.tableName].NewRow() : this.currentDataRow);
                    dataRow["name"] = this.tbMovieName.Text.Trim();
                    dataRow["genre_id"] = this.dataBase.GetIdByName("Genres", this.cbMovieGenre.SelectedItem.ToString());
                    dataRow["duration"] = Int32.Parse(this.tbMovieDuration.Text.ToString());
                    dataRow["year"] = Int32.Parse(this.cbMovieYear.SelectedItem.ToString());
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

        private void frmMovies_Shown(object sender, EventArgs e)
        {
            this.tbMovieName.Focus();
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
                    this.pbMoviePoster.Image = Image.FromFile(Path.Combine(path, myFileName));
                }
                catch (Exception exc)
                {
                    //MessageBox.Show(exc.Message + "\n" + exc.StackTrace, "Error!");
                    MessageBox.Show("В процессе копирования файла произошли ошибки\n" + exc.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.pbMoviePoster.Image = null;
                }
            }
        }

        private void btnDeleteImage_Click(object sender, EventArgs e)
        {
            this.imageName = null;
            this.pbMoviePoster.Image = this.GetImage(null);
        }

        protected override bool IsValidData()
        {
            //Имя

            if (this.tbMovieName.Text.Trim().Length < 1 || 128 < this.tbMovieName.Text.Trim().Length)
            {
                this.errorProvider.SetError(this.tbMovieName, "Некорректное имя фильма");
                return false;
            }
            else
            {
                this.errorProvider.SetError(this.tbMovieName, "");
            }

            //Жанр

            if (this.cbMovieGenre.SelectedIndex == -1)
            {
                this.errorProvider.SetError(this.cbMovieGenre, "Выберете жанр");
                return false;
            }
            else
            {
                this.errorProvider.SetError(this.cbMovieGenre, "");
            }

            //Продолжительность

            int tmp = 0;
            if (!Int32.TryParse(this.tbMovieDuration.Text, out tmp) || tmp < 1 || this.maxDuration < tmp)
            {
                this.errorProvider.SetError(this.tbMovieDuration, "Некорректная продолжительность фильма");
                return false;
            }
            else
            {
                this.errorProvider.SetError(this.tbMovieDuration, "");
            }

            return true;
        }

        protected override void FillControls()
        {
            this.cbMovieGenre.SelectedItem = this.dataBase.GetNameById("Genres", (int)this.currentDataRow["genre_id"]);
            this.tbMovieName.Text = this.currentDataRow["name"].ToString();
            this.tbMovieDuration.Text = this.currentDataRow["duration"].ToString();
            this.cbMovieYear.SelectedItem = (int)this.currentDataRow["year"];
            this.pbMoviePoster.Image = this.GetImage(this.currentDataRow["image"].ToString());
            this.imageName = this.currentDataRow["image"].ToString();
        }
    }
}