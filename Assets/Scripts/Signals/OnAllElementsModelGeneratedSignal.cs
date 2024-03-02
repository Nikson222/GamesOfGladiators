using System.Collections.Generic;
using Field;
using ScriptableObjects;

namespace Signals
{
    public class OnAllElementsModelGeneratedSignal
    {
        public List<ElementsTypeEnum> elementsTypesList;
        public FieldInstanceData fieldInstanceData;

        public OnAllElementsModelGeneratedSignal(List<ElementsTypeEnum> elementsTypesList, FieldInstanceData fieldInstanceData)
        {
            this.elementsTypesList = elementsTypesList;
            this.fieldInstanceData = fieldInstanceData;
        }
    }
}