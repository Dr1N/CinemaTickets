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
    public partial class frmBooking : Form
    {
        public List<Point> SoldTickets { get; set; }         //Занятые места
        public List<Point> ReservedTickets { get; set; }     //Зарезервированные места пользователем

        private DataRow session;
        private Placer placer;
        private DateTime BaseDate { get; set; }              //Базовая дата для отсчёта 

        private frmBooking()
        {
            InitializeComponent();
            this.FormClosing += frmBooking_FormClosing;
        }

        public frmBooking(DataRow session, List<Point> soldPlaces) : this()
        {
            this.BaseDate = new DateTime(2010, 1, 1, 0, 0, 0);
            this.session = session;
            this.SoldTickets = soldPlaces;
            int rows = (int)this.session["rows"];
            int places = (int)this.session["places"];
            this.placer = new Placer(rows, places, this.SoldTickets);
            this.placer.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            this.tlpPlacer.Controls.Add(placer);
        }

        private void frmBooking_Load(object sender, EventArgs e)
        {
            this.ShowSessionInformation();
        }

        private void frmBooking_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Сохраняем выбранные места

            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                this.ReservedTickets = this.placer.ReservedPlaces;
            }
        }

        /// <summary>
        /// Вывести информацию о выбранном сеансе
        /// </summary>
        private void ShowSessionInformation()
        {
            DateTime begin = this.BaseDate.AddSeconds((int)this.session["beginning"]);
            this.Text = String.Format("{0} в {1} [{2}]", this.session["movie"], this.session["cinema"], begin.ToLongDateString() + " " + begin.ToShortTimeString());

            this.pbMovie.Image = this.GetImage(this.session["movimage"].ToString());
            this.pbCinema.Image = this.GetImage(this.session["cinimage"].ToString());

            this.lbMovieName.Text = this.session["movie"].ToString();
            this.lbMovieGenre.Text = this.session["genre"].ToString();
            this.lbMovieYear.Text = this.session["year"].ToString();
            this.lbMovieDuration.Text = this.session["duration"].ToString() + " мин";

            this.lbCinemaName.Text = this.session["cinema"].ToString();
            this.lbCinemaAddress.Text = this.session["address"].ToString();
        }

        /// <summary>
        /// Получить объект Image по имени файла в хранилище файлов
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <returns>Объект Image накапуслирующий файл</returns>
        private Image GetImage(string fileName)
        {
            try
            {
                Image image = null;
                string noImageFile = ConfigurationManager.AppSettings["no_image_file"];
                string imagePath = ConfigurationManager.AppSettings["image_path"];
                if (fileName != null && File.Exists(Path.Combine(imagePath, fileName)))
                {
                    image = Image.FromFile(Path.Combine(imagePath, fileName));
                }
                else
                {
                    image = Image.FromFile(Path.Combine(imagePath, noImageFile));
                }
                return image;
            }
            catch
            {
                return null;
            }
        }
    }
}