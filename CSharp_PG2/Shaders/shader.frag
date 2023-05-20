#version 330

// Needs to have same name as in the vertex shader
in vec3 color;
out vec4 outputColor;

void main()
{
    outputColor = vec4(color, 1.0);
}