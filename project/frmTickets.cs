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
    public partial class frmTickets : DialogWindow
    {
        private string tableName = "Tickets";
        private string separator = " -> ";
        private List<DataRow> sessionsInSelectedDay = new List<DataRow>();

        public frmTickets(DbHelper db) 
            : base(db)
        {
            InitializeComponent();

            this.Load += frmTickets_Load;
            this.FormClosing += frmTickets_FormClosing;
            this.Shown += frmTickets_Shown;
        }

        public frmTickets(DbHelper db, DataRow dr, FormMode mode) 
            : base(db, dr, mode)
        {
            InitializeComponent();

            this.Load += frmTickets_Load;
            this.FormClosing += frmTickets_FormClosing;
            this.Shown += frmTickets_Shown;
        }

        private void frmTickets_Load(object sender, EventArgs e)
        {
            //Заполняем поля

            switch (this.Mode)
            {
                case FormMode.NEW:
                    this.Text = "Добавление билета";
                    this.dtpTicketDate.Value = DateTime.Now;
                    break;
                case FormMode.EDIT:
                    this.Text = "Редактирование билета";
                    this.FillControls();
                    break;
                case FormMode.VIEW:
                    this.Text = "Просмотр билета";
                    this.FillControls();

                    this.dtpTicketDate.Enabled = false;
                    this.cbTicketCinema.Enabled = false;
                    this.cbTicketMovie.Enabled = false;
                    this.cbTicketRow.Enabled = false;
                    this.cbTicketPlace.Enabled = false;
                    break;
                default:
                    throw new Exception("Неизвестный режим отображения формы");
            }
        }
       
        private void frmTickets_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Mode == FormMode.VIEW) { return; }

            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (this.IsValidData() == true)
                {
                    DataRow dataRow = (this.Mode == FormMode.NEW ? this.dataBase.Tables[this.tableName].NewRow() : this.currentDataRow);
                    dataRow["session_id"] = this.GetSessionId();
                    dataRow["row"] = (int)this.cbTicketRow.SelectedItem;
                    dataRow["place"] = (int)this.cbTicketPlace.SelectedItem;

                    if (this.Mode == FormMode.NEW)
                    {
                        this.dataBase.Tables[this.tableName].Rows.Add(dataRow);
                    }

                    return;
                }
                e.Cancel = true;
            }
        }

        private void frmTickets_Shown(object sender, EventArgs e)
        {
            this.cbTicketMovie.Focus();
        }

        //При изменении даты заполнить список кинотеатров, в которых есть сеансы в выбранную дату

        private void dtpTicketDate_ValueChanged(object sender, EventArgs e)
        {

            //Получить id кинотеатров для выбранной даты, в которых есть сеансы

            DateTime selectedDate = this.dtpTicketDate.Value;
            List<int> cinemaIdWithSessions = new List<int>();

            this.sessionsInSelectedDay.Clear();
            foreach (DataRow item in this.dataBase.Tables["Sessions"].Rows)
	        {
                DateTime sessionDate = this.dataBase.GetDateFromSecond((int)item["beginning"]);
                if (sessionDate.Year == selectedDate.Year && sessionDate.Month == selectedDate.Month && sessionDate.Day == selectedDate.Day)
                {
                    this.sessionsInSelectedDay.Add(item);
                    cinemaIdWithSessions.Add((int)item["cinema_id"]);
                }
	        }

            //Заполняем список кинотеатров (убираем дупликаты)

            var cinemaIdWithSessionsNoDuplicate = (from cid in cinemaIdWithSessions select cid).Distinct();
            this.cbTicketCinema.Items.Clear();
            this.cbTicketMovie.Items.Clear();
            this.cbTicketRow.Items.Clear();
            this.cbTicketPlace.Items.Clear();
            foreach (int item in cinemaIdWithSessionsNoDuplicate)
            {
                string cinemaName = this.dataBase.GetNameById("Cinema", item);
                this.cbTicketCinema.Items.Add(cinemaName);
            }

            //Выбираем первый кинотеатр

            if (this.cbTicketCinema.Items.Count > 0)
            {
                this.cbTicketCinema.SelectedIndex = 0;
            }
        }

        private void cbTicketCinema_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Заполняем места и ряды

            string cinemaName = this.cbTicketCinema.SelectedItem.ToString();
            int cinemaId = this.dataBase.GetIdByName("Cinema", cinemaName);
            DataRow cinema = this.dataBase.GetDataRowById("Cinema", cinemaId);

            int cinemaRows = (int)cinema["rows"];
            int cinemaPlaces = (int)cinema["places"];
            
            this.cbTicketRow.Items.Clear();
            for (int i = 1; i <= cinemaRows; i++)
            {
                this.cbTicketRow.Items.Add(i);
            }

            this.cbTicketPlace.Items.Clear();
            for (int i = 1; i <= cinemaPlaces; i++)
            {
                this.cbTicketPlace.Items.Add(i);
            }

            this.cbTicketRow.SelectedIndex = 0;
            this.cbTicketPlace.SelectedIndex = 0;

            //Фильмы в выбранном кинотеатре

            this.cbTicketMovie.Items.Clear();
            foreach (DataRow item in this.sessionsInSelectedDay)
            {
                if((int)item["cinema_id"] == cinemaId)
                {
                    int movieId = (int)item["movie_id"];
                    string movieName = this.dataBase.GetNameById("Movies", movieId);
                    DateTime movieBegin = this.dataBase.GetDateFromSecond((int)item["beginning"]);
                    string movieTitle = String.Format("{0}{2}[{1}]", movieName, movieBegin.ToShortTimeString(), this.separator);
                    this.cbTicketMovie.Items.Add(movieTitle);
                }
            }

            if (this.cbTicketMovie.Items.Count > 0)
            {
                this.cbTicketMovie.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Получить id сеанса по выбранному кинотеатру и фильму
        /// </summary>
        /// <returns>id сеанса</returns>
        private int GetSessionId()
        {
            string cinemaName = this.cbTicketCinema.SelectedItem.ToString();
            int cinemaId = this.dataBase.GetIdByName("Cinema", cinemaName);

            string movieTitle = this.cbTicketMovie.SelectedItem.ToString();
            //string movieName = movieTitle.Substring(movieTitle.IndexOf(this.separator));
            string movieName = movieTitle.Substring(0, movieTitle.IndexOf(this.separator));

            int movieId = this.dataBase.GetIdByName("Movies", movieName);

            foreach (DataRow item in this.sessionsInSelectedDay)
            {
                if ((int)item["cinema_id"] == cinemaId && (int)item["movie_id"] == movieId)
                {
                    return (int)item["id"];
                }
            }

            return 0;
        }

        protected override bool IsValidData()
        {
            //Выбор билета

            if (this.cbTicketCinema.SelectedIndex == -1)
            {
                this.errorProvider.SetError(this.cbTicketCinema, "Выберете кинотеатр");
                return false;
            }
            else
            {
                this.errorProvider.SetError(this.cbTicketCinema, "");
            }

            if (this.cbTicketCinema.SelectedIndex == -1)
            {
                this.errorProvider.SetError(this.cbTicketMovie, "Выберете фильм");
                return false;
            }
            else
            {
                this.errorProvider.SetError(this.cbTicketMovie, "");
            }

            if (this.cbTicketRow.SelectedIndex == -1)
            {
                this.errorProvider.SetError(this.cbTicketRow, "Выберете ряд");
                return false;
            }
            else
            {
                this.errorProvider.SetError(this.cbTicketRow, "");
            }

            if (this.cbTicketPlace.SelectedIndex == -1)
            {
                this.errorProvider.SetError(this.cbTicketPlace, "Выберете место");
                return false;
            }
            else
            {
                this.errorProvider.SetError(this.cbTicketPlace, "");
            }

            //Не продан ли билет

            int sessionId = this.GetSessionId();
            int row = (int)this.cbTicketRow.SelectedItem;
            int place = (int)this.cbTicketPlace.SelectedItem;

            foreach (DataRow item in this.dataBase.Tables["Tickets"].Rows)
            {
                if (this.Mode == FormMode.EDIT || this.Mode == FormMode.VIEW)
                {
                    if ((int)item["id"] == (int)this.currentDataRow["id"]) { continue; }
                }
                if ((int)item["session_id"] == sessionId && (int)item["row"] == row && (int)item["place"] == place)
                {
                    this.errorProvider.SetError(this.cbTicketRow, "Билет уже продан");
                    this.errorProvider.SetError(this.cbTicketPlace, "Билет уже продан");
                    return false;
                }
            }
            
            this.errorProvider.SetError(this.cbTicketRow, "");
            this.errorProvider.SetError(this.cbTicketPlace, "");

            return true;
        }

        protected override void FillControls()
        {
            //Сеанс

            int sessionId = (int)this.currentDataRow["session_id"];
            DataRow session = this.dataBase.GetDataRowById("Sessions", sessionId);

            //Дата сенса

            DateTime dateTime = this.dataBase.GetDateFromSecond((int)session["beginning"]);
            this.dtpTicketDate.Value = dateTime;

            //Кинотеатр

            int cinemaId = (int)session["cinema_id"];
            string cinemaName = this.dataBase.GetNameById("Cinema", cinemaId);

            this.cbTicketCinema.SelectedItem = cinemaName;

            //Фильм

            int movieId = (int)session["movie_id"];
            string movieName = this.dataBase.GetNameById("Movies", movieId);
            string movieTitle = String.Format("{0}{2}[{1}]", movieName, dateTime.ToShortTimeString(), this.separator);

            this.cbTicketMovie.SelectedItem = movieTitle;

            //Место

            int row = (int)this.currentDataRow["row"];
            int place = (int)this.currentDataRow["place"]; ;

            this.cbTicketRow.SelectedItem = row;
            this.cbTicketPlace.SelectedItem = place;
        }
    }
}