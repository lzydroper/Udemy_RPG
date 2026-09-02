using Unity.Mathematics;

namespace Stats
{
    public readonly struct FloatModifier : IModifier<FloatModifier, float>
    {
        public float BaseValue { get; }
        public float Value { get; }

        public FloatModifier(float baseValue, float value)
        {
            BaseValue = baseValue;
            Value = value;
        }
    
        // 隐式转换
        public static implicit operator (float b, float v)(FloatModifier f) => (f.BaseValue, f.Value);
        public static implicit operator FloatModifier((float b, float v) f) => new(f.b, f.v);
        public static implicit operator float(FloatModifier              f) => f.Value;
        public static implicit operator FloatModifier(float              f) => new(f, f);
    
        public FloatModifier Add(float    amount)     => new(BaseValue, Value + amount);
        public FloatModifier Sub(float    amount)     => Add(-amount);
        public FloatModifier Mul(float    multiplier) => new(BaseValue, Value * multiplier);
        public FloatModifier PerAdd(float percentage) => new(BaseValue, 
            Value * (1f + math.clamp(percentage, 0f, 1f)));
        public FloatModifier PerSub(float percentage) => new(BaseValue,
            Value * (1f - math.clamp(percentage, 0f, 1f)));
        public FloatModifier Reset()                  => BaseValue;
    }
}