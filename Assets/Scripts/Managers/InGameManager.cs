using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : SubManager
{
    private Player player;
    public Player Player => player;

    public override void ManagerInitailized()
    {
    }

    public override void SettingManagerForNextScene(int nextSceneIndex)
    {
        if (!isActivated) SetActivated(true);
    }

}
