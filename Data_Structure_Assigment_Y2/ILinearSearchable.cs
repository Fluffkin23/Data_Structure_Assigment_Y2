using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// This is an interface that represents a linear searchable object
public interface ILinearSearchable<T>
{
    IEnumerable<T> LinearSearch(SortBy sortBy, string searchTerm);
    List<T> BinarySearch(SortBy sortBy, string searchTerm);

}