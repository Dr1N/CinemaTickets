using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cinema
{
    public partial class frmSessions : DialogWindow
    {
        private string tableName = "Sessions";

        public frmSessions(DbHelper db) 
            : base(db)
        {
            InitializeComponent();

            this.Load += frmSessions_Load;
            this.FormClosing += frmSessions_FormClosing;
            this.Shown += frmSessions_Shown;
        }

        public frmSessions(DbHelper db, DataRow dr, FormMode mode)
            : base(db, dr, mode)
        {
            InitializeComponent();

            this.Load += frmSessions_Load;
            this.FormClosing += frmSessions_FormClosing;
            this.Shown += frmSessions_Shown;
        }

        private void frmSessions_Load(object sender, EventArgs e)
        {
            //Фильмы

            for (int i = 0; i < this.dataBase.Tables["Movies"].Rows.Count; i++)
            {
                this.cbSessionMovie.Items.Add(this.dataBase.Tables["Movies"].Rows[i]["name"].ToString());
            }

            //Кинотеатры

            for (int i = 0; i < this.dataBase.Tables["Cinema"].Rows.Count; i++)
            {
                this.cbSessionCinema.Items.Add(this.dataBase.Tables["Cinema"].Rows[i]["name"].ToString());
            }

            //Настройка датапикера

            this.dtpBeginning.Format = DateTimePickerFormat.Custom;
            this.dtpBeginning.CustomFormat = "dMMMMyyyy HH:mm";
            this.dtpBeginning.ShowUpDown = true;
            this.dtpBeginning.MinDate = this.dataBase.BaseDate;
            this.dtpBeginning.MaxDate = this.dataBase.BaseDate.AddYears(5);

            //Заполняем поля

            switch (this.Mode)
            {
                case FormMode.NEW:
                    this.Text = "Добавление сеанса";
                    this.cbSessionMovie.SelectedIndex = 0;
                    this.cbSessionCinema.SelectedIndex = 0;
                    this.dtpBeginning.Value = DateTime.Now;
                    break;
                case FormMode.EDIT:
                    this.Text = "Редактирование сеанса";
                    this.FillControls();
                    break;
                case FormMode.VIEW:
                    this.Text = "Просмотр сеанса";
                    this.FillControls();

                    this.cbSessionMovie.Enabled = false;
                    this.cbSessionCinema.Enabled = false;
                    this.dtpBeginning.Enabled = false;
                    break;
                default:
                    throw new Exception("Неизвестный режим отображения формы");
            }
        }

        private void frmSessions_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Mode == FormMode.VIEW) { return; }

            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (this.IsValidData() == true)
                {
                    DataRow dataRow = (this.Mode == FormMode.NEW ? this.dataBase.Tables[this.tableName].NewRow() : this.currentDataRow);
                    dataRow["movie_id"] = this.dataBase.GetIdByName("Movies", this.cbSessionMovie.SelectedItem.ToString());
                    dataRow["cinema_id"] = this.dataBase.GetIdByName("Cinema", this.cbSessionCinema.SelectedItem.ToString());
                    DateTime begin = this.dtpBeginning.Value;
                    begin = begin.AddSeconds((-1) * begin.Second);
                    dataRow["beginning"] = this.dataBase.GetSecondsFromDate(begin);
                    if (this.Mode == FormMode.NEW)
                    {
                        this.dataBase.Tables[this.tableName].Rows.Add(dataRow);
                    }
                    return;
                }
                e.Cancel = true;
            }
        }

        private void frmSessions_Shown(object sender, EventArgs e)
        {
            this.cbSessionMovie.Focus();
        }

        protected override bool IsValidData()
        {
            //Начало фильма

            DateTime begin = this.dtpBeginning.Value;
            begin = begin.AddSeconds((-1) * begin.Second);

            int beginMovieTime = this.dataBase.GetSecondsFromDate(begin);

            //Продожлительность выбранного фильма фильма

            int durationInSecond = -1;
            for (int i = 0; i < this.dataBase.Tables["Movies"].Rows.Count; i++)
			{
                if (this.dataBase.Tables["Movies"].Rows[i]["name"].ToString() == this.cbSessionMovie.SelectedItem.ToString())
                {
                    durationInSecond = 60 * (int)this.dataBase.Tables["Movies"].Rows[i]["duration"];
                }
			}

            //Конец фильма

            int endMovieTime = beginMovieTime + durationInSecond;

            //Выбранный кинотеатр
            
            int cinemaId = this.dataBase.GetIdByName("Cinema", this.cbSessionCinema.SelectedItem.ToString());

            //Пересечение сеансов. Заполняем коллекцию сеансов для данного кинотеатра

            List<SessionTime> cinemaSessions = this.GetSessionTimes(cinemaId);
            foreach (SessionTime item  in cinemaSessions)
            {
                //Проверка начала сеанса

                if (item.BeginTime <= beginMovieTime && beginMovieTime <= item.EndTime)
                {
                    this.errorProvider.SetError(this.dtpBeginning, "Начало сеанса пересекается с существующим");
                    return false;
                }

                //Проверка конца сеанса
                
                if(item.BeginTime <= endMovieTime && endMovieTime <= item.EndTime)
                {
                    this.errorProvider.SetError(this.dtpBeginning, "Конец сеанса пересекается с существующим");
                    return false;
                }
            }

            //Вариант где сеанс "поглотил" другой

            foreach (SessionTime item in cinemaSessions)
            {
                if (beginMovieTime < item.BeginTime && item.EndTime < endMovieTime)
                {
                    this.errorProvider.SetError(this.dtpBeginning, "Сеанс включает в себя другой сеанс");
                    return false;
                }
            }

            this.errorProvider.SetError(this.dtpBeginning, "");

            return true;
        }

        protected override void FillControls()
        {
            this.cbSessionMovie.SelectedItem = this.dataBase.GetNameById("Movies", (int)this.currentDataRow["movie_id"]);
            this.cbSessionCinema.SelectedItem = this.dataBase.GetNameById("Cinema",(int)this.currentDataRow["cinema_id"]);
            this.dtpBeginning.Value = this.dataBase.GetDateFromSecond((int)this.currentDataRow["beginning"]);
        }

        /// <summary>
        /// Получить список начала и конца сеансов кинотеатра
        /// </summary>
        /// <param name="cinemaId">Ид кинотеатра</param>
        /// <returns>Список начала и конца сеансов</returns>
        private List<SessionTime> GetSessionTimes(int cinemaId)
        {
            List<SessionTime> sessionsTime = new List<SessionTime>();

            //Перебираем все сеансы

            for (int ses = 0; ses < this.dataBase.Tables["Sessions"].Rows.Count; ses++)
            {
                //Пропуск текущего сеанса при редактировании

                if (this.Mode == FormMode.EDIT && (int)this.currentDataRow["id"] == (int)this.dataBase.Tables["Sessions"].Rows[ses]["id"])
                {
                    continue;
                }

                int beginInSecond = -1;
                int currentMovieDuration = -1;

                //Для сеансов данного кинотеатра

                if ((int)this.dataBase.Tables["Sessions"].Rows[ses]["cinema_id"] == cinemaId)
                {
                    //Начало каждого сеанса

                    beginInSecond = (int)this.dataBase.Tables["Sessions"].Rows[ses]["beginning"];

                    //Продолжительность фильма текущего сеанса. Перебираем фильмы

                    for (int mov = 0; mov < this.dataBase.Tables["Movies"].Rows.Count; mov++)
                    {
                        //Получаем продолжительность

                        if ((int)this.dataBase.Tables["Movies"].Rows[mov]["id"] == (int)this.dataBase.Tables["Sessions"].Rows[ses]["movie_id"])
                        {
                            currentMovieDuration = 60 * (int)this.dataBase.Tables["Movies"].Rows[mov]["duration"];
                        }
                    }
                    
                    //Сохраняем время сеанса

                    sessionsTime.Add(new SessionTime(beginInSecond, beginInSecond + currentMovieDuration));
                }
            }

            return sessionsTime;
        }
    }

    class SessionTime
    {
        public int BeginTime { get; set; }
        public int EndTime { get; set; }

        public SessionTime(int bt, int et)
        {
            this.BeginTime = bt;
            this.EndTime = et;
        }
    }
}