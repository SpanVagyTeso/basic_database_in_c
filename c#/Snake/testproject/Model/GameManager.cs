using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using testproject.DataAccess;

namespace testproject
{
    public struct Tuple
    {
        public int x, y;
        public Tuple(int _x,int _y)
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

    public class GameManager
    {
        private int[,] table;
        SnakeTable snakeTable;
        private IDataAccess _dataAccess;
        int size;
        bool ate;
        

        public event EventHandler GameOver;
        public event EventHandler setupTable;

        public int Size { get => snakeTable.Size; }

        public int getField(int x, int y)
        {
            if (x < 0 || x >= table.Length || y < 0 || y >= table.Length) return -1;
            return table[x, y];
        }

        public GameManager(DataAccess.DataAccess data)
        {
            snakeTable = new SnakeTable(0);
            _dataAccess = data;
        }

        public void changeWay(int _way)
        {
            Tuple temp = snakeTable.snake.Last.Value;
            int collType = -1;
            switch (_way)
            {
                case 0:
                    temp.x -= 1;
                    collType = checkCollision(temp);
                    break;
                case 1:
                    temp.y += 1;
                    collType = checkCollision(temp);
                    break;
                case 2:
                    temp.x += 1;
                    collType = checkCollision(temp);
                    break;
                case 3:
                    temp.y -= 1;
                    collType = checkCollision(temp);
                    break;
            }
            if (collType == 1) return;
            snakeTable.way = _way;
        }

        public void initGame(int n)
        {
            size = n;
            snakeTable.eggDB = 0;
            table = new int[n, n];
            snakeTable = new SnakeTable(size);
            snakeTable.snake.AddLast(new Tuple(0, 0));
            table[0, 0] = 1;
            snakeTable.snake.AddLast(new Tuple(0, 1));
            table[0, 1] = 1;
            snakeTable.snake.AddLast(new Tuple(0, 2));
            table[0, 2] = 1;
            snakeTable.snake.AddLast(new Tuple(0, 3));
            table[0, 3] = 1;
            snakeTable.snake.AddLast(new Tuple(0, 4));
            table[0, 4] = 1;
            snakeTable.way = 1;
            placeEgg();

        }

        public void tick()
        {
            Console.WriteLine("Tick");
            Tuple temp;
            if (!ate) {
                temp = snakeTable.snake.First.Value;
                table[temp.x, temp.y] = 0;
                snakeTable.snake.RemoveFirst();
            }
            ate = false;
            temp = snakeTable.snake.Last.Value;
            table[temp.x, temp.y] = 1;
            int collType=-1;
            switch (snakeTable.way)
            {
                case 0:
                    temp.x -= 1;
                    collType=checkCollision(temp);
                    break;
                case 1:
                    temp.y += 1;
                    collType=checkCollision(temp);
                    break;
                case 2:
                    temp.x += 1;
                    collType=checkCollision(temp);
                    break;
                case 3:
                    temp.y -= 1;
                    collType=checkCollision(temp);
                    break;
            }
            if (collType == 1)
            {
                GameOver(this,EventArgs.Empty);
                return;
            }
            else if(collType == 2)
            {
                ate = true;
                snakeTable.eggDB++;
                placeEgg();
            }
            snakeTable.snake.AddLast(temp);
            table[temp.x, temp.y] = 1;

        }

        public int getEggs()
        {
            return snakeTable.eggDB;
        }

        private int checkCollision(Tuple pos)
        {
            Console.WriteLine("x: {0}, y: {1}",pos.x,pos.y);
            //Wall collision
            if(pos.x<0 || pos.y < 0 || pos.x>=size || pos.y >=size)
            {
                return 1;
            }
            //Egg Collision
            if (pos == snakeTable.eggPos)
            {
                return 2;
            }
            //Snake Collision
            foreach (var n in snakeTable.snake)
            {
                if (pos == n) return 1;
            }
            

            return 0;
        }
        
        private void placeEgg()
        {
            Random rand=new Random();
            int x, y;
            Tuple temp;
            do
            {
                x = rand.Next(0, size);
                y = rand.Next(0, size);
                temp = new Tuple(x, y);
            }
            while (checkCollision(temp) != 0);
            snakeTable.setEggPos(temp);
            Console.WriteLine("New egg pos: {0}, {1}", x, y);
            table[x, y] = 2;
        }

        private void loadInitGame(int n)
        {
            size = n;
            table = new int[n, n];
            setupTable(this, EventArgs.Empty);
        }

        public async Task SaveGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            await _dataAccess.SaveAsync(path, snakeTable);

        }

        public async Task LoadGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            snakeTable = await _dataAccess.LoadAsync(path);
            loadInitGame(snakeTable.Size);
            foreach(var n in snakeTable.snake)
            {
                table[n.x, n.y] = 1;
            }
            table[snakeTable.eggPos.x, snakeTable.eggPos.y]=2;
        }

    }
}
