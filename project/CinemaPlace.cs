using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public class CinemaPlace
    {
        public Point Position;

        public CinemaPlace(Point p)
        {
            this.Position = p;
        }

        public override string ToString()
        {
            return String.Format("Ряд: {0} Место: {1}", this.Position.X, this.Position.Y);
        }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null)) { return false; }
            CinemaPlace cp = obj as CinemaPlace;
            if (cp == null) { return false; }
           
            return Position.X == cp.Position.X && Position.Y == cp.Position.Y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
