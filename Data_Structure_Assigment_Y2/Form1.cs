using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Data_Structure_Assigment_Y2
{
    public partial class Form1 : Form
    {
        private MyLinkedList<Book> booksList; // Declare private field of MyLinkedList<Book> type

        private MyLinkedList<Book> halfList; // Declare private field of MyLinkedList<Book> type

        private SortBy selectedSortBy; // Declare private field of SortBy type

        private String searchTerm; // Declare private field of String type
        public Form1()
        {
            InitializeComponent(); // Initialize Form 1 components
            comboBox1.SelectedIndex = -1; // Set selected index of comboBox1 to -1
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
