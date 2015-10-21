using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cinema
{
    public class DbException : ApplicationException
    {
        public DbException(string message) : base(message) { }
    }

    public class DbHelper
    {
        private string[] tableNames = { "Genres", "Movies", "Cinema", "Sessions", "Tickets" };

        public string Provider 
        {
            get
            {
                return this.provider;
            }
        }
        private string provider;

        public string ConnectionString 
        {
            get
            {
                return connectionString;
            }
        }
        private string connectionString;

        private DbProviderFactory dbFactory;
        private DbConnection dbConnection;
        private DbDataAdapter dbAdapter;

        private DataSet dbSet;
        public DataSet DataSet 
        {
            get
            {
                return this.dbSet;
            }
        }
        public DataTableCollection Tables
        {
            get
            {
                return this.dbSet.Tables;
            }
        }
        public DataTable this[string tableName]
        {
            get
            {
                return this.dbSet.Tables[tableName];
            }
        }

        public DateTime BaseDate { get; set; }
       
        public DbHelper(string provider, string connStr)
        {
            if (provider == null || connStr == null) { throw new ArgumentNullException(); }
            
            this.BaseDate = new DateTime(2010, 1, 1, 0, 0, 0);

            this.provider = provider;
            this.connectionString = connStr;
                
            //Соединение и объекты для работы с базой 
                
            this.dbFactory = DbProviderFactories.GetFactory(this.Provider);
            this.dbConnection = this.dbFactory.CreateConnection();
            this.dbAdapter = this.dbFactory.CreateDataAdapter();
            this.dbConnection.ConnectionString = this.ConnectionString;

            this.dbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            //Кешируем базу

            this.ReadDataBase();
        }

        #region Работа с СУБД
        
        /// <summary>
        /// Cинхронизировать DataSet с БД
        /// </summary>
        public void UpdateDataSetFromDb()
        {
            this.ReadDataBase();    //(!!!) Из-за неправильного добавление записей в DataTable DataAdapter ом с включённым MissingSchemaAction.AddWithKey;
        }

        /// <summary>
        /// Синхронизировать базу с DataSet
        /// </summary>
        public void UpdateDbFromDataSet()
        {
            for (int i = 0; i < dbSet.Tables.Count; i++)
            {
                this.UpdateDbFromTable(this.dbSet.Tables[i].TableName);
            }
        }

        /// <summary>
        /// Синхронизировать БД с таблицей
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        public void UpdateDbFromTable(string tableName)
        {
            this.dbAdapter.InsertCommand = this.CreateInsertCommand(this.dbSet.Tables[tableName]);
            this.dbAdapter.UpdateCommand = this.CreateUpdateCommand(this.dbSet.Tables[tableName]);
            this.dbAdapter.DeleteCommand = this.CreateDeleteCommand(this.dbSet.Tables[tableName]);
            this.dbAdapter.Update(this.dbSet.Tables[tableName]);
        }

        /// <summary>
        /// Прочитать базу данных в DataSet
        /// </summary>
        private void ReadDataBase()
        {
            this.dbSet = new DataSet();
            DbCommand cmd = this.dbConnection.CreateCommand();
            for (int i = 0; i < this.tableNames.Length; i++)
            {
                cmd.CommandText = "SELECT * FROM " + tableNames[i] + " ORDER BY id";
                this.dbAdapter.SelectCommand = cmd;
                this.dbAdapter.Fill(dbSet, tableNames[i]);
            }
            this.ImposeConstraints();
        }

        #endregion

        #region Работа с данными
        
        /// <summary>
        /// Возращает ссылку на DataTable из DataSet по имени таблицы
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        /// <returns>Ссылка на таблицу</returns>
        public DataTable GetTableByName(string tableName)
        {
            if (tableName == null) throw new ArgumentNullException();
            return this.dbSet.Tables[tableName];
        }

        /// <summary>
        /// Получить имя сущности БД по id
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="id">id идентификатор (поле id)</param>
        /// <returns>Имя объекта (поле name)</returns>
        public string GetNameById(string tableName, int id)
        {
            for (int i = 0; i < this.dbSet.Tables[tableName].Rows.Count; i++)
            {
                if ((int)this.dbSet.Tables[tableName].Rows[i]["id"] == id)
                {
                    return this.dbSet.Tables[tableName].Rows[i]["name"].ToString();
                }
            }
            return null;
        }

        /// <summary>
        /// Вернуть id объекта по имени
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="name">Имя сущности (поле name)</param>
        /// <returns>Идентификатор объекта (поле id)</returns>
        public int GetIdByName(string tableName, string name)
        {
            for (int i = 0; i < this.dbSet.Tables[tableName].Rows.Count; i++)
            {
                if (this.dbSet.Tables[tableName].Rows[i]["name"].ToString() == name)
                {
                    return (int)this.dbSet.Tables[tableName].Rows[i]["id"];
                }
            }
            return -1;
        }

        /// <summary>
        /// Получить ссылку на DataRow по id объекта
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="id">id объекта в базе</param>
        /// <returns>Ссылка на DataRow</returns>
        public DataRow GetDataRowById(string tableName, int id)
        {
            foreach (DataRow item in this.dbSet.Tables[tableName].Rows)
            {
                if ((int)item["id"] == id)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// Получить DateTime
        /// </summary>
        /// <param name="seconds">Количество секунд от базовой даты</param>
        /// <returns>Структура DateTime</returns>
        public DateTime GetDateFromSecond(int seconds)
        {
            return this.BaseDate.AddSeconds(seconds);
        }

        /// <summary>
        /// Получить количество секунд относительно базовой даты
        /// </summary>
        /// <param name="dt">Дата, относительно которой считаем секунды от базовой даты</param>
        /// <returns>Количество секунд прошедших относительно базовой даты</returns>
        public int GetSecondsFromDate(DateTime dt)
        {
            TimeSpan ts = dt - this.BaseDate;
            return (int)ts.TotalSeconds;
        }

        /// <summary>
        /// Добавить ограничения в DataSet
        /// </summary>
        private void ImposeConstraints()
        {
            DataRelation relMovieGenre = new DataRelation("MovieGenre", this.dbSet.Tables["Genres"].Columns["id"], this.dbSet.Tables["Movies"].Columns["genre_id"], true);
            this.dbSet.Relations.Add(relMovieGenre);
            relMovieGenre.ChildKeyConstraint.UpdateRule = Rule.None;
            relMovieGenre.ChildKeyConstraint.DeleteRule = Rule.None;

            DataRelation relSessionCinema = new DataRelation("SessionCinema", this.dbSet.Tables["Cinema"].Columns["id"], this.dbSet.Tables["Sessions"].Columns["cinema_id"], true);
            this.dbSet.Relations.Add(relSessionCinema);
            relSessionCinema.ChildKeyConstraint.UpdateRule = Rule.None;
            relSessionCinema.ChildKeyConstraint.DeleteRule = Rule.None;

            DataRelation relSessionMovie = new DataRelation("SessionMovie", this.dbSet.Tables["Movies"].Columns["id"], this.dbSet.Tables["Sessions"].Columns["movie_id"], true);
            this.dbSet.Relations.Add(relSessionMovie);
            relSessionMovie.ChildKeyConstraint.UpdateRule = Rule.None;
            relSessionMovie.ChildKeyConstraint.DeleteRule = Rule.None;

            DataRelation relTicketSession = new DataRelation("TicketSession", this.dbSet.Tables["Sessions"].Columns["id"], this.dbSet.Tables["Tickets"].Columns["session_id"], true);
            this.dbSet.Relations.Add(relTicketSession);
            relTicketSession.ChildKeyConstraint.UpdateRule = Rule.None;
            relTicketSession.ChildKeyConstraint.DeleteRule = Rule.None;
        }

        #endregion

        #region Создание команд
        
        private DbCommand CreateInsertCommand(DataTable table)
        {
            DbCommand cmd = this.dbConnection.CreateCommand();
            
            //Имена столбцов
            
            List<string> columnNamesList = new List<string>();
            for (int i = 1; i < table.Columns.Count; i++)
            {
                columnNamesList.Add(table.Columns[i].ColumnName);
            }


            //Добавление параметров

            List<string> paramsList = new List<string>();
            for (int i = 1; i < table.Columns.Count; i++)
            {
                DbParameter param = cmd.CreateParameter();
                paramsList.Add("@" + table.Columns[i].ColumnName);
                param.ParameterName = "@" + table.Columns[i].ColumnName;
                param.DbType = this.GetDbType(table.Columns[i].DataType.Name);
                param.SourceColumn = table.Columns[i].ColumnName;
                cmd.Parameters.Add(param);
            }

            //Собирание команды

            string strCmd = "INSERT INTO " + table.TableName + "(" + String.Join(",", columnNamesList.ToArray()) + ") VALUES ";
            strCmd += "(" + String.Join(",", paramsList.ToArray()) + ")";
            cmd.CommandText = strCmd;
            
            return cmd;
        }

        private DbCommand CreateUpdateCommand(DataTable table)
        {
            DbCommand cmd = this.dbConnection.CreateCommand();

            string strCmd = String.Format("UPDATE {0} SET ", table.TableName);

            //SET часть

            string strParams = "";
            for (int i = 1; i < table.Columns.Count; i++)
            {
                strParams += table.Columns[i].ColumnName + " = " + "@" + table.Columns[i].ColumnName + ", ";
                DbParameter param = cmd.CreateParameter();
                param.ParameterName = "@" + table.Columns[i].ColumnName;
                param.DbType = this.GetDbType(table.Columns[i].DataType.Name);
                param.SourceColumn = table.Columns[i].ColumnName;
                cmd.Parameters.Add(param);
            }
            strParams = strParams.Substring(0, strParams.Length - 2);      //Удалить последнюю ", "
            strCmd += strParams;

            //WHERE часть

            strCmd += " WHERE Id = @Id";
            DbParameter sqlParamId = cmd.CreateParameter();
            sqlParamId.ParameterName = "@Id";
            sqlParamId.DbType = DbType.Int32;
            sqlParamId.SourceColumn = table.Columns["Id"].ColumnName;
            cmd.Parameters.Add(sqlParamId);
            cmd.CommandText = strCmd;

            return cmd;
        }

        private DbCommand CreateDeleteCommand(DataTable table)
        {
            DbCommand cmd = this.dbConnection.CreateCommand();

            string strCmd = String.Format("DELETE FROM {0} WHERE Id = @Id", table.TableName);
            cmd.CommandText = strCmd;
            DbParameter param = cmd.CreateParameter();
            param.ParameterName = "@Id";
            param.DbType = DbType.Int32;
            param.SourceColumn = table.Columns["Id"].ColumnName;
            cmd.Parameters.Add(param);

            return cmd;
        }

        private DbType GetDbType(string dataTypeName)
        {
            DbType sqlType;
            switch (dataTypeName)
            {
                case "Int32":
                    sqlType = DbType.Int32;
                    break;
                case "Int16":
                    sqlType = DbType.Int16;
                    break;
                case "String":
                    sqlType = DbType.String;
                    break;
                default:
                    throw new DbException("Не известный тип данных: " + dataTypeName);
            }
            return sqlType;
        }

        #endregion
    }
}