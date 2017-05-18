namespace LiteIoCContainer.Tests.TestObjects
{
    public class MultiConstructorObject
    {
        public bool wasDefaultConstructorUsed { get; }

        public MultiConstructorObject()
        {
            wasDefaultConstructorUsed = true;
        }

        public MultiConstructorObject(ISimple simple)
        {
            wasDefaultConstructorUsed = false;
        }
    }
}
