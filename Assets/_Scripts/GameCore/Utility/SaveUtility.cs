using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using QFramework;
using UnityEngine;
using ZToolKit;

[AutoRegister(typeof(GameCoreMgr))]
public class SaveUtility : 
    IUtility,
    ICanSendEvent
{
    private SaveConfig mConfig;
    
    private Dictionary<string, Archive> mArchives => mConfig.archives;

    private Archive CurArchive => mArchives[curSave.name];
    
    public Save curSave;

    
    /// <summary>
    /// 初始化
    /// </summary>
    public void InitCfg()
    {
        mConfig = SaveTool.ReadConfig();
        
    }

    /// <summary>
    /// 获得默认存档名称
    /// </summary>
    /// <returns></returns>
    public string GetDefaultSaveName()
    {
        return DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
    }
    
    /// <summary>
    /// 保存当前存档
    /// </summary>
    public void SaveCurrent(string name)
    {
        var arc = mArchives[curSave.archive];

        if (!arc.TryAddSave(name))//没有重名
        {
            SaveTool.WriteSave(curSave);
            if (arc.Count() > GameConfig.SaveCountInAnArchive)
            {
                arc.RemoveTheOldest();
                this.SendEvent(new EvtArchivesChanged(mArchives, true));
            }
        }
        else
        {
            this.SendEvent(new EvtSaveNameRepeat());
        }
    }

    /// <summary>
    /// 删除存档
    /// </summary>
    /// <param name="archiveName"></param>
    /// <param name="saveName"></param>
    public void DeleteSave(string archiveName, string saveName)
    {
        var arc = mArchives[archiveName];
        arc.RemoveSave(saveName);
        SaveTool.DeleteSave(archiveName,saveName);

        if (arc.Count() <= 0)
        {
            mConfig.archives.Remove(archiveName);
        }
        
        SaveTool.WriteArchive(mConfig);
        this.SendEvent(new EvtArchivesChanged(mArchives, true));
    }

    public IArchitecture GetArchitecture()
    {
        return GameCoreMgr.Interface;
    }
}