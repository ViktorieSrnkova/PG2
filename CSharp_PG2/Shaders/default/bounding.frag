#version 330 core

uniform vec3 edgeColor;

out vec4 outColor;

void main()
{
    outColor = vec4(edgeColor, 1.0);
}