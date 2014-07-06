﻿using System;
using System.Drawing;

namespace Fortune.FromJS {
    /// <summary>
    /// Adapted from http://philogb.github.io/blog/assets/voronoijs/voronoi.html
    /// </summary>


    public class Site : IEquatable<Site>, IComparable<Site> {
        public float X { get; private set; }
        public float Y { get; private set; }

        public Site(float x, float y) {
            X = x;
            Y = y;
        }



        public bool Equals(Site other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Site) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (X.GetHashCode()*397) ^ Y.GetHashCode();
            }
        }

        public static bool operator ==(Site left, Site right) { return Equals(left, right); }
        public static bool operator !=(Site left, Site right) { return !Equals(left, right); }



        public int CompareTo(Site other) {
            if (other == null) return 1;
            if (Y < other.Y) return -1;
            if (Y > other.Y) return 1;
            if (X < other.X) return -1;
            if (X > other.X) return 1;
            return 0;
        }

        public static implicit operator Point(Site p) {
            return new Point((int)p.X, (int)p.Y);
        }
        public static implicit operator PointF(Site p) {
            return new PointF((int)p.X, (int)p.Y);
        }

        public override string ToString() { return string.Format("[{0},{1}]", (int)X, (int)Y); }
    }

    
}