using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * This code defines a custom MyHashSet<T> class that implements the IEnumerable<T>, ILinearSearchable<T>, and ISortable<T> interfaces, where T must be of type Book. 
 * The MyHashSet<T> class works like a set, ensuring that only unique Book objects are stored within it. 
 * It also provides several methods for sorting and searching the stored books based on various criteria.
 * 
 * This class provide the following functionalities:
 * 
 * Storage and management of a collection of unique items of type Book.
 * Implements IEnumerable<T> to enable iteration over the items in the collection.
 * Implements ILinearSearchable<T> to provide linear searching capabilities based on different criteria.
 * Implements ISortable<T> to provide sorting methods, such as insertion sort and bubble sort, based on different criteria.
 * Provides methods for standard set operations, such as adding, removing, and checking for the existence of items in the collection.
 */

public class MyHashSet<T> : IEnumerable<T>, ILinearSearchable<T>, ISortable<T> where T : Book // T must be of type Book
{
    private List<T> items;

    public int Count => items.Count;

    public MyHashSet()
    {
        items = new List<T>();
    }

    public void Add(T item)
    {
        if (!Contains(item))
        {
            items.Add(item);
        }
    }

    public bool Contains(T item)
    {
        return items.Contains(item);
    }

    public bool Remove(T item)
    {
        return items.Remove(item);
    }

    public void Clear()
    {
        items.Clear();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private int Compare(T item1, T item2, SortBy sortBy) // Compare method
    {

        if (typeof(T) == typeof(Book))
        {
            Book book1 = item1 as Book;
            Book book2 = item2 as Book;

            switch (sortBy)
            {
                case SortBy.Title:
                    return book1.Title.CompareTo(book2.Title); // Compare the titles of the books
                case SortBy.Author:
                    return book1.Author.CompareTo(book2.Author); // Compare the authors of the books
                case SortBy.Year:
                    return book1.Year.CompareTo(book2.Year); // Compare the years of the books
                case SortBy.Genre:
                    return book1.Genre.CompareTo(book2.Genre); // Compare the genres of the books
                case SortBy.Publisher:
                    return book1.Publisher.CompareTo(book2.Publisher); // Compare the publishers of the books
                default:
                    throw new ArgumentException($"Invalid criterion: {sortBy}"); // Throw an exception if the sortBy parameter is invalid
            }
        }

        throw new InvalidOperationException("Items must be of type Book."); // Throw an exception if 
    }

    private void UpdateFromItemArray(T[] itemArray) // UpdateFromItemArray method
    {
        items.Clear();
        foreach (T item in itemArray) // add the items to the list
        {
            items.Add(item);
        }
    }

    public void InsertionSort(SortBy sortBy)
    {
        int n = items.Count; // Get the number of items in the list

        for (int i = 1; i < n; i++) // Loop through the list
        {
            T key = items[i]; // Get the item at index i
            int j = i - 1; // Get the item at index i - 1

            while (j >= 0 && Compare(key, items[j], sortBy) < 0) // Loop through the list
            {
                items[j + 1] = items[j]; // Swap the items
                j--;
            }
            items[j + 1] = key; // Swap the items
        }
    }

    public IEnumerable<T> LinearSearch(SortBy sortBy, string searchTerm) // LinearSearch method
    {
        IEnumerable<T> matchingItems; // Create an IEnumerable of type T

        switch (sortBy)
        {
            case SortBy.Title: // If the sortBy parameter is SortBy.Title
                matchingItems = items.Where(item => item.Title.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)); // Get the items that match the search term
                break;
            case SortBy.Author:
                matchingItems = items.Where(item => item.Author.Equals(searchTerm, StringComparison.OrdinalIgnoreCase));
                break;
            case SortBy.Year:
                matchingItems = items.Where(item => item.Year.ToString().Equals(searchTerm, StringComparison.OrdinalIgnoreCase));
                break;
            case SortBy.Genre:
                matchingItems = items.Where(item => item.Genre.Equals(searchTerm, StringComparison.OrdinalIgnoreCase));
                break;
            case SortBy.Publisher:
                matchingItems = items.Where(item => item.Publisher.Equals(searchTerm, StringComparison.OrdinalIgnoreCase));
                break;
            default:
                throw new ArgumentException("Invalid SortBy value.", nameof(sortBy)); // Throw an exception if the sortBy parameter is invalid
        }

        return matchingItems;
    }

    public void BubbleSort(SortBy sortBy) // BubbleSort method
    {
        int n = items.Count; // Get the number of items in the list
        bool swapped; // Create a boolean variable

        for (int i = 0; i < n - 1; i++) // Loop through the list
        {
            swapped = false;
            for (int j = 0; j < n - i - 1; j++) // Loop through the list
            {
                if (Compare(items[j], items[j + 1], sortBy) > 0) // Compare the items
                {
                    T temp = items[j];  // Swap the items
                    items[j] = items[j + 1];
                    items[j + 1] = temp;
                    swapped = true;
                }
            }

            if (!swapped) break; // If the items are not swapped, break out of the loop
        }
    }

    private int CompareBySearchTerm(T item, string searchTerm, SortBy sortBy) // CompareBySearchTerm method
    {
        if (item is Book book)
        {
            switch (sortBy)
            {
                case SortBy.Title: // If the sortBy parameter is SortBy.Title
                    return book.Title.CompareTo(searchTerm);  // Compare the titles of the books
                case SortBy.Author:
                    return book.Author.CompareTo(searchTerm);
                case SortBy.Year:
                    return book.Year.ToString().CompareTo(searchTerm);
                case SortBy.Genre:
                    return book.Genre.CompareTo(searchTerm);
                case SortBy.Publisher:
                    return book.Publisher.CompareTo(searchTerm);
                default:
                    throw new ArgumentException($"Invalid criterion: {sortBy}"); // Throw an exception if the sortBy parameter is invalid
            }
        }
        throw new InvalidOperationException("Items must be of type Book."); // Throw an exception if the items are not of type Book
    }

    public List<T> BinarySearch(SortBy sortBy, string searchTerm) // BinarySearch method
    {
        // First, sort the items using the provided sortBy criterion
        BubbleSort(sortBy);

        int left = 0;
        int right = items.Count - 1;
        List<T> foundItems = new List<T>();

        while (left <= right) // Loop through the list
        {
            int mid = (left + right) / 2; // Get the middle index
            T midItem = items[mid]; // Get the item at the middle index
            int comparisonResult = CompareBySearchTerm(midItem, searchTerm, sortBy); // Compare the item at the middle index with the search term

            if (comparisonResult == 0) // Found the item
            {
                // Add the found item to the list
                foundItems.Add(midItem);

                // Search for additional occurrences on both sides of the found item
                int currentIndex = mid - 1;
                while (currentIndex >= 0 && CompareBySearchTerm(items[currentIndex], searchTerm, sortBy) == 0)
                {
                    foundItems.Add(items[currentIndex]);
                    currentIndex--;
                }

                currentIndex = mid + 1;
                while (currentIndex < items.Count && CompareBySearchTerm(items[currentIndex], searchTerm, sortBy) == 0)
                {
                    foundItems.Add(items[currentIndex]);
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