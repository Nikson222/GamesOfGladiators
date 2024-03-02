using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ElementsSettingsDB", menuName = "ScriptableObjects/ElementsSettingsDB", order = 1)]
    public class ElementsSettingsDB : ScriptableObject
    {
        public ElementsSettings[] elementsStyleSettings;

        public ElementsSettings GetElementStyleSettings(ElementsTypeEnum elementsTypeEnum)
        {
            return elementsStyleSettings.First(x => x.elementsTypeType == elementsTypeEnum);
        }
    }

    [Serializable]
    public class ElementsSettings
    {
        public ElementsTypeEnum elementsTypeType;
        public Sprite sprite;
        public Sprite winSprite;

        public ElementWinSettings winSettings;

        public float GetWinCoefficients(int elementCount, int fieldElementsCount)
        {
            float winCoeficient = 0;

            foreach (var winCoeficientSettings in winSettings.winCoeficients)
            {
                float winCountMultiplier =
                    (float)winCoeficientSettings.winCount / winSettings.standartFieldElementCounts;
                int winCount = (int)(fieldElementsCount * winCountMultiplier);

                if ((fieldElementsCount * winCountMultiplier) % 1 >= 0.1f)
                    winCount++;
                
                if (elementCount >= winCount)
                    winCoeficient = winCoeficientSettings.coefficient;
                else
                    break;
            }

            return winCoeficient;
        }
    }

    [Serializable]
    public class ElementWinSettings
    {
        public int standartFieldElementCounts = 30;

        public List<WinCoefficient> winCoeficients;
    }

    [Serializable]
    public class WinCoefficient
    {
        public int winCount;
        public float coefficient;
    }

    public enum ElementsTypeEnum
    {
        one,
        two,
        three,
        four,
        five,
        six,
        seven,
        eight
    }
}