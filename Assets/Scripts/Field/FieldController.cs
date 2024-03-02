using Enums;
using MainMenu;
using Signals;
using Zenject;

namespace Field
{
    public class FieldController
    {
        private readonly SignalBus _signalBus;
        private readonly FieldModel _fieldModel;
    
        public FieldController(FieldModel fieldModel, MainMenuPanel mainMenuPanel,
            SignalBus signalBus)
        {
            _signalBus = signalBus;
            _fieldModel = fieldModel;
            
            mainMenuPanel.AddListenerField4x3Button(() => HandleFieldSizeInput(FieldSizeEnum.Raws3Columns4));
            mainMenuPanel.AddListenerField5x4Button(() => HandleFieldSizeInput(FieldSizeEnum.Raws4Columns5));
            mainMenuPanel.AddListenerField6x5Button(() => HandleFieldSizeInput(FieldSizeEnum.Raws5Columns6));
            mainMenuPanel.AddListenerField7x6Button(() => HandleFieldSizeInput(FieldSizeEnum.Raws6Columns7));
            
            _signalBus.Subscribe<OnFieldCreatedSignal>( HandleFieldInstanceCreated);
            _signalBus.Subscribe<OnSuccessSpinSignal>(HandleSuccesSpin);
        }

        private void HandleFieldSizeInput(FieldSizeEnum fieldSizeType)
        {
            _fieldModel.SetFieldSize(fieldSizeType);
        }

        private void HandleFieldInstanceCreated(OnFieldCreatedSignal signal)
        {
            _fieldModel.SetCurrentFieldInstanceData(signal.FieldInstance);
        }

        private void HandleSuccesSpin(OnSuccessSpinSignal signal)
        {
            _fieldModel.GenerateAllElements();
        }
    }
}