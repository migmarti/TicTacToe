using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TicTacToe.Classes {
    class Agent {
        public String value;
        public String opposingValue;
        private Random random;
        private Board board;

        public Agent(String value) {
            this.value = value;
            this.random = new Random();
            if (this.value == "X") {
                this.opposingValue = "O";
            }
            else if (this.value == "O") {
                this.opposingValue = "X";
            }
        }

        public void setBoard(Board board) {
            this.board = board;
        }

        public void handleTurn() {
            if (board != null && !board.isFull()) {
                bool handled = false;
                Cell[,] cells = board.getCells();
                while (handled == false) {
                    int i = random.Next(0, cells.GetLength(0));
                    int j = random.Next(0, cells.GetLength(0));
                    if (cells[i, j].value == "") {
                        board.putValue(i, j, board.currentValue);
                        handled = true;
                        break;
                    }
                }
            }
        }

        public Move minimax(int depth, bool maxPlayer) {

            Move best = new Move();

            if (maxPlayer) {
                best = new Move(-1, -1, -100000);
            }
            else {
                best = new Move(-1, -1, 100000);
            }

            if (depth == 0 || (board.gameWon || board.gameTie)) {
                int score = evaluateBoard();
                board.gameWon = false;
                board.gameTie = false;
                return new Move(-1, -1, score);
            }

            foreach (Cell cell in board.getEmptyCells()) {
                if (maxPlayer) {
                    board.putValue(cell.row, cell.column, this.value);
                    Move move = minimax(depth - 1, false);
                    board.takeBack(cell.row, cell.column);
                    move.row = cell.row;
                    move.column = cell.column;
                    if (move.score > best.score) {
                        best = move;
                    }
                }
                else {
                    board.putValue(cell.row, cell.column, this.opposingValue);
                    Move move = minimax(depth - 1, true);
                    board.takeBack(cell.row, cell.column);
                    move.row = cell.row;
                    move.column = cell.column;
                    if (move.score < best.score) {
                        best = move;
                    }
                }
            }
            return best;
        }

        private int evaluateBoard() {
            if (board.gameWon && board.currentValue == this.value) {
                return 1;
            }
            if (board.gameWon && board.currentValue == this.opposingValue) {
                return -1;
            }
            return 0;
        }
    }
}
