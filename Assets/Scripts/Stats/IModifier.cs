namespace Stats
{
    // 泛型协变，对输出参数进行转换，限定只能用于返回，集合多态
// 泛型逆变，对输入参数进行转换，限定只能用于入参，泛型委托
// 奇异递归模板模式，基类将派生类作为模板参数，派生类实现接口时将自己作为泛型参数，实现静态多态：
//      1.性能开销，避免虚函数表查找，结构体实现接口会有装箱拆箱GC问题
//      2.类型信息，父类or接口方法返沪this或接收同类
//      特点：在编译期就绑定了
    public interface IModifier<out TModifier, TValue>           // 泛型协变
        where TModifier : struct, IModifier<TModifier, TValue>  // CRTP
        where TValue : struct
    {
        // 命令查询分离(CQS)，查询只有get，命令只有set，不同时做两件事
        // 不可变模式，对象一旦被创建，内部数据就无法更改了（这里表现为所有命令直接new回去一个），防止数值污染
        // 二元数值模型
        // 优势：低内存开销；代价：无法单独撤销某一步操作，只能全部重置（另一种树状修饰器列表List<IModifier>可以单独撤销）
        TValue BaseValue { get; }
        TValue Value { get; }
    
        TModifier Add(TValue amount);
        TModifier Sub(TValue amount);
        TModifier Mul(TValue    multiplier);
        TModifier PerAdd(TValue percentage);
        TModifier PerSub(TValue percentage);
        TModifier Reset();
    }
}