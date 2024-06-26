﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace JumperLibrary;

public class ScoreManager
{
    private ScoreManager()
        : this(new List<Score>())
    {
    }

    private ScoreManager(List<Score> scores)
    {
        Scores = scores;
        Highscores = new List<int>(5) { 0, 0, 0, 0, 0 };

        UpdateHighscores();
    }

    public List<int> Highscores { get; }

    public List<Score> Scores { get; set; }

    public void Add(Score score, string playername)
    {
        var exists = Exists(playername);
        if (exists)
        {
            var index = Scores.FindIndex(a => a.PlayerName == playername);
            if (Scores[index].Value < score.Value)
                Scores[index].Value = score.Value;
        }
        else
        {
            Scores.Add(score);
        }

        Scores = Scores.OrderByDescending(c => c.Value).ToList();

        UpdateHighscores();
    }

    public bool Exists(string playerName)
    {
        return Scores.Any(item => item.PlayerName == playerName);
    }

    public int BestOfYou(string playername)
    {
        List<Score> bestofyou;
        bestofyou = Scores.Where(name => name.PlayerName == playername).OrderByDescending(c => c.Value).ToList();
        return bestofyou.Count > 0 ? bestofyou[0].Value : 0;
    }

    public static ScoreManager Load(string fileName)
    {
        // If there isn't a file to load - create a new instance of "ScoreManager"
        var virusJumpDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VirusJump");
        // Check if the VirusJump directory exists, and create it if it doesn't
        if (!Directory.Exists(virusJumpDirectory))
        {
            Directory.CreateDirectory(virusJumpDirectory);
        }
        var filePath = Path.Combine(virusJumpDirectory, fileName);

        if (!File.Exists(filePath))
            return new ScoreManager();

        // Otherwise we load the file
        using var reader = new StreamReader(new FileStream(filePath, FileMode.Open));
        var serilizer = new XmlSerializer(typeof(List<Score>));

        var scores = (List<Score>)serilizer.Deserialize(reader);

        return new ScoreManager(scores);
    }

    private void UpdateHighscores()
    {
        if (Scores.Count > 0)
            Highscores[0] = Scores[0].Value;
        else
            Highscores[0] = 0;
        if (Scores.Count > 1)
            Highscores[1] = Scores[1].Value;
        else
            Highscores[1] = 0;
        if (Scores.Count > 2)
            Highscores[2] = Scores[2].Value;
        else
            Highscores[2] = 0;
        if (Scores.Count > 3)
            Highscores[3] = Scores[3].Value;
        else
            Highscores[3] = 0;
        if (Scores.Count > 4)
            Highscores[4] = Scores[4].Value;
        else
            Highscores[4] = 0;
    }

    public static void Save(ScoreManager scoreManager, string fileName)
    {
        var virusJumpDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VirusJump");
        var filePath = Path.Combine(virusJumpDirectory, fileName);
        
        // Overrides the file if it alreadt exists
        using var writer = new StreamWriter(new FileStream(filePath, FileMode.Create));
        var serilizer = new XmlSerializer(typeof(List<Score>));

        serilizer.Serialize(writer, scoreManager.Scores);
    }
}