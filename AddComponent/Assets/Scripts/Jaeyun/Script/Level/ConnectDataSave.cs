using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects.Jaeyun.Script.Development_Tool;

namespace UnityTemplateProjects.Jaeyun.Script.Level
{
    public class ConnectDataSave : MonoBehaviour
    {
        public class ConnectData
        {
            public int componentButtonIndex;
            public int actorIndex;

            public ConnectData(Drag drag, ConnectDataInfo connectDataInfo)
            {
                componentButtonIndex = connectDataInfo.GetIndex(drag.GetButton());
                actorIndex = connectDataInfo.GetIndex(drag.GetActor());
            }
        }
        
        public List<ConnectData> connectDatas = new List<ConnectData>();


        public void SaveConnectData()
        {
            connectDatas.Clear();

            var drags = FindObjectsOfType<Drag>();
            var connectInfo = FindObjectOfType<ConnectDataInfo>();
            foreach (var drag in drags)
            {
                var newConnectData = new ConnectData(drag, connectInfo);
                connectDatas.Add(newConnectData);
            }
        }

        public void LoadConnectData()
        {
            var connectInfo = FindObjectOfType<ConnectDataInfo>();
            foreach (var connectData in connectDatas)
            {
                var actor = connectInfo.GetActor(connectData.actorIndex);
                var button = connectInfo.GetButton(connectData.componentButtonIndex);
                var drag = button.LinkToDrag();
                
                drag.LinkToActor(actor);
            }
        }
        
    }
}