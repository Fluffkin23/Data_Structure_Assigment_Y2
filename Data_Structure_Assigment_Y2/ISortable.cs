using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// This is an interface that represents a sortable object
public interface ISortable<T>
{
    void BubbleSort(SortBy sortBy);
    void InsertionSort(SortBy sortBy);
}