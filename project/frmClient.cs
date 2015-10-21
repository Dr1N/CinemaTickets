using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cinema
{
    public partial class frmClient : Form
    {
        private DateTime BaseDate { get; set; }         //Базовая дата для отсчёта 
        private int deltaMinutes = 60;                  //+/- минут при поиске сенсов со временем
        private string windowTitle = "Идём в кино";

        //СУБД объекты

        private DbProviderFactory dbFactory;
        private DbConnection dbConnection;
        private DbDataAdapter dbAdapter;
        private DataTable searchResult = new DataTable("Result");

        public frmClient()
        {
            InitializeComponent();

            this.dtpDate.MinDate = DateTime.Now;
            this.lvResult.SmallImageList = this.imlResult;
            this.BaseDate = new DateTime(2010, 1, 1, 0, 0, 0);

            this.Load += frmClient_Load;
            this.FormClosing += frmClient_FormClosing;
            
            //ToolTip

            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 300;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;

            toolTip1.SetToolTip(tbMovieName, "Пустое поле - поиск по всем фильмам");
            toolTip1.SetToolTip(tbCinemaName, "Пустое поле - поиск по всем кинотеатрам");
            toolTip1.SetToolTip(lvResult, "Двойной клик для бронирования");

            //ListView результата

            this.AddColumnsInListView();
        }

        private void frmClient_Load(object sender, EventArgs e)
        {
            try
            {
                string provider = ConfigurationManager.ConnectionStrings["dbCinema"].ProviderName;
                string connStr = ConfigurationManager.ConnectionStrings["dbCinema"].ConnectionString;

                this.dbFactory = DbProviderFactories.GetFactory(provider);
                this.dbConnection = this.dbFactory.CreateConnection();
                this.dbAdapter = this.dbFactory.CreateDataAdapter();
                this.dbConnection.ConnectionString = connStr;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void frmClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите выйти?", "Поиск", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        #region События элементов управления
        
        private void btnReset_Click(object sender, EventArgs e)
        {
            this.tbCinemaName.Text = "";
            this.tbMovieName.Text = "";
            this.dtpDate.Value = DateTime.Now;
            this.dtpTime.Value = DateTime.Now;
            this.lvResult.Items.Clear();
            this.imlResult.Images.Clear();
            this.searchResult.Rows.Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.searchResult.Rows.Clear();
                DbCommand selCmd = this.dbConnection.CreateCommand();
                selCmd.CommandText = this.GetQueryString();
                this.dbAdapter.SelectCommand = selCmd;
                this.dbAdapter.Fill(this.searchResult);
                this.ShowSearchResult();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message + "\n" + exc.StackTrace, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbDateTime_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbx = sender as CheckBox;
            if (cbx != null)
            {
                if (cbx.Name == "cbDate")
                {
                    //вкл дату
                    this.dtpDate.Enabled = cbx.Checked;
                    this.label3.Enabled = cbx.Checked;
                    this.cbTime.Enabled = cbx.Checked;
                    //вкл время
                    if (this.cbTime.Checked == true)
                    {
                        this.dtpTime.Enabled = cbx.Checked;
                        this.label4.Enabled = cbx.Checked;
                    }
                }
                else if (cbx.Name == "cbTime")
                {
                    this.dtpTime.Enabled = cbx.Checked;
                    this.label4.Enabled = cbx.Checked;
                }
            }
        }

        private void mnAdmin_Click(object sender, EventArgs e)
        {
            frmAdmin adminWindow = new frmAdmin();
            adminWindow.ShowDialog();
        }

        private void mnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmnBooking_Click(object sender, EventArgs e)
        {
            this.BookingPlace();
        }

        private void lvResult_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView lvResult = sender as ListView;
            if (lvResult != null)
            {
                if (lvResult.SelectedIndices.Count == 1)
                {
                    this.BookingPlace();
                }
            }
        }

        #endregion

        #region Helpers
        
        /// <summary>
        /// Фильтровать опасные символы
        /// </summary>
        /// <param name="source">Входящая строка</param>
        /// <returns>Строка с отфильтрованными/экранированными символами</returns>
        private string FilterQuery(string source)
        {
            return source.Replace("'", "''");
        }

        /// <summary>
        /// Получить строку, содержащую SELECT запрос, согласно выбору пользователя
        /// </summary>
        /// <returns>Строка запроса</returns>
        private string GetQueryString()
        {
            //Выудим всю информацию супер-пупер запросом

            string select = "SELECT " + 
                "Sessions.id AS session" +
                ", Movies.name AS movie" +
                ", Movies.duration AS duration" +
                ", Movies.year AS year" +
                ", Movies.image AS movimage" +
                ", Cinema.name AS cinema" +
                ", Cinema.address AS address" +
                ", Cinema.rows AS rows" +
                ", Cinema.places AS places" +
                ", Cinema.image AS cinimage" + 
                ", Sessions.beginning AS beginning" +
                ", Genres.name AS genre " +
                "FROM (Sessions INNER JOIN (Movies INNER JOIN Genres ON Movies.genre_id = Genres.id) ON Sessions.movie_id = Movies.id) INNER JOIN Cinema ON Sessions.cinema_id = Cinema.id ";
            
            List<string> conditions = new List<string>();

            //Условие дата/время

            if (this.cbDate.Checked == true)
            {
                DateTime selectedDateTime;
                DateTime minDate = new DateTime();
                DateTime maxDate = new DateTime();

                //Дата и время
                if (this.dtpDate.Enabled == true && this.dtpTime.Enabled == true)
                {
                    selectedDateTime = new DateTime(this.dtpDate.Value.Year, this.dtpDate.Value.Month, this.dtpDate.Value.Day, this.dtpTime.Value.Hour, this.dtpTime.Value.Minute, 0);
                    minDate = selectedDateTime.AddMinutes((-1) * this.deltaMinutes);
                    maxDate = selectedDateTime.AddMinutes(this.deltaMinutes);
                }
                //Дата только
                else if (this.dtpDate.Enabled == true && this.dtpTime.Enabled == false)
                {
                    minDate = new DateTime(this.dtpDate.Value.Year, this.dtpDate.Value.Month, this.dtpDate.Value.Day, 0, 0, 0);
                    maxDate = minDate.AddDays(1.0);
                }

                int minSec = (int)(minDate - this.BaseDate).TotalSeconds;
                int maxSec = (int)(maxDate - this.BaseDate).TotalSeconds;
                conditions.Add(String.Format("Sessions.beginning BETWEEN {0} AND {1}", minSec, maxSec));
            }

            //Фильм и кинотеатр

            string movie = this.FilterQuery(this.tbMovieName.Text.Trim());
            if (movie.Length > 0)
            {
                conditions.Add(String.Format("Movies.name LIKE('%{0}%')", movie));
            }

            string cinema = this.FilterQuery(this.tbCinemaName.Text.Trim());
            if (cinema.Length > 0)
            {
                conditions.Add(String.Format("Cinema.name LIKE('%{0}%')", cinema));
            }

            //Условия есть

            if (conditions.Count > 0)
            {
                select += "WHERE " + String.Join(" AND ", conditions.ToArray());
            }

            return select;
        }

        /// <summary>
        /// Добавить колонки в ListView результата
        /// </summary>
        private void AddColumnsInListView()
        {
            ColumnHeader ch = new ColumnHeader();
            ch.Text = "Фильм";
            this.lvResult.Columns.Add(ch);

            ch = new ColumnHeader();
            ch.Text = "Кинотеатр";
            this.lvResult.Columns.Add(ch);

            ch = new ColumnHeader();
            ch.Text = "Начало";
            this.lvResult.Columns.Add(ch);

            ch = new ColumnHeader();
            ch.Text = "Длительность";
            this.lvResult.Columns.Add(ch);

            ch = new ColumnHeader();
            ch.Text = "Адрес";
            this.lvResult.Columns.Add(ch);
        }

        /// <summary>
        /// Отобразить результат поиска
        /// </summary>
        private void ShowSearchResult()
        {
            this.Text = String.Format("{0} [Найдено: {1}]", this.windowTitle, this.searchResult.Rows.Count);
            
            this.lvResult.Items.Clear();
            this.imlResult.Images.Clear();

            foreach (DataRow item in this.searchResult.Rows)
            {
                string movieTitle = String.Format("{0} ({1}, {2} год)", item["movie"].ToString(), item["genre"].ToString(), (int)item["year"]);

                Image movieImage = this.GetImage(item["movimage"].ToString());
                if (movieImage != null)
                {
                    Bitmap bmp = this.GetThumb(movieImage, this.imlResult.ImageSize);
                    this.imlResult.Images.Add(item["movie"].ToString(), bmp);
                }

                ListViewItem lvi = new ListViewItem(movieTitle);
                lvi.ImageKey = item["movie"].ToString();
                lvi.SubItems.Add(item["cinema"].ToString());
                DateTime begin = this.BaseDate.AddSeconds((int)item["beginning"]);
                lvi.SubItems.Add(begin.ToLongDateString() + " " + begin.ToShortTimeString());
                string duration = String.Format("{0} мин", item["duration"]);
                lvi.SubItems.Add(duration);
                lvi.SubItems.Add(item["address"].ToString());
                lvi.Tag = item;
                this.lvResult.Items.Add(lvi);
            }
                        
            for (int i = 0; i < this.lvResult.Columns.Count; i++)
            {
                ColumnHeaderAutoResizeStyle style = (this.lvResult.Items.Count != 0) ? ColumnHeaderAutoResizeStyle.ColumnContent : ColumnHeaderAutoResizeStyle.HeaderSize;
                this.lvResult.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        /// <summary>
        /// Бронирование билетов
        /// </summary>
        private void BookingPlace()
        {
            if (this.lvResult.SelectedIndices.Count != 1) { return; }

            try
            {
                //Сеанс

                DataRow session = this.lvResult.SelectedItems[0].Tag as DataRow;
                if (session == null) { return; }

                //Проданные билеты на сеанс

                DataTable sessionTickets = new DataTable("Places");
                DbCommand cmd = this.dbConnection.CreateCommand();
                int sessionId = (int)session["session"];
                cmd.CommandText = String.Format("SELECT * FROM Tickets WHERE session_id = {0}", sessionId);
                this.dbAdapter.SelectCommand = cmd;
                this.dbAdapter.Fill(sessionTickets);

                List<Point> soldTickets = new List<Point>();
                foreach (DataRow item in sessionTickets.Rows)
                {
                    soldTickets.Add(new Point((int)item["row"], (int)item["place"]));
                }

                //Отображение формы заказа билетов

                frmBooking frmBooking = new frmBooking(session, soldTickets);
                if (frmBooking.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //Обновление таблицы

                    foreach (Point item in frmBooking.ReservedTickets)
                    {
                        DataRow ticket = sessionTickets.NewRow();
                        ticket["session_id"] = sessionId;
                        ticket["row"] = item.X;
                        ticket["place"] = item.Y;
                        sessionTickets.Rows.Add(ticket);
                    }
                    
                    //Создание команды и обновление
                    
                    DbCommand insertCmd = this.dbConnection.CreateCommand();
                    insertCmd.CommandText = String.Format("INSERT INTO Tickets (session_id, row, place) VALUES({0}, @row, @place)", sessionId);
                    DbParameter rowParam = insertCmd.CreateParameter();
                    rowParam.ParameterName = "@row";
                    rowParam.DbType = DbType.Int32;
                    rowParam.SourceColumn = "row";
                    insertCmd.Parameters.Add(rowParam);

                    DbParameter placeParam = insertCmd.CreateParameter();
                    placeParam.ParameterName = "@place";
                    placeParam.DbType = DbType.Int32;
                    placeParam.SourceColumn = "place";
                    insertCmd.Parameters.Add(placeParam);
                    this.dbAdapter.InsertCommand = insertCmd;
                    this.dbAdapter.Update(sessionTickets);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n" + e.StackTrace, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        /// <summary>
        /// Получить миниатюру для списка изображений
        /// </summary>
        /// <param name="image">Исходное изображение</param>
        /// <param name="size">Размер миниатюры</param>
        /// <returns>Миниатюра</returns>
        public Bitmap GetThumb(Image image, Size size)
        {
            int tw, th, tx, ty;
            int w = image.Width;
            int h = image.Height;

            double whRatio = (double)w / h;
            if (image.Width >= image.Height)
            {
                tw = size.Width;
                th = (int)(tw / whRatio);
            }
            else
            {
                th = size.Height;
                tw = (int)(th * whRatio);
            }

            tx = (size.Width - tw) / 2;
            ty = (size.Height - th) / 2;

            Bitmap thumb = new Bitmap(size.Width, size.Height, image.PixelFormat);
            Graphics g = Graphics.FromImage(thumb);
            g.Clear(Color.White);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(image, new Rectangle(tx, ty, tw, th),new Rectangle(0, 0, w, h), GraphicsUnit.Pixel);
            return thumb;
        }

        #endregion
    }
}