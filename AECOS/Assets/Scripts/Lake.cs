using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lake
{
    public int width;
    public int height;
    public int left;
    public int top;

    public int right()
    {
        return left + width - 1;
    }

    public int bottom()
    {
        return top + height - 1;
    }
    public bool CollidesWith(Lake other)
    {
        if (left > other.right() + 1)
            return false;

        if (top > other.bottom() + 1)
            return false;

        if (right() + 1 < other.left)
            return false;

        if (bottom() + 1 < other.top)
            return false;

        return true;
    }
}
