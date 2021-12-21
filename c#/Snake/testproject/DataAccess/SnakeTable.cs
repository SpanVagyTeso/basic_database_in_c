using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testproject.DataAccess
{
    public class SnakeTable
    {
        private LinkedList<Tuple> _snake;
        private Tuple eggpos;
        readonly int tableSize;
        private int _eggDB;
        private int _way;

        public int way { get => _way; set => _way = value; }

        public int Size { get { return tableSize; } }
     
        public int eggDB { get { return _eggDB; } set { if (value < 0) return; else _eggDB = value; } }

        public Tuple eggPos { get { return eggpos; } }

        public SnakeTable(int _tablesize)
        {
            tableSize = _tablesize;
            _snake = new LinkedList<Tuple>();
        }

        public LinkedList<Tuple> snake { get { return _snake; } }

        public void addSnakeElem(Tuple x)
        {
            if(x.x <0 || x.y<0|| x.x >=tableSize || x.y >= tableSize)
            {
                throw new ArgumentOutOfRangeException("x or y", "One of the coordinates is out of range.");
            }
            _snake.AddLast(x);
        }

        public void setEggPos(Tuple x)
        {
            if (x.x < 0 || x.y < 0 || x.x >= tableSize || x.y >= tableSize)
            {
                throw new ArgumentOutOfRangeException("x or y", "One of the coordinates is out of range.");
            }
            eggpos = x;
        }

    }
}
