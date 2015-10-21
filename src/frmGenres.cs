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
    public partial class frmGenres : DialogWindow
    {
        private string tableName = "Genres";
        
        public frmGenres(DbHelper db) 
            : base(db) 
        {
            InitializeComponent();

            this.Load += frmGenres_Load;
            this.FormClosing += frmGenres_FormClosing;
            this.Shown += frmGenres_Shown;
        }

        public frmGenres(DbHelper db, DataRow dr, FormMode mode)
            : base(db, dr, mode)
        {
            InitializeComponent();

            this.Load += frmGenres_Load;
            this.FormClosing += frmGenres_FormClosing;
            this.Shown += frmGenres_Shown;
        }

        private void frmGenres_Load(object sender, EventArgs e)
        {
            switch (this.Mode)
            {
                case FormMode.NEW:
                    this.Text = "Добавление жанра";
                    break;
                case FormMode.EDIT:
                    this.Text = "Редактирование жанра";
                    this.FillControls();
                    break;
                case FormMode.VIEW:
                    this.Text = "Просмотр жанра";
                    this.FillControls();
                    this.tbGenreName.ReadOnly = true;
                    break;
                default:
                    throw new Exception("Неизвестный режим отображения формы");
            }
        }

        private void frmGenres_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Mode == FormMode.VIEW) { return; }

            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (this.IsValidData() == true)
                {
                    DataRow dataRow = (this.Mode == FormMode.NEW ? this.dataBase.Tables[this.tableName].NewRow() : this.currentDataRow);
                    dataRow["name"] = this.tbGenreName.Text.Trim();

                    if (this.Mode == FormMode.NEW)
                    {
                        this.dataBase.Tables[this.tableName].Rows.Add(dataRow);
                    }
                    return;
                }
                e.Cancel = true;
            }
        }

        private void frmGenres_Shown(object sender, EventArgs e)
        {
            this.tbGenreName.Focus();
        }

        protected override void FillControls()
        {
            this.tbGenreName.Text = this.currentDataRow["name"].ToString();
        }

        protected override bool IsValidData()
        {
            if (this.tbGenreName.Text.Trim().Length < 3 || 64 < this.tbGenreName.Text.Trim().Length)
            {
                this.errorProvider.SetError(this.tbGenreName, "Некорректное имя жанра");
                return false;
            }
            this.errorProvider.SetError(this.tbGenreName, "");
            return true;
        }
    }
}