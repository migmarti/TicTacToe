using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Classes {
    class Move {
        public int row, column, score;
        public Move(int row, int column, int score) {
            this.row = row;
            this.row = column;
            this.score = score;
        }
        public Move() {

        }
    }
}
