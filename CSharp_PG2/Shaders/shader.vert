#version 330 core

// On 0th position we have position of vertex
layout(location = 0) in vec3 aPosition;
// On 1st position we have color of vertex
layout(location = 1) in vec3 aColor;
layout(location = 2) in vec3 aNormal;

uniform mat4 model = mat4(1.0);
uniform mat4 view = mat4(1.0);
uniform mat4 proj = mat4(1.0);

out vec3 color;
out vec3 Normal;
out vec3 crntPos;

void main(void)
{
    // Position of vertex is multiplied by model, view and projection matrix
    gl_Position = proj * view * model * vec4(aPosition, 1.0);
    Normal = mat3(transpose(inverse(model)))* aNormal;
    // Every position has its own color
    color = aColor;
    crntPos = vec3(model * vec4(aPosition, 1.0f));
  
}