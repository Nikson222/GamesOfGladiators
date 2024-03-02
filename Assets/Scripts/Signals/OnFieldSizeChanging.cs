using Enums;

namespace Signals
{
    public class OnFieldSizeChanging
    {
        public FieldSizeEnum FieldSizeType { get; set; }

        public OnFieldSizeChanging(FieldSizeEnum fieldSizeType)
        {
            FieldSizeType = fieldSizeType;
        }
    }
}