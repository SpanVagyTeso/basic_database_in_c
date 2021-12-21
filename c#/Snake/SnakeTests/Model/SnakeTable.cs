using System;
using System.Collections;
using System.Collections.Generic;

namespace testproject
{
    public class SnakeTable
    {
        private LinkedList<Tuple> _snake;
        private Tuple eggpos;
        private ArrayList walls;
        readonly int tableSize;
        private int _eggDB;
        private int _way;

        public int way { get => _way; set => _way = value; }

        public int Size { get { return tableSize; } }

        public int eggDB { get { return _eggDB; } set { if (value < 0) return; else _eggDB = value; } }

        public void addWall(Tuple wall)
        {
            walls.Add(wall);
        }

        public ArrayList getWalls()
        {
            return walls;
        }

        public Tuple eggPos { get { return eggpos; } }

        public SnakeTable(int _tablesize)
        {
            tableSize = _tablesize;
            _snake = new LinkedList<Tuple>();
            walls = new ArrayList();
            eggpos = new Tuple();
        }

        public LinkedList<Tuple> snake { get { return _snake; } }

        public void addSnakeElem(Tuple x)
        {
            if (x.x < 0 || x.y < 0 || x.x >= tableSize || x.y >= tableSize)
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

    public struct Tuple
    {
        public int x, y;
        public Tuple(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
        public static Boolean operator ==(Tuple lhs, Tuple rhs)
        {
            if (lhs.x == rhs.x && lhs.y == rhs.y) return true;
            return false;
        }
        public static Boolean operator !=(Tuple lhs, Tuple rhs)
        {
            return !(lhs == rhs);
        }
        public override string ToString()
        {
            return x.ToString() + "," + y.ToString();
        }
    }
}
