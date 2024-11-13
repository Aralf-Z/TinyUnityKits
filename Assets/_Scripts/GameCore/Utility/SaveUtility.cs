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

    private string CurArchive => mConfig.curArchive;

    public bool CanLoadCurArchiveNewArchive =>
        mConfig.archives.ContainsKey(CurArchive) && mConfig.archives[CurArchive].saves.Count > 0;
    
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
    /// 加载当前档案最新存档
    /// </summary>
    public void LoadCurArchiveLastSave()
    {
        var curArc = mConfig.archives[CurArchive];
        curSave = SaveTool.ReadSave<Save>(curArc, curArc.saves.FirstOrDefault());
    }

    /// <summary>
    /// 加载存档
    /// </summary>
    /// <param name="archiveName"></param>
    /// <param name="saveName"></param>
    public void LoadSave(string archiveName, string saveName)
    {
        // ReSharper disable once RedundantCheckBeforeAssignment
        if (mConfig.curArchive != archiveName)
        {
            mConfig.curArchive = archiveName;
            SaveTool.WriteConfig(mConfig);
        }
        curSave = SaveTool.ReadSave<Save>(mConfig.archives[archiveName], saveName);
    }
    
    /// <summary>
    /// 保存当前存档
    /// </summary>
    public void SaveCurrent(string name)
    {
        var arc = mArchives[curSave.archive];
        var saves = arc.saves;

        if (string.IsNullOrEmpty(name))
        {
            this.SendEvent(new EvtNameEmpty());
        }
        else if (saves.Contains(name))
        {
            this.SendEvent(new EvtNameRepeat());
        }
        else
        {
            SaveTool.WriteSave(curSave);
            saves.Add(name);
            if (saves.Count > arc.maxCount)
            {
                saves.RemoveAt(0);
            }
            this.SendEvent(new EvtArchivesChanged(mArchives, true));
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
        var saves = arc.saves;

        //移出存档列表，删除存档文件
        saves.Remove(saveName);
        SaveTool.DeleteSave(archiveName,saveName);

        //档案存档数量归0，删除档案，当前存档归空
        if (saves.Count <= 0)
        {
            mConfig.archives.Remove(archiveName);
            SaveTool.WriteConfig(mConfig);

            if (mConfig.curArchive == archiveName)
            {
                mConfig.curArchive = string.Empty;
            }
        }

        this.SendEvent(new EvtArchivesChanged(mArchives, true));
    }

    /// <summary>
    /// 新建档案
    /// </summary>
    /// <param name="name"></param>
    public void NewArchive(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            this.SendEvent(new EvtNameEmpty());
        }
        else if (mConfig.archives.ContainsKey(name))
        {
            this.SendEvent(new EvtNameRepeat());
        }
        else
        {
            mConfig.archives.Add(name, new Archive(name));
            this.SendEvent(new EvtArchivesChanged(mArchives, true));
        }
    }
    
    /// <summary>
    /// 删除档案
    /// </summary>
    /// <param name="name"></param>
    public void DeleteArchive(string name)
    {
        mConfig.archives.Remove(name);
        this.SendEvent(new EvtArchivesChanged(mArchives, true));
    }

    public IArchitecture GetArchitecture()
    {
        return GameCoreMgr.Interface;
    }
}