using Field;

namespace Signals
{
    public class OnFieldCreatedSignal
    {
        public FieldInstanceData FieldInstance;

        public OnFieldCreatedSignal(FieldInstanceData fieldInstanceData)
        {
            FieldInstance = fieldInstanceData;
        }
    }
}