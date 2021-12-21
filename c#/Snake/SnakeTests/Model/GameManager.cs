
using SnakeWPF.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;


namespace testproject
{
    

    public class GameManager
    {
        private int[,] table;
        SnakeTable snakeTable;
       // private IDataAccess _dataAccess;
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
        /*
        public GameManager(IDataAccess data)
        {
            snakeTable = new SnakeTable(0);
            _dataAccess = data;
        }*/
        public GameManager()
        {
            snakeTable = new SnakeTable(0);
        }


        public void changeWay(int _way)
        {
            Tuple temp = snakeTable.snake.Last.Value;

            switch (_way)
            {
                case 0:
                    temp.x -= 1;

                    break;
                case 1:
                    temp.y += 1;

                    break;
                case 2:
                    temp.x += 1;

                    break;
                case 3:
                    temp.y -= 1;
                    break;
            }
            if (snakeTable.snake.FindLast(temp) == null) ;
            else if (snakeTable.snake.FindLast(temp).Next == snakeTable.snake.Last) return;
            snakeTable.way = _way;
        }

        public void initGame(Difficulty d)
        {
            
            switch (d)
            {
                case Difficulty.Easy: initGame(10); break;
                case Difficulty.Medium: initGame(20); break;
                case Difficulty.Hard: initGame(40); break;
            }
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
            placeWalls();
            placeEgg();


        }

        public void placeWalls()
        {
            Tuple temp;
            temp = new Tuple(size / 2, size / 2);
            snakeTable.addWall(temp);
            table[size / 2, size / 2] = 3;
            temp = new Tuple(size / 2, size / 2 + 1);
            snakeTable.addWall(temp);
            table[size / 2, size / 2 + 1] = 3;
            temp = new Tuple(size / 2 + 1, size / 2);
            snakeTable.addWall(temp);
            table[size / 2 + 1, size / 2] = 3;
            temp = new Tuple(size / 2 + 1, size / 2 + 1);
            snakeTable.addWall(temp);
            table[size / 2 + 1, size / 2 + 1] = 3;

        }

        public void tick()
        {
            Console.WriteLine("Tick");
            Console.WriteLine("Snake Size:" + snakeTable.snake.Count);
            Tuple temp;
            if (!ate)
            {
                temp = snakeTable.snake.First.Value;
                table[temp.x, temp.y] = 0;
                snakeTable.snake.RemoveFirst();
            }
            ate = false;
            temp = snakeTable.snake.Last.Value;
            table[temp.x, temp.y] = 1;
            int collType = -1;
            switch (snakeTable.way)
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
            if (collType == 1)
            {
                GameOver(this, EventArgs.Empty);
                return;
            }
            else if (collType == 2)
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
            //Console.WriteLine("x: {0}, y: {1}", pos.x, pos.y);
            //Wall collision
            if (pos.x < 0 || pos.y < 0 || pos.x >= size || pos.y >= size)
            {
                Console.WriteLine("WallType1");
                return 1;
            }
            //Egg Collision
            if (pos == snakeTable.eggPos)
            {
                Console.WriteLine("Egg");
                return 2;
            }
            //Snake Collision
            foreach (var n in snakeTable.snake)
            {

                if (pos == n)
                {
                    Console.WriteLine("Snake");
                    return 1;
                }
            }
            ArrayList list = snakeTable.getWalls();
            for (int i = 0; i < list.Count; i++)
            {
                
                Tuple n = (Tuple)list[i];
                if (pos == n)
                {
                    Console.WriteLine("WallType2");
                    return 1;
                }
            }

            return 0;
        }

        public void test_placeEgg(int x, int y)
        {
            Tuple temp;
            do
            {

                temp = new Tuple(x, y);
            }
            while (checkCollision(temp) != 0);
            snakeTable.setEggPos(temp);
            Console.WriteLine("New egg pos: {0}, {1}", x, y);
            table[x, y] = 2;
        }

        private void placeEgg()
        {
            Random rand = new Random();
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
            for(int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++) 
                                table[i, j] = 0;
            }
            setupTable(this, EventArgs.Empty);
        }
        /*
        public async Task LoadGameAsync(String name)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");
            //snakeTable = new SnakeTable(0);
            snakeTable = await _dataAccess.LoadAsync(name);

            loadInitGame(snakeTable.Size);
            foreach (var n in snakeTable.snake)
            {
                table[n.x, n.y] = 1;
            }
            foreach (Tuple n in snakeTable.getWalls())
            {
                table[n.x, n.y] = 3;
            }
            table[snakeTable.eggPos.x, snakeTable.eggPos.y] = 2;

            setupTable(this, EventArgs.Empty);
        }
        public async Task SaveGameAsync(String name)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            await _dataAccess.SaveAsync(name, snakeTable);
        }

        public async Task<ICollection<SaveEntry>> ListGamesAsync()
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            return await _dataAccess.ListAsync();
        }
        /*
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
            foreach (var n in snakeTable.snake)
            {
                table[n.x, n.y] = 1;
            }
            foreach (Tuple n in snakeTable.getWalls())
            {
                table[n.x, n.y] = 3;
            }
            table[snakeTable.eggPos.x, snakeTable.eggPos.y] = 2;
        }
        */
    }
}
