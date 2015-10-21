using System;
using System.Collections.Generic;
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
    public enum FormMode { VIEW, NEW, EDIT };

    /// <summary>
    /// Базовый класс для далоговых окон добавления/редактирования/просмотра сущностей БД
    /// </summary>
    public class DialogWindow : Form
    {
        protected FormMode Mode { get; set; }       //Режим диалогового окна
        protected DbHelper dataBase;                //Ссылка на класс, инкапсулирующий работу с БД
        protected DataRow currentDataRow;           //Ссылка на просматриваемую/редактироваемую запись в БД
        protected ErrorProvider errorProvider;

        private DialogWindow()
        {
            this.errorProvider = new ErrorProvider();
        }

        public DialogWindow(DbHelper db) : this()
        {
            this.dataBase = db;
            this.Mode = FormMode.NEW;
        }

        public DialogWindow(DbHelper db, DataRow dr, FormMode mode) : this(db)
        {
            this.currentDataRow = dr;
            this.Mode = mode;
        }

        /// <summary>
        /// Получить объект Image по имени файла в хранилище файлов
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <returns>Объект Image накапуслирующий файл</returns>
        protected Image GetImage(string fileName)
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
        /// Проверка валидности введённых данных
        /// </summary>
        /// <returns>true - валидны</returns>
        virtual protected bool IsValidData() 
        {
            throw new NotImplementedException("Необходимо переопределить метод");
        }

        /// <summary>
        /// Заполнить элементы управления данными
        /// </summary>
        virtual protected void FillControls()
        {
            throw new NotImplementedException("Необходимо переопределить метод");
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DialogWindow
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "DialogWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }
    }
}