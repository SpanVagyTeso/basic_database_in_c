using Microsoft.VisualStudio.TestTools.UnitTesting;
using testproject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testproject.Tests
{
    [TestClass]
    public class GameManagerTests
    {
        private GameManager gm;

        [TestInitialize]
        public void Initialize()
        {
            gm = new GameManager();
            gm.setupTable += new EventHandler(gm_setupTable);
            gm.GameOver += new EventHandler(gm_GO);
        }

        [TestMethod]
        public void modellInitGame()
        {
            gm.initGame(10);
            Assert.AreEqual(10, gm.Size);
            Assert.AreEqual(1, gm.getField(0, 0));
            int eggCount = 0;
            for (int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    if (gm.getField(i, j) == 2) eggCount++;
                }
            }
            Assert.AreEqual(1, eggCount);

        }

        [TestMethod]
        public void tickTest()
        {
            gm.initGame(10);
            Assert.AreEqual(1, gm.getField(0, 0));
            gm.tick();
            gm.tick();
            gm.tick();
            Assert.AreEqual(0, gm.getField(0, 0));
            Assert.AreEqual(0, gm.getField(2, 0));
        }

        [TestMethod]
        public void gm_GoTest()
        {
            gm.initGame(10);
            gm.tick();
            gm.tick();
            gm.tick();
            gm.tick();
            gm.tick();
            gm.tick();

        }

        public void gm_GO(Object sender, EventArgs e)
        {
            Assert.IsTrue((gm.getEggs() >= 0));
        }

        public void gm_setupTable(Object sender, EventArgs e)
        {

        }
    }
}