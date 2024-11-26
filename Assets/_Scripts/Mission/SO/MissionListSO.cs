using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionPanel", menuName = "CustomizedSO/MissionPanel")]
public class MissionListSO : ScriptableObject
{
    public List<MissionInformation> MissionList;

}
[System.Serializable]
public class MissionInformation
{
    [Header("������Ϣ")]
    public int MissionId;
    public string MissionName;
    public string ClientName;
    public string TimeLimit;
    [TextArea]
    public string MissionDetail;
}
