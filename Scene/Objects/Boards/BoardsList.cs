using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumperLibrary;

public class BoardsList
{
    private readonly BoardsCoords _boardsCoords;

    private readonly Dictionary<string, Texture2D> _textures;

    public BoardsList(Dictionary<string, Texture2D> textures)
    {
        _textures = textures;
        _boardsCoords = new BoardsCoords();
        Initialize();
    }

    public Board[] BoardList { get; private set; }

    public FakeBoard[] FakeBoardList { get; private set; }

    public GoneBoard[] GoneBoardList { get; private set; }

    public MovingBoard[] MovingBoardList { get; private set; }

    public void Initialize()
    {
        BoardList = new Board[_boardsCoords.BoardsCoordsArr.Count];
        FakeBoardList = new FakeBoard[_boardsCoords.FakeBoardsCoordsArr.Count];
        GoneBoardList = new GoneBoard[_boardsCoords.GoneBoardsCoordsArr.Count];
        MovingBoardList = new MovingBoard[_boardsCoords.MovingBoardsCoordsArr.Count];
        FillArrays();
    }

    private void FillArrays()
    {
        for (var i = 0; i < BoardList.Length; i++)
            BoardList[i] = new Board(_textures["assets/p1"], _boardsCoords.BoardsCoordsArr[i]);
        for (var i = 0; i < FakeBoardList.Length; i++)
            FakeBoardList[i] = new FakeBoard(_textures["assets/p3"], _boardsCoords.FakeBoardsCoordsArr[i]);
        for (var i = 0; i < GoneBoardList.Length; i++)
            GoneBoardList[i] = new GoneBoard(_textures["assets/p4"], _boardsCoords.GoneBoardsCoordsArr[i]);
        for (var i = 0; i < MovingBoardList.Length; i++)
        {
            var rand = new Random();
            var num = rand.Next(-4, 4);
            if (num == 0) num = 1;
            MovingBoardList[i] =
                new MovingBoard(_textures["assets/p2"], _boardsCoords.MovingBoardsCoordsArr[i], num);
        }
    }

    public void Collision(bool thingsCollisionCheck, bool collisionCheck, bool gameOver, Player player, Sound sound, bool SoundEffectCheck)
    {
        if (thingsCollisionCheck)
        {
            foreach (var board in BoardList)
                if (board.Visible && board.Collision(player) && !gameOver && collisionCheck)
                {
                    player.Speed = new Vector2(player.Speed.X, -13);
                    if (SoundEffectCheck) sound.Board.Play();
                }

            for (var i = 0; i < MovingBoardList.Length; i++)
            {
                if (MovingBoardList[i].Collision(player) && !gameOver && collisionCheck)
                {
                    player.Speed = new Vector2(player.Speed.X, -13);
                    if (SoundEffectCheck) sound.Board.Play();
                }

                if (FakeBoardList[i].Visible && FakeBoardList[i].Collision(player) &&
                    !gameOver && collisionCheck)
                    FakeBoardList[i].Visible = false;

                if (GoneBoardList[i].Visible && GoneBoardList[i].Collision(player) &&
                    !gameOver && collisionCheck)
                {
                    player.Speed = new Vector2(player.Speed.X, -13);
                    if (SoundEffectCheck) sound.Board.Play();
                    GoneBoardList[i].Visible = false;
                }
            }
        }
    }
}