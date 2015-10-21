using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Cinema
{
    enum PLACE_STATE { FREE, BUSY, RESERVED };

    public partial class Placer : UserControl
    {
        public int Rows { get; set; }                   //Рядов в зале
        public int Places { get; set; }                 //Мест в ряде
        public List<Point> SoldPlaces { get; set; }     //Занятые места
        public List<Point> ReservedPlaces               //Забронированные места
        {
            get 
            {
                List<Point> list = new List<Point>();
                foreach (CinemaPlace item in reservedList)
                {
                    list.Add(new Point(item.Row + 1, item.Place + 1));
                }
                return list;
            }
        } 
        
        //Двойная буферизация

        private BufferedGraphicsContext context;
        private BufferedGraphics buffer;

        private List<CinemaPlace> placesList = new List<CinemaPlace>();     //Список мест
        private List<CinemaPlace> reservedList = new List<CinemaPlace>();   //Список зарезервированных мест
        private Point basePoint;                                            //Точка, с которой начинаются места в зале
        private float delta;                                                //Зазор между местами в зале

        private Placer()
        {
            InitializeComponent();

            Paint += Placer_Paint;
            Resize += Placer_Resize;
            MouseClick += Placer_MouseClick;
            
            DoubleBuffered = true;
            context = BufferedGraphicsManager.Current;
            buffer = context.Allocate(CreateGraphics(), new Rectangle(0, 0, Width, Height));
        }

        public Placer(int rows, int places, List<Point> soldPlaces) : this()
        {
            if (rows <= 0 || rows > 40 || places <= 0 || places > 40) 
            {
                throw new ApplicationException("Неправильно заданы размеры кинозала");
            }

            this.Rows = rows;
            this.Places = places;
            this.basePoint = new Point(Width / (Places + 1), 0);
            this.delta = 4.0f;
            this.SoldPlaces = soldPlaces;
            
            //Создание списка мест

            float placeWidth = (float)(Width - basePoint.X) / Places;
            float placeHeight = (float)(Height - basePoint.Y) / Rows;
            for (int row = 0; row < Rows; row++)
            {
                for (int place = 0; place < Places; place++)
                {
                    RectangleF R = new RectangleF(
                        basePoint.X + place * placeWidth + delta,
                        basePoint.Y + row * placeHeight + delta,
                        placeWidth - delta,
                        placeHeight - delta);
                    PLACE_STATE state = SoldPlaces.Contains(new Point(row + 1, place + 1)) ? PLACE_STATE.BUSY : PLACE_STATE.FREE;
                    placesList.Add(new CinemaPlace(row, place, state, R));
                }
            }
        }

        private void Placer_Resize(object sender, EventArgs e)
        {
            //Пересоздать буфер (новый размер)

            buffer.Dispose();
            buffer = context.Allocate(CreateGraphics(), new Rectangle(0, 0, this.Width + 1, this.Height + 1));

            //Пересчёт размеров и позиции мест

            this.basePoint = new Point(Width / (Places + 1), 0);
            foreach (var item in placesList)
            {
                float placeWidth = (float)(Width - basePoint.X) / Places;
                float placeHeight = (float)(Height - basePoint.Y) / Rows;
                item.PlaceRect = new RectangleF(
                    basePoint.X + item.Place * placeWidth + delta,
                    basePoint.Y + item.Row * placeHeight + delta,
                    placeWidth - delta,
                    placeHeight - delta);
            }
            
            Invalidate();
        }

        private void Placer_Paint(object sender, PaintEventArgs e)
        {
            //Обновляется место

            if (e.ClipRectangle.Width != Width || e.ClipRectangle.Height != Height)
            {
                buffer.Render(e.Graphics);
                return;
            }

            //Обновляется зал

            Graphics graphics = buffer.Graphics;
            graphics.Clear(BackColor);

            //Отрисовка мест

            foreach (CinemaPlace item in placesList)
            {
                item.DrawPlace(graphics);
            }

            //Подписи к рядам

            float rowCellWidth = placesList[0].PlaceRect.Width;
            float rowCellHeight = placesList[0].PlaceRect.Height + delta;

            for (int row = 0; row < Rows; row++)
            {
                string rowStr = String.Format("Ряд {0}", row + 1);
                float em = 0.5f * rowCellHeight * 72 / graphics.DpiX;
                Font font = new Font(FontFamily.GenericSansSerif, em > 2.0f ? em : 2);
                SizeF textSize = graphics.MeasureString(rowStr, font);
                PointF centerRowCell = new PointF(rowCellWidth / 2.0f, (0.5f + row) * rowCellHeight);
                PointF point = new PointF(centerRowCell.X - textSize.Width / 2, centerRowCell.Y - textSize.Height / 2);
                graphics.DrawString(rowStr, font, Brushes.Black, point);
            }

            buffer.Render(e.Graphics);
        }

        private void Placer_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (CinemaPlace item in placesList)
            {
                if (item.State == PLACE_STATE.BUSY) { continue; }
                if (item.PlaceRect.Contains(e.Location))
                {
                    switch (item.State)
                    {
                        case PLACE_STATE.FREE:
                            item.State = PLACE_STATE.RESERVED;
                            reservedList.Add(item);
                            break;
                        case PLACE_STATE.RESERVED:
                            item.State = PLACE_STATE.FREE;
                            reservedList.Remove(item);
                            break;
                        default:
                            break;
                    }
                    item.DrawPlace(buffer.Graphics);
                    Invalidate(new Rectangle((int)item.PlaceRect.X, (int)item.PlaceRect.Y, (int)item.PlaceRect.Width, (int)item.PlaceRect.Height));
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Класс описывает и визуализирует место в кинотеатре
    /// </summary>
    class CinemaPlace
    {
        public int Row { get; set; }                //Ряд
        public int Place { get; set; }              //Место
        public PLACE_STATE State { get; set; }      //Состояние
        public RectangleF PlaceRect { get; set; }   //Занимаемый прямоугольник на элементе управления

        public CinemaPlace(int row, int place, PLACE_STATE state, RectangleF rect)
        {
            Row = row;
            Place = place;
            State = state;
            PlaceRect = rect;
        }

        public void DrawPlace(Graphics g)
        {
            //Инструменты

            Brush brush = Brushes.Red;
            Pen pen = new Pen(Brushes.Black, 2.0f);
            float em = 0.75f * PlaceRect.Height * 72 / g.DpiX;
            Font font = new Font(FontFamily.GenericSerif, em > 2 ? em : 2) ;

            switch (State)
            {
                case PLACE_STATE.FREE:
                    brush = Brushes.White;
                    break;
                case PLACE_STATE.BUSY:
                    brush = Brushes.Black;
                    break;
                case PLACE_STATE.RESERVED:
                    brush = Brushes.Green;
                    break;
                default:
                    break;
            }
            
            //Рисование

            g.FillRectangle(brush, PlaceRect);
            g.DrawRectangle(pen, PlaceRect.X, PlaceRect.Y, PlaceRect.Width - 1, PlaceRect.Height - 1);

            SizeF textSize = g.MeasureString((Place + 1).ToString(), font);
            PointF point = new PointF(PlaceRect.X + (PlaceRect.Width - textSize.Width) / 2, PlaceRect.Y + (PlaceRect.Height - textSize.Height) / 2);
            g.DrawString((Place + 1).ToString(), font, Brushes.Black, point);
        }

        public override bool Equals(object obj)
        {
            CinemaPlace cp = obj as CinemaPlace;
            if (cp != null)
            {
                return Row == cp.Row && Place == cp.Place && State == cp.State;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Row.GetHashCode() ^ Place.GetHashCode() ^ State.GetHashCode() ^ PlaceRect.GetHashCode();
        }
    }
}