using System.Collections.Generic;
using System.Linq;
using Elements;
using Enums;
using ScriptableObjects;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Field
{
    public class FieldFactory : IFactory<FieldInstanceData>
    {
        //TODO change factory out type to fieldView
        private SignalBus _signalBus;
        private const string ELEMENT_PREFAB_NAME = "Element";
        
        private FieldSizeDB _fieldSizeDB;
        private FieldSizeEnum _fieldSizeType;
        private GameFieldParent _gameFieldParent;
        
        private List<FieldInstanceData> _fieldInstances;
        
        public FieldFactory(FieldSizeDB fieldSizeDB, SignalBus signalBus, GameFieldParent gameFieldParent)
        {
            _fieldSizeDB = fieldSizeDB;
            _signalBus = signalBus;
            _gameFieldParent = gameFieldParent;
            
            _signalBus.Subscribe<OnFieldSizeChanging>(OnFieldSizeChanging);
            _signalBus.Subscribe<OnGamePanelShowSignal>(() => Create());
            
            _fieldInstances = new List<FieldInstanceData>();
        }

        private void OnFieldSizeChanging(OnFieldSizeChanging signal)
        { 
            _fieldSizeType = signal.FieldSizeType;
        }
        
        public FieldInstanceData Create()
        {
            foreach (var fieldInstance in _fieldInstances) fieldInstance.FieldPanelInstance.gameObject.SetActive(false);

            if (_fieldInstances.Exists(x => x.FieldSizeType == _fieldSizeType))
            {
                var currentFieldData = 
                    _fieldInstances.First(x => x.FieldSizeType == _fieldSizeType);
                
                currentFieldData.FieldPanelInstance.gameObject.SetActive(true);
                
                _signalBus.Fire(new OnFieldCreatedSignal(currentFieldData));

                return currentFieldData;
            }

            var newFieldInstance = InstantiateField();
            
            _signalBus.Fire(new OnFieldCreatedSignal(newFieldInstance));
            
            return newFieldInstance;
        }

        private FieldInstanceData InstantiateField()
        {
            var prefabField = _fieldSizeDB.GetFieldSizeSettings(_fieldSizeType).FieldPanelPrefab;
            
            var scaleSize = prefabField.transform.localScale;
            
            var field = Object.Instantiate(prefabField, _gameFieldParent.transform);
            field.transform.localScale = scaleSize;

            ObjectPooler<ElementView> objectPooler = new ObjectPooler<ElementView>(12);
            objectPooler.Init(Resources.Load<ElementView>(ELEMENT_PREFAB_NAME), field.transform);
            
            var fieldInstanceSave = new FieldInstanceData
            {
                FieldSizeType = _fieldSizeType,
                FieldPanelInstance = field,
                ObjectPooler = objectPooler
            };

            _fieldInstances.Add(fieldInstanceSave);
            
            return fieldInstanceSave;
        }
    }
    
    
    
    public class FieldInstanceData
    {
        public FieldSizeEnum FieldSizeType;
        public FieldView FieldPanelInstance;
        public ObjectPooler<ElementView> ObjectPooler; 
    }
}
