using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Landmark : MonoBehaviour 
{
    [SerializeField]
    private List<Rect> bounds;
    public bool Overlaps(Landmark other)
    {
        return bounds.Exists(bound =>
        {
            var myBound = new Rect((Vector2)transform.position + bound.position, bound.size);
            return other.bounds.Exists(bound2 =>
            {
                var otherBound = new Rect((Vector2)other.transform.position + bound2.position, bound2.size);
                return myBound.Overlaps(otherBound);
            });
        });
    }
    public bool Contains(Vector2 position, float buffer) 
    {
        return bounds.Exists(bound =>
        {
            var myBound = new Rect((Vector2)transform.position + bound.position - Vector2.one * buffer, bound.size + 2f * buffer * Vector2.one);
            return myBound.Contains(position);
        });
    }
    public Rect ExtendBoundingRect(Rect boundingRect)
    {
        bounds.ForEach(bound =>
        {
            var myBound = new Rect
            {
                min = bound.min,
                max = bound.max
            };
            myBound.position += (Vector2)transform.position;

            boundingRect.xMin = Mathf.Min(boundingRect.xMin, myBound.xMin);
            boundingRect.yMin = Mathf.Min(boundingRect.yMin, myBound.yMin);
            boundingRect.xMax = Mathf.Max(boundingRect.xMax, myBound.xMax);
            boundingRect.yMax = Mathf.Max(boundingRect.yMax, myBound.yMax);
        });
        return boundingRect;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        bounds.ForEach(bound =>
        {
            var rect = new Rect((Vector2)transform.position + bound.position, bound.size);
            Handles.DrawSolidRectangleWithOutline(rect, new Color(0, 0, 0, 0), Color.red);
        });
    }
#endif
}
