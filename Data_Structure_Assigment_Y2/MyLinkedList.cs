using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * This code defines a custom generic linked list class called MyLinkedList<T>, where the generic type T is constrained to be a Book or a derived type of Book.
 * The class implements several interfaces: IEnumerable<T>, ILinearSearchable<T>, and ISortable<T>. 
 * The purpose of this class is to create a linked list that can store and manipulate a collection of Book objects. 
 * It provides various methods for adding, removing, and searching for books, as well as sorting them based on different criteria.
 * The class contains a nested private class called Node, which represents a single node in the linked list.
 * Each node has a Value of type T (a Book in this case) and a reference to the next node in the list (Next).
 */

public class MyLinkedList<T> : IEnumerable<T>, ILinearSearchable<T>, ISortable<T> where T : Book // T is a generic type that is a Book
{
    private Node head;

    private int count;

    public MyLinkedList()
    {
        head = null;
        count = 0;
    }
    private class Node
    {
        public T Value { get; set; }
        public Node Next { get; set; }

        public Node(T value)
        {
            Value = value;
            Next = null;
        }
    }

    public void AddFirst(T value)
    {
        Node node = new Node(value);
        node.Next = head;
        head = node;
        count++;
    }

    public void AddLast(T value) // AddLast method
    {
        Node node = new Node(value);
        if (head == null)
        {
            head = node;
        }
        else
        {
            Node current = head;
            while (current.Next != null)
            {
                current = current.Next;
            }
            current.Next = node;
        }
        count++;
    }

    public bool Remove(T value) // Remove method
    {
        Node current = head;

        if (head == null)
        {
            return false;
        }
        if (head.Value.Equals(value))
        {
            head = head.Next;
            count--;
            return true;
        }

        while (current.Next != null && !current.Next.Value.Equals(value)) // Loop through the list until the value is found or the end of the list is reached 
        {
            current = current.Next;
        }
        if (current.Next == null)
        {
            return false;
        }
        current.Next = current.Next.Next;
        count--;
        return true;
    }

    public bool Contains(T value)
    {
        Node current = head;
        while (current != null)
        {
            if (current.Value.Equals(value))
            {
                return true;
            }
            current = current.Next;
        }
        return false;
    }

    public IEnumerator<T> GetEnumerator()
    {
        Node current = head;
        while (current != null)
        {
            yield return current.Value;
            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() // GetEnumerator method
    {
        return GetEnumerator(); // Return the keys of the dictionary
    }

    private void Swap(Node node1, Node node2) // Swap method
    {
        T temp = node1.Value;
        node1.Value = node2.Value;
        node2.Value = temp;
    }

    private Node Insert(Node sorted, Node node, Comparison<T> comparison)
    {
        if (sorted == null || comparison(sorted.Value, node.Value) > 0) // If the list is empty or the node should be inserted at the beginning of the list 
        {
            node.Next = sorted;
            return node;
        }

        Node current = sorted;
        while (current.Next != null && comparison(current.Next.Value, node.Value) < 0) // Loop through the list until the correct position is found 
        {
            current = current.Next;
        }

        node.Next = current.Next; // Insert the node at the correct position
        current.Next = node;

        return sorted;
    }

    public void InsertionSort(SortBy sortBy) // InsertionSort method
    {
        if (head == null || head.Next == null) // list is empty or has only one element
            return;

        Node sorted = null;
        Node current = head;

        while (current != null)
        {
            Node next = current.Next;

            switch (sortBy)
            {
                case SortBy.Title: // Sort by title
                    sorted = Insert(sorted, current, (x, y) => string.Compare(x.Title, y.Title)); // Insert the node at the correct position in the sorted list
                    break;
                case SortBy.Author: // Sort by author
                    sorted = Insert(sorted, current, (x, y) => string.Compare(x.Author, y.Author)); // Insert the node at the correct position in the sorted list
                    break;
                case SortBy.Year:
                    sorted = Insert(sorted, current, (x, y) => x.Year.CompareTo(y.Year));
                    break;
                case SortBy.Genre:
                    sorted = Insert(sorted, current, (x, y) => string.Compare(x.Genre, y.Genre));
                    break;
                case SortBy.Publisher:
                    sorted = Insert(sorted, current, (x, y) => string.Compare(x.Publisher, y.Publisher));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sortBy), sortBy, null);
            }

            current = next;
        }

        head = sorted; // The sorted list becomes the new list
    }

    public void BubbleSort(SortBy sortBy) // BubbleSort method
    {
        int n = count; // Number of elements in the list
        bool swapped; // Boolean variable to check if the list is sorted

        for (int i = 0; i < n - 1; i++) // Loop through the list
        {
            swapped = false;
            Node current = head;
            for (int j = 0; j < n - i - 1; j++) // Loop through the list
            {
                Node next = current.Next; // Get the next node

                switch (sortBy)
                {
                    case SortBy.Title: // Sort by title
                        if (string.Compare(current.Value.Title, next.Value.Title) > 0) // Compare the titles of the two nodes
                        {
                            Swap(current, next); // Swap the nodes
                            swapped = true; // The list is not sorted
                        }
                        break;
                    case SortBy.Author:
                        if (string.Compare(current.Value.Author, next.Value.Author) > 0)
                        {
                            Swap(current, next);
                            swapped = true;
                        }
                        break;
                    case SortBy.Year:
                        if (current.Value.Year > next.Value.Year)
                        {
                            Swap(current, next);
                            swapped = true;
                        }
                        break;
                    case SortBy.Genre:
                        if (string.Compare(current.Value.Genre, next.Value.Genre) > 0)
                        {
                            Swap(current, next);
                            swapped = true;
                        }
                        break;
                    case SortBy.Publisher:
                        if (string.Compare(current.Value.Publisher, next.Value.Publisher) > 0)
                        {
                            Swap(current, next);
                            swapped = true;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(sortBy), sortBy, null); // Throw an exception if the sort criteria is not valid
                }

                current = current.Next; // Move to the next node
            }

            if (!swapped) // If the list is sorted
                break;
        }
    }

    public IEnumerable<T> LinearSearch(SortBy sortBy, string searchTerm) // LinearSearch method 
    {
        List<T> matchingBooks = new List<T>(); // List to store the matching books

        // Iterate over each book in the linked list
        foreach (T book in this)
        {
            // Check if the book matches the search term based on the sort criteria
            switch (sortBy)
            {
                case SortBy.Title: // Sort by title 
                    if (book.Title.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)) // Check if the title of the book matches the search term 
                    {
                        matchingBooks.Add(book); // Add the book to the list of matching books 
                    }
                    break;

                case SortBy.Author: // Sort by author
                    if (book.Author.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)) // Check if the author of the book matches the search term
                    {
                        matchingBooks.Add(book);
                    }
                    break;

                case SortBy.Year:
                    if (book.Year.ToString().Equals(searchTerm, StringComparison.OrdinalIgnoreCase))
                    {
                        matchingBooks.Add(book);
                    }
                    break;

                case SortBy.Genre:
                    if (book.Genre.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))
                    {
                        matchingBooks.Add(book);
                    }
                    break;

                case SortBy.Publisher:
                    if (book.Publisher.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))
                    {
                        matchingBooks.Add(book);
                    }
                    break;
            }
        }

        return matchingBooks; // Return the list of matching books
    }

    private Node GetNodeAtIndex(int index) // GetNodeAtIndex method
    {
        if (index < 0 || index >= count) // Check if the index is valid
        {
            throw new ArgumentOutOfRangeException(nameof(index)); // Throw an exception if the index is invalid
        }

        Node current = head; // Get the head of the list
        for (int i = 0; i < index; i++) // Loop through the list
        {
            current = current.Next; // Move to the next node
        }

        return current;
    }

    private int CompareBySearchTerm(T item, string searchTerm, SortBy sortBy) // CompareBySearchTerm method 
    {
        if (item is Book book) // Check if the item is a book 
        {
            switch (sortBy)
            {
                case SortBy.Title: // Sort by title
                    return book.Title.CompareTo(searchTerm); // Compare the title of the book with the search term
                case SortBy.Author:
                    return book.Author.CompareTo(searchTerm);
                case SortBy.Year:
                    return book.Year.ToString().CompareTo(searchTerm);
                case SortBy.Genre:
                    return book.Genre.CompareTo(searchTerm);
                case SortBy.Publisher:
                    return book.Publisher.CompareTo(searchTerm);
                default:
                    throw new ArgumentException($"Invalid criterion: {sortBy}");
            }
        }

        throw new InvalidOperationException("Items must be of type Book."); // Throw an exception if the item is not a book 
    }

    public List<T> BinarySearch(SortBy sortBy, string searchTerm) // BinarySearch method 
    {
        // First, sort the items using the provided sortBy criterion
        BubbleSort(sortBy);

        int left = 0;
        int right = count - 1;
        List<T> foundItems = new List<T>();

        while (left <= right) // Loop through the list 
        {
            int mid = (left + right) / 2; // Get the middle index of the list 
            Node midNode = GetNodeAtIndex(mid); // Get the node at the middle index 
            T midItem = midNode.Value; // Get the value of the node at the middle index
            int comparisonResult = CompareBySearchTerm(midItem, searchTerm, sortBy); // Compare the value of the node at the middle index with the search term

            if (comparisonResult == 0) // Found the item
            {
                // Add the found item to the list
                foundItems.Add(midItem);

                // Search for additional occurrences on both sides of the found item
                int currentIndex = mid - 1; // Get the index of the node before the found item 
                Node currentNode; // Create a new node 
                while (currentIndex >= 0) // Loop through the list 
                {
                    currentNode = GetNodeAtIndex(currentIndex); // Get the node at the current index
                    if (CompareBySearchTerm(currentNode.Value, searchTerm, sortBy) == 0) // Check if the value of the node at the current index matches the search term
                    {
                        foundItems.Add(currentNode.Value); // Add the node to the list of found items
                    }
                    else
                    {
                        break;
                    }
                    currentIndex--; // Move to the previous node
                }

                currentIndex = mid + 1; // Get the index of the node after the found item

                while (currentIndex < count) // Loop through the list
                {
                    currentNode = GetNodeAtIndex(currentIndex); // Get the node at the current index
                    if (CompareBySearchTerm(currentNode.Value, searchTerm, sortBy) == 0) // Check if the value of the node at the current index matches the search term
                    {
                        foundItems.Add(currentNode.Value); // Add the node to the list of found items
                    }
                    else
                    {
                        break;
                    }
                    currentIndex++;
                }

                // Break the loop as we have found all occurrences
                break;
            }
            else if (comparisonResult < 0) // searchTerm is greater, search the right half
            {
                left = mid + 1;
            }
            else // searchTerm is smaller, search the left half
            {
                right = mid - 1;
            }
        }

        return foundItems; // Return the list of found items, or an empty list if searchTerm not found
    }
}


