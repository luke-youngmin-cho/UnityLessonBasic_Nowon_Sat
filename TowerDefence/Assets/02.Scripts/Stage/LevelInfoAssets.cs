using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ���� ���� ������ ������ �������� Ŭ����
/// </summary>
public class LevelInfoAssets : MonoBehaviour
{
    private static LevelInfoAssets _instance;
    public static LevelInfoAssets instance
    {
        get
        {
            if (_instance == null)
                _instance = Instantiate(Resources.Load<LevelInfoAssets>("Assets/LevelInfoAssets"));
            return _instance;
        }
    }

    public List<LevelInfo> levelInfos = new List<LevelInfo>();

    /// <summary>
    /// Ư�� ������ Ư�� �������� ������ ��ȯ��
    /// </summary>
    /// <param name="level"> �˻��� ���� </param>
    /// <param name="stage"> �˻��� �������� </param>
    /// <returns></returns>
    public static StageInfo GetStageInfo(int level, int stage)
    {
        // ã�����ϴ� ���� ���� �˻�
        LevelInfo levelInfo = instance.levelInfos.Find(x => x.level == level);

        // ���� ���� �˻� ������
        if (levelInfo != null)
        {
            // �ش� ������ ã�����ϴ� �������� ���� �˻�
            return levelInfo.stageInfos.Find(x => x.stage == stage);
        }
        return null;
    }

    /// <summary>
    /// Ư�� ������ ��� �������� ������ ��ȯ
    /// </summary>
    /// <param name="level"> �˻��� ����</param>
    /// <returns></returns>
    public static StageInfo[] GetAllStageInfo(int level)
    {
        // ã�����ϴ� ���� ���� �˻�
        LevelInfo levelInfo = instance.levelInfos.Find(x => x.level == level);

        // ���� ���� �˻� ������
        if (levelInfo != null)
        {
            return levelInfo.stageInfos.ToArray();
        }
        return null;
    }
}