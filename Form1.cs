using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TicTacToe.Classes;

namespace TicTacToe {
    public partial class Form1 : Form {

        Board board;
        Agent agent;

        public Form1() {
            InitializeComponent();
            agent = new Agent("X");
            board = new Board(panel1, label1, 3, agent);
            //board = new Board(panel1, label1, 3, null);
        }

        private void button1_Click(object sender, EventArgs e) {
            board.clearValues(false);
        }

        private void button2_Click(object sender, EventArgs e) {
            board.clearValues(true);
        }

        //Eliminate flicker
        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;

                return cp;
            }
        }
    }
}
