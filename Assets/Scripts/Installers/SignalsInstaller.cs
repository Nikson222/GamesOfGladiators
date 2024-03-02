using Signals;
using Zenject;

namespace Installers
{
    public class SignalsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<OnFieldSizeChanging>();
            Container.DeclareSignal<OnGamePanelShowSignal>();
            Container.DeclareSignal<OnMainMenuShowSignal>();
            Container.DeclareSignal<OnFieldCreatedSignal>();
            Container.DeclareSignal<OnAllElementsModelGeneratedSignal>();
            Container.DeclareSignal<OnGamePanelShowedSignal>();
            Container.DeclareSignal<OnSpinButtonClickSignal>();
            Container.DeclareSignal<OnSuccessSpinSignal>();
            Container.DeclareSignal<OnWinningRelustRollingSignal>();
            Container.DeclareSignal<OnChangeWinningElementsGeneratedSignal>();
            Container.DeclareSignal<OnElementsViewWinningsChangedSignal>();
            Container.DeclareSignal<OnAllViewElementsGeneratedSignal>();
            Container.DeclareSignal<OnAnimationSpawnAllElementsFinishSignal>();
            Container.DeclareSignal<OnRollingFullEndedSignal>();
            Container.DeclareSignal<OnAutoRollEndedSignal>();
        }
    }
}