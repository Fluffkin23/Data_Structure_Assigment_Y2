using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * This code defines a custom dictionary class called MyDictionary. 
 * The class is generic, meaning it can work with various types of keys and values. 
 * Specifically, it takes two generic parameters: TKey and TValue.
 * TKey represents the type of the key and TValue represents the type of the value. 
 * In this case, TValue is constrained to be a Book or a derived type of Book.
 * 
 * The MyDictionary class has an inner private class Entry to represent key-value pairs. 
 * It also has properties for getting the keys, values, count of entries, and whether the dictionary is read-only.
 * The class provides methods for standard dictionary operations, such as adding, removing, and checking for the existence of keys or key-value pairs. 
 * It also implements methods for copying the entries to an array and getting an enumerator for the dictionary.
 */

// This is a class that represents a dictionary
public class MyDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ILinearSearchable<TValue>, ISortable<TValue> where TValue : Book
{
    private List<Entry> entries;

    public MyDictionary()
    {
        entries = new List<Entry>();
    }

    private class Entry // This is a class that represents an entry in the dictionary
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public Entry(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    public TValue this[TKey key] // This is a property that represents the value of the dictionary
    {
        get
        {
            foreach (Entry entry in entries)
            {
                if (entry.Key.Equals(key))
                {
                    return entry.Value;
                }
            }
            throw new KeyNotFoundException(); // If the key is not found, throw an exception
        }
        set
        {
            for (int i = 0; i < entries.Count; i++) // if the keyy is not found replace the value
            {
                if (entries[i].Key.Equals(key))
                {
                    entries[i] = new Entry(key, value);
                    return;
                }
            }
            entries.Add(new Entry(key, value)); // If the key is not found, add it to the dictionary
        }
    }

    public ICollection<TKey> Keys // This is a property that represents the keys of the dictionary
    {
        get
        {
            List<TKey> keys = new List<TKey>(); // Create a list of keys
            foreach (Entry entry in entries)  // Add the keys to the list
            {
                keys.Add(entry.Key);
            }
            return keys;
        }
    }

    public ICollection<TValue> Values // This is a property that represents the values of the dictionary
    {
        get
        {
            List<TValue> values = new List<TValue>();
            foreach (Entry entry in entries)
            {
                values.Add(entry.Value);
            }
            return values;
        }
    }
    public int Count // This is a property that represents the number of entries in the dictionary
    {
        get { return entries.Count; } // Return the number of entries
    }

    public bool IsReadOnly // this is a property that represents if the dictionary is read only
    {
        get { return false; }
    }

    public void Add(TKey key, TValue value) // This is a method that adds an entry to the dictionary
    {
        for (int i = 0; i < entries.Count; i++) // If the key is already in the dictionary, throw an exception
        {
            if (entries[i].Key.Equals(key))
            {
                throw new ArgumentException("An item with the same key has already been added.");
            }
        }
        entries.Add(new Entry(key, value)); // If the key is not in the dictionary, add it
    }

    public bool ContainsKey(TKey key) // This is a method that checks if the dictionary contains a key
    {
        foreach (Entry entry in entries)
        {
            if (entry.Key.Equals(key))
            {
                return true;
            }
        }
        return false;
    }

    public bool Remove(TKey key) // This is a method that removes an entry from the dictionary
    {
        for (int i = 0; i < entries.Count; i++)
        {
            if (entries[i].Key.Equals(key))
            {
                entries.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public bool TryGetValue(TKey key, out TValue value) // This is a method that tries to get the value of a key
    {
        foreach (Entry entry in entries)
        {
            if (entry.Key.Equals(key))
            {
                value = entry.Value;
                return true;
            }
        }
        value = default(TValue);
        return false;
    }

    public void Add(KeyValuePair<TKey, TValue> item) // This is a method that adds an entry to the dictionary
    {
        Add(item.Key, item.Value);
    }

    public void Clear()
    {
        entries.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) // This is a method that checks if the dictionary contains an entry
    {
        foreach (Entry entry in entries)
        {
            if (entry.Key.Equals(item.Key) && entry.Value.Equals(item.Value))
            {
                return true;
            }
        }
        return false;
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) // This is a method that copies the entries to an array
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array)); // If the array is null, throw an exception
        }
        if (arrayIndex < 0 || arrayIndex >= array.Length) // If the array index is out of range, throw an exception
        {
            throw new ArgumentOutOfRangeException(nameof(arrayIndex)); // If the array index is out of range, throw an exception
        }
        if (array.Length - arrayIndex < Count) // If the number of items in the dictionary exceeds the capacity of the destination array, throw an exception
        {
            throw new ArgumentException("The number of items in the dictionary exceeds the capacity of the destination array."); // If the number of items in the dictionary exceeds the capacity of the destination array, throw an exception
        }
        foreach (Entry entry in entries) // Copy the entries to the array
        {
            array[arrayIndex++] = new KeyValuePair<TKey, TValue>(entry.Key, entry.Value); // Copy the entries to the array
        }
    }

    public bool Remove(KeyValuePair<TKey, TValue> item) // This is a method that removes an entry from the dictionary
    {
        for (int i = 0; i < entries.Count; i++) // If the entry is not in the dictionary, return false
        {
            if (entries[i].Key.Equals(item.Key) && entries[i].Value.Equals(item.Value)) // If the entry is in the dictionary, remove it and return true
            {
                entries.RemoveAt(i); // If the entry is in the dictionary, remove it and return true
                return true;
            }
        }
        return false;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() // This is a method that returns an enumerator that iterates through the dictionary
    {
        foreach (Entry entry in entries) // Return the entries
        {
            yield return new KeyValuePair<TKey, TValue>(entry.Key, entry.Value); // Return the entries
        }
    }

    IEnumerator IEnumerable.GetEnumerator() // This is a method that returns an enumerator that iterates through the dictionary
    {
        return GetEnumerator();
    }

    public void InsertionSort(SortBy sortBy) // This is a method that sorts the dictionary using insertion sort
    {
        int n = entries.Count; // Get the number of entries

        for (int i = 1; i < n; i++) // Sort the entries
        {
            Entry key = entries[i]; // Sort the entries
            Book keyBook = key.Value as Book; // Sort the entries
            int j = i - 1;

            while (j >= 0 && Compare(entries[j].Value as Book, keyBook, sortBy) > 0) // Sort the entries
            {
                entries[j + 1] = entries[j]; // Sort the entries
                j--;
            }
            entries[j + 1] = key; // Sort the entries
        }
    }

    private int Compare(Book book1, Book book2, SortBy sortBy) // This is a method that compares two books
    {
        switch (sortBy)
        {
            case SortBy.Title:
                return book1.Title.CompareTo(book2.Title); // Compare the books
            case SortBy.Author:
                return book1.Author.CompareTo(book2.Author); // Compare the books
            case SortBy.Year:
                return book1.Year.CompareTo(book2.Year); // Compare the books
            case SortBy.Genre:
                return book1.Genre.CompareTo(book2.Genre); // Compare the books
            case SortBy.Publisher:
                return book1.Publisher.CompareTo(book2.Publisher); // Compare the books
            default:
                throw new ArgumentException("Invalid sort criteria."); // If the sort criteria is invalid, throw an exception
        }
    }

    public IEnumerable<TValue> LinearSearch(SortBy sortBy, string searchTerm) // This is a method that searches the dictionary using linear search
    {
        List<Book> matchingBooks = new List<Book>(); // Create a list of matching books
        foreach (var entry in entries)
        {
            Book book = entry.Value as Book;
            if (book != null)
            {
                switch (sortBy)
                {
                    case SortBy.Title:
                        if (book.Title.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)) matchingBooks.Add(book);  // If the book matches the search term, add it to the list of matching books
                        break;
                    case SortBy.Author:
                        if (book.Author.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)) matchingBooks.Add(book); // If the book matches the search term, add it to the list of matching books
                        break;
                    case SortBy.Year:
                        if (int.TryParse(searchTerm, out int year) && book.Year == year) matchingBooks.Add(book); // If the book matches the search term, add it to the list of matching books
                        break;
                    case SortBy.Genre:
                        if (book.Genre.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)) matchingBooks.Add(book); // If the book matches the search term, add it to the list of matching books
                        break;
                    case SortBy.Publisher:
                        if (book.Publisher.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)) matchingBooks.Add(book); // If the book matches the search term, add it to the list of matching books
                        break;
                    default:
                        throw new ArgumentException("Invalid search criterion", nameof(sortBy)); // If the search criterion is invalid, throw an exception
                }
            }
        }
        return matchingBooks.Cast<TValue>(); // Return the list of matching books
    }

    public void BubbleSort(SortBy sortBy) // This is a method that sorts the dictionary using bubble sort
    {
        int n = entries.Count;
        bool swapped;

        for (int i = 0; i < n - 1; i++) // Sort the entries
        {
            swapped = false; // Sort the entries
            for (int j = 0; j < n - i - 1; j++) // Sort the entries
            {
                Book book1 = entries[j].Value as Book; // Sort the entries
                Book book2 = entries[j + 1].Value as Book; // Sort the entries

                if (Compare(book1, book2, sortBy) > 0) // Sort the entries
                {
                    Entry temp = entries[j]; // Sort the entries
                    entries[j] = entries[j + 1];    // Swap entries[j] and entries[j+1]
                    entries[j + 1] = temp; // Sort the entries
                    swapped = true; // Sort the entries
                }
            }

            if (!swapped) // If no two elements were swapped by inner loop, then break
                break;
        }

    }

    public List<TValue> BinarySearch(SortBy sortBy, string searchTerm) // This is a method that searches the dictionary using binary search
    {
        // Sort entries using BubbleSort
        BubbleSort(sortBy);

        // Perform binary search
        int left = 0;
        int right = entries.Count - 1;
        List<TValue> foundItems = new List<TValue>();

        while (left <= right)
        {
            int middle = (left + right) / 2;
            string currentSearchValue;

            switch (sortBy)
            {
                case SortBy.Title:
                    currentSearchValue = entries[middle].Value.Title; // Get the value of the middle entry
                    break;
                case SortBy.Author:
                    currentSearchValue = entries[middle].Value.Author;
                    break;
                case SortBy.Year:
                    currentSearchValue = entries[middle].Value.Year.ToString();
                    break;
                case SortBy.Genre:
                    currentSearchValue = entries[middle].Value.Genre;
                    break;
                case SortBy.Publisher:
                    currentSearchValue = entries[middle].Value.Publisher;
                    break;
                default:
                    throw new ArgumentException("Invalid sortBy value.");
            }

            int comparisonResult = searchTerm.CompareTo(currentSearchValue);

            if (comparisonResult == 0)
            {
                // Add the found item to the list
                foundItems.Add(entries[middle].Value);

                // Search for additional occurrences on both sides of the found item
                int currentIndex = middle - 1;
                while (currentIndex >= 0 && searchTerm.CompareTo(GetFieldValue(entries[currentIndex], sortBy)) == 0) // This is a method that searches the dictionary using binary search
                {
                    foundItems.Add(entries[currentIndex].Value); // This is a method that searches the dictionary using binary search
                    currentIndex--; // This is a method that searches the dictionary using binary search
                }

                currentIndex = middle + 1; // Reset the index
                while (currentIndex < entries.Count && searchTerm.CompareTo(GetFieldValue(entries[currentIndex], sortBy)) == 0) // This is a method that searches the dictionary using binary search
                {
                    foundItems.Add(entries[currentIndex].Value); // This is a method that searches the dictionary using binary search
                    currentIndex++; // This is a method that searches the dictionary using binary search
                }

                // Break the loop as we have found all occurrences
                break;
            }
            else if (comparisonResult < 0)
            {
                right = middle - 1;
            }
            else
            {
                left = middle + 1;
            }
        }

        return foundItems;
    }

    private string GetFieldValue(Entry entry, SortBy sortBy) // This is a method that gets the field value
    {
        switch (sortBy)
        {
            case SortBy.Title: // This is a method that gets the field value
                return entry.Value.Title; // This is a method that gets the field value
            case SortBy.Author:
                return entry.Value.Author;
            case SortBy.Year:
                return entry.Value.Year.ToString();
            case SortBy.Genre:
                return entry.Value.Genre;
            case SortBy.Publisher:
                return entry.Value.Publisher;
            default:
                throw new ArgumentException("Invalid sortBy value."); // This is a method that gets the field value
        }
    }
}


