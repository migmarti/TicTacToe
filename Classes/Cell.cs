using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe.Classes {
    class Cell {
        Point startPoint;
        private Rectangle area;
        public String value { get; set; }
        public int row { get; set; }
        public int column { get; set; }
        public int index { get; set; }
        public bool highlight { get; set; }
        public bool analyzed { get; set; }

        public Cell(int row, int column, Point startPoint, int cellSizeX, int cellSizeY) {
            this.row = row;
            this.column = column;
            this.startPoint = startPoint;
            this.value = "";
            this.area = new Rectangle(startPoint.X, startPoint.Y, cellSizeX, cellSizeY);
            this.highlight = false;
        }

        public bool hasPoint(int x, int y) {
            return this.area.Contains(x, y);
        }

        public void draw(Pen pen, Graphics g) {
            int padding = 30;
            int thickness = this.area.Width / (padding/5);
            int shadowOffset = 5;

            if (highlight) {
                g.FillRectangle(new SolidBrush(Color.FromArgb(100, 255, 255, 255)), this.area);
            }
            g.DrawRectangle(pen, this.area);

            Pen xShadowPen = new Pen(Color.DarkBlue, thickness);
            Pen oShadowPen = new Pen(Color.DarkRed, thickness);

            if (value == "X") {
                Pen xPen = new Pen(Color.Blue, thickness);
                g.DrawLine(xShadowPen,
                   this.area.X + padding + shadowOffset,
                   this.area.Y + padding + shadowOffset,
                   this.area.X + this.area.Width - padding + shadowOffset,
                   this.area.Y + this.area.Height - padding + shadowOffset);
                g.DrawLine(xShadowPen,
                   this.area.X + this.area.Width - padding + shadowOffset,
                   this.area.Y + padding + shadowOffset,
                   this.area.X + padding + shadowOffset,
                   this.area.Y + this.area.Height - padding + shadowOffset);
                g.DrawLine(xPen,
                   this.area.X + padding,
                   this.area.Y + padding,
                   this.area.X + this.area.Width - padding,
                   this.area.Y + this.area.Height - padding);
                g.DrawLine(xPen,
                   this.area.X + this.area.Width - padding,
                   this.area.Y + padding,
                   this.area.X + padding,
                   this.area.Y + this.area.Height - padding);
                xPen.Dispose();
            }
            else if (value == "O") {
                Pen oPen = new Pen(Color.Red, thickness);
                g.DrawEllipse(oShadowPen,
                    this.area.X + padding + shadowOffset,
                    this.area.Y + padding + shadowOffset,
                    this.area.Width - padding * 2,
                    this.area.Height - padding * 2);
                g.DrawEllipse(oPen, 
                    this.area.X + padding,
                    this.area.Y + padding,
                    this.area.Width - padding * 2,
                    this.area.Height - padding * 2);
                oPen.Dispose();
            }
            xShadowPen.Dispose();
            oShadowPen.Dispose();
        }
    }
}
