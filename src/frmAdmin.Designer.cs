namespace Cinema
{
    partial class frmAdmin
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdmin));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnOperations = new System.Windows.Forms.ToolStripMenuItem();
            this.mnAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.mnAddGenre = new System.Windows.Forms.ToolStripMenuItem();
            this.mnAddMovie = new System.Windows.Forms.ToolStripMenuItem();
            this.mnAddCinema = new System.Windows.Forms.ToolStripMenuItem();
            this.mnAddSession = new System.Windows.Forms.ToolStripMenuItem();
            this.mnAddTicket = new System.Windows.Forms.ToolStripMenuItem();
            this.mnEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.mnClear = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tcAdmin = new System.Windows.Forms.TabControl();
            this.tpGenres = new System.Windows.Forms.TabPage();
            this.lvGenres = new System.Windows.Forms.ListView();
            this.cmnLvMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmnAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tpMovies = new System.Windows.Forms.TabPage();
            this.lvMovies = new System.Windows.Forms.ListView();
            this.tpCinema = new System.Windows.Forms.TabPage();
            this.lvCinema = new System.Windows.Forms.ListView();
            this.tpSessions = new System.Windows.Forms.TabPage();
            this.lvSessions = new System.Windows.Forms.ListView();
            this.tpTickets = new System.Windows.Forms.TabPage();
            this.lvTickets = new System.Windows.Forms.ListView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsBtnView = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsBtnAdd = new System.Windows.Forms.ToolStripButton();
            this.tsBtnEdit = new System.Windows.Forms.ToolStripButton();
            this.tsBtnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1.SuspendLayout();
            this.tcAdmin.SuspendLayout();
            this.tpGenres.SuspendLayout();
            this.cmnLvMenu.SuspendLayout();
            this.tpMovies.SuspendLayout();
            this.tpCinema.SuspendLayout();
            this.tpSessions.SuspendLayout();
            this.tpTickets.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnOperations,
            this.mnHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(729, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnOperations
            // 
            this.mnOperations.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnAdd,
            this.mnEdit,
            this.mnDelete,
            this.toolStripMenuItem1,
            this.toolStripSeparator3,
            this.mnExit});
            this.mnOperations.Name = "mnOperations";
            this.mnOperations.Size = new System.Drawing.Size(75, 20);
            this.mnOperations.Text = "&Операции";
            this.mnOperations.DropDownOpening += new System.EventHandler(this.mnOperations_DropDownOpening);
            // 
            // mnAdd
            // 
            this.mnAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnAddGenre,
            this.mnAddMovie,
            this.mnAddCinema,
            this.mnAddSession,
            this.mnAddTicket});
            this.mnAdd.Name = "mnAdd";
            this.mnAdd.Size = new System.Drawing.Size(163, 22);
            this.mnAdd.Text = "Добавить";
            // 
            // mnAddGenre
            // 
            this.mnAddGenre.Name = "mnAddGenre";
            this.mnAddGenre.Size = new System.Drawing.Size(152, 22);
            this.mnAddGenre.Text = "Жанр...";
            this.mnAddGenre.Click += new System.EventHandler(this.mnAddGenre_Click);
            // 
            // mnAddMovie
            // 
            this.mnAddMovie.Name = "mnAddMovie";
            this.mnAddMovie.Size = new System.Drawing.Size(152, 22);
            this.mnAddMovie.Text = "Фильм...";
            this.mnAddMovie.Click += new System.EventHandler(this.mnAddMovie_Click);
            // 
            // mnAddCinema
            // 
            this.mnAddCinema.Name = "mnAddCinema";
            this.mnAddCinema.Size = new System.Drawing.Size(152, 22);
            this.mnAddCinema.Text = "Кинотеатр...";
            this.mnAddCinema.Click += new System.EventHandler(this.mnAddCinema_Click);
            // 
            // mnAddSession
            // 
            this.mnAddSession.Name = "mnAddSession";
            this.mnAddSession.Size = new System.Drawing.Size(152, 22);
            this.mnAddSession.Text = "Сеанс...";
            this.mnAddSession.Click += new System.EventHandler(this.mnAddSession_Click);
            // 
            // mnAddTicket
            // 
            this.mnAddTicket.Name = "mnAddTicket";
            this.mnAddTicket.Size = new System.Drawing.Size(152, 22);
            this.mnAddTicket.Text = "Билет...";
            this.mnAddTicket.Click += new System.EventHandler(this.mnAddTicket_Click);
            // 
            // mnEdit
            // 
            this.mnEdit.Name = "mnEdit";
            this.mnEdit.Size = new System.Drawing.Size(163, 22);
            this.mnEdit.Text = "Редактировать...";
            this.mnEdit.Click += new System.EventHandler(this.mnEdit_Click);
            // 
            // mnDelete
            // 
            this.mnDelete.Name = "mnDelete";
            this.mnDelete.Size = new System.Drawing.Size(163, 22);
            this.mnDelete.Text = "Удалить";
            this.mnDelete.Click += new System.EventHandler(this.mnDelete_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnRefresh,
            this.mnClear});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(163, 22);
            this.toolStripMenuItem1.Text = "Дополнительно";
            // 
            // mnRefresh
            // 
            this.mnRefresh.Name = "mnRefresh";
            this.mnRefresh.Size = new System.Drawing.Size(173, 22);
            this.mnRefresh.Text = "Обновить из базы";
            this.mnRefresh.ToolTipText = "Обновить данные из базы данных";
            this.mnRefresh.Click += new System.EventHandler(this.mnRefresh_Click);
            // 
            // mnClear
            // 
            this.mnClear.Name = "mnClear";
            this.mnClear.Size = new System.Drawing.Size(173, 22);
            this.mnClear.Text = "Очистить";
            this.mnClear.ToolTipText = "Удалить из базы прошедше сеансы";
            this.mnClear.Click += new System.EventHandler(this.mnClear_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(160, 6);
            // 
            // mnExit
            // 
            this.mnExit.Name = "mnExit";
            this.mnExit.Size = new System.Drawing.Size(163, 22);
            this.mnExit.Text = "Выход";
            this.mnExit.Click += new System.EventHandler(this.mnExit_Click);
            // 
            // mnHelp
            // 
            this.mnHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnAbout});
            this.mnHelp.Name = "mnHelp";
            this.mnHelp.Size = new System.Drawing.Size(65, 20);
            this.mnHelp.Text = "&Справка";
            // 
            // mnAbout
            // 
            this.mnAbout.Name = "mnAbout";
            this.mnAbout.Size = new System.Drawing.Size(149, 22);
            this.mnAbout.Text = "&О программе";
            this.mnAbout.Click += new System.EventHandler(this.mnAbout_Click);
            // 
            // tcAdmin
            // 
            this.tcAdmin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcAdmin.Controls.Add(this.tpGenres);
            this.tcAdmin.Controls.Add(this.tpMovies);
            this.tcAdmin.Controls.Add(this.tpCinema);
            this.tcAdmin.Controls.Add(this.tpSessions);
            this.tcAdmin.Controls.Add(this.tpTickets);
            this.tcAdmin.Location = new System.Drawing.Point(13, 74);
            this.tcAdmin.Name = "tcAdmin";
            this.tcAdmin.SelectedIndex = 0;
            this.tcAdmin.Size = new System.Drawing.Size(704, 455);
            this.tcAdmin.TabIndex = 1;
            this.tcAdmin.SelectedIndexChanged += new System.EventHandler(this.tcAdmin_SelectedIndexChanged);
            // 
            // tpGenres
            // 
            this.tpGenres.Controls.Add(this.lvGenres);
            this.tpGenres.Location = new System.Drawing.Point(4, 22);
            this.tpGenres.Name = "tpGenres";
            this.tpGenres.Padding = new System.Windows.Forms.Padding(3);
            this.tpGenres.Size = new System.Drawing.Size(696, 429);
            this.tpGenres.TabIndex = 0;
            this.tpGenres.Text = "Жанры";
            this.tpGenres.UseVisualStyleBackColor = true;
            // 
            // lvGenres
            // 
            this.lvGenres.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvGenres.ContextMenuStrip = this.cmnLvMenu;
            this.lvGenres.FullRowSelect = true;
            this.lvGenres.GridLines = true;
            this.lvGenres.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvGenres.Location = new System.Drawing.Point(7, 7);
            this.lvGenres.Name = "lvGenres";
            this.lvGenres.Size = new System.Drawing.Size(683, 416);
            this.lvGenres.TabIndex = 0;
            this.lvGenres.UseCompatibleStateImageBehavior = false;
            this.lvGenres.View = System.Windows.Forms.View.Details;
            this.lvGenres.SelectedIndexChanged += new System.EventHandler(this.lv_SelectedIndexChanged);
            this.lvGenres.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lv_KeyUp);
            this.lvGenres.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lv_MouseDoubleClick);
            // 
            // cmnLvMenu
            // 
            this.cmnLvMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnAdd,
            this.cmnEdit,
            this.cmnDelete});
            this.cmnLvMenu.Name = "cmnLvMenu";
            this.cmnLvMenu.Size = new System.Drawing.Size(164, 70);
            this.cmnLvMenu.Opening += new System.ComponentModel.CancelEventHandler(this.cmnLvMenu_Opening);
            // 
            // cmnAdd
            // 
            this.cmnAdd.Name = "cmnAdd";
            this.cmnAdd.Size = new System.Drawing.Size(163, 22);
            this.cmnAdd.Text = "Добавить...";
            this.cmnAdd.Click += new System.EventHandler(this.cmnAdd_Click);
            // 
            // cmnEdit
            // 
            this.cmnEdit.Name = "cmnEdit";
            this.cmnEdit.Size = new System.Drawing.Size(163, 22);
            this.cmnEdit.Text = "Редактировать...";
            this.cmnEdit.Click += new System.EventHandler(this.cmnEdit_Click);
            // 
            // cmnDelete
            // 
            this.cmnDelete.Name = "cmnDelete";
            this.cmnDelete.Size = new System.Drawing.Size(163, 22);
            this.cmnDelete.Text = "Удалить";
            this.cmnDelete.Click += new System.EventHandler(this.cmnDelete_Click);
            // 
            // tpMovies
            // 
            this.tpMovies.Controls.Add(this.lvMovies);
            this.tpMovies.Location = new System.Drawing.Point(4, 22);
            this.tpMovies.Name = "tpMovies";
            this.tpMovies.Padding = new System.Windows.Forms.Padding(3);
            this.tpMovies.Size = new System.Drawing.Size(696, 429);
            this.tpMovies.TabIndex = 1;
            this.tpMovies.Text = "Фильмы";
            this.tpMovies.UseVisualStyleBackColor = true;
            // 
            // lvMovies
            // 
            this.lvMovies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvMovies.ContextMenuStrip = this.cmnLvMenu;
            this.lvMovies.FullRowSelect = true;
            this.lvMovies.GridLines = true;
            this.lvMovies.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvMovies.Location = new System.Drawing.Point(7, 6);
            this.lvMovies.Name = "lvMovies";
            this.lvMovies.Size = new System.Drawing.Size(838, 438);
            this.lvMovies.TabIndex = 1;
            this.lvMovies.UseCompatibleStateImageBehavior = false;
            this.lvMovies.View = System.Windows.Forms.View.Details;
            this.lvMovies.SelectedIndexChanged += new System.EventHandler(this.lv_SelectedIndexChanged);
            this.lvMovies.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lv_KeyUp);
            this.lvMovies.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lv_MouseDoubleClick);
            // 
            // tpCinema
            // 
            this.tpCinema.Controls.Add(this.lvCinema);
            this.tpCinema.Location = new System.Drawing.Point(4, 22);
            this.tpCinema.Name = "tpCinema";
            this.tpCinema.Padding = new System.Windows.Forms.Padding(3);
            this.tpCinema.Size = new System.Drawing.Size(696, 429);
            this.tpCinema.TabIndex = 2;
            this.tpCinema.Text = "Кинотеатры";
            this.tpCinema.UseVisualStyleBackColor = true;
            // 
            // lvCinema
            // 
            this.lvCinema.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvCinema.ContextMenuStrip = this.cmnLvMenu;
            this.lvCinema.FullRowSelect = true;
            this.lvCinema.GridLines = true;
            this.lvCinema.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvCinema.Location = new System.Drawing.Point(7, 6);
            this.lvCinema.Name = "lvCinema";
            this.lvCinema.Size = new System.Drawing.Size(838, 438);
            this.lvCinema.TabIndex = 1;
            this.lvCinema.UseCompatibleStateImageBehavior = false;
            this.lvCinema.View = System.Windows.Forms.View.Details;
            this.lvCinema.SelectedIndexChanged += new System.EventHandler(this.lv_SelectedIndexChanged);
            this.lvCinema.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lv_KeyUp);
            this.lvCinema.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lv_MouseDoubleClick);
            // 
            // tpSessions
            // 
            this.tpSessions.Controls.Add(this.lvSessions);
            this.tpSessions.Location = new System.Drawing.Point(4, 22);
            this.tpSessions.Name = "tpSessions";
            this.tpSessions.Padding = new System.Windows.Forms.Padding(3);
            this.tpSessions.Size = new System.Drawing.Size(696, 429);
            this.tpSessions.TabIndex = 3;
            this.tpSessions.Text = "Сеансы";
            this.tpSessions.UseVisualStyleBackColor = true;
            // 
            // lvSessions
            // 
            this.lvSessions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvSessions.ContextMenuStrip = this.cmnLvMenu;
            this.lvSessions.FullRowSelect = true;
            this.lvSessions.GridLines = true;
            this.lvSessions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvSessions.Location = new System.Drawing.Point(7, 6);
            this.lvSessions.Name = "lvSessions";
            this.lvSessions.Size = new System.Drawing.Size(838, 438);
            this.lvSessions.TabIndex = 1;
            this.lvSessions.UseCompatibleStateImageBehavior = false;
            this.lvSessions.View = System.Windows.Forms.View.Details;
            this.lvSessions.SelectedIndexChanged += new System.EventHandler(this.lv_SelectedIndexChanged);
            this.lvSessions.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lv_KeyUp);
            this.lvSessions.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lv_MouseDoubleClick);
            // 
            // tpTickets
            // 
            this.tpTickets.Controls.Add(this.lvTickets);
            this.tpTickets.Location = new System.Drawing.Point(4, 22);
            this.tpTickets.Name = "tpTickets";
            this.tpTickets.Padding = new System.Windows.Forms.Padding(3);
            this.tpTickets.Size = new System.Drawing.Size(696, 429);
            this.tpTickets.TabIndex = 4;
            this.tpTickets.Text = "Билеты";
            this.tpTickets.UseVisualStyleBackColor = true;
            // 
            // lvTickets
            // 
            this.lvTickets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvTickets.ContextMenuStrip = this.cmnLvMenu;
            this.lvTickets.FullRowSelect = true;
            this.lvTickets.GridLines = true;
            this.lvTickets.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvTickets.Location = new System.Drawing.Point(7, 6);
            this.lvTickets.Name = "lvTickets";
            this.lvTickets.Size = new System.Drawing.Size(838, 438);
            this.lvTickets.TabIndex = 1;
            this.lvTickets.UseCompatibleStateImageBehavior = false;
            this.lvTickets.View = System.Windows.Forms.View.Details;
            this.lvTickets.SelectedIndexChanged += new System.EventHandler(this.lv_SelectedIndexChanged);
            this.lvTickets.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lv_KeyUp);
            this.lvTickets.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lv_MouseDoubleClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsBtnView,
            this.toolStripSeparator2,
            this.tsBtnAdd,
            this.tsBtnEdit,
            this.tsBtnDelete});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(729, 47);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsBtnView
            // 
            this.tsBtnView.AutoSize = false;
            this.tsBtnView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnView.Enabled = false;
            this.tsBtnView.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnView.Image")));
            this.tsBtnView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnView.Name = "tsBtnView";
            this.tsBtnView.Size = new System.Drawing.Size(44, 44);
            this.tsBtnView.Text = "Просмотр";
            this.tsBtnView.Click += new System.EventHandler(this.tsBtnView_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 47);
            // 
            // tsBtnAdd
            // 
            this.tsBtnAdd.AutoSize = false;
            this.tsBtnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnAdd.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnAdd.Image")));
            this.tsBtnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnAdd.Name = "tsBtnAdd";
            this.tsBtnAdd.Size = new System.Drawing.Size(44, 44);
            this.tsBtnAdd.Text = "Добавить";
            this.tsBtnAdd.Click += new System.EventHandler(this.tsBtnAdd_Click);
            // 
            // tsBtnEdit
            // 
            this.tsBtnEdit.AutoSize = false;
            this.tsBtnEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnEdit.Enabled = false;
            this.tsBtnEdit.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnEdit.Image")));
            this.tsBtnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnEdit.Name = "tsBtnEdit";
            this.tsBtnEdit.Size = new System.Drawing.Size(44, 44);
            this.tsBtnEdit.Text = "Редактировать";
            this.tsBtnEdit.Click += new System.EventHandler(this.tsBtnEdit_Click);
            // 
            // tsBtnDelete
            // 
            this.tsBtnDelete.AutoSize = false;
            this.tsBtnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnDelete.Enabled = false;
            this.tsBtnDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnDelete.Image")));
            this.tsBtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnDelete.Name = "tsBtnDelete";
            this.tsBtnDelete.Size = new System.Drawing.Size(44, 44);
            this.tsBtnDelete.Text = "Удалить";
            this.tsBtnDelete.Click += new System.EventHandler(this.tsBtnDelete_Click);
            // 
            // frmAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 541);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tcAdmin);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "frmAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Управление";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tcAdmin.ResumeLayout(false);
            this.tpGenres.ResumeLayout(false);
            this.cmnLvMenu.ResumeLayout(false);
            this.tpMovies.ResumeLayout(false);
            this.tpCinema.ResumeLayout(false);
            this.tpSessions.ResumeLayout(false);
            this.tpTickets.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnOperations;
        private System.Windows.Forms.ToolStripMenuItem mnHelp;
        private System.Windows.Forms.ToolStripMenuItem mnAbout;
        private System.Windows.Forms.TabControl tcAdmin;
        private System.Windows.Forms.TabPage tpGenres;
        private System.Windows.Forms.TabPage tpMovies;
        private System.Windows.Forms.TabPage tpCinema;
        private System.Windows.Forms.TabPage tpSessions;
        private System.Windows.Forms.TabPage tpTickets;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsBtnAdd;
        private System.Windows.Forms.ToolStripButton tsBtnEdit;
        private System.Windows.Forms.ToolStripButton tsBtnDelete;
        private System.Windows.Forms.ToolStripMenuItem mnExit;
        private System.Windows.Forms.ListView lvGenres;
        private System.Windows.Forms.ContextMenuStrip cmnLvMenu;
        private System.Windows.Forms.ToolStripMenuItem cmnAdd;
        private System.Windows.Forms.ToolStripMenuItem cmnEdit;
        private System.Windows.Forms.ToolStripMenuItem cmnDelete;
        private System.Windows.Forms.ListView lvMovies;
        private System.Windows.Forms.ListView lvCinema;
        private System.Windows.Forms.ListView lvSessions;
        private System.Windows.Forms.ListView lvTickets;
        private System.Windows.Forms.ToolStripButton tsBtnView;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem mnAdd;
        private System.Windows.Forms.ToolStripMenuItem mnEdit;
        private System.Windows.Forms.ToolStripMenuItem mnDelete;
        private System.Windows.Forms.ToolStripMenuItem mnAddGenre;
        private System.Windows.Forms.ToolStripMenuItem mnAddMovie;
        private System.Windows.Forms.ToolStripMenuItem mnAddCinema;
        private System.Windows.Forms.ToolStripMenuItem mnAddSession;
        private System.Windows.Forms.ToolStripMenuItem mnAddTicket;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnRefresh;
        private System.Windows.Forms.ToolStripMenuItem mnClear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}

