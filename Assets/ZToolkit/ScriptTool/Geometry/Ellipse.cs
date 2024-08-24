/*

*/

using UnityEngine;

namespace ZToolKit
{   
    public class Ellipse
    {
        public float EllipseA => mAValue;
        public float EllipseB => mBValue;
        
        private readonly float mAValue;
        private readonly float mBValue;

        private readonly float mXDelta;
        private readonly float mYDelta;

        public Ellipse(float aValue, float bValue, float xDelta = 0, float yDelta = 0)
        {
            mAValue = aValue;
            mBValue = bValue;
            mXDelta = xDelta;
            mYDelta = yDelta;
        }

        public (float yMax, float yMin) GetYFromX(float x)
        {
            float y = mBValue * Mathf.Sqrt(1 - (x + mXDelta) * (x + mXDelta) / (mAValue * mAValue));

            return (y - mYDelta, -y - mYDelta);
        }
    }
} 