namespace BWolf.PointModifications
{
    public class PointModifier
    {
        public string name;

        public int value;

        public bool modifiesCurrent;

        public bool modifiesCurrentWithMax;

        public virtual bool IsFinished { get; private set; }

        public virtual bool AddsValue => value >= 0;
        

        public PointModifier(string name, int value, bool modifiesCurrent = true, bool modifiesCurrentWithMax = false)
        {
            this.name = name;
            this.value = value;
            this.modifiesCurrent = modifiesCurrent;
            this.modifiesCurrentWithMax = modifiesCurrentWithMax;
        }

        public virtual PointModification Modify(PointSystem pointSystem)
        {
            IsFinished = true;
            
            return new PointModification
            {
                value = value,
                modifiesCurrent = modifiesCurrent,
                modifiesCurrentWithMax = modifiesCurrentWithMax
            };
        }
    }
}
