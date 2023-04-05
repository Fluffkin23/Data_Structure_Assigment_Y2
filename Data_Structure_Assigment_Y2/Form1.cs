using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
            FillComboBoxWithEnumValues(); // Call FillComboBoxWithEnumValues method

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

        private MyLinkedList<Book> CreateBooksLinked(MyLinkedList<Book> bookList) // Declare private method to create books linked list
        {
            if (checkBox1.Checked) // Check if checkBox1 is checked
            {
                bookList = getHalfList(); // Call getHalfList method
            }
            else
            {
                bookList = getBooksList(); // Call getBooksList method
            }

            MyLinkedList<Book> myLinked = new MyLinkedList<Book>(); // Declare myLinked variable of MyLinkedList<Book> type
            if (bookList == null) // Check if bookList is null
            {
                MessageBox.Show("Please import a JSON first"); // Show message box
            }
            else
            {
                foreach (Book book in bookList) // Iterate through bookList
                {
                    myLinked.AddFirst(book); // Add book to myLinked
                }
            }

            return myLinked; // Return myLinked
        }

        private void button1_Click(object sender, EventArgs e) // Declare private method for button1 click event
        {
            MyLinkedList<Book> testList = new MyLinkedList<Book>(); // Declare testList variable of MyLinkedList<Book> type

            if (checkBox1.Checked)  // Check if checkBox1 is checked
            {
                testList = getHalfList(); // Call getHalfList method
            }
            else
            {
                testList = getBooksList(); // Call getBooksList method

            }

            MyDictionary<string, Book> booksDict = CreateBooksDictionary(testList); // Declare booksDict variable of MyDictionary<string, Book> type and assign it to CreateBooksDictionary method

            richTextBox1.Clear(); // Clear richTextBox1

            if (testList == null)  // Check if testList is null
            {
                MessageBox.Show("Please import a JSON First"); // Show message box
            }

            if (comboBox1.SelectedItem != null) // Check if comboBox1 selected item is not null
            {
                this.selectedSortBy = (SortBy)comboBox1.SelectedItem; // Assign selected item to selectedSortBy variable


                if (DictionaryRadio.Checked) // Check if DictionaryRadio is checked
                {

                    if (BubbleRadio.Checked) // Check if BubbleRadio is checked
                    {
                        if (comboBox1.SelectedItem == null) // Check if comboBox1 selected item is null
                        {
                            richTextBox1.AppendText("Please add sort by"); // Append text to richTextBox1
                        }
                        else
                        {
                            this.selectedSortBy = (SortBy)comboBox1.SelectedItem; // Assign selected item to selectedSortBy variable

                        }

                        Stopwatch stopwatch = new Stopwatch(); // Declare stopwatch variable of Stopwatch type
                        stopwatch.Start(); // Start stopwatch
                        booksDict.BubbleSort(this.selectedSortBy); // Call BubbleSort method

                        stopwatch.Stop(); // Stop stopwatch
                        long elapsedTicks = stopwatch.ElapsedTicks; // Declare elapsedTicks variable of long type and assign it to stopwatch elapsed ticks
                        double ticksPerNanosecond = Stopwatch.Frequency / 1_000_000_000.0; // Declare ticksPerNanosecond variable of double type and assign it to stopwatch frequency divided by 1_000_000_000.0
                        double elapsedNanoseconds = elapsedTicks / ticksPerNanosecond; // Declare elapsedNanoseconds variable of double type and assign it to elapsedTicks divided by ticksPerNanosecond


                        richTextBox1.AppendText("------------------------Interface BubbleSort MyDictionary---------------\n"); // Append text to richTextBox1
                        richTextBox1.AppendText("The time is" + elapsedNanoseconds + "nano Seconds" + "\n"); // Append text to richTextBox1
                        foreach (string title in booksDict.Keys) // Iterate through booksDict keys
                        {
                            Book book = booksDict[title]; // Declare book variable of Book type and assign it to booksDict title
                            richTextBox1.AppendText($"- {book.Year} by {book.Title}\n"); // Append text to richTextBox1
                        }
                    }
                    else if (InsertionRadio.Checked) // Check if InsertionRadio is checked
                    {
                        Stopwatch stopwatch = new Stopwatch(); // Declare stopwatch variable of Stopwatch type
                        stopwatch.Start(); // Start stopwatch
                        if (comboBox1.SelectedItem == null) // Check if comboBox1 selected item is null
                        {
                            MessageBox.Show("Please sortBy"); // Show message box
                        }
                        booksDict.InsertionSort(selectedSortBy); // Call InsertionSort method

                        stopwatch.Stop(); // Stop stopwatch

                        // double elapsedTime = Math.Round(stopwatch.Elapsed.TotalSeconds, 1);
                        long elapsedTicks = stopwatch.ElapsedTicks; // Declare elapsedTicks variable of long type and assign it to stopwatch elapsed ticks
                        double ticksPerNanosecond = Stopwatch.Frequency / 1_000_000_000.0; // Declare ticksPerNanosecond variable of double type and assign it to stopwatch frequency divided by 1_000_000_000.0
                        double elapsedNanoseconds = elapsedTicks / ticksPerNanosecond; // Declare elapsedNanoseconds variable of double type and assign it to elapsedTicks divided by ticksPerNanosecond

                        richTextBox1.Clear(); // Clear richTextBox1
                        richTextBox1.AppendText("------------------------Interface Insertion Sort MyDictionary---------------\n"); // Append text to richTextBox1
                        richTextBox1.AppendText("The time is" + elapsedNanoseconds + "\n"); // Append text to richTextBox1
                        foreach (string title in booksDict.Keys) // Iterate through booksDict keys
                        {
                            Book book = booksDict[title]; // Declare book variable of Book type and assign it to booksDict title
                            richTextBox1.AppendText($"- {book.Year} by {book.Title}\n"); // Append text to richTextBox1
                        }
                    }
                    else if (LinearButton.Checked) // Check if LinearButton is checked
                    {
                        richTextBox1.AppendText("------------------------Interface Dictionary LinearSearch---------------\n"); // Append text to richTextBox1
                        if (String.IsNullOrEmpty(textBox1.Text)) // Check if textBox1 text is null or empty
                        {
                            MessageBox.Show("Please search for something!"); // Show message box
                        }
                        Stopwatch stopwatch = new Stopwatch(); // Declare stopwatch variable of Stopwatch type
                        stopwatch.Start(); // Start stopwatch

                        IEnumerable<Book> results = booksDict.LinearSearch(selectedSortBy, textBox1.Text); // Declare results variable of IEnumerable<Book> type and assign it to booksDict LinearSearch method
                        stopwatch.Stop(); // Stop stopwatch

                        //double elapsedTime = Math.Round(stopwatch.Elapsed.TotalSeconds, 1);
                        long elapsedTicks = stopwatch.ElapsedTicks; // Declare elapsedTicks variable of long type and assign it to stopwatch elapsed ticks
                        double ticksPerNanosecond = Stopwatch.Frequency / 1_000_000_000.0; // Declare ticksPerNanosecond variable of double type and assign it to stopwatch frequency divided by 1_000_000_000.0
                        double elapsedNanoseconds = elapsedTicks / ticksPerNanosecond; // Declare elapsedNanoseconds variable of double type and assign it to elapsedTicks divided by ticksPerNanosecond
                        richTextBox1.AppendText("The time is" + elapsedNanoseconds + "\n"); // Append text to richTextBox1


                        if (results.Any()) // Check if results has any
                        {
                            foreach (Book book in results) // Iterate through results
                            {
                                richTextBox1.AppendText(book.Title + " by " + book.Author + book.Publisher + book.Genre + book.Year + "\n"); // Append text to richTextBox1
                            }
                        }
                        else
                        {
                            richTextBox1.AppendText("Could not find:\n"); // Append text to richTextBox1

                        }
                    }
                    else if (BinaryButton.Checked) // Check if BinaryButton is checked
                    {
                        if (String.IsNullOrEmpty(textBox1.Text)) // Check if textBox1 text is null or empty
                        {
                            MessageBox.Show("Please search for something!"); // Show message box
                        }
                        richTextBox1.AppendText("------------------------Interface Dictionary BinarySearch---------------\n"); // Append text to richTextBox1

                        Stopwatch stopwatch = new Stopwatch(); // Declare stopwatch variable of Stopwatch type
                        stopwatch.Start(); // Start stopwatch

                        IEnumerable<Book> foundBookBinary = booksDict.BinarySearch(selectedSortBy, textBox1.Text); // Declare foundBookBinary variable of IEnumerable<Book> type and assign it to booksDict BinarySearch method

                        stopwatch.Stop();   // Stop stopwatch

                        //double elapsedTime = Math.Round(stopwatch.Elapsed.TotalSeconds, 1);
                        long elapsedTicks = stopwatch.ElapsedTicks; // Declare elapsedTicks variable of long type and assign it to stopwatch elapsed ticks
                        double ticksPerNanosecond = Stopwatch.Frequency / 1_000_000_000.0; // Declare ticksPerNanosecond variable of double type and assign it to stopwatch frequency divided by 1_000_000_000.0
                        double elapsedNanoseconds = elapsedTicks / ticksPerNanosecond; // Declare elapsedNanoseconds variable of double type and assign it to elapsedTicks divided by ticksPerNanosecond
                        richTextBox1.AppendText("The time is" + elapsedNanoseconds + "\n"); // Append text to richTextBox1

                        if (foundBookBinary != null) // Check if foundBookBinary is not null
                        {
                            foreach (Book book in foundBookBinary) // Iterate through foundBookBianry
                            {
                                richTextBox1.AppendText(book.Title + " by " + book.Author + book.Publisher + book.Genre + book.Year + "\n"); // Append text to richTextBox1
                            }
                        }
                        else
                        {
                            richTextBox1.AppendText("Could not find:\n"); // Append text to richTextBox1
                        }
                    }

                }
                else if (HashSetRadio.Checked) // Check if HashSetRadio is checked
                {
                    MyLinkedList<Book> hashList = new MyLinkedList<Book>(); // Declare hashList variable of MyLinkedList<Book> type and assign it to new MyLinkedList<Book>

                    if (checkBox1.Checked) // Check if checkBox1 is checked
                    {
                        hashList = getHalfList(); // Assign hashList to getHalfList method
                    }
                    else
                    {
                        hashList = getBooksList(); // Assign hashList to getBooksList method
                    }

                    MyHashSet<Book> myHash = CreateBooksHashSet(getBooksList()); // Declare myHash variable of MyHashSet<Book> type and assign it to CreateBooksHashSet method
                    if (BubbleRadio.Checked) // Check if BubbleRadio is checked
                    {
                        Stopwatch stopwatch = new Stopwatch(); // Declare stopwatch variable of Stopwatch type
                        stopwatch.Start(); // Start stopwatch

                        myHash.BubbleSort(selectedSortBy); // Call myHash BubbleSort method

                        stopwatch.Stop(); // Stop stopwatch

                        // double elapsedTime = Math.Round(stopwatch.Elapsed.TotalSeconds, 1);
                        long elapsedTicks = stopwatch.ElapsedTicks; // Declare elapsedTicks variable of long type and assign it to stopwatch elapsed ticks
                        double ticksPerNanosecond = Stopwatch.Frequency / 1_000_000_000.0; // Declare ticksPerNanosecond variable of double type and assign it to stopwatch frequency divided by 1_000_000_000.0
                        double elapsedNanoseconds = elapsedTicks / ticksPerNanosecond; // Declare elapsedNanoseconds variable of double type and assign it to elapsedTicks divided by ticksPerNanosecond

                        richTextBox1.AppendText("------------------------Interface BubbleSort MyHashSet---------------\n"); // Append text to richTextBox1
                        richTextBox1.AppendText("The time is" + elapsedNanoseconds + "\n"); // Append text to richTextBox1

                        foreach (Book book in myHash) // Iterate through myHash
                        {

                            richTextBox1.AppendText($"- {book.Title} by {book.Author} + {book.Publisher} + {book.Genre} + {book.Year}\n"); // Append text to richTextBox1
                        }
                    }
                    else if (InsertionRadio.Checked) // Check if InsertionRadio is checked
                    {
                        Stopwatch stopwatch = new Stopwatch(); // Declare stopwatch variable of Stopwatch type
                        stopwatch.Start(); // Start stopwatch

                        myHash.InsertionSort(selectedSortBy); // Call myHash InsertionSort method

                        stopwatch.Stop(); // Stop stopwatch

                        //double elapsedTime = Math.Round(stopwatch.Elapsed.TotalSeconds, 1);
                        long elapsedTicks = stopwatch.ElapsedTicks; // Declare elapsedTicks variable of long type and assign it to stopwatch elapsed ticks
                        double ticksPerNanosecond = Stopwatch.Frequency / 1_000_000_000.0;  // Declare ticksPerNanosecond variable of double type and assign it to stopwatch frequency divided by 1_000_000_000.0
                        double elapsedNanoseconds = elapsedTicks / ticksPerNanosecond; // Declare elapsedNanoseconds variable of double type and assign it to elapsedTicks divided by ticksPerNanosecond

                        richTextBox1.Clear(); // Clear richTextBox1
                        richTextBox1.AppendText("------------------------Interface BubbleSort MyHashSet---------------\n"); // Append text to richTextBox1
                        richTextBox1.AppendText("The time is" + elapsedNanoseconds + "\n"); // Append text to richTextBox1

                        foreach (Book book in myHash) // Iterate through myHash
                        {

                            richTextBox1.AppendText($"- {book.Title} by {book.Author} + {book.Publisher} + {book.Genre} + {book.Year}\n"); // Append text to richTextBox1
                        }
                    }
                    else if (LinearButton.Checked) // Check if LinearButton is checked
                    {
                        if (String.IsNullOrEmpty(textBox1.Text)) // Check if textBox1 is null or empty
                        {
                            MessageBox.Show("Please search for something!"); // Show message box
                        }
                        richTextBox1.AppendText("------------------------Interface MyHashSet LinearSearch---------------\n"); // Append text to richTextBox1

                        Stopwatch stopwatch = new Stopwatch(); // Declare stopwatch variable of Stopwatch type
                        stopwatch.Start(); // Start stopwatch

                        IEnumerable<Book> results = myHash.LinearSearch(selectedSortBy, textBox1.Text); // Declare results variable of IEnumerable<Book> type and assign it to myHash LinearSearch method

                        stopwatch.Stop(); // Stop stopwatch

                        //double elapsedTime = Math.Round(stopwatch.Elapsed.TotalSeconds, 1);
                        long elapsedTicks = stopwatch.ElapsedTicks; // Declare elapsedTicks variable of long type and assign it to stopwatch elapsed ticks
                        double ticksPerNanosecond = Stopwatch.Frequency / 1_000_000_000.0; // Declare ticksPerNanosecond variable of double type and assign it to stopwatch frequency divided by 1_000_000_000.0
                        double elapsedNanoseconds = elapsedTicks / ticksPerNanosecond; // Declare elapsedNanoseconds variable of double type and assign it to elapsedTicks divided by ticksPerNanosecond
                        richTextBox1.AppendText("The time is" + elapsedNanoseconds + "\n"); // Append text to richTextBox1

                        if (results.Any()) // Check if results has any elements
                        {
                            foreach (Book book in results) // Iterate through results
                            {
                                richTextBox1.AppendText(book.Title + " by " + book.Author + book.Publisher + book.Genre + book.Year + "\n"); // Append text to richTextBox1
                            }
                        }
                        else
                        {
                            richTextBox1.AppendText("Could not find:\n"); // Append text to richTextBox1

                        }
                    }
                    else if (BinaryButton.Checked) // Check if BinaryButton is checked
                    {
                        if (String.IsNullOrEmpty(textBox1.Text)) // Check if textBox1 is null or empty
                        {
                            MessageBox.Show("Please search for something!"); // Show message box
                        }
                        richTextBox1.AppendText("------------------------Interface MyHashSet BinarySearch---------------\n"); // Append text to richTextBox1

                        Stopwatch stopwatch = new Stopwatch(); // Declare stopwatch variable of Stopwatch type
                        stopwatch.Start(); // Start stopwatch

                        IEnumerable<Book> binarBook = myHash.BinarySearch(selectedSortBy, textBox1.Text); // Declare binarBook variable of IEnumerable<Book> type and assign it to myHash BinarySearch method

                        stopwatch.Stop(); // Stop stopwatch

                        //double elapsedTime = Math.Round(stopwatch.Elapsed.TotalSeconds, 1);
                        long elapsedTicks = stopwatch.ElapsedTicks; // Declare elapsedTicks variable of long type and assign it to stopwatch elapsed ticks
                        double ticksPerNanosecond = Stopwatch.Frequency / 1_000_000_000.0; // Declare ticksPerNanosecond variable of double type and assign it to stopwatch frequency divided by 1_000_000_000.0
                        double elapsedNanoseconds = elapsedTicks / ticksPerNanosecond; // Declare elapsedNanoseconds variable of double type and assign it to elapsedTicks divided by ticksPerNanosecond
                        richTextBox1.AppendText("The time is" + elapsedNanoseconds + "\n"); // Append text to richTextBox1

                        if (binarBook.Any()) // Check if binarBook has any elements
                        {
                            foreach (Book book in binarBook) // Iterate through binarBook
                            {
                                richTextBox1.AppendText(book.Title + " by " + book.Author + book.Publisher + book.Genre + book.Year + "\n"); // Append text to richTextBox1
                            }
                        }
                        else
                        {
                            richTextBox1.AppendText("Could not find:\n"); // Append text to richTextBox1

                        }
                    }
                }
                else if (LinkedListRadio.Checked) // Check if LinkedListRadio is cheked
                {
                    MyLinkedList<Book> linkedList = new MyLinkedList<Book>();

                    if (checkBox1.Checked)  // Check if checkBox1 is checked
                    {
                        linkedList = getHalfList();  // Assign linkedList to getHalfList method
                    }
                    linkedList = getBooksList(); // Assign linkedList to getBooksList method

                    MyLinkedList<Book> myLinked = CreateBooksLinked(linkedList); // Declare myLinked variable of MyLinkedList<Book> type and assign it to CreateBooksLinked method

                    if (BubbleRadio.Checked) // Check if BubbleRadio is checked
                    {
                        Stopwatch stopwatch = new Stopwatch(); // Declare stopwatch variable of Stopwatch type
                        stopwatch.Start(); // Start stopwatch

                        myLinked.BubbleSort(selectedSortBy); // Call BubbleSort method

                        stopwatch.Stop();   // Stop stopwatch

                        // double elapsedTime = Math.Round(stopwatch.Elapsed.TotalSeconds, 1);
                        long elapsedTicks = stopwatch.ElapsedTicks; // Declare elapsedTicks variable of long type and assign it to stopwatch elapsed ticks
                        double ticksPerNanosecond = Stopwatch.Frequency / 1_000_000_000.0; // Declare ticksPerNanosecond variable of double type and assign it to stopwatch frequency divided by 1_000_000_000.0
                        double elapsedNanoseconds = elapsedTicks / ticksPerNanosecond; // Declare elapsedNanoseconds variable of double type and assign it to elapsedTicks divided by ticksPerNanosecond
                        richTextBox1.AppendText("The time is" + elapsedNanoseconds + "\n"); // Append text to richTextBox1

                        richTextBox1.AppendText("------------------------Interface BubbleSort MyLinkedList---------------\n"); // Append text to richTextBox1
                        foreach (Book book in myLinked) // Iterate through myLinked
                        {

                            richTextBox1.AppendText($"- {book.Title} by {book.Author} + {book.Publisher} + {book.Genre} + {book.Year}\n"); // Append text to richTextBox1
                        }
                    }
                    else if (InsertionRadio.Checked) // Check if InsertionRadio is checked
                    {

                        Stopwatch stopwatch = new Stopwatch(); // Declare stopwatch variable of Stopwatch type
                        stopwatch.Start(); // Start stopwatch

                        myLinked.InsertionSort(selectedSortBy); // Call InsertionSort method

                        stopwatch.Stop(); // Stop stopwatch
                        double elapsedTime = Math.Round(stopwatch.Elapsed.TotalSeconds, 1); // Declare elapsedTime variable of double type and assign it to stopwatch elapsed total seconds rounded to 1 decimal place
                        richTextBox1.AppendText("The time is" + elapsedTime + "\n"); // Append text to richTextBox1

                        richTextBox1.Clear();   // Clear richTextBox1
                        richTextBox1.AppendText("------------------------Interface Insertion MyDictionary---------------\n"); // Append text to richTextBox1
                        foreach (Book book in myLinked) // Iterate through myLinked
                        {

                            richTextBox1.AppendText($"- {book.Title} by {book.Author} + {book.Publisher} + {book.Genre} + {book.Year}\n"); // Append text to richTextBox1
                        }
                    }
                    else if (LinearButton.Checked) // Check if LinearButton is checked
                    {
                        if (String.IsNullOrEmpty(textBox1.Text)) // Check if textBox1 is null or empty
                        {
                            MessageBox.Show("Please search for something!"); // Show message box
                        }
                        richTextBox1.AppendText("------------------------Interface MyLinkedList LinearSearch---------------\n"); // Append text to richTextBox1

                        Stopwatch stopwatch = new Stopwatch(); // Declare stopwatch variable of Stopwatch type
                        stopwatch.Start(); // Start stopwatch

                        IEnumerable<Book> results = myLinked.LinearSearch(selectedSortBy, textBox1.Text); // Declare results variable of IEnumerable<Book> type and assign it to myLinked LinearSearch method

                        stopwatch.Stop(); // Stop stopwatch

                        // double elapsedTime = Math.Round(stopwatch.Elapsed.TotalSeconds, 1);
                        long elapsedTicks = stopwatch.ElapsedTicks; // Declare elapsedTicks variable of long type and assign it to stopwatch elapsed ticks
                        double ticksPerNanosecond = Stopwatch.Frequency / 1_000_000_000.0; // Declare ticksPerNanosecond variable of double type and assign it to stopwatch frequency divided by 1_000_000_000.0
                        double elapsedNanoseconds = elapsedTicks / ticksPerNanosecond; // Declare elapsedNanoseconds variable of double type and assign it to elapsedTicks divided by ticksPerNanosecond
                        richTextBox1.AppendText("The time is" + elapsedNanoseconds + "\n"); // Append text to richTextBox1

                        if (results.Any()) // Check if results has any elements
                        {
                            foreach (Book book in results) // Iterate through resultds
                            {
                                richTextBox1.AppendText(book.Title + " by " + book.Author + book.Publisher + book.Genre + book.Year + "\n"); // Append text to richTextBox1
                            }
                        }
                        else
                        {
                            richTextBox1.AppendText("Could not find:\n"); // Append text to richTextBox1

                        }
                    }
                    else if (BinaryButton.Checked) // Check if BinaryButton is checked
                    {
                        if (String.IsNullOrEmpty(textBox1.Text)) // Check if textBox1 is null or empty
                        {
                            MessageBox.Show("Please search for something!"); // Show message box
                        }
                        richTextBox1.AppendText("------------------------Interface MyLinkedList BinarySearch---------------\n"); // Append text to richTextBox1

                        Stopwatch stopwatch = new Stopwatch(); // Declare stopwatch variable of Stopwatch type
                        stopwatch.Start(); // Start stopwatch

                        IEnumerable<Book> binaryLinked = myLinked.LinearSearch(selectedSortBy, textBox1.Text); // Declare binaryLinked variable of IEnumerable<Book> type and assign it to myLinked LinearSearch method

                        stopwatch.Stop(); // Stop stopwatch

                        // double elapsedTime = Math.Round(stopwatch.Elapsed.TotalSeconds, 1);
                        long elapsedTicks = stopwatch.ElapsedTicks; // Declare elapsedTicks variable of long type and assign it to stopwatch elapsed ticks
                        double ticksPerNanosecond = Stopwatch.Frequency / 1_000_000_000.0; // Declare ticksPerNanosecond variable of double type and assign it to stopwatch frequency divided by 1_000_000_000.0
                        double elapsedNanoseconds = elapsedTicks / ticksPerNanosecond; // Declare elapsedNanoseconds variable of double type and assign it to elapsedTicks divided by ticksPerNanosecond
                        richTextBox1.AppendText("The time is" + elapsedNanoseconds + "\n"); // Append text to richTextBox1

                        if (binaryLinked.Any()) // Check if binaryLinked has any elements
                        {
                            foreach (Book book in binaryLinked)  // Iterate through binaryLinked
                            {
                                richTextBox1.AppendText(book.Title + " by " + book.Author + book.Publisher + book.Genre + book.Year + "\n"); // Append text to richTextBox1
                            }
                        }
                        else
                        {
                            richTextBox1.AppendText("Could not find:\n"); // Append text to richTextBox1

                        }
                    }
                }
            }
            else
            {
                richTextBox1.AppendText("Please add a sortBy"); // Append text to richTextBox1
            }

        }
    }
}
    

