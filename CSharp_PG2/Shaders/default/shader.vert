#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec3 aNormal;
layout(location = 2) in vec2 aTexCoord;

uniform mat4 model = mat4(1.0);
uniform mat4 view = mat4(1.0);
uniform mat4 proj = mat4(1.0);

out vec2 texCoord;
out vec3 Normal;
out vec3 FragPos;

void main(void)
{
    // Position of vertex is multiplied by model, view and projection matrix
    
    Normal = aNormal;//mat3(transpose(inverse(model))) * aNormal;
    // Every position has its own color
    texCoord = aTexCoord;
    FragPos = vec3(model * vec4(aPosition, 1.0f));
    gl_Position = proj * view * model * vec4(aPosition, 1.0);
  
}