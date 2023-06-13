namespace Structs
{
    [System.Serializable]
    public struct Status
    {
        public Enums.UnitNameTable unitName;

        public float maxHp;
        public float curHp;

        public float perceiveRange;
        public float attRange;
        public float maxSpeed;
        public float curSpeed;
        public float maxRunSpeed;
        public float curRunSpeed;

        public float rotationSpeed;

        public float strength;
        public float luck;

        public int hitCount;

        internal bool isCombat;

        public Status(Enums.UnitNameTable name, float maxHp, float perceiveRange, float attRange, float maxSpeed,
                      float maxRunSpeed, float rotationSpeed, float strength, float luck)
        {
            this.unitName                    = name;
            this.maxHp = this.curHp          = maxHp;
            this.perceiveRange               = perceiveRange;
            this.attRange                    = attRange;
            this.curRunSpeed = this.maxSpeed = maxSpeed;
            this.curSpeed                    = .0f;
            this.maxRunSpeed                 = maxRunSpeed;
            this.rotationSpeed               = rotationSpeed;
            this.strength                    = strength;
            this.luck                        = luck;
            this.hitCount                    = 0;
            this.isCombat                    = false;
        }
    }
}