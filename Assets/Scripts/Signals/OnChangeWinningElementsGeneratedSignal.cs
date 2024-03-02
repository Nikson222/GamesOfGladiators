using System.Collections.Generic;
using ScriptableObjects;

namespace Signals
{
    public class OnChangeWinningElementsGeneratedSignal
    {
        public ElementsTypeEnum winningElementType;
        public List<ElementsTypeEnum> winningElementsList = new List<ElementsTypeEnum>();

        public OnChangeWinningElementsGeneratedSignal(ElementsTypeEnum winningElementType, List<ElementsTypeEnum> winningElementsList)
        {
            this.winningElementType = winningElementType;
            this.winningElementsList = winningElementsList;
        }
    }
}