#if UNITY_EDITOR || BALANCE_UPDATER
using System;
using System.Collections.Generic;
using MergeMarines;

public partial class BalanceImporter
{
    private class QuestDataImport : DataImport
    {
        public QuestDataImport(string dataUrl, string dataName) : base(dataUrl, dataName)
        {
        }

        protected override void ImportImpl(string dataName, string[] lines, ref int curLine)
        {
            string[] columnNames = new string[0];
            string[] valuesStr;

            var data = ParseSettingsJson<Dictionary<string, QuestData>>(dataName);
            data.Clear();

            QuestData questData = null;

            for (curLine++; curLine < lines.Length && lines[curLine].Trim().Length > 0; curLine++)
            {
                valuesStr = lines[curLine].Split('\t');

                string className = valuesStr[0];
                if (className.Trim().Length > 0)
                {
                    columnNames = lines[curLine].Split('\t');
                    continue;
                }

                string typeStr = valuesStr[1];

                if (typeStr.Trim().Length > 0 && columnNames.Length > 0)
                {
                    string dataClassName = "MergeMarines." + columnNames[0];
                    var questDataType = Type.GetType(dataClassName);

                    if (questDataType == null)
                    {
                        throw new Exception(
                            $"[{typeof(QuestDataImport)}] Import: Type {dataClassName} for {typeStr} enemy was not found!");
                    }

                    questData = Activator.CreateInstance(questDataType) as QuestData;
                    data.Add(typeStr, questData);

                    var type = valuesStr[1].Split("_").First();
                    ImportUtility.ParseAndSetPropertyValue(questData, "Type", type);

                    for (int i = 1; i < columnNames.Length && columnNames[i].Trim().Length > 0; i++)
                    {
                        string propName = columnNames[i];
                        string value = valuesStr[i];

                        var property = ImportUtility.GetProperty(questData, propName, false);

                        if (property != null && value.Trim().Length > 0)
                        {
                            if (property.PropertyType == typeof(RewardItem))
                            {
                                var reward = new RewardItem();
                                ImportUtility.ParseAndSetPropertyValue(reward, "Type", valuesStr[i]);

                                switch (reward.Type)
                                {
                                    case ItemType.Power:
                                        ImportUtility.ParseAndSetPropertyValue(reward, "Power", valuesStr[i + 1]);
                                        break;

                                    case ItemType.Chest:
                                        ImportUtility.ParseAndSetPropertyValue(reward, "ChestType", valuesStr[i + 1]);
                                        break;

                                    default:
                                        break;
                                }

                                ImportUtility.ParseAndSetPropertyValue(reward, "Count", valuesStr[i + 2]);
                                ImportUtility.SetPropertyValue(questData, "Reward", reward);
                                i += 2;
                            }
                            else
                            {
                                ImportUtility.ParseAndSetPropertyValue(questData, propName, value);
                            }
                        }
                    }
                }
            }
            
            SaveSettingsJson(data, dataName, true);
        }
    }
}
#endif