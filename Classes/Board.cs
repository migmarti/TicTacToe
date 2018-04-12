using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe.Classes {
    class Board {

        int size, width, height, cellCount, setValues;
        Panel panel;
        Label label;
        Cell[,] cells;
        Agent agent;
        String currentValue;
        bool gameWon = false;

        public Board(Panel p, Label l, int cellDensity, Agent agent) {
            this.panel = p;
            this.label = l;
            this.size = cellDensity;
            this.cells = new Cell[size, size];
            this.width = p.Width;
            this.height = p.Height;
            this.cellCount = cellDensity * cellDensity;
            this.setValues = 0;
            this.agent = agent;
            initializeCells();
            p.Paint += new PaintEventHandler(panelPaint);
            p.Click += panelClick;
            randomizeFirst();
            refresh();
        }

        public Cell[,] getCells() {
            return this.cells;
        }

        public bool isFull() {
            return cellCount == setValues;
        }

        public void clearValues() {
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    this.cells[i, j].value = "";
                    this.cells[i, j].highlight = false;
                }
            }
            setValues = 0;
            gameWon = false;
            randomizeFirst();
            refresh();
        }

        public void putValue(int row, int column) {
            if (this.cells[row, column].value == "") {
                this.cells[row, column].value = currentValue;
                if (currentValue == "O") {
                    currentValue = "X";
                }
                else {
                    currentValue = "O";
                }
                setValues++;
                checkWin();
                refresh();
            }
        }

        private void panelPaint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black, 1);
            draw(g, pen, panel.Width, panel.Height);
            g.Dispose();
            pen.Dispose();
        }

        private void panelClick(object sender, EventArgs e) {
            Point point = panel.PointToClient(Cursor.Position);
            handleClick(point.X, point.Y);
        }

        private void randomizeFirst() {
            Random random = new Random();
            int num = random.Next(0, 2);
            if (num == 0) {
                this.currentValue = "O";
            }
            else if (num == 1) {
                this.currentValue = "X";
            }
            handleAgent();
        }

        private void handleAgent() {
            if (agent != null && currentValue == agent.value && !gameWon) {
                System.Threading.Thread.Sleep(250);
                agent.handleTurn(this);
            }
        }

        private void initializeCells() {
            int cellSizeX = this.width / this.size;
            int cellSizeY = this.height / this.size;
            this.cells = new Cell[this.size, this.size];
            for (int i = 0; i < this.size; i++) {
                for (int j = 0; j < this.size; j++) {
                    Point startPoint = new Point((i * this.width) / this.size, (j * this.height) / this.size);
                    this.cells[j, i] = new Cell(j, i, startPoint, cellSizeX, cellSizeY);
                }
            }

        }

        private void refresh() {
            if (!gameWon) {
                setTurnLabel();
            }
            this.panel.Invalidate();
            this.panel.Update();
        }

        private void handleClick(int x, int y) {
            if (!gameWon) {
                bool found = false;
                for (int i = 0; i < size; i++) {
                    for (int j = 0; j < size; j++) {
                        if (this.cells[i, j].hasPoint(x, y)) {
                            putValue(i, j);
                            found = true;
                            break;
                        }
                    }
                    if (found) {
                        handleAgent();
                        break;
                    }
                }
            }
        }

        private void setTurnLabel() {
            if (setValues <= cellCount) {
                if (agent != null && currentValue == agent.value) {
                    label.Text = currentValue + "'s Turn (CPU)";
                }
                else {
                    label.Text = currentValue + "'s Turn";
                }
            }
            if (isFull() && !gameWon)  {
                label.Text = "It's a tie.";
            }
            Task.Delay(500);
        }

        private void checkWin() {
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    if (this.cells[i, j].value != "") {
                        checkLines(this.cells[i, j]);
                    }
                    if (gameWon) break;
                }
                if (gameWon) break;
            }
        }

        private void checkValues(Cell c1, Cell c2, Cell c3) {
            if (c1.value == c2.value && c3.value == c2.value) {
                label.Text = c1.value + " wins!";
                c1.highlight = true;
                c2.highlight = true;
                c3.highlight = true;
                gameWon = true;
            }
        }

        private void checkLines(Cell cell) {
            bool columnsInBounds = cell.column - 1 >= 0 && cell.column + 1 < size;
            bool rowsInBounds = cell.row - 1 >= 0 && cell.row + 1 < size;

            if (columnsInBounds) {
                Cell leftCell = this.cells[cell.row, cell.column - 1];
                Cell rightCell = this.cells[cell.row, cell.column + 1];
                checkValues(leftCell, cell, rightCell);
                if (gameWon) return;
            }
            if (rowsInBounds) {
                Cell topCell = this.cells[cell.row - 1, cell.column];
                Cell bottomCell = this.cells[cell.row + 1, cell.column];
                checkValues(topCell, cell, bottomCell);
                if (gameWon) return;
            }
            if (columnsInBounds && rowsInBounds) {
                Cell topLeftCell = this.cells[cell.row - 1, cell.column - 1];
                Cell bottomRightCell = this.cells[cell.row + 1, cell.column + 1];
                checkValues(topLeftCell, cell, bottomRightCell);
                if (gameWon) return;

                Cell topRightCell = this.cells[cell.row - 1, cell.column + 1];
                Cell bottomLeftCell = this.cells[cell.row + 1, cell.column - 1];
                checkValues(topRightCell, cell, bottomLeftCell);
            }
        }

        private void draw(Graphics g, Pen pen, int width, int height) {
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    this.cells[i, j].draw(pen, g);
                }   
            }
        }
    }
}
