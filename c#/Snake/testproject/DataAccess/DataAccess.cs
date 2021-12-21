using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testproject.DataAccess
{
    public class DataAccess : IDataAccess
    {

        public async Task<SnakeTable> LoadAsync(String path)
        {
            Console.WriteLine("Eljut");
            try
            {
                using (StreamReader reader = new StreamReader(path)) 
                {
                    String line = await reader.ReadLineAsync();
                    String[] numbers = line.Split(' ');
                    Int32 tableSize = Int32.Parse(numbers[0]);
                    int way = Int32.Parse(numbers[1]);
                    SnakeTable table = new SnakeTable(tableSize);
                    table.way = way;

                    line = await reader.ReadLineAsync();
                    numbers = line.Split(' ');
                    String[] num;
                    num = numbers[0].Split(',');
                    Tuple eggpos=new Tuple(Int32.Parse( num[0]),Int32.Parse(num[1]));
                    table.eggDB = Int32.Parse(numbers[1]);
                    table.setEggPos(eggpos);

                    line = await reader.ReadLineAsync();
                            
                    numbers = line.Split(' ');

                    for (Int32 i = 0; i < numbers.Length-1; i++)
                    {
                        num = numbers[i].Split(',');
                        table.addSnakeElem(new Tuple(Int32.Parse(num[0]), Int32.Parse(num[1])));
                    }
                    
                    return table;
                }
            }
            catch
            {
                throw new DataException();
            }
        }

        public async Task SaveAsync(String path, SnakeTable table)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path)) // fájl megnyitása
                {
                    writer.WriteLine(table.Size+" "+table.way); // kiírjuk a méreteket
                    await writer.WriteLineAsync(table.eggPos.ToString()+ " "+table.eggDB);
                    foreach (var n in table.snake)
                    {
                        writer.Write(n.ToString() + " ");
                    }
                }
            }
            catch
            {
                throw new DataException();
            }
        }

    }
}
