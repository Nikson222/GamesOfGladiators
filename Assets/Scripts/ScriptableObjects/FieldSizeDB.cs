using System;
using System.Linq;
using Enums;
using Field;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using System.Text.RegularExpressions;
#endif

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "FieldSizeDB", menuName = "ScriptableObjects/FieldSizeDB", order = 56)]
    public class FieldSizeDB : ScriptableObject
    {
        [SerializeField] private FieldSizeSettings[] _fieldSizes;

        #region Validate

#if UNITY_EDITOR
        private void OnValidate()
        {
            foreach (var fieldSize in _fieldSizes)
            {
                string input = Enum.GetName(typeof(FieldSizeEnum), fieldSize.fieldSizeType);

                Match match = Regex.Match(input, @"Raws(\d+)Columns(\d+)");

                if (match.Success)
                {
                    int rows = int.Parse(match.Groups[1].Value);
                    int columns = int.Parse(match.Groups[2].Value);

                    fieldSize.columns = columns;
                    fieldSize.rows = rows;
                }
            }
        }
#endif

        #endregion
        
        public FieldSizeSettings GetFieldSizeSettings(FieldSizeEnum fieldSizeType)
        {
            return _fieldSizes.First(x => x.fieldSizeType == fieldSizeType);
        }
    }

    [Serializable]
    public class FieldSizeSettings
    {
        [FormerlySerializedAs("fieldSize")] public FieldSizeEnum fieldSizeType;
        public int rows;
        public int columns;

        public FieldView FieldPanelPrefab;
    }
}