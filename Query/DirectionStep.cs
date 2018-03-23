using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query
{
    public class DirectionStep : GraphQuery
    {
        private enum DirectionEnum { TO, FROM };

        private DirectionEnum Direction;
        private string Path;

        private DirectionStep(DirectionEnum direction, string path)
        {
            Direction = direction;
            Path = path;
        }

        public static DirectionStep To(string vertexID)
        {
            return new DirectionStep(DirectionEnum.TO, "g.V('" + Sanitize(vertexID) + "')");
        }

        public static DirectionStep To(VertexQuery vertices)
        {
            return new DirectionStep(DirectionEnum.TO, vertices.ToString());
        }

        public static DirectionStep From(string vertexID)
        {
            return new DirectionStep(DirectionEnum.FROM, "g.V('" + Sanitize(vertexID) + "')");
        }

        public static DirectionStep From(VertexQuery vertices)
        {
            return new DirectionStep(DirectionEnum.FROM, vertices.ToString());
        }

        public override string ToString()
        {
            string step = Direction == DirectionEnum.TO ? "to(" : "from(";
            step += Path;
            step += ")";
            return step;
        }
    }
}
