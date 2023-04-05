using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

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

        public MyLinkedList<Book> getBooksList() // Declare public method to get booksList
        {
            return this.booksList; // Return booksList field
        }

        public void setBookList(MyLinkedList<Book> booklist) // Declare public method to set booklist
        {
            this.booksList = booklist; // Set booksList field to booklist parameter
        }

        public MyLinkedList<Book> getHalfList() // Declare public method to get halfList
        {
            return this.halfList; // Return halfList field
        }

        public SortBy getSelectedSortBy() // Declare public method to get selectedSortBy
        {
            return this.selectedSortBy; // Return selectedSortBy field
        }

        public String getSearchedTerm() // Declare public method to get searchedTerm
        {
            searchTerm = textBox1.Text; // Set searchTerm field to text in textBox1
            return this.searchTerm; // Return searchTerm field
        }

        private MyLinkedList<Book> ImportBooks() // Declare private method to import books
        {
            MyLinkedList<Book> list = null; // Declare list variable of MyLinkedList<Book> type and set it to null
            MyLinkedList<Book> listHalf = new MyLinkedList<Book>(); // Declare listHalf variable of MyLinkedList<Book> type

            OpenFileDialog openFileDialog = new OpenFileDialog // Declare openFileDialog variable of OpenFileDialog type
            {
                Filter = "JSON Files (*.json)|*.json", // Set filter for openFileDialog
                Title = "Select a JSON file" // Set title for openFileDialog
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK) // Check if user selected a file
            {
                try // Try block for file reading
                {
                    string jsonFilePath = openFileDialog.FileName; // Get the file path of the selected file
                    string jsonContent = File.ReadAllText(jsonFilePath); // Read the content of the file

                    // Create a JsonSerializerSettings object and add the custom converter to it
                    var settings = new JsonSerializerSettings(); // Declare settings variable of JsonSerializerSettings type

                    // Add a custom converter to the Json.NET settings for the Book linked list type
                    settings.Converters.Add(new MyLinkedListConverter<Book>());


                    // Deserialize the JSON content into a Book linked list using the Json.NET settings
                    list = JsonConvert.DeserializeObject<MyLinkedList<Book>>(jsonContent, settings);

                    // Assign the deserialized linked list to a variable named booksList
                    booksList = list;

                    // Calculate the number of elements to take from the first half of the linked list
                    int halfCount = list.Count() / 2;

                    // Iterate through the first half of the linked list and add each element to a new linked list named listHalf
                    for (int i = 0; i < halfCount; i++)
                    {
                        listHalf.AddLast(list.ElementAt(i));
                    }

                    // Assign the first half linked list to a variable named halfList
                    halfList = listHalf;

                    // Iterate through the booksList linked list and add each element to the RichTextBox named RichBoxText
                    foreach (Book book in halfList)
                    {
                        HalfRichBoxText.AppendText($"Title: {book.Title}\n");
                        HalfRichBoxText.AppendText($"Author: {book.Author}\n");
                        HalfRichBoxText.AppendText($"Year: {book.Year}\n");
                        HalfRichBoxText.AppendText($"Genre: {book.Genre}\n");
                        HalfRichBoxText.AppendText($"Publisher: {book.Publisher}\n");
                        HalfRichBoxText.AppendText("------------\n");
                    }

                    // Iterate through the booksList linked list and add each element to the RichTextBox named RichBoxText
                    JsonRichBox.Text = jsonContent;
                }
                catch (Exception ex) // Catch block for file reading
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Show error message
                }
            }

            return booksList; // Return booksList
        }



        private void importToolStripMenuItem_Click(object sender, EventArgs e) // Declare private method for importToolStripMenuItem click event
        {
            ImportBooks(); // Call ImportBooks method
        }

        public void FillComboBoxWithEnumValues() // Declare public method to fill comboBox1 with enum values
        {
            comboBox1.Items.Clear(); // Clear comboBox1 items

            foreach (var sortBy in Enum.GetValues(typeof(SortBy))) // Iterate through enum values of SortBy type
            {
                comboBox1.Items.Add(sortBy); // Add enum value to comboBox1 items
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private MyDictionary<string, Book> CreateBooksDictionary(MyLinkedList<Book> bookList) // Declare private method to create books dictionary
        {
            MyDictionary<string, Book> booksDict = new MyDictionary<string, Book>(); // Declare booksDict variable of MyDictionary<string, Book> type
            if (checkBox1.Checked)  // Check if checkBox1 is checked
            {
                bookList = getHalfList(); // Call getHalfList method
            }
            else
            {
                bookList = getBooksList(); // Call getBooksList method
            }
            if (bookList == null)  // Check if bookList is null
            {
                MessageBox.Show("Please import a JSON first"); // Show message box
            }
            else
            {

                foreach (Book book in bookList) // Iterate through bookList
                {
                    booksDict.Add(book.Title, book); // Add book to booksDict
                }
            }
            return booksDict; // Return booksDict
        }
        private MyHashSet<Book> CreateBooksHashSet(MyLinkedList<Book> bookList) // Declare private method to create books hash set
        {
            if (checkBox1.Checked)  // Check if checkBox1 is checked
            {
                bookList = getHalfList(); // Call getHalfList method
            }
            else
            {
                bookList = getBooksList(); // Call getBooksList method
            }

            MyHashSet<Book> myHash = new MyHashSet<Book>(); // Declare myHash variable of MyHashSet<Book> type
            if (bookList == null)
            {
                MessageBox.Show("Please import a JSON first"); // Show message box
            }
            else
            {
                foreach (Book book in bookList)  // Iterate through bookList
                {
                    myHash.Add(book); // Add book to myHash
                }
            }

            return myHash; // Return myHash
        }
    }
}
