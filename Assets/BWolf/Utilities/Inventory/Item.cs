using System;
using System.Text;

namespace BWolf.Gameplay
{
    /// <summary>
    /// Represents an item usable in an inventory.
    /// </summary>
    public struct Item : IEquatable<Item>, IFormattable
    {
        /// <summary>
        /// Whether the item is fully stacked.
        /// </summary>
        public bool IsStacked => stackCount == stackLimit;

        /// <summary>
        /// The name of the item.
        /// </summary>
        public string name;

        /// <summary>
        /// The current stack count of the item.
        /// </summary>
        public int stackCount;
        
        /// <summary>
        /// The stack limit of the item.
        /// </summary>
        public int stackLimit;

        /// <summary>
        /// Creates a new instance of an item.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="stackLimit">The stack count of the item.</param>
        /// <param name="stackCount">The stack limit of the item.</param>
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
}