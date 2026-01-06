using System;

namespace CypherSheet.Domain
{
    public class Pool
    {
        public string Name { get; private set; }
        public int Max { get; private set; }
        public int Current { get; private set; }
        public int Edge { get; private set; }

        public Pool(string name, int max, int edge = 0)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Pool name cannot be empty.", nameof(name));
            if (max < 0) throw new ArgumentException("Max pool cannot be negative.", nameof(max));
            
            Name = name;
            Max = max;
            Current = max;
            Edge = edge;
        }
        
        // Constructor for persistence/serialization if needed
        public Pool(string name, int max, int current, int edge)
        {
            Name = name;
            Max = max;
            Current = current;
            Edge = edge;
        }

        public void Spend(int amount)
        {
            if (amount < 0) throw new ArgumentException("Cannot spend negative amount.", nameof(amount));
            if (amount > Current) throw new InvalidOperationException("Not enough points in pool.");
            
            Current -= amount;
        }

        public void Recover(int amount)
        {
            if (amount < 0) throw new ArgumentException("Cannot recover negative amount.", nameof(amount));
            
            Current += amount;
            if (Current > Max) Current = Max;
        }

        public void SetMax(int newMax)
        {
            if (newMax < 0) throw new ArgumentException("Max cannot be negative.");
            Max = newMax;
            if (Current > Max) Current = Max;
        }
        
        public void SetEdge(int newEdge)
        {
            if (newEdge < 0) throw new ArgumentException("Edge cannot be negative.");
            Edge = newEdge;
        }
    }
}
