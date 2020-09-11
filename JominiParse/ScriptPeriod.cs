namespace JominiParse
{
    public class ScriptPeriod : ScriptObject
    {
        public override string ToScript()
        {
            return Name + @" = { 
    " + periodType + "=" + value.GetIntValue() + @"
        }";
    }

        public int Days
        {
            get
            {
                switch (periodType)
                {
                    case Period.days:
                        return value.GetIntValue();
                    case Period.months:
                        return value.GetIntValue() * 30;
                    case Period.years:
                        return value.GetIntValue() * 365;
                }

                return 0;
            }
        }
        public float Years
        {
            get
            {
                switch (periodType)
                {
                    case Period.days:
                        return value.GetIntValue() / 365.0f;
                    case Period.months:
                        return value.GetIntValue() / 12.0f;
                    case Period.years:
                        return value.GetIntValue();
                }

                return 0;
            }
        }
        public ScriptValue value { get; set; }
        public Period periodType { get; set; }
     
        public override string ToString()
        {
            return value.GetIntValue() + " " + periodType;
        }

        public ScriptPeriod(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
    }
}