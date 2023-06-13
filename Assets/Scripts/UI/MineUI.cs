using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineUI : UserInterface
{
    public void GoSmithyScene() { GameManager.sceneCtrl.GoStage(Enums.SceneIndex.Smithy); }

}
