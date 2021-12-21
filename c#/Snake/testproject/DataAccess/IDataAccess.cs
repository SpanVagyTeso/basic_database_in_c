using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testproject.DataAccess
{
    public interface IDataAccess
    {
        Task<SnakeTable> LoadAsync(String path);

        Task SaveAsync(String path, SnakeTable table);
    }
}
