using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cinema
{
    public partial class frmAdmin : Form
    {
        private Dictionary<string, string> columnNames = new Dictionary<string, string>();
        private DbHelper dataBase;

        public frmAdmin()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            this.FormClosing += frmAdmin_FormClosing;

            //Словарь для заголовков столбцов

            this.columnNames["id"] = "Ид";
            this.columnNames["name"] = "Название";
            this.columnNames["genre_id"] = "Жанр";
            this.columnNames["duration"] = "Продолжительность";
            this.columnNames["year"] = "Год выхода";
            this.columnNames["image"] = "Изображение";
            this.columnNames["address"] = "Адрес";
            this.columnNames["rows"] = "Рядов";
            this.columnNames["places"] = "Мест я ряде";
            this.columnNames["movie_id"] = "Фильм";
            this.columnNames["cinema_id"] = "Кинотеатр";
            this.columnNames["beginning"] = "Время начала";
            this.columnNames["session_id"] = "Сеанс";
            this.columnNames["row"] = "Ряд";
            this.columnNames["place"] = "Место";

            //ToolTip

            string tip = "Двойной клик для просмотра";

            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;

            toolTip1.SetToolTip(this.lvGenres, tip);
            toolTip1.SetToolTip(this.lvMovies, tip);
            toolTip1.SetToolTip(this.lvCinema, tip);
            toolTip1.SetToolTip(this.lvSessions, tip);
            toolTip1.SetToolTip(this.lvTickets, tip);
        }

        #region События элементов управления

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //Соединение и чтение базы

                string provider = ConfigurationManager.ConnectionStrings["dbCinema"].ProviderName;
                string connStr = ConfigurationManager.ConnectionStrings["dbCinema"].ConnectionString;
                this.dataBase = new DbHelper(provider, connStr);
                
                //Конфгурирование столбцов в ListView's

                for (int i = 0; i < this.dataBase.Tables.Count; i++)
                {
                    string controlName = "lv" + this.dataBase.Tables[i].TableName;
                    Control[] controls = this.Controls.Find(controlName, true);
                    if (controls.Length == 1)
                    {
                        ListView currentListView = controls[0] as ListView;
                        if (currentListView != null)
                        {
                            this.AddListViewColumnsFromDataTable(currentListView, this.dataBase.Tables[i]);
                        }
                    }
                }

                //Вывод информации

                this.ShowData();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message + "\n" + exc.StackTrace, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void frmAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите выйти?", "Управление", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        #region Меню
        
        private void mnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Система поиска билетов в кино", "Сinema", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void mnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnAddGenre_Click(object sender, EventArgs e)
        {
            this.tcAdmin.SelectedTab = this.tpGenres;
            this.AddObject();
        }

        private void mnAddMovie_Click(object sender, EventArgs e)
        {
            this.tcAdmin.SelectedTab = this.tpMovies;
            this.AddObject();
        }

        private void mnAddCinema_Click(object sender, EventArgs e)
        {
            this.tcAdmin.SelectedTab = this.tpCinema;
            this.AddObject();
        }

        private void mnAddSession_Click(object sender, EventArgs e)
        {
            this.tcAdmin.SelectedTab = this.tpSessions;
            this.AddObject();
        }

        private void mnAddTicket_Click(object sender, EventArgs e)
        {
            this.tcAdmin.SelectedTab = this.tpTickets;
            this.AddObject();
        }

        private void mnEdit_Click(object sender, EventArgs e)
        {
            this.EditObject();
        }

        private void mnDelete_Click(object sender, EventArgs e)
        {
            this.DeleteObject();
        }

        private void mnOperations_DropDownOpening(object sender, EventArgs e)
        {
            ListView lvActive = this.GetActiveListView();
            if (lvActive != null)
            {
                this.SetControlsState(lvActive);
            }
        }

        private void mnRefresh_Click(object sender, EventArgs e)
        {
            this.dataBase.UpdateDataSetFromDb();
            this.ShowData();
        }

        private void mnClear_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Удаление сеансов до текущей даты", "to-do");
        }

        #endregion

        #region Контекстное меню
        
        private void cmnLvMenu_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip cm = sender as ContextMenuStrip;
            ListView lv = cm.SourceControl as ListView;
            if (lv != null) 
            {
                this.SetControlsState(lv);
            }
        }

        private void cmnAdd_Click(object sender, EventArgs e)
        {
            this.AddObject();
        }

        private void cmnEdit_Click(object sender, EventArgs e)
        {
            this.EditObject();
        }

        private void cmnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите удалть объект(ы)?", "Cinema", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                this.DeleteObject();
            }
        }

        #endregion

        #region ListView и TabControl

        private void lv_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView lvSender = sender as ListView;
            if (lvSender != null)
            {
                ListViewItem lvi = lvSender.GetItemAt(e.X, e.Y);
                if (lvi == null || lvSender.SelectedIndices.Count == 0) { return; }
                this.ViewObject();
            }
        }

        private void lv_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView lv = sender as ListView;
            if (lv != null) 
            {
                this.SetControlsState(lv);
            }
        }

        private void lv_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                ListView lvActive = this.GetActiveListView();
                if (lvActive != null)
                {
                    this.DeleteObject();
                }
            }
        }

        private void tcAdmin_SelectedIndexChanged(object sender, EventArgs e)
        {
            //При смене вкладки выделенных элеменотв нет

            this.tsBtnView.Enabled = false;
            this.tsBtnEdit.Enabled = false;
            this.tsBtnDelete.Enabled = false;
        }

        #endregion

        #region Панель инструментов

        private void tsBtnAdd_Click(object sender, EventArgs e)
        {
            this.AddObject();
        }

        private void tsBtnView_Click(object sender, EventArgs e)
        {
            this.ViewObject();
        }

        private void tsBtnEdit_Click(object sender, EventArgs e)
        {
            this.EditObject();
        }

        private void tsBtnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите удалть объект(ы)?", "Cinema", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                this.DeleteObject();
            }
        }

        #endregion

        #endregion

        #region Helpers

        /// <summary>
        /// Добавить столбцы в ListView элемент согласно столбцам DataTable
        /// </summary>
        /// <param name="dt">DataTable источник</param>
        /// <param name="lv">ListView которому добавляются колонки</param>
        private void AddListViewColumnsFromDataTable(ListView lv, DataTable dt)
        {
            if (lv == null || dt == null) { throw new ArgumentNullException(); }
            
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ColumnHeader ch = new ColumnHeader();
                string key = dt.Columns[i].ColumnName;
                ch.Text = this.columnNames[key];
                lv.Columns.Add(ch);
            }
        }

        /// <summary>
        /// Отобразить содержание DataTable в ListView элементе
        /// </summary>
        /// <param name="lv">ListView в котором отображаются данные</param>
        /// <param name="dt">DataTable из которого брать данные</param>
        private void ShowDataTableInListView(ListView lv, DataTable dt)
        {
            if(lv == null ||  dt == null) { throw new ArgumentNullException(); }
            if (lv.Columns.Count != dt.Columns.Count) { throw new ApplicationException("Не совпадает колчество столбоцов в ListView и DataTable"); }

            //Заполнение ListView элемента

            if (lv.Items.Count != 0) { lv.Items.Clear(); }
            lv.Tag = dt;                                        //сохраняем ссылку на отображаемую таблицу

            ListViewItem lvi;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lvi = new ListViewItem();
                lvi.Text = dt.Rows[i]["id"].ToString();         //Столбец ИД(id)
                for (int j = 1; j < dt.Columns.Count; j++)
                {
                    //lvi.SubItems.Add(dt.Rows[i][j].ToString());
                    lvi.SubItems.Add(this.GetColumnText(dt.Rows[i], j));
                }
                lvi.Tag = dt.Rows[i];                           //сохраняем ссылку на отоброжаемую строку
                lv.Items.Add(lvi);
            }

            //Установка ширины столбцов(по заголовку)

            for (int i = 0; i < lv.Columns.Count; i++)
            {
                ColumnHeaderAutoResizeStyle style = (lv.Items.Count != 0) ? ColumnHeaderAutoResizeStyle.ColumnContent : ColumnHeaderAutoResizeStyle.HeaderSize;
                lv.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        /// <summary>
        /// Вывести информацию из DataSet в ListView's фомы
        /// </summary>
        private void ShowData()
        {
            for (int i = 0; i < this.dataBase.Tables.Count; i++)
            {
                string controlName = "lv" + this.dataBase.Tables[i].TableName;
                Control[] controls = this.Controls.Find(controlName, true);
                if (controls.Length == 1)
                {
                    ListView currentListView = controls[0] as ListView;
                    if (currentListView != null)
                    {
                        this.ShowDataTableInListView(currentListView, this.dataBase.Tables[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Обновить данные после внесения изменений в таблицу
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        private void UpdateData(string tableName)
        {
            try
            {
                this.dataBase.UpdateDbFromTable(tableName);
                this.dataBase.UpdateDataSetFromDb();
                this.ShowData();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n" + e.StackTrace, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Добавление нового объекта в БД
        /// </summary>
        private void AddObject()
        {
            try
            {
                string tableName = this.tcAdmin.SelectedTab.Name.Substring(2);

                //Проверка возможности создать объект

                switch (tableName)
                {
                    case "Movies":
                        if (this.dataBase.Tables["Genres"].Rows.Count == 0)
                        {
                            MessageBox.Show("Невозможно добавить фильм. Добавте жанр", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        break;
                    case "Sessions":
                        if (this.dataBase.Tables["Movies"].Rows.Count == 0)
                        {
                            MessageBox.Show("Невозможно добавить сеанс. Добавте фильм", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        } 
                        else if (this.dataBase.Tables["Cinema"].Rows.Count == 0)
                        {
                            MessageBox.Show("Невозможно добавить сеанс. Добавте кинотеатр", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        break;
                    case "Tickets":
                        if (this.dataBase.Tables["Sessions"].Rows.Count == 0)
                        {
                            MessageBox.Show("Невозможно добавить билет. Добавте сеанс", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        } 
                        break;
                    default:
                        break;
                }


                DialogWindow form = this.GetDialogWindow(FormMode.NEW);
                
                if (form == null) { throw new Exception("Не удалось создать объект диалогового окна"); }

                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.UpdateData(tableName);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n" + e.StackTrace, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Редактирование выделенного объекта
        /// </summary>
        private void EditObject()
        {
            try
            {
                DialogWindow form = this.GetDialogWindow(FormMode.EDIT);

                if (form == null) { throw new Exception("Не удалось создать объект диалогового окна"); }

                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string tableName = this.tcAdmin.SelectedTab.Name.Substring(2);
                    ListView lvActive = this.GetActiveListView();
                    this.UpdateData(tableName);
                    this.SetControlsState(lvActive);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n" + e.StackTrace, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Просмотр выделенного объекта
        /// </summary>
        private void ViewObject()
        {
            try
            {
                DialogWindow form = this.GetDialogWindow(FormMode.VIEW);
                if (form == null) { throw new Exception("Не удалось создать объект диалогового окна"); }
                form.ShowDialog();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n" + e.StackTrace, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Удаление выделенных объекта/ов
        /// </summary>
        private void DeleteObject()
        {
            try
            {
                ListView lvActive = this.GetActiveListView();
                if (lvActive != null)
                {
                    string tableName = lvActive.Name.Substring(2);
                    foreach (ListViewItem item in lvActive.SelectedItems)
                    {
                        DataRow dr = item.Tag as DataRow;
                        if (dr != null)
                        {
                            this.DeletePoster(dr);
                            dr.Delete();
                        }
                    }

                    this.UpdateData(tableName);
                    this.SetControlsState(lvActive);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n" + e.StackTrace, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Удаление постера удаляемого объекта
        /// </summary>
        /// <param name="dr">Удаляемая строка</param>
        private void DeletePoster(DataRow dr)
        {
            if (dr.Table.TableName == "Movies" || dr.Table.TableName == "Cinema")
            {
                try
                {
                    if (dr["image"] != null && dr["image"].ToString() != "")
                    {
                        string imagePath = ConfigurationManager.AppSettings["image_path"];
                        if (File.Exists(Path.Combine(imagePath, dr["image"].ToString())))
                        {
                            File.Delete(Path.Combine(imagePath, dr["image"].ToString()));
                        }
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Получить экземпляр диалогового окна для добавдения/просмотра/редактирования объекта
        /// </summary>
        /// <param name="mode">Режим (добавление, редактирование, удаление)</param>
        /// <returns>Ссылка на диалоговое окно</returns>
        private DialogWindow GetDialogWindow(FormMode mode)
        {
            if (this.tcAdmin.SelectedIndex == -1) { return null; }

            TabPage currentTabPage = this.tcAdmin.SelectedTab;
            string formClassName = "Cinema.frm" + currentTabPage.Name.Substring(2);

            //Получаем тип диалогового окна

            Type formType = Type.GetType(formClassName);
            if (formType == null) { throw new Exception("Не найден класс диалогового окна: " + formClassName); }

            //Получаем конструктор (диалоговое окно для создания объекта)
            
            List<Type> types = new List<Type>();
            types.Add(typeof(DbHelper));
            if (mode == FormMode.EDIT || mode == FormMode.VIEW) 
            { 
                types.Add(typeof(DataRow));
                types.Add(typeof(FormMode));
            }
            ConstructorInfo formCtor = formType.GetConstructor(types.ToArray());
           
            if (formCtor == null) { throw new Exception("Не найден конструктор диалогового окна"); }

            //Создаём диалоговое окно

            List<Object> param = new List<object>();
            param.Add(this.dataBase);
            if(mode == FormMode.EDIT || mode == FormMode.VIEW)
            {
                //Актвный элемент ListView

                ListView lvActive = this.GetActiveListView();

                //Выделенный элемент

                if (lvActive.SelectedItems.Count != 1) { throw new Exception("Неправильное количество выделенных элементов"); }

                ListViewItem lviActive = lvActive.SelectedItems[0];
                DataRow dataRow = (DataRow)lviActive.Tag;
                param.Add(dataRow);
                param.Add(mode);
            }
            return (DialogWindow)formCtor.Invoke(param.ToArray());
        }

        /// <summary>
        /// Получить ссылку на активный элемент ListView
        /// </summary>
        /// <returns>Активный ListView</returns>
        private ListView GetActiveListView()
        {
            string tableName = this.tcAdmin.SelectedTab.Name.Substring(2);
            Control[] control = this.Controls.Find("lv" + tableName, true);
            if (control.Length != 1) { throw new ApplicationException("Элемент ListView не найден, или найдено более одного элемента"); }
            
            return control[0] as ListView;
        }

        /// <summary>
        /// Установить состояние элементов управления (вкл/выкл)
        /// </summary>
        /// <param name="lvActive">Активный контрол ListView</param>
        private void SetControlsState(ListView lvActive)
        {
            //Ничего не выбрано

            if (lvActive.SelectedItems.Count == 0)
            {
                //Панель инструментов

                this.tsBtnView.Enabled = false;
                this.tsBtnEdit.Enabled = false;
                this.tsBtnDelete.Enabled = false;

                //Контекстное меню

                this.cmnAdd.Enabled = true;
                this.cmnEdit.Enabled = false;
                this.cmnDelete.Enabled = false;

                //Меню

                this.mnEdit.Enabled = false;
                this.mnDelete.Enabled = false;
            }

            //Выбран один элемент

            else if (lvActive.SelectedItems.Count == 1)
            {
                //Панель инструментов

                this.tsBtnView.Enabled = true;
                this.tsBtnEdit.Enabled = true;
                this.tsBtnDelete.Enabled = true;

                //Контекстное меню

                this.cmnAdd.Enabled = true;
                this.cmnEdit.Enabled = true;
                this.cmnDelete.Enabled = true;

                //Меню

                this.mnEdit.Enabled = true;
                this.mnDelete.Enabled = true;
            }

            //Выбрано несколько элементов

            else if (lvActive.SelectedItems.Count > 1)
            {
                //Панель инструментов

                this.tsBtnView.Enabled = false;
                this.tsBtnEdit.Enabled = false;
                this.tsBtnDelete.Enabled = true;

                //Контекстное меню

                this.cmnAdd.Enabled = true;
                this.cmnEdit.Enabled = false;
                this.cmnDelete.Enabled = true;

                //Меню

                this.mnEdit.Enabled = false;
                this.mnDelete.Enabled = true;
            }
        }

        /// <summary>
        /// Вернуть название объекта представленного в виде значения ключа
        /// </summary>
        /// <param name="dr">Строка описывающая сущность</param>
        /// <param name="columnIndex">Индекс столбца в строке, значение в котором нужно вернуть в виде текста</param>
        /// <returns>Текстовое представление столбца из заданной строки</returns>
        private string GetColumnText(DataRow dr, int columnIndex)
        {
            DataTable table = dr.Table;
            switch (table.TableName)
            {
                case "Movies":
                    if (table.Columns[columnIndex].ColumnName == "genre_id")
                    {
                        DataRow genre = dr.GetParentRow("MovieGenre");
                        return genre["name"].ToString();
                    }
                    break;
                case "Sessions":
                    if (table.Columns[columnIndex].ColumnName == "movie_id")
                    {
                        DataRow movie = dr.GetParentRow("SessionMovie");
                        return movie["name"].ToString();
                    }
                    else if (table.Columns[columnIndex].ColumnName == "cinema_id")
                    {
                        DataRow cinema = dr.GetParentRow("SessionCinema");
                        return cinema["name"].ToString();
                    }
                    else if (table.Columns[columnIndex].ColumnName == "beginning")
                    {
                        DateTime begin = this.dataBase.GetDateFromSecond((int)dr["beginning"]);
                        return begin.ToLongDateString() + " " + begin.ToShortTimeString();
                    }
                    break;
                case "Tickets":
                    if (table.Columns[columnIndex].ColumnName == "session_id")
                    {
                        DataRow session = dr.GetParentRow("TicketSession");
                        DataRow movie = session.GetParentRow("SessionMovie");
                        DataRow cinema = session.GetParentRow("SessionCinema");
                        DateTime time = this.dataBase.GetDateFromSecond((int)session["beginning"]);
                        string column = String.Format("{0} ({1}) [{2}]", movie["name"].ToString(), cinema["name"].ToString(), time.ToShortDateString() + " " + time.ToShortTimeString());
                        return column;
                    }
                    break;
                default:
                    break;
            }
            return dr[columnIndex].ToString();
        }

        #endregion
    }
}