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

        public int size, width, height, cellCount;
        Panel panel;
        Label label;
        Cell[,] cells;
        Agent agent;
        public String currentValue;
        public bool gameWon = false;
        public bool gameTie = false;

        public Board(Panel p, Label l, int cellDensity, Agent agent) {
            this.panel = p;
            this.label = l;
            this.size = cellDensity;
            this.cells = new Cell[size, size];
            this.width = p.Width;
            this.height = p.Height;
            this.cellCount = cellDensity * cellDensity;
            this.agent = agent;
            initializeCells();
            p.Paint += new PaintEventHandler(panelPaint);
            p.Click += panelClick;
            this.agent.setBoard(this);
            randomizeFirst();
            //this.currentValue = "O";
            refresh();
        }

        public Cell[,] getCells() {
            return this.cells;
        }

        public List<Cell> getEmptyCells() {
            List<Cell> emptyCells = new List<Cell>();
            foreach (Cell cell in this.cells) {
                if (cell.value == "") {
                    emptyCells.Add(cell);
                }
            }
            return emptyCells;
        }

        public bool isFull() {
            return getEmptyCells().Count == 0;
        }

        public void clearValues(bool clearAgent) {
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    this.cells[i, j].value = "";
                    this.cells[i, j].highlight = false;
                }
            }
            gameWon = false;
            gameTie = false;
            if (clearAgent) {
                this.agent = null;
            }
            else {
                this.agent = new Agent("X");
                this.agent.setBoard(this);
            }
            randomizeFirst();
            //this.currentValue = "O";
            refresh();
        }

        public Cell getCellByIndex(int index) {
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    if (this.cells[i, j].index == index) {
                        return this.cells[i, j];
                    }
                }
            }
            return null;
        }

        public void putValue(int row, int column, String value) {
            if (row < cells.GetLength(0) && column < cells.GetLength(1)) {
                if (this.cells[row, column].value == "") {
                    this.cells[row, column].value = value;
                    currentValue = value;
                    checkWin();
                }
            }
        }

        public void takeBack(int row, int column) {
            //Console.WriteLine("TOOK BACK! " + row + " " + column);
            this.cells[row, column].value = "";
        }

        private void cycleTurn() {
            if (currentValue == "O") {
                currentValue = "X";
            }
            else {
                currentValue = "O";
            }       
        }

        private void panelPaint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black, 1);
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    this.cells[i, j].draw(pen, g);
                }
            }
            g.Dispose();
            pen.Dispose();
        }

        private void panelClick(object sender, EventArgs e) {
            Point point = panel.PointToClient(Cursor.Position);
            if ((!gameWon && !gameTie) && (agent == null || currentValue != agent.value)) {
                bool found = false;
                for (int i = 0; i < size; i++) {
                    for (int j = 0; j < size; j++) {
                        if (this.cells[i, j].hasPoint(point.X, point.Y) && this.cells[i, j].value == "") {
                            putValue(i, j, currentValue);
                            cycleTurn();
                            found = true;
                            break;
                        }
                    }
                    if (found) {
                        refresh();
                        handleAgent();
                        break;
                    }
                }
            }
        }

        private void initializeCells() {
            int cellSizeX = this.width / this.size;
            int cellSizeY = this.height / this.size;
            int index = 0;
            this.cells = new Cell[this.size, this.size];
            for (int i = 0; i < this.size; i++) {
                for (int j = 0; j < this.size; j++) {
                    Point startPoint = new Point((i * this.width) / this.size, (j * this.height) / this.size);
                    this.cells[j, i] = new Cell(j, i, startPoint, cellSizeX, cellSizeY);
                    this.cells[j, i].index = index;
                    index++;
                }
            }

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
            refresh();
            handleAgent();
        }

        private void handleAgent() {
            if (agent != null && currentValue == agent.value && (!gameWon && !gameTie) ) {
                List<Cell> emptyCells = getEmptyCells();
                if (emptyCells.Count > 0) {
                    if (emptyCells.Count == cellCount) {
                        agent.handleTurn();
                    }
                    else {
                        Move move = agent.minimax(emptyCells.Count, true);
                        foreach (Cell c in this.cells) {
                            c.highlight = false;
                        }
                        putValue(move.row, move.column, agent.value);
                    }
                    cycleTurn();
                    refresh();
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

        private void setTurnLabel() {
            if (!gameTie && !gameWon) {
                if (agent != null && currentValue == agent.value) {
                    label.Text = currentValue + "'s Turn (CPU)";
                }
                else {
                    label.Text = currentValue + "'s Turn";
                }
            }

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
            if (isFull() && !gameWon) {
                label.Text = "It's a tie.";
                gameTie = true;
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
    }
}
