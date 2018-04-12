using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TicTacToe.Classes {
    class Agent {
        public String value { get; set; }
        Random random;

        public Agent(String value) {
            this.value = value;
            this.random = new Random();
        }

        public void handleTurn(Board board) {
            if (!board.isFull()) {
                bool handled = false;
                Cell[,] cells = board.getCells();
                while (handled == false) {
                    int i = random.Next(0, cells.GetLength(0));
                    int j = random.Next(0, cells.GetLength(0));
                    if (cells[i, j].value == "") {
                        board.putValue(i, j);
                        handled = true;
                        break;
                    }
                }
            }
        }
    }
}
