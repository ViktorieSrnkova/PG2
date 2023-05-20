using System;
using System.Collections.Generic;
using CSharp_PG2.Utils;
using OpenTK.Mathematics;

namespace CSharp_PG2
{
    public class Annulus
    {
        private readonly Mesh _mesh;

        public Matrix4 Model { get; set; } = Matrix4.Identity;
        public Matrix4 View { get; set; } = Matrix4.LookAt(new Vector3(0, 0, 3), Vector3.Zero, Vector3.UnitY);
        public Matrix4 Projection;

        public Annulus(Shader shader, Matrix4 projection)
        {
            var vertices = new List<Vertex>();
            var indices = new List<uint>();

            var segmentCount = 1000;
            var innerRadius = 0.5f;
            var outerRadius = 1.0f;

            for (int i = 0; i < segmentCount; i++)
            {
                // Calculate angle using formula: angle = 2 * pi * i / segmentCount
                var angle = (float)i / segmentCount * MathHelper.TwoPi;
                var x = (float)Math.Cos(angle);
                var y = (float)Math.Sin(angle);

                // Calculate inner vertex position
                var innerPosition = new Vector3(x * innerRadius, y * innerRadius, 0f);
                // var innerColor = ColorUtils.ColorFromHSV(i * 1f / segmentCount, 1f, 1f);
                var innerColor = ColorUtils.ColorFromHSV(0, 1f, 1f);

                // Calculate outer vertex position
                var outerPosition = new Vector3(x * outerRadius, y * outerRadius, 0f);
                // var outerColor = ColorUtils.ColorFromHSV(i * 1f / segmentCount, 1f, 1f);
                var outerColor = new Vector3(0f, 0, 1f);

                // Add to list of vertices
                vertices.Add(new Vertex { Position = innerPosition, Color = innerColor });
                vertices.Add(new Vertex { Position = outerPosition, Color = outerColor });

                // Calculate indices
                indices.Add((uint)(i * 2));
                indices.Add((uint)(i * 2 + 1));
                indices.Add((uint)((i + 1) % segmentCount * 2));

                indices.Add((uint)(i * 2 + 1));
                indices.Add((uint)((i + 1) % segmentCount * 2 + 1));
                indices.Add((uint)((i + 1) % segmentCount * 2));
            }
            
            Projection = projection;
            // _mesh = new Mesh(shader, vertices.ToArray());
        }
        
        public void Draw()
        {
            _mesh.Draw(Model, View, Projection);
        }
    }
}