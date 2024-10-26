using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZToolKit
{
    internal interface IGameSave
    {
       Save LoadGame();
       void SaveGame(Save save);
    }
}
