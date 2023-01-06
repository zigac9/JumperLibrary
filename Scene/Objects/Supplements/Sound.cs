using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace JumperLibrary;

public class Sound
{
    private bool _check;

    public Sound(IReadOnlyDictionary<string, SoundEffect> soundEffectsDictionary,
        IReadOnlyDictionary<string, Song> songDictionary)
    {
        Background = songDictionary["assets/background"];
        End = songDictionary["assets/patmat"];
        Board = soundEffectsDictionary["assets/jump"];
        PlayerShoot = soundEffectsDictionary["assets/shootPlayer"];
        EnemyShoot = soundEffectsDictionary["assets/enemyShot"];
        Dead = soundEffectsDictionary["assets/deadSound"];
        Initialize();
    }

    public bool PlayCheck { get; set; }

    public Song Background { get; }

    public Song End { get; }

    public SoundEffect Board { get; }

    public SoundEffect PlayerShoot { get; }

    public SoundEffect EnemyShoot { get; }

    public SoundEffect Dead { get; }

    public void Initialize()
    {
        _check = true;
        PlayCheck = true;
    }
}