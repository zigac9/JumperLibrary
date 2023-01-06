using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JumperLibrary;

public class BoardsCoords
{
    public BoardsCoords()
    {
        BoardsCoordsArr = new List<Rectangle>();
        GoneBoardsCoordsArr = new List<Rectangle>();
        FakeBoardsCoordsArr = new List<Rectangle>();
        MovingBoardsCoordsArr = new List<Rectangle>();
        BoardsCoordsArr.Add(new Rectangle(40, 700, 80, 14));
        BoardsCoordsArr.Add(new Rectangle(240, 640, 60, 14));
        BoardsCoordsArr.Add(new Rectangle(400, 620, 80, 14));
        BoardsCoordsArr.Add(new Rectangle(250, 600, 60, 14));
        BoardsCoordsArr.Add(new Rectangle(290, 570, 80, 14));
        BoardsCoordsArr.Add(new Rectangle(240, 550, 60, 14));
        BoardsCoordsArr.Add(new Rectangle(200, 580, 80, 14));
        BoardsCoordsArr.Add(new Rectangle(160, 530, 60, 14));
        BoardsCoordsArr.Add(new Rectangle(100, 500, 80, 14));
        BoardsCoordsArr.Add(new Rectangle(180, 460, 60, 14));
        BoardsCoordsArr.Add(new Rectangle(270, 430, 80, 14));
        BoardsCoordsArr.Add(new Rectangle(310, 390, 60, 14));
        BoardsCoordsArr.Add(new Rectangle(350, 360, 80, 14));
        BoardsCoordsArr.Add(new Rectangle(310, 320, 60, 14));
        BoardsCoordsArr.Add(new Rectangle(310, 290, 80, 14));
        BoardsCoordsArr.Add(new Rectangle(200, 250, 60, 14));
        BoardsCoordsArr.Add(new Rectangle(160, 220, 80, 14));
        BoardsCoordsArr.Add(new Rectangle(140, 180, 60, 14));
        BoardsCoordsArr.Add(new Rectangle(400, 140, 80, 14));
        BoardsCoordsArr.Add(new Rectangle(360, 110, 60, 14));
        BoardsCoordsArr.Add(new Rectangle(300, 70, 80, 14));
        BoardsCoordsArr.Add(new Rectangle(240, 40, 60, 14));

        GoneBoardsCoordsArr.Add(new Rectangle(100, 263, 60, 14));
        GoneBoardsCoordsArr.Add(new Rectangle(30, 330, 80, 14));
        GoneBoardsCoordsArr.Add(new Rectangle(410, -50, 60, 14));
        GoneBoardsCoordsArr.Add(new Rectangle(74, 660, 80, 14));

        MovingBoardsCoordsArr.Add(new Rectangle(100, 335, 60, 14));
        MovingBoardsCoordsArr.Add(new Rectangle(250, 15, 80, 14));
        MovingBoardsCoordsArr.Add(new Rectangle(0, -20, 60, 14));
        MovingBoardsCoordsArr.Add(new Rectangle(400, 630, 60, 14));

        FakeBoardsCoordsArr.Add(new Rectangle(420, 435, 80, 14));
        FakeBoardsCoordsArr.Add(new Rectangle(50, 120, 60, 14));
        FakeBoardsCoordsArr.Add(new Rectangle(20, -35, 80, 14));
        FakeBoardsCoordsArr.Add(new Rectangle(250, 525, 60, 14));
    }

    public List<Rectangle> BoardsCoordsArr { get; set; }

    public List<Rectangle> FakeBoardsCoordsArr { get; set; }

    public List<Rectangle> GoneBoardsCoordsArr { get; set; }

    public List<Rectangle> MovingBoardsCoordsArr { get; set; }
}