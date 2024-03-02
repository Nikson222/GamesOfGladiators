using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using ScriptableObjects;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace Field
{
    public class FieldModel
    {
        private SignalBus _signalBus;
        private FieldSizeEnum _fieldSize;
        private FieldInstanceData _currentFieldInstanceData;

        private FieldSizeDB _fieldSizeDB;
        private ElementsSettingsDB _elementsDB;

        private List<ElementsTypeEnum> _currentFieldElements;

        public FieldSizeEnum FieldSize => _fieldSize;

        private int _currentAutoplayCount;
        
        public FieldModel(SignalBus signalBus, FieldSizeDB fieldSizeDB, ElementsSettingsDB elementsDB)
        {
            _signalBus = signalBus;
            _fieldSizeDB = fieldSizeDB;
            _elementsDB = elementsDB;

            _currentFieldElements = new List<ElementsTypeEnum>();

            _signalBus.Subscribe<OnGamePanelShowedSignal>(GenerateAllElements);
            _signalBus.Subscribe<OnAllViewElementsGeneratedSignal>(ApplyResultRolling);
            _signalBus.Subscribe<OnElementsViewWinningsChangedSignal>(ApplyResultRolling);
        }

        public void SetFieldSize(FieldSizeEnum fieldSize)
        {
            _fieldSize = fieldSize;

            _signalBus.Fire(new OnFieldSizeChanging(_fieldSize));
        }

        public void SetCurrentFieldInstanceData(FieldInstanceData fieldInstanceData)
        {
            _currentFieldInstanceData = fieldInstanceData;
        }

        public void GenerateAllElements()
        {
            _currentFieldElements.Clear();
            int elementsCount = _fieldSizeDB.GetFieldSizeSettings(_currentFieldInstanceData.FieldSizeType).columns
                                * _fieldSizeDB.GetFieldSizeSettings(_currentFieldInstanceData.FieldSizeType).rows;

            for (int i = 0; i < elementsCount; i++)
            {
                ElementsTypeEnum elementType =
                    (ElementsTypeEnum)Random.Range(0, Enum.GetValues(typeof(ElementsTypeEnum)).Length);
                _currentFieldElements.Add(elementType);
            }

            _signalBus.Fire(new OnAllElementsModelGeneratedSignal(_currentFieldElements, _currentFieldInstanceData));
        }

        private void ApplyResultRolling()
        {
            var winCoefficient = CheckWinCondition();

            if (winCoefficient.isWin)
            {
                _signalBus.Fire(new OnWinningRelustRollingSignal(winCoefficient.coefficient));
                
                GenerateChangeWinningElements(winCoefficient.winElementType);
            }
            else
            {
                _signalBus.Fire<OnRollingFullEndedSignal>();
            }
        }

        private void GenerateChangeWinningElements(ElementsTypeEnum elementType)
        {
            int elementsCount = _fieldSizeDB.GetFieldSizeSettings(_currentFieldInstanceData.FieldSizeType).columns
                                * _fieldSizeDB.GetFieldSizeSettings(_currentFieldInstanceData.FieldSizeType).rows;

            for (int i = 0; i < elementsCount; i++)
            {
                if (_currentFieldElements[i] == elementType)
                {
                    ElementsTypeEnum newElementType =
                        (ElementsTypeEnum)Random.Range(0, Enum.GetValues(typeof(ElementsTypeEnum)).Length);
                    
                    _currentFieldElements[i] = newElementType;
                }
            }

            _signalBus.Fire(new OnChangeWinningElementsGeneratedSignal(elementType, _currentFieldElements));
        }
        
        private WinCoefficient CheckWinCondition()
        {
            var maxElement = GetSomeMaxElementCount();
            
            int elementsCount = _fieldSizeDB.GetFieldSizeSettings(_currentFieldInstanceData.FieldSizeType).columns
                                * _fieldSizeDB.GetFieldSizeSettings(_currentFieldInstanceData.FieldSizeType).rows;
            
            var winRate = _elementsDB.GetElementStyleSettings(maxElement.Type).GetWinCoefficients(maxElement.Count, elementsCount);

            return new WinCoefficient
            {
                isWin = winRate > 0,
                winElementType = maxElement.Type,
                coefficient = winRate
            };
        }

        private MaxElement GetSomeMaxElementCount()
        {
            var winElement = _currentFieldElements.GroupBy(x => x)
                .Select(g => new MaxElement() { Type = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count).FirstOrDefault();
            
            return winElement;
        }

        private struct MaxElement
        {
            public ElementsTypeEnum Type;
            public int Count;
        }

        private struct WinCoefficient
        {
            public bool isWin;
            public ElementsTypeEnum winElementType;
            public float coefficient;
        }
    }
}