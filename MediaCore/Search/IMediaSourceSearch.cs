using System.Collections.Generic;

namespace MediaCore.Search
{
    public interface IMediaSourceSearch
    {
        IEnumerable<object> Search(string query, int category = (int) SearchCategory.All, int sortBy = (int) SearchSortBy.Title,
            int sortOrder = (int) SearchSortOrder.Ascending);
    }
}