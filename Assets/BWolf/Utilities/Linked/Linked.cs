using UnityEngine;

public class Linked<T>
{
    public T Value { get; private set; }
    
    public readonly Linked<T>[] links;

    public Linked(int count = 1)
    {
        links = new Linked<T>[count];
    }

    public Linked<T> AddLink(T value, int count = 1)
    {
        Linked<T> link = new Linked<T>(count);
        link.Value = value;
        
        AddLinkAtFirstNonNullIndex(link);
        
        return link;
    }

    private void AddLinkAtFirstNonNullIndex(Linked<T> link)
    {
        for (int i = 0; i < links.Length; i++)
        {
            if (links[i] != null)
            {
                links[i] = link;
                break;
            }
        }
                
    }
}
