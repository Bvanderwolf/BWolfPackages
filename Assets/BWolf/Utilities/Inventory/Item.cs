using System;
using System.Text;

public struct Item : IEquatable<Item>, IFormattable
{
    public bool ReachedLimit => stackCount == stackLimit;
        
    public string name;

    public int stackCount;

    public int stackLimit;

    public Item(string name, int stackLimit = 1, int stackCount = 1)
    {
        this.name = name;
        this.stackCount = stackCount;
        this.stackLimit = stackLimit;
    }

    public override bool Equals(object other)
    {
        if (!(other is Item item))
            return false;

        return Equals(item);
    }

    public bool Equals(Item other) => name == other.name && stackLimit == other.stackLimit;

    public override int GetHashCode() => name.GetHashCode();

    public static bool operator ==(Item lhs, Item rhs) => lhs.Equals(rhs);

    public static bool operator !=(Item lhs, Item rhs) => !(lhs == rhs);

    public override string ToString() => ToString(null, null);

    public string ToString(string format) => ToString(format, null);

    public string ToString(string format, IFormatProvider formatProvider)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("{ ");
        builder.Append(name);
        builder.Append(" , (");
        builder.Append(stackCount);
        builder.Append("/");
        builder.Append(stackLimit);
        builder.Append(") }");
        return builder.ToString();
    }
}
