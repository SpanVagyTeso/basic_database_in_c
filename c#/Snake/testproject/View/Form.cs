using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace testproject
{
    public partial class Form1 : Form
    {
        #region fields
        int size;
        System.Timers.Timer myTimer = new System.Timers.Timer(1000);
        GameManager gm;
        private DataAccess.DataAccess _dataAccess;
        int state;
        bool timer;
        #endregion

        public Form1()
        {
            InitializeComponent();
            _dataAccess = new DataAccess.DataAccess();
            gm = new GameManager(_dataAccess);
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            myTimer.Enabled = true;
            myTimer.AutoReset = true;
            gm.GameOver += new EventHandler(gm_GameOver);
            KeyDown += new KeyEventHandler(Form_KeyDown);
            _panel.Paint += new PaintEventHandler(Panel_Paint);
            changeState(0);
            _menuNewGame.Click += new EventHandler(newGame);
            _menuFileLoadGame.Click += new EventHandler(loadGame);
            _menuFileSaveGame.Click += new EventHandler(saveGame);
            gm.setupTable += new EventHandler(loadInitTable);

        }

        public void initTable(int n)
        {
            gm.initGame(n);
            size = n;
            changeState(1);
        }

        public void loadInitTable(object sender, EventArgs e)
        {
            size = gm.Size;
            changeState(1);
        }

        private async void loadGame(object sender, EventArgs a)
        {
            Boolean restartTimer = myTimer.Enabled;
            myTimer.Stop();

            if (_openFileDialog.ShowDialog() == DialogResult.OK) // ha kiválasztottunk egy fájlt
            {
                try
                {
                    // játék betöltése
                    await gm.LoadGameAsync(_openFileDialog.FileName);
                    _menuFileSaveGame.Enabled = true;
                }
                catch (DataException e)
                {
                    MessageBox.Show("Játék betöltése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    initTable(0);
                    _menuFileSaveGame.Enabled = true;
                }
                
            }

            if (restartTimer)
                myTimer.Start();
        }

        private async void saveGame(object sender, EventArgs a)
        {
            Boolean restartTimer = myTimer.Enabled;
            myTimer.Stop();

            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // játé mentése
                    await gm.SaveGameAsync(_saveFileDialog.FileName);
                }
                catch (DataException)
                {
                    MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (restartTimer)
                myTimer.Start();
        }

        private async void Form_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    gm.changeWay(0);
                    break;
                case Keys.D:
                    gm.changeWay(1);
                    break;
                case Keys.S:
                    gm.changeWay(2);
                    break;
                case Keys.A:
                    gm.changeWay(3);
                    break;
            }
            if (!e.Control)
            {
                return;
            }
            switch (e.KeyCode)
            {
                case Keys.P:
                    if (timer)
                    {
                        myTimer.Stop();
                        timer = false;
                    }
                    else
                    {
                        myTimer.Start();
                        timer = true;
                    }
                    break;
                case Keys.N:
                    changeState(0);
                    break;
            }
        }

        private async void MenuFileLoadGame_Click(Object sender, EventArgs e)
        {
            Boolean restartTimer = myTimer.Enabled;
            myTimer.Stop();

            if (_openFileDialog.ShowDialog() == DialogResult.OK) // ha kiválasztottunk egy fájlt
            {
                try
                {
                    // játék betöltése
                    await gm.LoadGameAsync(_openFileDialog.FileName);
                    _menuFileSaveGame.Enabled = true;
                }
                catch (DataException)
                {
                    MessageBox.Show("Játék betöltése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    initTable(0);
                    _menuFileSaveGame.Enabled = true;
                }


            }

            if (restartTimer)
                myTimer.Start();
        }

        private void newGame(object sender, EventArgs a)
        {
            changeState(0);
        }

        private void gm_GameOver(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                changeState(0);
            }));
            MessageBox.Show("Meghaltál :c tojások száma: "+gm.getEggs().ToString());
            
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            gm.tick();
            BeginInvoke(new Action(()=>{
                _panel.Refresh();
            }));
            
        }

        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bitmap = new Bitmap(_panel.Width, _panel.Height); // kép a hatékony kirajzoláshoz

            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White); // háttér fehérré festése

            // játéktábla rácsai
            Int32 fieldWidth = _panel.Width / size;
            Int32 fieldHeight = _panel.Height / size;
            
            for(int i = 0; i <= size; i++)
            {
                graphics.DrawLine(Pens.Black, 0, fieldHeight*i, _panel.Width, fieldHeight*i);
                graphics.DrawLine(Pens.Black, fieldWidth*i, 0, fieldWidth*i, _panel.Height);
            }
            
            // a mezőtartalmak
            for (Int32 i = 0; i < size; i++)
                for (Int32 j = 0; j < size; j++)
                {
                    switch (gm.getField(j,i))
                    {
                        case 1:
                            graphics.FillEllipse(Brushes.Green, i * fieldWidth + fieldWidth / 10, j * fieldHeight + fieldHeight / 10, 8 * fieldWidth / 10, 8 * fieldHeight / 10);
                            break;
                        case 2:
                            graphics.DrawLine(new Pen(Color.Orange, _panel.Width / 50), i * fieldWidth + fieldWidth / 10, j * fieldHeight + fieldHeight / 10, i * fieldWidth + 9 * fieldWidth / 10, j * fieldHeight + 9 * fieldHeight / 10);
                            graphics.DrawLine(new Pen(Color.Orange, _panel.Width / 50), i * fieldWidth + 9 * fieldWidth / 10, j * fieldHeight + fieldHeight / 10, i * fieldWidth + fieldWidth / 10, j * fieldHeight + 9 * fieldHeight / 10);
                            break;
                    }
                }
            e.Graphics.DrawImage(bitmap, 0, 0);
            //_panel.DrawToBitmap(bitmap);
        }

        private void changeState(int _state)
        {
            state = _state;
            if (state == 0)
            {
                btn.Enabled = true;
                btn.Visible = true;
                // Console.WriteLine(list.IsDisposed);
                list.Visible = true;
                list.Enabled = true;
                
                myTimer.Stop();
                timer = false;
                _panel.Visible = false;
            }
            else if (state == 1)
            {
                btn.Enabled = false;
                btn.Visible = false;
                list.Visible = false;
                list.Enabled = false;
                
                
                
                _panel.Visible = true;
                myTimer.Start();
                timer = true;
            }
            
           
        }

        private void btn_Click(object sender, EventArgs e)
        {

            int n=list.SelectedIndex;
            Console.WriteLine(n);
            if (n == 0) n = 10;
            else if (n == 1) n = 20;
            else if (n == 2) n = 40;
            else return;
            initTable(n);
        }

        
    }
}
