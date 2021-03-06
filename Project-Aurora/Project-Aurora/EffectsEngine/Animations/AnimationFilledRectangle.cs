﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Aurora.EffectsEngine.Animations
{
    public class AnimationFilledRectangle : AnimationRectangle
    {
        public AnimationFilledRectangle() : base()
        {
        }

        public AnimationFilledRectangle(RectangleF dimension, Color color, float duration = 0.0f) : base(dimension, color, 1, duration)
        {
        }

        public AnimationFilledRectangle(PointF center, float rect_width, float rect_height, Color color, float duration = 0.0f) : base(center, rect_width, rect_height, color, 1, duration)
        {
        }

        public AnimationFilledRectangle(float x, float y, float rect_width, float rect_height, Color color, float duration = 0.0f) : base(x, y, rect_width, rect_height, color, 1, duration)
        {
        }

        public override void Draw(Graphics g, float scale = 1.0f)
        {
            if (_brush == null || _invalidated)
            {
                _brush = new SolidBrush(_color);
                _invalidated = false;
            }

            RectangleF _scaledDimension = new RectangleF(_dimension.X * scale, _dimension.Y * scale, _dimension.Width * scale, _dimension.Height * scale);

            PointF rotatePoint = new PointF(_scaledDimension.X + (_scaledDimension.Width / 2.0f), _scaledDimension.Y + (_scaledDimension.Height / 2.0f));

            Matrix rotationMatrix = new Matrix();
            rotationMatrix.RotateAt(-_angle, rotatePoint, MatrixOrder.Append);

            Matrix originalMatrix = g.Transform;
            g.Transform = rotationMatrix;
            g.FillRectangle(_brush, _scaledDimension);
            g.Transform = originalMatrix;
        }

        public override AnimationFrame BlendWith(AnimationFrame otherAnim, double amount)
        {
            if (!(otherAnim is AnimationFilledRectangle))
            {
                throw new FormatException("Cannot blend with another type");
            }

            amount = GetTransitionValue(amount);

            RectangleF newrect = new RectangleF((float)CalculateNewValue(_dimension.X, otherAnim._dimension.X, amount),
                (float)CalculateNewValue(_dimension.Y, otherAnim._dimension.Y, amount),
                (float)CalculateNewValue(_dimension.Width, otherAnim._dimension.Width, amount),
                (float)CalculateNewValue(_dimension.Height, otherAnim._dimension.Height, amount)
                );

            float newAngle = (float)CalculateNewValue(_angle, otherAnim._angle, amount);

            return new AnimationFilledRectangle(newrect, Utils.ColorUtils.BlendColors(_color, otherAnim._color, amount)).SetAngle(newAngle);
        }

        public override AnimationFrame GetCopy()
        {
            RectangleF newrect = new RectangleF(_dimension.X,
                _dimension.Y,
                _dimension.Width,
                _dimension.Height
                );

            return new AnimationFilledRectangle(newrect, Color.FromArgb(_color.A, _color.R, _color.G, _color.B), _duration).SetAngle(_angle).SetTransitionType(_transitionType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AnimationFilledRectangle)obj);
        }

        public bool Equals(AnimationFilledRectangle p)
        {
            return _color.Equals(p._color) &&
                _dimension.Equals(p._dimension) &&
                _width.Equals(p._width) &&
                _duration.Equals(p._duration) &&
                _angle.Equals(p._angle);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + _color.GetHashCode();
                hash = hash * 23 + _dimension.GetHashCode();
                hash = hash * 23 + _width.GetHashCode();
                hash = hash * 23 + _duration.GetHashCode();
                hash = hash * 23 + _angle.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"AnimationFilledRectangle [ Color: {_color.ToString()} Dimensions: {_dimension.ToString()} Duration: {_duration} Angle: {_angle} ]";
        }
    }
}
