using System;
using System.Text;

public struct Item : IEquatable<Item>, IFormattable
{
    public bool ReachedLimit => count == limit;
        
    public string name;

    public int count;

    public int limit;

    public Item(string name, int count) : this(name, 1, count)
    {
    }
    
    public Item(string name, int limit = 1, int count = 1)
    {
        this.name = name;
        this.count = count;
        this.limit = limit;
    }

    public override bool Equals(object other)
    {
        if (!(other is Item item))
            return false;

        return Equals(item);
    }

    public bool Equals(Item other) => name == other.name && limit == other.limit;

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
        builder.Append(count);
        builder.Append("/");
        builder.Append(limit);
        builder.Append(") }");
        return builder.ToString();
    }
}
