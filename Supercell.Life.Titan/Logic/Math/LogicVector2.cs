namespace Supercell.Life.Titan.Logic.Math
{
    using Supercell.Life.Titan.DataStream;

    public class LogicVector2
    {
        public int X;
        public int Y;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicVector2"/> class.
        /// </summary>
        public LogicVector2()
        {
            // LogicVector2.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicVector2"/> class.
        /// </summary>
        public LogicVector2(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Gets the angle.
        /// </summary>
        public int Angle
        {
            get
            {
                return LogicMath.GetAngle(this.X, this.Y);
            }
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        public int Length
        {
            get
            {
                int length = 0x7FFFFFFF;

                if ((uint)(46340 - this.X) <= 92680)
                {
                    if ((uint)(46340 - this.Y) <= 92680)
                    {
                        int lengthX = this.X * this.X;
                        int lengthY = this.Y * this.Y;

                        if ((uint)lengthY < (lengthX ^ 0x7FFFFFFFu))
                        {
                            length = lengthX + lengthY;
                        }
                    }
                }

                return LogicMath.Sqrt(length);
            }
        }

        /// <summary>
        /// Gets the length squared.
        /// </summary>
        public int LengthSquared
        {
            get
            {
                int length = 0x7FFFFFFF;

                if ((uint)(46340 - this.X) <= 92680)
                {
                    if ((uint)(46340 - this.Y) <= 92680)
                    {
                        int lengthX = this.X * this.X;
                        int lengthY = this.Y * this.Y;

                        if ((uint)lengthY < (lengthX ^ 0x7FFFFFFFu))
                        {
                            length = lengthX + lengthY;
                        }
                    }
                }

                return length;
            }
        }

        /// <summary>
        /// Adds the X and Y from the specified <see cref="LogicVector2"/> to this instance's X and Y.
        /// </summary>
        public void Add(LogicVector2 vector2)
        {
            this.X += vector2.X;
            this.Y += vector2.Y;
        }

        /// <summary>
        /// Subtracts the X and Y from the specified <see cref="LogicVector2"/> from this instance's X and Y.
        /// </summary>
        public void Subtract(LogicVector2 vector2)
        {
            this.X -= vector2.X;
            this.Y -= vector2.Y;
        }

        /// <summary>
        /// Multiplies this instance's X and Y by X and Y in the the specified <see cref="LogicVector2"/>.
        /// </summary>
        public void Multiply(LogicVector2 vector2)
        {
            this.X *= vector2.X;
            this.Y *= vector2.Y;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        public LogicVector2 Clone()
        {
            return new LogicVector2(this.X, this.Y);
        }

        /// <summary>
        /// Calculates the dot product.
        /// </summary>
        public int Dot(LogicVector2 vector2)
        {
            return this.X * vector2.X + this.Y * vector2.Y;
        }

        /// <summary>
        /// Gets the angle between specified X and Y.
        /// </summary>
        public int GetAngleBetween(int x, int y)
        {
            return LogicMath.GetAngleBetween(LogicMath.GetAngle(this.X, this.Y), LogicMath.GetAngle(x, y));
        }

        /// <summary>
        /// Gets the distance.
        /// </summary>
        public int GetDistance(LogicVector2 vector2)
        {
            int x = this.X - vector2.X;
            int distance = 0x7FFFFFFF;

            if ((uint)(x + 46340) <= 92680)
            {
                int y = this.Y - vector2.Y;

                if ((uint)(y + 46340) <= 92680)
                {
                    int distanceX = x * x;
                    int distanceY = y * y;

                    if ((uint)distanceY < (distanceX ^ 0x7FFFFFFFu))
                    {
                        distance = distanceX + distanceY;
                    }
                }
            }

            return LogicMath.Sqrt(distance);
        }

        /// <summary>
        /// Gets the distance squared.
        /// </summary>
        public int GetDistanceSquared(LogicVector2 vector2)
        {
            int x = this.X - vector2.X;
            int distance = 0x7FFFFFFF;

            if ((uint)(x + 46340) <= 92680)
            {
                int y = this.Y - vector2.Y;

                if ((uint)(y + 46340) <= 92680)
                {
                    int distanceX = x * x;
                    int distanceY = y * y;

                    if ((uint)distanceY < (distanceX ^ 0x7FFFFFFFu))
                    {
                        distance = distanceX + distanceY;
                    }
                }
            }

            return distance;
        }

        /// <summary>
        /// Gets the distance squared to the specified X and Y
        /// </summary>
        public int GetDistanceSquaredTo(int x, int y)
        {
            int distance = 0x7FFFFFFF;

            x -= this.X;

            if ((uint)(x + 46340) <= 92680)
            {
                y -= this.Y;

                if ((uint)(y + 46340) <= 92680)
                {
                    int distanceX = x * x;
                    int distanceY = y * y;

                    if ((uint)distanceY < (distanceX ^ 0x7FFFFFFFu))
                    {
                        distance = distanceX + distanceY;
                    }
                }
            }

            return distance;
        }

        /// <summary>
        /// Determines whether the specified <see cref="LogicVector2"/>'s X and Y are equal to the X and Y in this instance.
        /// </summary>
        public bool IsEqual(LogicVector2 vector2)
        {
            return this.X == vector2.X && this.Y == vector2.Y;
        }

        /// <summary>
        /// Determines whether the X and Y are in an area.
        /// </summary>
        public bool IsInArea(int minX, int minY, int maxX, int maxY)
        {
            if (this.X >= minX && this.Y >= minY)
            {
                return this.X < minX + maxX && this.Y < maxY + minY;
            }

            return false;
        }

        /// <summary>
        /// Normalizes the X and Y using the specified value.
        /// </summary>
        public int Normalize(int value)
        {
            int length = this.Length;

            if (length != 0)
            {
                this.X = this.X * value / length;
                this.Y = this.Y * value / length;
            }

            return length;
        }

        /// <summary>
        /// Rotates the X and Y by the specified number of degrees.
        /// </summary>
        public void Rotate(int degrees)
        {
            this.X = LogicMath.GetRotatedX(this.X, this.Y, degrees);
            this.Y = LogicMath.GetRotatedY(this.X, this.Y, degrees);
        }

        /// <summary>
        /// Sets the X and Y.
        /// </summary>
        public void Set(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        
        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public void Decode(ByteStream stream)
        {
            this.X = stream.ReadInt();
            this.Y = stream.ReadInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public void Encode(ChecksumEncoder stream)
        {
            stream.WriteInt(this.X);
            stream.WriteInt(this.Y);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return $"({this.X}, {this.Y})";
        }
    }
}
