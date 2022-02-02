using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.ViewModels
{
    public class ModelWiseProblemViewModel
    {
        public string ModelName { get; set; }
        public int Dead { get; set; }
        public int NetworkProblem { get; set; }
        public int LCDProblem { get; set; }
        public int Batterycantstartmobile { get; set; }
        public int DCpowercantstartmobile { get; set; }
        public int Autopoweron { get; set; }
        public int Autoreboot { get; set; }
        public int Cantpoweroff { get; set; }
        public int Noincomingcall { get; set; }
        public int Communicationecho { get; set; }
        public int AutoCalldrop { get; set; }
        public int Noincomingvideocall { get; set; }
        public int Nooutgoingvideocall { get; set; }
        public int LowsoundatreceiverendOutgoing { get; set; }
        public int Nosoundatreceiverendoutgoing { get; set; }
        public int Nosoundatcallersendincoming { get; set; }
        public int Lowsoundatcallersendincoming { get; set; }
        public int Nosignal { get; set; }
        public int Weaksignal { get; set; }
        public int Unstablesignal { get; set; }
        public int Nonetwork { get; set; }
        public int Searchnetwork { get; set; }
        public int Noservice { get; set; }
        public int CantidentifySIMcard { get; set; }
        public int CantidentifyTorSDcard { get; set; }
        public int WiFidoesnotgetthesignal { get; set; }
        public int WiFidoesnotconnected { get; set; }
        public int CantdualSimdualstandby { get; set; }
        public int BTcanttransferfiles { get; set; }
        public int DataConnectionProblem { get; set; }
        public int Notcharging { get; set; }
        public int WarningBadContact { get; set; }
        public int Chargerindicatorlightabnormal { get; set; }
        public int Chargedoesnotstore { get; set; }
        public int LowBatteryshutdown { get; set; }
        public int Batteryexplosion { get; set; }
        public int Lowstandbytime { get; set; }
        public int Autocharge { get; set; }
        public int Displaylinemissing { get; set; }
        public int Flickerscreen { get; set; }
        public int Nobacklight { get; set; }
        public int Blackwhitedot { get; set; }
        public int Displaywhite { get; set; }
        public int DisplayBlack { get; set; }
        public int DisplayBlackDot { get; set; }
        public int Noringtonesound { get; set; }
        public int Lowringtonesound { get; set; }
        public int NoisySound { get; set; }
        public int Novibration { get; set; }
        public int Vibrationnoise { get; set; }
        public int Weakvibration { get; set; }
        public int Missingcomponents { get; set; }
        public int SIMCardbasebroken { get; set; }
        public int CameraHang { get; set; }
        public int Earphonenoise { get; set; }
        public int Cantuseearphone { get; set; }
        public int Nonsteadyhang { get; set; }
        public int Steadyhang { get; set; }
        public int Ioportliquiddamage { get; set; }
        public int Liquiddamage { get; set; }
        public int Displaybroken { get; set; }
        public int Firedamage { get; set; }
        public int Ioconnectorbroken { get; set; }
        public int Mainlensbroken { get; set; }
        public int SIMReaderbroken { get; set; }
        public int Uppershellbroken { get; set; }
        public int TouchPadBroken { get; set; }
        public int InvalidIMEI { get; set; }
        public int Phonepasswordblock { get; set; }
        public int ChargeNotStay { get; set; }
        public int HPSymbolShow { get; set; }
        public int KeypadProblem { get; set; }
        public int AutoPowerOnOff { get; set; }
        public int CameraProblem { get; set; }
        public int BatteryProblem { get; set; }
        public int MemoryNotFound { get; set; }
        public int AutoOff { get; set; }
        public int TouchLightProblem { get; set; }
        public int SoftwareProblem { get; set; }
        public int SpeakerProblem { get; set; }
        public int MicrophoneProblem { get; set; }
        public int ReceiverProblem { get; set; }
        public int AllCheck { get; set; }
        public int HeadPhoneSymbolShow { get; set; }
        public int GiftBoxDamage { get; set; }
        public int ChargerProblem { get; set; }
        public int BatteryCoverProblem { get; set; }
        public int TouchPadProblem { get; set; }
        public int LCDLanceBroken { get; set; }
        public int OverHeatProblem { get; set; }
        public int LEDKeyProblem { get; set; }
        public int LEDKeyMissing { get; set; }
    }
}
