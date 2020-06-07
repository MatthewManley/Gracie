namespace Gracie.Gateway
{
    public class GatewayPayload : Core.Payload
    {
        public Opcode GatewayOpcode
        {
            get => (Opcode)Opcode;
            set => Opcode = (byte)value;
        }
    }
}
