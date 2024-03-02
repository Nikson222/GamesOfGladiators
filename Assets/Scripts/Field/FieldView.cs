using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using DG.Tweening;
using Elements;
using ModestTree;
using ScriptableObjects;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Field
{
    public class FieldView : MonoBehaviour
    {
        private SignalBus _signalBus;
        private ElementsSettingsDB _elementsSettingsDB;

        private FieldInstanceData _currentFieldInstanceData;

        private List<ElementViewType> _currentElementViews = new List<ElementViewType>();

        private ElementAnimator _elementAnimator;

        [Inject]
        public void Construct(SignalBus signalBus, ElementsSettingsDB elementsSettingsDB,
            ElementAnimator elementAnimator)
        {
            _signalBus = signalBus;
            _elementsSettingsDB = elementsSettingsDB;
            _elementAnimator = elementAnimator;

            _signalBus.Subscribe<OnAllElementsModelGeneratedSignal>(CreateAllElements);
            _signalBus.Subscribe<OnMainMenuShowSignal>(DisableAllElements);
            _signalBus.Subscribe<OnChangeWinningElementsGeneratedSignal>(CreateNewWinningElements);
        }

        private void CreateAllElements(OnAllElementsModelGeneratedSignal signal)
        {
            bool isFirstField = _currentElementViews.IsEmpty();

            if (!isFirstField)
            {
                List<Vector3> positions = new List<Vector3>();
            
                foreach (var elementView in _currentElementViews)
                {
                    positions.Add(elementView.View.transform.localPosition);
                }
            
                var sequence = _elementAnimator.AnimateAllHideElements(_currentElementViews);
                sequence.onComplete += () => SpawnElements(signal, isFirstField, positions);
            }
            else
            {
                SpawnElements(signal, isFirstField);
            }
        }

        private void SpawnElements(OnAllElementsModelGeneratedSignal signal, bool isFirstField, List<Vector3> positions = null)
        {
            if (!isFirstField)
                DisableAllElements();

            _currentElementViews.Clear();

            _currentFieldInstanceData = signal.fieldInstanceData;

            var objectPooler = signal.fieldInstanceData.ObjectPooler;
            
            List<ElementView> elements = new List<ElementView>();

            foreach (var elementsType in signal.elementsTypesList)
            {
                var element = objectPooler.GetObject();

                element.transform.localScale = Vector3.zero;
                element.transform.position = Vector3.zero;

                var sprite = _elementsSettingsDB.GetElementStyleSettings(
                    isFirstField ? ElementsTypeEnum.one : elementsType).sprite;

                element.SetSprite(sprite);

                var viewType = new ElementViewType
                {
                    Type = elementsType,
                    View = element
                };
                elements.Add(element);
                _currentElementViews.Add(viewType);
            }

            var sequence = _elementAnimator.AnimateAllSpawnElements(elements, positions);

            if (!isFirstField)
                sequence.onComplete += () => _signalBus.Fire(new OnAllViewElementsGeneratedSignal());
            ;
        }


        //TODO: fix
        private void CreateNewWinningElements(OnChangeWinningElementsGeneratedSignal signal)
        {
            List<ElementViewType> winElements = new List<ElementViewType>();

            foreach (var element in _currentElementViews)
            {
                if (element.Type == signal.winningElementType)
                {
                    var viewType = new ElementViewType
                    {
                        Type = element.Type,
                        View = element.View
                    };
                    
                    winElements.Add(viewType);
                }
            }
            
            List<ElementsTypeEnum> newWinningElementsTypes = new List<ElementsTypeEnum>();
            
            for (int i = 0; i < signal.winningElementsList.Count; i++)
            {
                if (_currentElementViews[i].Type == signal.winningElementType)
                {
                    newWinningElementsTypes.Add(signal.winningElementsList[i]);
                    
                    _currentElementViews[i].Type = signal.winningElementsList[i];
                }
            }

            var sequence = _elementAnimator.AnimateScaleWinningElements(winElements, newWinningElementsTypes);

            sequence.onComplete += () => _signalBus.Fire(new OnElementsViewWinningsChangedSignal());
        }

        private void DisableAllElements()
        {
            foreach (var view in _currentElementViews)
            {
                _currentFieldInstanceData.ObjectPooler.ReturnObject(view.View);
            }

            _currentElementViews.Clear();
        }

        public class ElementViewType
        {
            public ElementsTypeEnum Type;
            public ElementView View;
        }
    }
}